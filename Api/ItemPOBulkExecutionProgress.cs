
using Data;
using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Api  //入荷情報詳細
{
    public class ItemPOBulkExecutionProgress : ControllerBase
    {
        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");

        [FunctionName("GetItemPOBulkExecutionProgress")]
        public ObjectResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ItemPOBulkExecutionProgresses")] HttpRequest req, ILogger log)
        {
            try
            {
                var sql = "dbo.SFM_S19_ItemPOListArrival";
                var ItemPOList = req.Query["ItemPOList"];

                using (var conn = new SqlConnection(defaultConnection))
                {
                    conn.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Cmd", "ItemPOList");
                    parameters.Add("@CompanyCode", "");
                    parameters.Add("@ItemNo", "");


                    conn.Execute(sql, parameters, commandType: CommandType.StoredProcedure);

                    var outputMessage = parameters.Get<string>("@Msg");
                    var itemPOGroupList = parameters.Get<List<Data.ItemPOBulkExecutionProgress>>("@UT_WHTask");

                    return new ObjectResult(itemPOGroupList);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error occurred");
                return new BadRequestObjectResult(new { Message = ex.Message });
            }
        }
    }


}