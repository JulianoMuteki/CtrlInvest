using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities.InvestimentsPortifolios;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services
{
    public class InvestPortfolioService: IInvestPortfolioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvestPortfolioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BrokerageNote Add(BrokerageNote entity)
        {
            throw new NotImplementedException();
        }

        public Task<BrokerageNote> AddAsync(BrokerageNote entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<BrokerageNote> entity)
        {
            try
            {
                _unitOfWork.Repository<BrokerageNote>().AddRange(entity);
                _unitOfWork.CommitSync();
                return 1;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<InvestPortfolioService>("Unexpected error fetching get", nameof(this.AddRange), ex);
            }
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICollection<BrokerageNote> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<BrokerageNote>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public BrokerageNote GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BrokerageNote> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public BrokerageNote Update(BrokerageNote updated)
        {
            throw new NotImplementedException();
        }

        public Task<BrokerageNote> UpdateAsync(BrokerageNote updated)
        {
            throw new NotImplementedException();
        }
    }
}
