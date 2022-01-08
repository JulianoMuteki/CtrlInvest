using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services.Dtos
{
    public class BrokerageNoteDto
    {
        public string DateIssue { get; set; }
        //public string ExpireDate { get; set; }
        private string expireDate;

        public string ExpireDate
        {
            get { return expireDate.Replace("-","").Trim(); }
            set { expireDate = value; }
        }

        private string price;

        public string Price
        {
            get { return price.Replace("R$", "").Trim(); }
            set { price = value; }
        }

        private string totalAmount;

        public string TotalAmount
        {
            get { return totalAmount.Replace("R$", "").Trim(); }
            set { totalAmount = value; }
        }

        public string Quantity { get; set; }
        public string TicketCode { get; set; }

        public string TypeMarket { get; set; }
        public string TypeDeal { get; set; }

        public string FinancialInstitution { get; set; }
    }
}
