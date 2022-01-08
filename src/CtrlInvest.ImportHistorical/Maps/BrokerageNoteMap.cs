using CsvHelper.Configuration;
using CtrlInvest.Services.Dtos;
using System;

namespace CtrlInvest.ImportHistorical.Maps
{
    public class BrokerageNoteMap : ClassMap<BrokerageNoteDto>
    {
        public BrokerageNoteMap()
        {
            Map(m => m.DateIssue).Name("data_negocio");
            Map(m => m.TypeDeal).Name("tipo_movimentacao");
            Map(m => m.TypeMarket).Name("mercado");
            Map(m => m.ExpireDate).Name("vencimento");
            Map(m => m.FinancialInstitution).Name("instituicao");
            Map(m => m.TicketCode).Name("ticket_code");
            Map(m => m.Quantity).Name("quantidade");
            Map(m => m.Price).Name("preco");
            Map(m => m.TotalAmount).Name("valor");
        }
    }
}