using CtrlInvest.Domain.Interfaces.Application;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            // Get tha latest historical

            // Check if is needed to get more historical
            // Check if exists historical
            // else
            // Check if latest historical is not >= today
            // strategy.DownloadDataFromYahoo("TAEE11.SA", latest historical + 1, today);


            dataImport.DownloadDataFromYahoo("TAEE11.SA", 1194912000, 1634256000);

            //save in database
        }
    }
}