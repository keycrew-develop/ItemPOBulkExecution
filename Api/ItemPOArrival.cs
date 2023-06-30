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

namespace Api  //入荷（入荷作業中）処理
{
    public class ItemPOArrival : ControllerBase
    {
        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");

        private readonly ILogger<ItemPOArrival> _logger;
        public ItemPOArrival(ILogger<ItemPOArrival> logger)
        {
            _logger = logger;
        }
        public string ErrorMessage2 => errorMessage2;
        public string errorMessage2;

        [FunctionName("PutItemPOArrival")]
        public IActionResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ItemPOArrivals")] HttpRequest req, ILogger log)
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

                        string cmd = "ItemPOArraival";
                        string companyCode = req.Form["CompanyCode"];
                        string itemPONo = req.Form["ItemPONo"];
                        string qtyArrival = req.Form["QtyArrival"];
                        string dateDelivFinish = req.Form["DateDelivFinish"];
                        string msg = req.Query["Msg"];

                        // ストアドプロシージャのパラメータを設定
                        command.Parameters.AddWithValue("@Cmd", cmd);
                        command.Parameters.AddWithValue("@CompanyCode", companyCode);
                        command.Parameters.AddWithValue("@ItemPONo", itemPONo);
                        command.Parameters.AddWithValue("@QtyArrival", qtyArrival);
                        command.Parameters.AddWithValue("@DateDelivFinish", dateDelivFinish);
                        command.Parameters.Add("@Msg", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        // トランザクションを開始
                        SqlTransaction transaction = connection.BeginTransaction();
                        command.Transaction = transaction;

                        int returnValue = (int)command.Parameters["return_value"].Value;


                        if (returnValue == 0)
                        {
                            _logger.LogInformation("ストアドプロシージャの実行に成功しました。");
                            return Ok();
                        }
                        else
                        {
                            errorMessage2 = $"3辺サイズが入っていません。";
                            _logger.LogError(errorMessage2);
                            return BadRequest(errorMessage2);
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    log.LogError(ex, "エラーが発生しました");
                    return StatusCode(500, "エラーが発生しました");
                }
            }

        }
    }
}
        
    


                    
        
    
