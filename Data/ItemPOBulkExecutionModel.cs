using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data
{
    public class ItemPOBulkExecutionProgress
    {
        public string ShippingPointCode { get; set; }
        public string CompanyNameS { get; set; }  //会社名
        public string CompanyCode { get; set; }   //会社コード
        public int QtyPO { get; set; }   //入荷物の合計数 
        public DateTime DateDelivPromise { get; set; }  //入荷予定日
        public string? WHTaskName { get; set; }   //付帯
        public string POStateName { get; set; }   //進捗

        public string ItemName { get; set; }
        public string ItemPONo { get; set; }
        public string? GTINCode { get; set; }
        public int QtyArrival { get; set; }
        public string? LocaNo { get; set; }
        public decimal? SizeW { get; set; }
        public decimal? SizeH { get; set; }
        public decimal? SizeD { get; set; }
        public DateTime? DateExpiration { get; set; }  //消費期限
        public string ItemTypeName { get; set; }
        public string ArrivalTranslnvNo { get; set; }
        public string SiteCode { get; set; }
        public string WHTaskCode { get; set; }   //付帯の略称
        public int SelectFlag { get; set; }
        public string POStateCode { get; set; }
        public DateTime DateDelivFinish { get;set; }


        public DateTime UpdateDateTime { get; set; }
        public string? Email { get; set; }
        public int QtyPOOrigin { get; set; }
        public string ItemNo { get; set; }
        public DateTime DateFinish { get; set; }
    }


}
