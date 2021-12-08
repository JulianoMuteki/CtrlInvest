using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Entities.StocksExchanges;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Services.Common;
using CtrlInvest.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services
{
    public class TicketAppService : ITicketAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Ticket Add(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket> AddAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<Ticket> entity)
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

        public ICollection<Ticket> GetAll()
        {
            try
            {
                //TODO: Refactoring in repository
                return _unitOfWork.Repository<Ticket>().FindAll(x => x.IsDisable == false).ToList();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<TicketSync>("Unexpected error fetching get", nameof(this.GetAll), ex);
            }
        }

        public Task<ICollection<Ticket>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public ICollection<TicketSync> GetAllTicketsSyncs()
        {
            try
            {
                return _unitOfWork.Repository<TicketSync>().FindAll(x => x.IsEnabled).ToList();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<TicketSync>("Unexpected error fetching get", nameof(this.GetAll), ex);
            }
        }

        public Ticket GetById(Guid id)
        {
            try
            {
                return _unitOfWork.Repository<Ticket>().GetById(id);
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<Ticket>("Unexpected error fetching get", nameof(this.GetById), ex);
            }
        }

        public Task<Ticket> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Earning GetLastEarningByTicker(Guid ticketID)
        {
            try
            {
                //TODO: Refactoring in repository
                return _unitOfWork.Repository<Earning>().FindAll(x => x.TickerID == ticketID).OrderByDescending(x => x.DateWith).FirstOrDefault();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<TicketSync>("Unexpected error fetching get", nameof(this.GetLastEarningByTicker), ex);
            }
        }

        public HistoricalPrice GetLatestHistoricalByTicker(string ticker)
        {
            try
            {
                //TODO: Refactoring in repository
                return _unitOfWork.Repository<HistoricalPrice>().FindAll(x => x.TickerCode == ticker).OrderByDescending(x => x.Date).FirstOrDefault();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<TicketSync>("Unexpected error fetching get", nameof(this.GetAll), ex);
            }
        }

        public void SaveEarningsList(IList<Earning> earningList)
        {
            try
            {
                _unitOfWork.Repository<Earning>().AddRange(earningList);
                _unitOfWork.CommitSync();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<HistoricalPrice>("Unexpected error fetching get", nameof(this.SaveEarningsList), ex);
            }
        }

        public void SaveHistoricalPricesList(IList<HistoricalPrice> historicalPricesList)
        {
            try
            {
                _unitOfWork.Repository<HistoricalPrice>().AddRange(historicalPricesList);
                  _unitOfWork.CommitSync();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<HistoricalPrice>("Unexpected error fetching get", nameof(this.SaveHistoricalPricesList), ex);
            }
        }

        public Ticket Update(Ticket updated)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket> UpdateAsync(Ticket updated)
        {
            throw new NotImplementedException();
        }
    }
}
