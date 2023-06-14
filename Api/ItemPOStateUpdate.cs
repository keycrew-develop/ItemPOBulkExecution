using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Api  //納期確定ステータス更新
{
    public class ItemPOStateUPDate : ControllerBase
    {
        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DeafaultDBConnection");

        [FunctionName("ItemPOStateUpdate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ItemPOBulkExecutionProgresses/{ShipID}")] HttpRequest req,
            ILogger log, string ShipID)
        {
            // データベース接続の準備
            using (SqlConnection connection = new SqlConnection(defaultConnection))
            {
                await connection.OpenAsync();

                // ストアドプロシージャの実行
                using (SqlCommand command = new SqlCommand("POStateUpdate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // パラメータの設定
                    command.Parameters.AddWithValue("@CompanyCode", "YourCompanyCode");
                    command.Parameters.AddWithValue("@ItemPONo", ShipID);

                    // メッセージパラメータの設定
                    SqlParameter messageParameter = new SqlParameter("@Msg", SqlDbType.NVarChar, 1000);
                    messageParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(messageParameter);

                    // 実行
                    await command.ExecuteNonQueryAsync();

                    // 処理結果の取得
                    int result = (int)command.Parameters["@RETURN_VALUE"].Value;
                    string message = command.Parameters["@Msg"].Value.ToString();

                    if (result == 1)
                    {
                        // エラーメッセージの処理
                        // ...
                    }
                    else
                    {
                        // 成功時の処理
                        // ...
                    }
                }
            }

            // 応答の作成
            // ...
        }
    }

}
