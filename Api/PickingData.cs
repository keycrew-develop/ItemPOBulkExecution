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
    public class PickingData : ControllerBase
    {

        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");

        [FunctionName("PickingDataShow")]
        public ObjectResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "pickingData/show")] HttpRequest req,
                ILogger log)
        {
            try
            {
                //ファイルの中身チェック
                string BarCodeValue = req.Query["BarCodeValue"];
                log.LogInformation(BarCodeValue);
                var CompanyCode = BarCodeValue.Substring(0, 6);
                var PickingNo = BarCodeValue.Substring(6, 7);
                var procedure = "[SFM_N17_DelivScan]";
                var values = new
                {
                    CompanyCode = CompanyCode,
                    PickingNo = PickingNo,
                    Cmd = "PickingDetail"
                };
                DBManager dbmanager = new DBManager();
                dbmanager.DbConect(defaultConnection);

                var pickingDetailList = new List<PickingDetail>();
                pickingDetailList = dbmanager.sqlConnection.Query<PickingDetail>(
                    procedure, values, commandType: CommandType.StoredProcedure
                    ).ToList();
                log.LogInformation(pickingDetailList.Count().ToString());
                return new OkObjectResult(pickingDetailList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(new { Message = ex.Message });
            }

        }


    }

}