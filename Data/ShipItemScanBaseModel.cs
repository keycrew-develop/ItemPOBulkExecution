using System;
using System.Collections.Generic;
using System.Text;
namespace Data;

public class PickingHead
{
    public string CompanyCode { get; set; }
    public string PickingNo { get; set; }
    public string ShippingName { get; set; }
}

public class PickingDetail
{
    public string ShippingGroupName { get; set; }
    public string ShipID { get; set; }
    public string CompanyName { get; set; }
    public string OrderNoList { get; set; }
    public string TransporterName { get; set; }
    public string ShippingName { get; set; }
    public string PickingDescription { get; set; }
    public string ReceiptNoList { get; set; }
    public string GTINCode { get; set; }
    public string ItemNo { get; set; }
    public string ItemImage { get; set; }
    //商品区分を判別するコード。納品書有無に見えるが関係ない
    public bool DelivSlipNonPrintFlag { get; set; }
    public string ItemName { get; set; }    
    //出荷数。印刷数ではない。
    public int QtyPrint { get; set; }
    public int QtyPick { get; set; }
    public List<ShipItem> shipItemList { get; set; }
    
}

public class ShipItem
{
    public string GTINCode { get; set; }
    public string ItemNo { get; set; }
    public string ItemImage { get; set; }
    //商品区分を判別するコード。納品書有無に見えるが関係ない
    
    public string ItemName { get; set; }
    //出荷数。印刷数ではない。
    public int QtyPrint { get; set; }
    public int QtyPick { get; set; }
    public bool DelivSlipNonPrintFlag { get; set; }
}

public class PickDtlUpdateParameter
{
    public string ShipID { get; set; }
}
