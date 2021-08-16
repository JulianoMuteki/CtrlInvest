using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Domain.Entities
{
    class PaymentMethod
    {
        public string AtSight { get; private set; }
        public string OnCredit { get; private set; }
        public string OnTerm { get; private set; }
    }
}
