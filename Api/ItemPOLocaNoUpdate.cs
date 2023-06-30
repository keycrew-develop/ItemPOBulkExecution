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
using System.Web.Http;

namespace Api  //入荷ロケーション更新
{
    public class ItemPOLocaNoUpdate : ControllerBase
    {
        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");
        private readonly ILogger<ItemPOLocaNoUpdate> _logger;
        private string errorMessage1;

        public ItemPOLocaNoUpdate(ILogger<ItemPOLocaNoUpdate> logger)
        {
            _logger = logger;
        }
        public string ErrorMessage1 => errorMessage1;

        [FunctionName("PutLocaNoUpdate")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "LocaNoUpdates")] HttpRequest req, ILogger log)
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


                        // パラメーターを直接取得する
                        string cmd = "LocaNoUpdate";
                        string siteCode = req.Form["SiteCode"];
                        string locaNo = req.Form["LocaNo"];
                        string companyCode = req.Form["CompanyCode"];
                        string itemPONo = req.Form["ItemPONo"];

                        // パラメーターを設定する
                        command.Parameters.AddWithValue("@Cmd", cmd);
                        command.Parameters.AddWithValue("@SiteCode", siteCode);
                        command.Parameters.AddWithValue("@LocaNo", locaNo);
                        command.Parameters.AddWithValue("@CompanyCode", companyCode);
                        command.Parameters.AddWithValue("@ItemPONo", itemPONo);

                        // 出力パラメーターを設定する
                        SqlParameter msgParameter = command.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                        msgParameter.Direction = ParameterDirection.Output;

                        // ストアドプロシージャを実行する
                        command.ExecuteNonQuery();

                        // 戻り値や出力パラメーターを取得する
                        int returnValue = (int)command.Parameters["return_value"].Value;


                        if (returnValue == 0)
                        {
                            _logger.LogInformation("ストアドプロシージャの実行に成功しました。");
                            return Ok();
                        }
                        else
                        {
                            errorMessage1 = $"入力された倉庫コードにロケーションNoが登録されていません。";
                            _logger.LogError(errorMessage1);
                            return BadRequest(errorMessage1);
                        }
                    }

                    // 応答を返す
                }
                catch (Exception ex)
                {
                    Console.WriteLine("エラーが発生しました: " + ex.Message);
                    return StatusCode(500);
                }
            }
        }
    }




}


