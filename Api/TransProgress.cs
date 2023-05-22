using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Data;
using Dapper;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Formats.Asn1;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using static System.Net.WebRequestMethods;
using System.Net.Mime;

namespace Api
{
    public class TransProgress : ControllerBase
    {

        private readonly string defaultConnection = Environment.GetEnvironmentVariable("DefaultDBConnection");

        [FunctionName("GetTransProgress")]
        public ObjectResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "TransProgresses")] HttpRequest req,
                ILogger log)
        {
            try
            {
                //ファイルの中身チェック
                string ShippingPointCode = req.Query["ShippingPointCode"];
                log.LogInformation(ShippingPointCode);
                DBManager dbmanager = new DBManager();
                dbmanager.DbConect(defaultConnection);
                var ProgressList = new List<Data.TransProgress>();

                //ヤマト佐川のリスト検索
                var YamatoSagawaList = GetYamatoSagawaProgresses(dbmanager, ShippingPointCode, log);
                if (YamatoSagawaList.Count == 0)
                {
                    ProgressList.Add(new Data.TransProgress
                        {
                            TransporterName = "ヤマト運輸",
                            TotalCount = 0,
                            UninspectedCount = 0,
                            InspectedCount = 0
                        }
                    );
                    ProgressList.Add(new Data.TransProgress
                        {
                            TransporterName = "佐川急便",
                            TotalCount = 0,
                            UninspectedCount = 0,
                            InspectedCount = 0
                        }
                    );
                }
                else if (YamatoSagawaList.Count == 1)
                {
                    foreach (var row in YamatoSagawaList)
                    {
                        if (row.TransporterName == "ヤマト運輸")
                        {
                            ProgressList.Add(row);
                            ProgressList.Add(new Data.TransProgress
                            {
                                TransporterName = "佐川急便",
                                TotalCount = 0,
                                UninspectedCount = 0,
                                InspectedCount = 0
                            });
                        }
                        else
                        {
                            ProgressList.Add(new Data.TransProgress
                            {
                                TransporterName = "ヤマト運輸",
                                TotalCount = 0,
                                UninspectedCount = 0,
                                InspectedCount = 0
                            });
                            ProgressList.Add(row);
                        }
                    }
                }
                else
                {
                    foreach (var row in YamatoSagawaList)
                    {
                        ProgressList.Add(row);
                    }
                }

                //その他配送方法のリスト検索
                var OtherTransporterList = GetOtherTransporterProgresses(dbmanager, ShippingPointCode, log);
                if (OtherTransporterList.Count == 0)
                {
                    ProgressList.Add(new Data.TransProgress
                        {
                            TransporterName = "その他",
                            TotalCount = 0,
                            UninspectedCount = 0,
                            InspectedCount = 0
                        }
                    );
                }
                else
                {
                    ProgressList.Add(OtherTransporterList.First());
                }

                //総計のリスト検索
                var TotalList = GetTotalProgresses(dbmanager, ShippingPointCode, log);
                if (TotalList.Count == 0)
                {
                    ProgressList.Add(new Data.TransProgress
                        {
                            TransporterName = "計",
                            TotalCount = 0,
                            UninspectedCount = 0,
                            InspectedCount = 0
                        }
                    );
                }
                else
                {
                    ProgressList.Add(TotalList.First());
                }

                return new OkObjectResult(ProgressList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(new { Message = ex.Message });
            }

        }

        public List<Data.TransProgress> GetYamatoSagawaProgresses(DBManager dbmanager,string ShippingPointCode, ILogger log)
        {
            var YamatoSagawaTransporterQuery = @"SELECT            CASE WHEN LEFT(TransporterCode, 1) = 1 THEN 'ヤマト運輸' WHEN LEFT(TransporterCode, 1) = 2 THEN '佐川急便'
					                                                --WHEN LEFT(TransporterCode, 1) = 3 THEN '日本郵便' 
					                                                WHEN LEFT(TransporterCode, 1) >=3 THEN 'その他' END AS TransporterName, COUNT(*) AS TotalCount, SUM(CASE WHEN pickingstatecode = '400' THEN 1 ELSE 0 END) AS UninspectedCount, SUM(CASE WHEN pickingstatecode = '500' THEN 1 ELSE 0 END) AS InspectedCount
                                                FROM              dbo.VW_PickingHed
                                                WHERE             (DateShip >= @Today) AND (DateScheShip >=@Today) AND (ShippingPointCode = @ShippingPointCode) AND (LEFT(TransporterCode, 1) IN ('1', '2')) OR
                                                                       (DateShip IS NULL) AND (DateScheShip >= @Today) AND (ShippingPointCode = @ShippingPointCode) AND (LEFT(TransporterCode, 1) IN ('1', '2'))
                                                GROUP BY       LEFT(TransporterCode, 1)";

            log.LogInformation("YamatoSagawaSQL BeforeExecute");

            var YamatoSagawaList = dbmanager.sqlConnection.Query<Data.TransProgress>(YamatoSagawaTransporterQuery, new { ShippingPointCode = ShippingPointCode, Today = DateTime.UtcNow.ToString("yyyy-MM-dd") }).AsList();
            log.LogInformation("YamatoSagawaSQL Executed");

            
            return YamatoSagawaList;
        }

        public List<Data.TransProgress> GetOtherTransporterProgresses(DBManager dbmanager, string ShippingPointCode, ILogger log)
        {
            var OtherTransporterQuery = @"SELECT            'その他' AS TransporterName, COUNT(*) AS TotalCount, ISNULL(SUM(CASE WHEN pickingstatecode = '400' THEN 1 ELSE 0 END), 0) AS UninspectedCount, 
                                                                       ISNULL(SUM(CASE WHEN pickingstatecode = '500' THEN 1 ELSE 0 END), 0) AS InspectedCount
                                                FROM              dbo.VW_PickingHed
                                                WHERE             (DateShip >= @Today) AND (DateScheShip >= @Today) AND (ShippingPointCode = @ShippingPointCode) AND (NOT (LEFT(TransporterCode, 1) IN ('1', '2'))) OR
                                                                       (DateShip IS NULL) AND (DateScheShip >= @Today) AND (ShippingPointCode = @ShippingPointCode) AND (NOT (LEFT(TransporterCode, 1) IN ('1', '2')))";

            log.LogInformation("OtherTransporterSQL BeforeExecute");

            var OtherTransporterList = dbmanager.sqlConnection.Query<Data.TransProgress>(OtherTransporterQuery, new { ShippingPointCode = ShippingPointCode, Today = DateTime.UtcNow.ToString("yyyy-MM-dd") }).AsList();
            log.LogInformation("OtherTransporterSQL Executed");


            return OtherTransporterList;
        }

        public List<Data.TransProgress> GetTotalProgresses(DBManager dbmanager, string ShippingPointCode, ILogger log)
        {
            var TotalQuery = @"SELECT            '計' AS TransporterName, COUNT(*) AS TotalCount, SUM(CASE WHEN pickingstatecode = '400' THEN 1 ELSE 0 END) AS UninspectedCount, SUM(CASE WHEN pickingstatecode = '500' THEN 1 ELSE 0 END) 
                                                    AS InspectedCount
                            FROM              dbo.VW_PickingHed
                            WHERE             (DateShip >= @Today) AND (DateScheShip >= @Today) AND (ShippingPointCode = @ShippingPointCode) OR
                                                    (DateShip IS NULL) AND (DateScheShip >= @Today) AND (ShippingPointCode = @ShippingPointCode)
";

            log.LogInformation("TotalSQL BeforeExecute");

            var TotalList = dbmanager.sqlConnection.Query<Data.TransProgress>(TotalQuery, new { ShippingPointCode = ShippingPointCode, Today = DateTime.UtcNow.ToString("yyyy-MM-dd") }).AsList();
            log.LogInformation("TotalSQL Executed");


            return TotalList;
        }


    }

}