using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Entities.StocksExchanges;
using CtrlInvest.Domain.Interfaces.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void ImportHistoricalByDates()
        {
            // Get Tickets with sync enable
            var ticketSyncs = this.ticketAppService.GetAllTicketsSyncs();
            foreach (var ticketSync in ticketSyncs)
            {
                Ticket ticket = this.ticketAppService.GetById(ticketSync.TickerID);
                //dataImport.DownloadHistoricalToText(ticket.Ticker, ticketSync.DateStart, DateTime.Now.AddDays(-1).Date);
                Save(ticket);
                // Get tha latest historical
                //var latestHistorical = this.ticketAppService.GetLatestHistoricalByTicker(ticket.Ticker);
                //// Check if is needed to get more historical
                //if (latestHistorical == null)
                //{
                //    dataImport.DownloadHistoricalToText(ticket.Ticker, ticketSync.DateStart, DateTime.Now.AddDays(-1).Date);
                //    Save(ticket);
                //}
                //else if (latestHistorical.Date < DateTime.Now.Date.AddDays(-1))
                //{
                //    dataImport.DownloadHistoricalToText(ticket.Ticker, latestHistorical.Date.AddDays(1), DateTime.Now.Date);
                //    Save(ticket);
                //}
            }
        }

        private void Save(Ticket ticket)
        {
            //save in database
            IList<Earning> earningList = dataImport.ConvertHistoricalToList(ticket);
           // this.ticketAppService.SaveHistoricalPricesList(earningList);
        }
    }
}
