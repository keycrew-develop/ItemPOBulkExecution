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
    public class PickingHead : ControllerBase
    {

        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");

        [FunctionName("PickingHeadShow")]
        public ObjectResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "pickingHead/show")] HttpRequest req,
                ILogger log)
        {
            try
            {
                //ファイルの中身チェック
                string BarCodeValue = req.Query["BarCodeValue"];
                string ShippingPointCode = req.Query["ShippingPointCode"];
                log.LogInformation(BarCodeValue);
                var CompanyCode = BarCodeValue.Substring(0, 6);
                var ShippingGroupNo = BarCodeValue.Substring(6, 7);
                var procedure = "[SFM_N17_DelivScan]";
                var values = new
                {
                    Cmd = "PickingList",
                    TransactionTypeCode = "310",
                    ShippingPointCode = ShippingPointCode,
                    CompanyCode = CompanyCode,
                    ShippingGroupNo = ShippingGroupNo
                    
                };
                DBManager dbmanager = new DBManager();
                dbmanager.DbConect(defaultConnection);

                var PickingHeadList = new List<Data.PickingHead>();
                PickingHeadList = dbmanager.sqlConnection.Query<Data.PickingHead>(
                    procedure, values, commandType: CommandType.StoredProcedure
                    ).ToList();
                log.LogInformation(PickingHeadList.Count().ToString());
                return new OkObjectResult(PickingHeadList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(new { Message = ex.Message });
            }

        }


    }

}