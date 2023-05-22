using System;
using System.Collections.Generic;
using System.Text;
namespace Data;

public class TransProgress
{
    public string TransporterName { get; set; }
    public int TotalCount { get; set; }
    public int UninspectedCount { get; set; }
    public int InspectedCount { get; set; }
}


public class ShippingGroupProgress
{
    public string ShippingGroupName { get; set; }
    public int TotalCount { get; set; }
    public int UninspectedCount { get; set; }
    public int InspectedCount { get; set; }

}
