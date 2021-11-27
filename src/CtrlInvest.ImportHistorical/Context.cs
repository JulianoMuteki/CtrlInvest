using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CtrlInvest.ImportHistorical
{
    /// <summary>
    /// The 'Context' class
    /// </summary>
    public class Context
    {
        DataImport dataImport;
        ITicketAppService ticketAppService;
        // Constructor
        public Context(DataImport dataImport, ServiceProvider serviceProvider)
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
                // Get tha latest historical
                var latestHistorical = this.ticketAppService.GetLatestHistoricalByTicker(ticket.Ticker);
                // Check if is needed to get more historical
                if (latestHistorical == null)
                {
                    dataImport.DownloadDataFromYahoo(ticket.Ticker, ticketSync.DateStart, DateTime.Now.AddDays(-2).Date);
                    Save(ticketSync);
                }
                else if (latestHistorical.Date < DateTime.Now.Date.AddDays(-1))
                {
                    dataImport.DownloadDataFromYahoo(ticket.Ticker, latestHistorical.Date.AddDays(1), DateTime.Now.Date);
                    Save(ticketSync);
                }                
            }
        }

        private void Save(TicketSync ticketSync)
        {
            //save in database
            IList<HistoricalPrice> historicalsList = dataImport.SaveHistoricalInDatabase(ticketSync.TickerID);
            this.ticketAppService.SaveHistoricalDateList(historicalsList);
        }
    }
}