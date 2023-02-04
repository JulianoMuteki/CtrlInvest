using CtrlInvest.Infra.Repository.Repositories;
using CtrlInvest.Infra.Context;
using CtrlInvest.Infra.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtrlInvest.Domain.Interfaces.Repository;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Base;

namespace CtrlInvest.Infra.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CtrlInvestContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public Dictionary<Type, object> Repositories
        {
            get { return _repositories; }
            set { Repositories = value; }
        }

        public UnitOfWork(CtrlInvestContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IGenericRepository<T>;
            }

            IGenericRepository<T> repo = new GenericRepository<T>(_dbContext);
            Repositories.Add(typeof(T), repo);
            return repo;
        }


        private readonly Dictionary<Type, object> _repositoriesCustom = new Dictionary<Type, object>();

        public Dictionary<Type, object> RepositoriesCustom
        {
            get { return _repositoriesCustom; }
            set { RepositoriesCustom = value; }
        }

        public T RepositoryCustom<T>() where T : class
        {
            if (!RepositoriesCustom.Keys.Contains(typeof(T)))
            {
                CreateReository<T>();
            }

            var obj = RepositoriesCustom[typeof(T)] as T;

            return obj;
        }

        private void CreateReository<T>() where T : class
        {
            if (typeof(IBankRepository).Equals((typeof(T))) && !RepositoriesCustom.Keys.Contains(typeof(T)))
            {
                IBankRepository repository = new BankRepository(_dbContext);
                RepositoriesCustom.Add(typeof(T), repository);
            }
            else if  (typeof(IParentTreeRepository).Equals((typeof(T))) && !RepositoriesCustom.Keys.Contains(typeof(T)))
            {
                IParentTreeRepository repository = new ParentTreeRepository(_dbContext);
                RepositoriesCustom.Add(typeof(T), repository);
            }
            else if (typeof(IStockExchangeRepository).Equals((typeof(T))) && !RepositoriesCustom.Keys.Contains(typeof(T)))
            {
                IStockExchangeRepository repository = new StockExchangeRepository(_dbContext);
                RepositoriesCustom.Add(typeof(T), repository);
            }
        }

        //public async Task<int> Commit()
        //{
        //    return await _dbContext.SaveChangesAsync();
        //}

        public void CommitSync()
        {
             _dbContext.SaveChanges();
        }

        public void Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }

        public void SetTrackAll()
        {
            _dbContext.SetTrackAll();
        }

        public void EnableLazyLoading()
        {
            _dbContext.EnableLazyLoading();
        }
    }
}
