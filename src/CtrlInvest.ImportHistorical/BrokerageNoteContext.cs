using CtrlInvest.Domain.Entities.InvestimentsPortifolios;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Services.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.ImportHistorical
{
    public class BrokerageNoteContext
    {
        BrokerageNoteB3 dataImport;
        IInvestPortfolioService iInvestPortfolioService;
        // Constructor
        public BrokerageNoteContext(BrokerageNoteB3 dataImport, ServiceProvider serviceProvider)
        {
            this.dataImport = dataImport;
            this.iInvestPortfolioService = serviceProvider.GetService<IInvestPortfolioService>();
        }
        public void ImportHistoricalByDates()
        {
            IList<BrokerageNoteDto> brokerageNoteDtos = dataImport.ConvertExcelToObjectList().ToList();
            Save(brokerageNoteDtos);
        }

        private void Save(IList<BrokerageNoteDto> brokerageNoteDtos)
        {
            ////save in database
            IList<BrokerageNote> brokerageNoteList = dataImport.ConvertCVSDataToBrokerageNoteList(brokerageNoteDtos);
            if (brokerageNoteList.Count > 0)
                this.iInvestPortfolioService.AddRange(brokerageNoteList);
        }
    }
}
