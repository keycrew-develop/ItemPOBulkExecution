using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ItemPOBulkExecutionProgress
    {
        public string CompanyName { get; set; }  //会社名
        public int CompanyCode { get; set; }   //会社コード
        public int QtyPO { get; set; }   //入荷物の合計数 
        public int Quantity { get; set; }  //入荷予定日
        public string WHTask { get; set; }   //付帯
        public string POStateName { get; set; }   //進捗

        public string CommodityName { get; set; }
        public int CommodityCode { get; set; }
        public int GTINCode { get; set; }
        public int TotalOrder { get; set; }
        public int TotalArrival { get; set; }
        public string LocationNo { get; set; }
        public int Vertical { get; set; }
        public int Horizontal { get; set; }
        public int Height { get; set; }
        public string Incidental { get; set; }
        public string ExpirationDate { get; set; }
        public string Catagory { get; set; }
        public string ArrivalDay { get; set; }
        public string Progress { get; set; }

    }
}
