using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services.Dtos
{
    public class EarningDto
    {
        public DateTime DateWith { get; set; }
        public double ValueIncome { get; set; }
        public string Type { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int Quantity { get; set; }
    }
}
