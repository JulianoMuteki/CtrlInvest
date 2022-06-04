using CtrlInvest.Domain.Entities.StocksExchanges;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlInvest.Infra.Context.Mapping
{
    public class EarningMap : EntityConfigurationBase<Earning>
    {
        protected override void Initialize(EntityTypeBuilder<Earning> builder)
        {
            builder.ToTable("Earnings");

            builder.Property(e => e.DateWith)
                     .IsRequired();

            builder.Property(e => e.ValueIncome)
                  .HasColumnType("double precision")
                  .IsRequired();

            builder.Property(e => e.PaymentDate)
                     .IsRequired(false);

            builder.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(150);

            builder.Property(e => e.Quantity)
                  .HasColumnType("integer")
                  .IsRequired();


            builder.HasOne(tk => tk.Ticket)
                 .WithMany(k => k.Earnings)
                 .HasForeignKey(tk => tk.TickerID);
        }
    }
}
