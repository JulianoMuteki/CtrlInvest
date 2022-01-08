using CtrlInvest.CrossCutting.Enums;
using CtrlInvest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Domain.Entities.InvestimentsPortifolios
{
    public class BrokerageNote : EntityBase
    {
        public DateTime DateIssue { get; private set; }
        public DateTime? ExpireDate { get; private set; }
        public double Price { get; private set; }
        public double TotalAmount { get; private set; }
        public int Quantity { get; private set; }
        public string TicketCode { get; private set; }

        public ETypeMarket TypeMarket { get; private set; }       
        public ETypeDeal TypeDeal { get; private set; }

        public Bank FinancialInstitution { get; private set; }
        public Guid FinancialInstitutionID { get; private set; }

        public BrokerageNote()
        {

        }

        public BrokerageNote(string dateIssue, string expireDate, string price, string totalAmount, string quantity, string ticketCode, string typeMarket, string typeDeal, string financialInstitutionID)
        {
            this.DateIssue = DateTime.ParseExact(dateIssue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if(!String.IsNullOrEmpty(expireDate))
                this.ExpireDate = DateTime.ParseExact(expireDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            this.Price = Double.Parse(price);
            this.TotalAmount = Double.Parse(totalAmount);
            this.Quantity = int.Parse(quantity);
            this.TicketCode = ticketCode;
            this.TypeMarket = GetTypeMarket(typeMarket);
            this.TypeDeal = GetTypeDeal(typeDeal);
            this.FinancialInstitutionID = GetBankID(financialInstitutionID);
        }

        private Guid GetBankID(string financialInstitutionID)
        {
            if (financialInstitutionID == "INTER DTVM LTDA")
                return new Guid("e069604c-70af-11ec-914b-0242ac130002");
            else
                return new Guid("fe616efa-70af-11ec-914b-0242ac130002");
        }

        private ETypeDeal GetTypeDeal(string typeDeal)
        {
            ETypeDeal eTypeDeal = ETypeDeal.Sell;
            switch (typeDeal)
            {
                case "Compra":
                    eTypeDeal = ETypeDeal.Buy;
                    break;
                case "Venda":
                    eTypeDeal = ETypeDeal.Sell;
                    break;
                default:
                    break;
            }

            return eTypeDeal;
        }

        private ETypeMarket GetTypeMarket(string typeMarket)
        {
            ETypeMarket eTypeMarket = ETypeMarket.CASH_MARKET;
            switch (typeMarket)
            {
                case "Mercado Fracionário":
                    eTypeMarket = ETypeMarket.ODD_LOT_MARKET;
                    break;
                case "Mercado à Vista":
                    eTypeMarket = ETypeMarket.CASH_MARKET;
                    break;
                default:
                    break;
            }

            return eTypeMarket;
        }
    }

    public class BrokerageNoteFacade
    {
        public static BrokerageNote CreateBrokerageNote(string dateIssue, string expireDate, string price, string totalAmount, 
            string quantity, string ticketCode, string typeMarket, string typeDeal, string financialInstitutionID)
        {
            return new BrokerageNote(dateIssue, expireDate, price, totalAmount, quantity, ticketCode, typeMarket, typeDeal,
                financialInstitutionID);
        }
    }
}
