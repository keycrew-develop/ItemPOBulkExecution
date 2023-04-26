using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class RequestModel
    {
        public Dictionary<string, string> inputData { get; set; }
        public List<Dictionary<string, string>> inputDataDetail { get; set; }
        public DateTime updateDateTime { get; set; }
    }
}
