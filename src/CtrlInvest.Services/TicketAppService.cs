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
                return _unitOfWork.Repository<TicketSync>().GetAll();
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
            throw new NotImplementedException();
        }

        public Task<Ticket> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
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
