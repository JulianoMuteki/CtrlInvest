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
    public class HistoricalPriceContext
    {
        HistoricalPriceYahoo dataImport;
        ITicketAppService ticketAppService;
        // Constructor
        public HistoricalPriceContext(HistoricalPriceYahoo dataImport, ServiceProvider serviceProvider)
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
                    dataImport.DownloadHistoricalToText(ticket.Ticker, ticketSync.DateStart, DateTime.Now.AddDays(-1).Date);
                    Save(ticket);
                }
                else if (latestHistorical.Date < DateTime.Now.Date.AddDays(-1))
                {
                    dataImport.DownloadHistoricalToText(ticket.Ticker, latestHistorical.Date.AddDays(1), DateTime.Now.Date);
                    Save(ticket);
                }                
            }
        }

        private void Save(Ticket ticket)
        {
            //save in database
            IList<HistoricalPrice> historicalPricesList = dataImport.ConvertHistoricalToList(ticket);
            this.ticketAppService.SaveHistoricalPricesList(historicalPricesList);
        }
    }
}