using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Services.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtrlInvest.Services.StocksExchanges
{ 
    public class HistoricalPriceService : IHistoricalPriceService
    {
        //private readonly ILogger<HistoricalEarningService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        // Constructor
        public HistoricalPriceService(IUnitOfWork unitOfWork)
        {
            //  _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public HistoricalPrice Add(HistoricalPrice entity)
        {
            try
            {
                HistoricalPrice earningExist = _unitOfWork.Repository<HistoricalPrice>().Find(x => x.TickerID == entity.TickerID && x.Date == entity.Date);

                if (earningExist == null)
                {
                    var entityReturn = _unitOfWork.Repository<HistoricalPrice>().Add(entity);
                    _unitOfWork.CommitSync();

                    return entityReturn;
                }
                else
                {

                }
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<HistoricalPrice>("Unexpected error add", nameof(this.Add), ex);
            }
            return null;
        }

        public Task<HistoricalPrice> AddAsync(HistoricalPrice entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<HistoricalPrice> entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICollection<HistoricalPrice> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<HistoricalPrice>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public HistoricalPrice GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<HistoricalPrice> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public void SaveInDatabaseOperation(string brokerMessage)
        {
            try
            {
                HistoricalPrice historicalPrice = ReadBrokerMessage(brokerMessage);
                if (!string.IsNullOrEmpty(historicalPrice.TickerCode))
                   Add(historicalPrice);
            }
            catch (Exception e)
            {
                throw e;
              //  _logger.LogError(e.Message);
            }
        }

        public HistoricalPrice Update(HistoricalPrice updated)
        {
            throw new NotImplementedException();
        }

        public Task<HistoricalPrice> UpdateAsync(HistoricalPrice updated)
        {
            throw new NotImplementedException();
        }

        private HistoricalPrice ReadBrokerMessage(string brokerMessage)
        {
            HistoricalPrice history = new HistoricalPrice();
            try
            {
                PackageMessage packageMessage = JsonSerialize.JsonDeserializeObject<PackageMessage>(brokerMessage);

                if (packageMessage.isValidMessage())
                {
                    string[] subs = packageMessage.Message.Split(',');

                    history = new HistoricalPrice()
                    {
                        TickerCode = packageMessage.TicketCode,
                        Date = Convert.ToDateTime(subs[0]),
                        Open = double.Parse(subs[1]),
                        High = double.Parse(subs[2]),
                        Low = double.Parse(subs[3]),
                        Close = double.Parse(subs[4]),
                        AdjClose = double.Parse(subs[5]),
                        Volume = Convert.ToInt32(subs[6]),
                        TickerID = packageMessage.TicketID
                    };
                }
            }
            catch (Exception e)
            {
                throw e;
               // _logger.LogError(e.Message);
            }

            return history;
        }
    }
}
