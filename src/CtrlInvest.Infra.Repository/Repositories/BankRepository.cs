using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Repository;
using CtrlInvest.Infra.Context;
using CtrlInvest.Infra.Repository.Common;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CtrlInvest.Infra.Repository.Repositories
{
    public class BankRepository : GenericRepository<Bank>, IBankRepository
    {
        public BankRepository(CtrlInvestContext context)
            : base(context)
        {

        }

        public ICollection<Bank> GetAllBanks()
        {
            var dir = Directory.GetParent(Directory.GetCurrentDirectory());
            string path = Path.Combine(dir.FullName, @"CtrlInvest.Resources\febraban_banks.json");
            var jsonString = File.ReadAllText(path);
            var banks = JsonSerializer.Deserialize<IList<Bank>>(jsonString);
            return banks;
        }
    }
}
