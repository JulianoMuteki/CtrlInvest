using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities.StocksExchanges;
using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Services.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CtrlInvest.Services.StocksExchanges
{
    public class HistoricalEarningService : IHistoricalEarningService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor
        public HistoricalEarningService(IUnitOfWork unitOfWork)
        {
          //  _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public Earning Add(Earning entity)
        {
            try
            {
                Earning earningExist = _unitOfWork.Repository<Earning>().Find(x => x.TickerID == entity.TickerID && x.DateWith == entity.DateWith && x.Type == entity.Type && x.ValueIncome == entity.ValueIncome);

                if (earningExist == null)
                {
                   var returnEntity = _unitOfWork.Repository<Earning>().Add(entity);
                    _unitOfWork.CommitSync();

                    return returnEntity;
                }
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<Earning>("Unexpected error add", nameof(this.Add), ex);
            }

            return null;
        }

        public Task<Earning> AddAsync(Earning entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<Earning> entity)
        {
            try
            {
                var ticketsIds = entity.GroupBy( x => x.TickerID).Select(g => g.Key).ToList();                
                var earnings_existents = _unitOfWork.Repository<Earning>().FindBy(x => ticketsIds.Any(id => id == x.TickerID)).ToList();

                //Search earnings that have been not saved
                var earnings_to_save = entity.Where(e => !earnings_existents.Any(id => id.TickerID == e.TickerID && id.DateWith == e.DateWith)).ToList();

                if (earnings_to_save.Any())
                {
                    var entityReturn = _unitOfWork.Repository<Earning>().AddRange(earnings_to_save);
                    _unitOfWork.CommitSync();
                }
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<Earning>("Unexpected error add", nameof(this.Add), ex);
            }
            return 1;
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Earning> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Earning>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Earning GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Earning> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public void SaveInDatabaseOperation(string brokerMessage)
        {
            try
            {
                Earning earning = ConvertBrokerMessageToEarning(brokerMessage);
                if (Guid.Empty != earning.TickerID)
                    Add(earning);
            }
            catch (Exception e)
            {
                throw e;
               // _logger.LogError(e.Message);
            }
        }

        public void SaveRangeInDatabaseOperation(List<string> brokerMessages)
        {
            try
            {
                IList<Earning> earnings = new List<Earning>();

                foreach (var brokerMessage in brokerMessages)
                {
                    Earning earning = ConvertBrokerMessageToEarning(brokerMessage);
                    if (Guid.Empty != earning.TickerID)
                        earnings.Add(earning);
                }

                AddRange(earnings);
            }
            catch (Exception e)
            {
                throw e;
                //  _logger.LogError(e.Message);
            }
        }

        public Earning Update(Earning updated)
        {
            throw new NotImplementedException();
        }

        public Task<Earning> UpdateAsync(Earning updated)
        {
            throw new NotImplementedException();
        }

        private Earning ConvertBrokerMessageToEarning(string brokerMessage)
        {
            Earning earning = new Earning();
            try
            {
                PackageMessage packageMessage = JsonSerialize.JsonDeserializeObject<PackageMessage>(brokerMessage);

                if (packageMessage.isValidMessage())
                {
                    string[] subs = packageMessage.Message.Split('|');
                    DateTime dateWith = DateTime.ParseExact(subs[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime datePayment;
                    DateTime? paymentDate;

                    if (!DateTime.TryParseExact(subs[3], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datePayment))
                    {
                        paymentDate = null;
                    }
                    else
                    {
                        paymentDate = datePayment;
                    }

                    earning = new Earning()
                    {
                        PaymentDate = paymentDate,
                        DateWith = dateWith,
                        ValueIncome = Double.Parse(subs[1]),
                        Type = subs[2],
                        Quantity = Int32.Parse(subs[4]),
                        TickerID = packageMessage.TicketID
                    };
                }
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
            }

            return earning;
        }
    }
}
