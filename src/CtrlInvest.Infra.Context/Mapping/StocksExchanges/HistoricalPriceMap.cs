using CtrlInvest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Infra.Context.Mapping
{
    public class HistoricalPriceMap : EntityConfigurationBase<HistoricalPrice>
    {
        protected override void Initialize(EntityTypeBuilder<HistoricalPrice> builder)
        {
            builder.ToTable("HistoricalPrices");
            builder.HasKey(t => new { t.Date, t.TickerCode });

            builder.Property(e => e.TickerCode)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Date)
                     .IsRequired();

            builder.Property(e => e.Open)
                  .HasColumnType("double precision")
                  .IsRequired();

            builder.Property(e => e.High)
                  .HasColumnType("double precision")
                  .IsRequired();
            builder.Property(e => e.Low)
                  .HasColumnType("double precision")
                  .IsRequired();
            builder.Property(e => e.Close)
                  .HasColumnType("double precision")
                  .IsRequired();
            builder.Property(e => e.AdjClose)
                  .HasColumnType("double precision")
                  .IsRequired();
            builder.Property(e => e.Volume)
                    .IsRequired();

            builder.HasOne(tk => tk.Ticket)
                 .WithMany(k => k.HistoricalPrices)
                 .HasForeignKey(tk => tk.TickerID);
        }
    }
}
