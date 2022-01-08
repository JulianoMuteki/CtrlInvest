using CsvHelper;
using CsvHelper.Configuration;
using CtrlInvest.Domain.Entities.InvestimentsPortifolios;
using CtrlInvest.ImportHistorical.Maps;
using CtrlInvest.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.ImportHistorical
{
    public class BrokerageNoteB3
    {
        public List<BrokerageNoteDto> ConvertExcelToObjectList()
        {
            string[] delimiter = { ";" };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = "|",
                HeaderValidated = null
            };

            List<BrokerageNoteDto> brokerageNotes;

            string basePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string fullPath = System.IO.Path.GetFullPath("FilesToRead/negociacao-2020.csv", basePath);

            using (var reader = new StreamReader(fullPath, Encoding.GetEncoding("iso-8859-1")))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<BrokerageNoteMap>();

                brokerageNotes = csv.GetRecords<BrokerageNoteDto>().ToList();
            }
            return brokerageNotes;
        }

        internal IList<BrokerageNote> ConvertCVSDataToBrokerageNoteList(IList<BrokerageNoteDto> brokerageNoteDtos)
        {
            IList<BrokerageNote> brokerageNotes = new List<BrokerageNote>();

            foreach (var item in brokerageNoteDtos)
            {
                BrokerageNote brokerageNote = BrokerageNoteFacade.CreateBrokerageNote(item.DateIssue, item.ExpireDate, item.Price, item.TotalAmount,
                   item.Quantity, item.TicketCode, item.TypeMarket, item.TypeDeal, item.FinancialInstitution);

                brokerageNotes.Add(brokerageNote);
            }

            return brokerageNotes;
        }
    }
}
