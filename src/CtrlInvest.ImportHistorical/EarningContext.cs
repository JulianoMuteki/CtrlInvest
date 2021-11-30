using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Entities.StocksExchanges;
using CtrlInvest.Domain.Interfaces.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CtrlInvest.ImportHistorical
{
    public class EarningContext
    {
        EarningFundamentus dataImport;
        ITicketAppService ticketAppService;
        // Constructor
        public EarningContext(EarningFundamentus dataImport, ServiceProvider serviceProvider)
        {
            this.dataImport = dataImport;
            this.ticketAppService = serviceProvider.GetService<ITicketAppService>();
        }
        public void ImportEarnings()
        {
            // Get Tickets with sync enable
            var ticketSyncs = this.ticketAppService.GetAllTicketsSyncs();
            foreach (var ticketSync in ticketSyncs)
            {
                Ticket ticket = this.ticketAppService.GetById(ticketSync.TickerID);
               
                // Get tha latest earning
                var latestEarning = this.ticketAppService.GetLastEarningByTicker(ticket.Id);
                // Check if is needed to get more historical
                if (latestEarning == null)
                {
                    dataImport.DownloadHistoricalToText(ticket.Ticker);
                    Save(ticket, ticketSync.DateStart, DateTime.Now.AddDays(-1).Date);
                }
                else if (latestEarning.DateWith < DateTime.Now.Date.AddDays(-1))
                {
                    dataImport.DownloadHistoricalToText(ticket.Ticker);
                    Save(ticket, latestEarning.DateWith.AddDays(1), DateTime.Now.Date);
                }
            }
        }

        private void Save(Ticket ticket, DateTime dtStart, DateTime dtEnd)
        {
            //save in database
            IList<Earning> earningList = dataImport.ConvertHistoricalToList(ticket, dtStart, dtEnd);
            if(earningList.Count > 0)
                this.ticketAppService.SaveEarningsList(earningList);
        }
    }
}
