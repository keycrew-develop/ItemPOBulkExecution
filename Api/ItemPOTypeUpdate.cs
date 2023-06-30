using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;

namespace Api  //商品区分更新
{
    public class ItemTypeUpdate : ControllerBase
    {
        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");
        private readonly ILogger<ItemTypeUpdate> _logger;
        public ItemTypeUpdate(ILogger<ItemTypeUpdate> logger)
        {
            _logger = logger;
        }

        [FunctionName("PutItemTypeUpdate")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ItemTypeUpdates")] HttpRequest req, ILogger log)
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

                        string cmd = "ItemTypeUpdate";
                        string companyCode = req.Form["CompanyCode"];
                        string itemPONo = req.Form["ItemPONo"];
                        string ItemTypeName = req.Form["ItemTypeCode"];

                        // パラメーターを設定する
                        command.Parameters.AddWithValue("@Cmd", cmd);
                        command.Parameters.AddWithValue("@CompanyCode", companyCode);
                        command.Parameters.AddWithValue("@ItemPONo", itemPONo);
                        command.Parameters.AddWithValue("@ItemTypeCode", ItemTypeName);
                        

                        // 出力パラメーターを設定する
                        SqlParameter msgParameter = command.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                        msgParameter.Direction = ParameterDirection.Output;

                        // ストアドプロシージャを実行する
                        command.ExecuteNonQuery();

                        // 結果を取得する
                        string returnValue = Convert.ToString(command.Parameters["@Msg"].Value);

                        if (string.IsNullOrEmpty(returnValue))
                        {
                            _logger.LogInformation("ストアドプロシージャの実行に成功しました。");
                            return Ok();
                        }
                        else
                        {
                            _logger.LogError("エラーが発生しました: {ErrorMessage}", returnValue);
                            return BadRequest(returnValue);
                        }
                    }
                }

                catch (Exception ex)
                {
                    _logger.LogError("エラーが発生しました: {ErrorMessage}", ex.Message);
                    return StatusCode(50);
                }
            }
        }

    }
}