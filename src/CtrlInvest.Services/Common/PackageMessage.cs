using CtrlInvest.CrossCutting;
using System;

namespace CtrlInvest.Services.Common
{
    public class PackageMessage
    {
        public PackageMessage()
        {
            this.MessageID = new Guid();
        }
        public Guid MessageID { get; private set; }

        public Guid TicketID { get; set; }
        public string TicketCode { get; set; }
        public string Message { get; set; }

        public bool isValidMessage()
        {
            if (string.IsNullOrEmpty(this.TicketCode) || this.TicketID == Guid.Empty)
                return false;
            else if (this.Message == "Date,Open,High,Low,Close,Adj Close,Volume")
                return false;
            else if (this.Message == "Date,Dividends")
                return false;            
            else if (this.Message.Split(',').Length >= 6 && this.Message.Split(',')[6] == "null")
                return false;
            else
                return true;
        }

        public string ToJson()
        {
            return JsonSerialize.JsonSerializer<PackageMessage>(this);
        }
    }
}
