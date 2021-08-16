using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities.FinancialClassification;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.AppService
{
    public class GrandChildTreeAppService : IGrandChildTreeAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GrandChildTreeAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public GrandChildTree Add(GrandChildTree entity)
        {
            try
            {
                var grandChildTree = _unitOfWork.Repository<GrandChildTree>().Add(entity);
                _unitOfWork.CommitSync();
                return grandChildTree;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<GrandChildTreeAppService>("Unexpected error fetching Add", nameof(this.Add), ex);
            }
        }

        public GrandChildTree GetById(Guid id)
        {
            try
            {
                return _unitOfWork.Repository<GrandChildTree>().GetById(id);
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<GrandChildTreeAppService>("Unexpected error fetching get", nameof(this.GetById), ex);
            }
        }

        public Task<GrandChildTree> AddAsync(GrandChildTree entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<GrandChildTree> entity)
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

        public ICollection<GrandChildTree> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GrandChildTree>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GrandChildTree> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public GrandChildTree Update(GrandChildTree updated)
        {
            throw new NotImplementedException();
        }

        public Task<GrandChildTree> UpdateAsync(GrandChildTree updated)
        {
            throw new NotImplementedException();
        }

        public ICollection<GrandChildTree> GetListByParentID(Guid parentID)
        {
            try
            {
                var childTrees = _unitOfWork.Repository<GrandChildTree>().FindBy(x => x.ParentNodeID == parentID).ToList();
                _unitOfWork.CommitSync();
                return childTrees;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<GrandChildTreeAppService>("Unexpected error fetching GetListByParentID", nameof(this.GetListByParentID), ex);
            }
        }
    }
}
