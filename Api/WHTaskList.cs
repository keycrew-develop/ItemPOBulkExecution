using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Data;
using Dapper;

namespace Api  //付帯作業リスト
{
    public class WHTaskList : ControllerBase
    {
        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");

        [FunctionName("GetWHTaskList")]
        public ObjectResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WHTaskLists")] HttpRequest req, ILogger log)
        {
            try
            {
                var sql = "dbo.SFM_S19_ItemPOListArrival";

                using (var conn = new SqlConnection(defaultConnection))
                {
                    conn.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Cmd", "WHTaskList");

                    var rows = conn.Query<dynamic>(sql, parameters, commandType: CommandType.StoredProcedure);

                    return new ObjectResult(rows);
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
