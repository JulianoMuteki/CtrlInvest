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
    class TicketSyncMap : IEntityTypeConfiguration<TicketSync>
    {
        public void Configure(EntityTypeBuilder<TicketSync> builder)
        {
            builder.ToTable("TicketSyncs");
            builder.HasKey(t => new { t.TickerID, t.TicketSyncID });

            builder.Property(e => e.DateStart)
                     .IsRequired();

            builder.Property(e => e.IsEnabled)
                  .IsRequired();

            builder.HasOne(tk => tk.Ticket)
                 .WithMany(k => k.TicketSyncs)
                 .HasForeignKey(tk => tk.TickerID);
        }
    }
}
