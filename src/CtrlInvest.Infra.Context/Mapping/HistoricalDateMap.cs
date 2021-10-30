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
   public class HistoricalDateMap : IEntityTypeConfiguration<HistoricalDate>
    {
        public void Configure(EntityTypeBuilder<HistoricalDate> builder)
        {           
            builder.ToTable("HistoricalDates");
// builder.HasKey(b => b.Id).HasName("HistoricalDateID");
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
                 .WithMany(k => k.HistoricalDates)
                 .HasForeignKey(tk => tk.TickerID);
        }
    }
}