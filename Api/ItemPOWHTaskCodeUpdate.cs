using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Dapper;

namespace Api  //付帯作業更新
{
    public class ItemPOWHTaskCodeUpdate : ControllerBase
        {
            private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");
            private readonly ILogger<ItemPOWHTaskCodeUpdate> _logger;
            public ItemPOWHTaskCodeUpdate(ILogger<ItemPOWHTaskCodeUpdate> logger)
            {
                _logger = logger;
            }
        [FunctionName("PutWHTaskCodeUpdate")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "WHTaskCodeUpdates")] HttpRequest req, ILogger log)
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
                        SqlParameter returnValueParameter = command.Parameters.Add("return_value", SqlDbType.Int);
                        returnValueParameter.Direction = ParameterDirection.ReturnValue;

                        string cmd = "WHTaskCodeUpdate";
                        string companyCode = req.Form["CompanyCode"];
                        string itemPONo = req.Form["ItemPONo"];
                        string[] whcodelist = req.Form["WHCodeList"].ToString().Split(",");
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("WHTaskCode");
                        foreach(var c in whcodelist)
                        {
                        DataRow row = dataTable.NewRow();
                            row["WHTaskCode"] = c;
                        dataTable.Rows.Add(row);
                        }


                        // パラメーターを設定する
                        command.Parameters.AddWithValue("@Cmd", cmd);
                        command.Parameters.AddWithValue("@CompanyCode", companyCode);
                        command.Parameters.AddWithValue("@ItemPONo", itemPONo);
                        command.Parameters.Add("@UT_WHTask", SqlDbType.Structured);
                        command.Parameters["@UT_WHTask"].TypeName = "UT_WHTaskTableType";
                        command.Parameters["@UT_WHTask"].Value = dataTable;
                        // command.Parameters.AddWithValue("@UT_WHTask", dataTable.AsTableValuedParameter("[dbo].[UT_ImportDataFormatSet]"));

                        // 出力パラメーターを設定する
                        SqlParameter msgParameter = command.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                        msgParameter.Direction = ParameterDirection.Output;

                        // ストアドプロシージャを実行する
                        command.ExecuteNonQuery();

                        // 結果を取得する
                        int returnValue = (int)command.Parameters["return_value"].Value;

                        if (returnValue == 0)
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
                    return StatusCode(500);
                }
            }
        }

    }
}
