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
    public class PickingDetailUpdate : ControllerBase
    {

        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");

        [FunctionName("PickingDetailUpdate")]
        public async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "pickingDetail/update/{ShipID}")] HttpRequest req,
                ILogger log, string ShipID)
        {
            try
            {
                // リクエストボディを JSON としてデシリアライズ
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Data.PickingDetail>(requestBody);
                var CompanyCode = data.ShipID.Substring(0,6);
                var PickingNo = data.ShipID.Substring(6, 7);
                var procedure = "[SFM_N17_DelivScan]";
                //var values = new
                //{
                //    Cmd = "PickingDetailUpdate",
                //    CompanyCode = CompanyCode,
                //    PickingNo = PickingNo
                //};
                var values = new DynamicParameters();
                values.Add("@Return Value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                values.Add("@Cmd", "PickingDetailUpdate", dbType: DbType.String, direction: ParameterDirection.Input);
                values.Add("@CompanyCode", CompanyCode, dbType: DbType.String, direction: ParameterDirection.Input);
                values.Add("@PickingNo", PickingNo, dbType: DbType.String, direction: ParameterDirection.Input);
                DBManager dbmanager = new DBManager();
                dbmanager.DbConect(defaultConnection);

                dbmanager.sqlConnection.Execute(
                    procedure, values, commandType: CommandType.StoredProcedure
                    );

                if (values.Get<int>("@Return Value") == 0)
                {
                    // 更新されたリソースをレスポンスとして返す
                    var responseMessage = new { Message = $"PickingData with ID {CompanyCode+PickingNo} has been updated." };
                    return new OkObjectResult(responseMessage);
                }
                else
                {
                    var responseMessage = new { Message = $"PickingData with ID {CompanyCode + PickingNo} has error" };
                    return new BadRequestObjectResult(responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(new { Message = ex.Message });
            }

        }


    }

}