using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities.FinancialClassification;
using CtrlInvest.Domain.Interfaces.Repository;
using CtrlInvest.Infra.Context;
using CtrlInvest.Infra.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CtrlInvest.Infra.Repository.Repositories
{
    public class ParentTreeRepository : GenericRepository<ParentTree>, IParentTreeRepository
    {
        public ParentTreeRepository(CtrlInvestContext context)
            : base(context)
        {

        }

        public ICollection<ParentTree> GetAll_WithChildrem()
        {
            try
            {
                return _context.Set<ParentTree>()
                    .Include(x => x.Children).ThenInclude(x => x.Children)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<ParentTree>("Unexpected error fetching GetAll_WithChildrem", nameof(this.GetAll_WithChildrem), ex);
            }
        }
    }
}
