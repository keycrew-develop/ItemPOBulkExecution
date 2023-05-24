using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Data;
using Dapper;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Formats.Asn1;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using static System.Net.WebRequestMethods;
using System.Net.Mime;

namespace Api
{
    public class ShippingGroupProgress : ControllerBase
    {

        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");

        [FunctionName("GetShippingGroupProgress")]
        public ObjectResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ShippingGroupProgresses")] HttpRequest req,
                ILogger log)
        {
            try
            {
                //ファイルの中身チェック
                string ShippingPointCode = req.Query["ShippingPointCode"];
                log.LogInformation(ShippingPointCode);
                DBManager dbmanager = new DBManager();
                dbmanager.DbConect(defaultConnection);
                //ヤマト佐川のリスト検索
                var ShippingGroupList = GetShippingGroupProgresses(dbmanager, ShippingPointCode, log);

                return new OkObjectResult(ShippingGroupList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(new { Message = ex.Message });
            }

        }

        public List<Data.ShippingGroupProgress> GetShippingGroupProgresses(DBManager dbmanager,string ShippingPointCode, ILogger log)
        {
            //TW_PickingHedはオールデータ、TW_zPickingHedは出荷日が今日以降のデータを抽出
            var ShippingGroupTransporterQuery = @"SELECT            ShippingGroupNo, ShippingGroupName, SUM(TotalCount) AS TotalCount, SUM(UninspectedCount) AS UninspectedCount, SUM(InspectedCount) AS InspectedCount
                                                FROM              (SELECT            dbo.TW_ShippingGroup.ShippingGroupNo, dbo.TW_ShippingGroup.ShippingGroupName, COUNT(*) AS TotalCount, SUM(CASE WHEN pickingstatecode = '400' THEN 1 ELSE 0 END) AS UninspectedCount, 
                                                                                                SUM(CASE WHEN pickingstatecode = '500' THEN 1 ELSE 0 END) AS InspectedCount
                                                                        FROM               dbo.TW_ShippingGroup INNER JOIN
                                                                                                dbo.TW_PickingHed ON dbo.TW_ShippingGroup.ShippingGroupNo = dbo.TW_PickingHed.ShippingGroupNo
                                                                        WHERE              (dbo.TW_PickingHed.ShippingPointCode = @ShippingPointCode)
                                                                        GROUP BY        dbo.TW_ShippingGroup.ShippingGroupNo, dbo.TW_ShippingGroup.ShippingGroupName
                                                                        UNION ALL
                                                                        SELECT            TW_ShippingGroup_1.ShippingGroupNo, TW_ShippingGroup_1.ShippingGroupName, COUNT(*) AS TotalCount, SUM(CASE WHEN pickingstatecode = '400' THEN 1 ELSE 0 END) AS UninspectedCount, 
                                                                                               SUM(CASE WHEN pickingstatecode = '500' THEN 1 ELSE 0 END) AS InspectedCount
                                                                        FROM              dbo.TW_ShippingGroup AS TW_ShippingGroup_1 INNER JOIN
                                                                                               dbo.TW_zPickingHed ON TW_ShippingGroup_1.ShippingGroupNo = dbo.TW_zPickingHed.ShippingGroupNo
                                                                        WHERE             (dbo.TW_zPickingHed.DateShip >= @Today) AND (dbo.TW_zPickingHed.ShippingPointCode = @ShippingPointCode)
                                                                        GROUP BY       TW_ShippingGroup_1.ShippingGroupNo, TW_ShippingGroup_1.ShippingGroupName) AS derivedtbl_1
                                                GROUP BY       ShippingGroupNo, ShippingGroupName
                                                ORDER BY       UninspectedCount DESC";

            log.LogInformation("ShippingGroupSQL BeforeExecute");

            var ShippingGroupList = dbmanager.sqlConnection.Query<Data.ShippingGroupProgress>(ShippingGroupTransporterQuery, new { ShippingPointCode = ShippingPointCode, Today = DateTime.UtcNow.ToString("yyyy-MM-dd") }).AsList();
            log.LogInformation("ShippingGroupSQL Executed");

            
            return ShippingGroupList;
        }

    }

}