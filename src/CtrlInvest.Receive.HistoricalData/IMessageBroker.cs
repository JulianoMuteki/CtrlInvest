﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Receive.HistoricalData
{
    public interface IMessageBroker
    {
        void DoReceiveOperation();
    }
}