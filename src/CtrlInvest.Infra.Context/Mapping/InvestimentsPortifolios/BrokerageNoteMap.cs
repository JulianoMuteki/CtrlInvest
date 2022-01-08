using CtrlInvest.Domain.Entities.InvestimentsPortifolios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Infra.Context.Mapping
{
    public class BrokerageNoteMap : EntityConfigurationBase<BrokerageNote>
    {
        protected override void Initialize(EntityTypeBuilder<BrokerageNote> builder)
        {
            builder.ToTable("BrokerageNotes");

            builder.Property(e => e.DateIssue)
                     .IsRequired();

            builder.Property(e => e.ExpireDate)
                .IsRequired(false);

            builder.Property(e => e.Price)
                  .HasColumnType("double precision")
                  .IsRequired();

            builder.Property(e => e.TotalAmount)
                  .HasColumnType("double precision")
                  .IsRequired();

            builder.Property(e => e.TicketCode)
                    .IsRequired()
                    .HasMaxLength(150);

            builder.Property(e => e.Quantity)
                  .HasColumnType("integer")
                  .IsRequired();

            builder.Property(e => e.TypeMarket)
                                    .HasColumnName("TypeMarket")
                                    .HasConversion<int>();

            builder.Property(e => e.TypeDeal)
                                    .HasColumnName("TypeDeal")
                                    .HasConversion<int>();

            builder.HasOne(tk => tk.FinancialInstitution)
                 .WithMany(k => k.BrokerageNotes)
                 .HasForeignKey(tk => tk.FinancialInstitutionID);

        }
    }
}
