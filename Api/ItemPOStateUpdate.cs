﻿using Microsoft.AspNetCore.Http;
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

namespace Api  //納期確定
{
    public class POStateUpdateUpdate : ControllerBase
    {
        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");
        private readonly ILogger<POStateUpdateUpdate> _logger;
        public POStateUpdateUpdate(ILogger<POStateUpdateUpdate> logger)
        {
            _logger = logger;
        }
        public string ErrorMessageState => errorMessageState;
        public string errorMessageState;

        [FunctionName("GetPOStateUpdate")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "POStateUpdates")] HttpRequest req, ILogger log)
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

                        string cmd = "POStateUpdate";
                        string companyCode = req.Form["CompanyCode"];
                        string itemPONo = req.Form["ItemPONo"];
                        string poStateCode = req.Form["POStateCode"];

                        command.Parameters.AddWithValue("@Cmd", cmd);
                        command.Parameters.AddWithValue("@CompanyCode", companyCode);
                        command.Parameters.AddWithValue("@ItemPONo", itemPONo);
                        command.Parameters.AddWithValue("@POStateCode", poStateCode);
                        command.Parameters.AddWithValue("@Msg", "");
                        command.Parameters["@Msg"].Direction = ParameterDirection.Output;


                        command.ExecuteNonQuery();

                        int returnValue = (int)command.Parameters["return_value"].Value;


                        if (returnValue == 0)
                        {
                            _logger.LogInformation("ストアドプロシージャの実行に成功しました。");
                            return Ok();
                        }
                        else
                        {
                            errorMessageState = $"エラーが発生しました。: {returnValue}";
                            _logger.LogError(errorMessageState);
                            return BadRequest(errorMessageState);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred.");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                }
            }
        }
    }
}

