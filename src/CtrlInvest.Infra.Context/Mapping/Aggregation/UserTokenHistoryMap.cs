using CtrlInvest.Domain.Entities.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Infra.Context.Mapping.Aggregation
{
    public class UserTokenHistoryMap : IEntityTypeConfiguration<UserTokenHistory>
    {
        public void Configure(EntityTypeBuilder<UserTokenHistory> builder)
        {
            builder.ToTable("UsersTokensHistories");
           
            builder.HasKey(t => t.UserTokenHistoryID);

            builder.Property(e => e.JwtId)
                     .IsRequired();

            builder.Property(e => e.IsUsed)
                  .IsRequired();

            builder.Property(e => e.IsRevorked)
                     .IsRequired();
            builder.Property(e => e.AddedDate)
                   .IsRequired();
            builder.Property(e => e.ExpireDate)
                    .IsRequired();
            builder.Property(e => e.Token)
                  .IsRequired();

                builder.HasOne(tk => tk.User)
                 .WithMany(k => k.UsersTokensHistories)
                 .HasForeignKey(tk => tk.UserId);
        }
    }
}
