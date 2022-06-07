using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities.Aggregates;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtrlInvest.Services.Identity
{
    public class IdentityCustomService : IIdentityCustomService
    {

        private readonly IUnitOfWork _unitOfWork;

        public IdentityCustomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserTokenHistory Add(UserTokenHistory entity)
        {
            try
            {
                var userTokenHistoryLast = _unitOfWork.Repository<UserTokenHistory>().Find(x => x.UserId == entity.UserId && !x.IsRevorked);
                if (userTokenHistoryLast != null)
                {
                    userTokenHistoryLast.Update();
                    _unitOfWork.Repository<UserTokenHistory>().Update(userTokenHistoryLast);

                }
                var entityReturn = _unitOfWork.Repository<UserTokenHistory>().Add(entity);
                _unitOfWork.CommitSync();

                return entityReturn;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<UserTokenHistory>("Unexpected error add", nameof(this.Add), ex);
            }
        }

        public Task<UserTokenHistory> AddAsync(UserTokenHistory entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<UserTokenHistory> entity)
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

        public ICollection<UserTokenHistory> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserTokenHistory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public UserTokenHistory GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserTokenHistory> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public UserTokenHistory Update(UserTokenHistory updated)
        {
            try
            {
               var entityReturn = _unitOfWork.Repository<UserTokenHistory>().Update(updated);
                _unitOfWork.CommitSync();

                return entityReturn;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<UserTokenHistory>("Unexpected error add", nameof(this.Add), ex);
            }
        }

        public Task<UserTokenHistory> UpdateAsync(UserTokenHistory updated)
        {
            throw new NotImplementedException();
        }
    }
}
