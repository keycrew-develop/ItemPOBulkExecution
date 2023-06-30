using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    using System.Data.SqlClient;
    using System.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs;

    namespace Api　　//消費期限更新
    {
        public class DateExpirationUpdate : ControllerBase
        {
            private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");
            private readonly ILogger<DateExpirationUpdate> _logger;
            public DateExpirationUpdate(ILogger<DateExpirationUpdate> logger)
            {
                _logger = logger;
            }

            [FunctionName("PutDateExpirationUpdate")]
            public IActionResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "DateExpirationUpdates")] HttpRequest req, ILogger log)
            {
                using (SqlConnection connection = new SqlConnection(defaultConnection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "dbo.SFM_S19_ItemPOListArrival";
                            command.CommandType = CommandType.StoredProcedure;

                            string cmd = "DateExpirationUpdate";
                            string companyCode = req.Form["CompanyCode"];
                            string itemPONo = req.Form["ItemPONo"];
                            string dateExpiration = req.Form["DateExpiration"];
                            // パラメーターを設定する
                            command.Parameters.AddWithValue("@Cmd", cmd);
                            command.Parameters.AddWithValue("@CompanyCode", companyCode);
                            command.Parameters.AddWithValue("@ItemPONo", itemPONo);
                            command.Parameters.AddWithValue("@DateExpiration", dateExpiration);

                            // ストアドプロシージャを実行する
                            command.ExecuteNonQuery();

                            _logger.LogInformation("ストアドプロシージャの実行に成功しました。");
                            return Ok();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("エラーが発生しました: {ErrorMessage}", ex.Message);
                        return StatusCode(500);
                    }
                }
            }
        }

    }

}
