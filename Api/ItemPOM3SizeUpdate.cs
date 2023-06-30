using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Api  //3辺サイズ更新
{
    public class ItemPOM3SizeUpdate : ControllerBase
    {
        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");
        private readonly ILogger<ItemPOM3SizeUpdate> _logger;
        public ItemPOM3SizeUpdate(ILogger<ItemPOM3SizeUpdate> logger)
        {
            _logger = logger;
        }
        public string ErrorMessage3 => errorMessage3;
        public string errorMessage3;

        [FunctionName("PutM3SizeUpdate")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "M3SizeUpdates")] HttpRequest req, ILogger log)
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
                        string cmd = "M3SizeUpdate";
                        string sizeW = req.Form["SizeW"];
                        string sizeH = req.Form["SizeH"];
                        string sizeD = req.Form["SizeD"];
                        string companyCode = req.Form["CompanyCode"];
                        string itemPONo = req.Form["ItemPONo"];

                        // パラメーターを設定する
                        if (sizeW != "" && sizeH != "" && sizeD != "")
                        {
                            command.Parameters.AddWithValue("@Cmd", cmd);
                            command.Parameters.AddWithValue("@SizeW", Decimal.Parse(sizeW));
                            command.Parameters.AddWithValue("@SizeH", Decimal.Parse(sizeH));
                            command.Parameters.AddWithValue("@SizeD", Decimal.Parse(sizeD));
                            command.Parameters.AddWithValue("@CompanyCode", companyCode);
                            command.Parameters.AddWithValue("@ItemPONo", itemPONo);
                        
                            
                                SqlParameter msgParameter = command.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                                msgParameter.Direction = ParameterDirection.Output;

                                // ストアドプロシージャを実行する
                                command.ExecuteNonQuery();

                                int returnValue = (int)command.Parameters["return_value"].Value;

                                // 戻り値や出力パラメーターを取得する

                                if (returnValue == 0)
                                {
                                    _logger.LogInformation("ストアドプロシージャの実行に成功しました。");
                                    return Ok();
                                }
                                else
                                {
                                    errorMessage3 = $"記入していない箇所があります: {returnValue}";
                                    _logger.LogError(errorMessage3);
                                    return BadRequest(errorMessage3);
                                }
                        }
                        else
                        {
                            errorMessage3 = $"3辺サイズは全て記入してください";
                            return BadRequest(errorMessage3);
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
