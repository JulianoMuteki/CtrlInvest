using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Domain.Interfaces.Base;
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
            throw new NotImplementedException();
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

        public HistoricalDate GetLatestHistoricalByTicker(string ticker)
        {
            try
            {
                //TODO: Refactoring in repository
                return _unitOfWork.Repository<HistoricalDate>().FindAll(x => x.TickerCode == ticker).OrderByDescending(x => x.Date).FirstOrDefault();
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

        public void SaveHistoricalDateList(IList<HistoricalDate> historicalsList)
        {
            try
            {
                _unitOfWork.Repository<HistoricalDate>().AddRange(historicalsList);
                  _unitOfWork.CommitSync();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<HistoricalDate>("Unexpected error fetching get", nameof(this.SaveHistoricalDateList), ex);
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
