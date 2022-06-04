using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Services.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                HistoricalPrice historicalPriceExist = _unitOfWork.Repository<HistoricalPrice>().Find(x => x.TickerID == entity.TickerID && x.Date == entity.Date);

                if (historicalPriceExist == null)
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
            try
            {
                //HistoricalPrice earningExist = _unitOfWork.Repository<HistoricalPrice>().Find(x => x.TickerID == entity.TickerID && x.Date == entity.Date);

                //if (earningExist == null)
                //{
                var entityReturn = _unitOfWork.Repository<HistoricalPrice>().AddRange(entity);
                _unitOfWork.CommitSync();

                return entityReturn;
                //}
                //else
                //{

                //}
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<HistoricalPrice>("Unexpected error add", nameof(this.Add), ex);
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

        public bool ExistByTickedCodeAndDate(string tickedCode, DateTime dateTime)
        {
            try
            {
                var entityExist = _unitOfWork.Repository<HistoricalPrice>().Exist(x => x.TickerCode == tickedCode && x.Date == dateTime);
                return entityExist;

            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<HistoricalPrice>("Unexpected error exists", nameof(this.ExistByTickedCodeAndDate), ex);
            }
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

        public async Task SaveRangeInDatabaseOperation(IList<string> brokerMessages)
        {
            try
            {
                IList<HistoricalPrice> historicalPrices = new List<HistoricalPrice>();

                foreach (var brokerMessage in brokerMessages)
                {
                    HistoricalPrice historicalPrice = ReadBrokerMessage(brokerMessage);
                    if (!string.IsNullOrEmpty(historicalPrice.TickerCode))
                        historicalPrices.Add(historicalPrice);
                }

                AddRange(historicalPrices);
                await Task.CompletedTask;
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

        private HistoricalPrice ReadBrokerMessage(string messageMQ)
        {
            HistoricalPrice history = new HistoricalPrice();
            try
            {
                PackageMessage packageMessage = JsonSerialize.JsonDeserializeObject<PackageMessage>(messageMQ);

                if (packageMessage.isValidMessage())
                {
                    string[] subs = packageMessage.Message.Split(',');
                    DateTime date = Convert.ToDateTime(subs[0]);

                    if (!ExistByTickedCodeAndDate(packageMessage.TicketCode, date))
                    {
                        history = new HistoricalPrice()
                        {
                            TickerCode = packageMessage.TicketCode,
                            Date = Convert.ToDateTime(subs[0]),
                            Open = double.Parse(subs[1], CultureInfo.InvariantCulture),
                            High = double.Parse(subs[2], CultureInfo.InvariantCulture),
                            Low = double.Parse(subs[3], CultureInfo.InvariantCulture),
                            Close = double.Parse(subs[4], CultureInfo.InvariantCulture),
                            AdjClose = double.Parse(subs[5], CultureInfo.InvariantCulture),
                            Volume = Convert.ToInt32(subs[6]),
                            TickerID = packageMessage.TicketID
                        };
                    }
                    else
                    {
                        //Console.WriteLine(brokerMessage);
                    }
                }
                else
                {
                    //TODO: Edit
                    Console.WriteLine(messageMQ);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error in ReadBrokerMessage", e);
                // _logger.LogError(e.Message);
            }

            return history;
        }
    }
}
