using CtrlInvest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CtrlInvest.Infra.Context.Mapping
{
    class TicketSyncMap : IEntityTypeConfiguration<TicketSync>
    {
        public void Configure(EntityTypeBuilder<TicketSync> builder)
        {
            builder.ToTable("TicketSyncs");
            builder.Property(x => x.Id).HasColumnName("TicketSyncID");
            builder.HasKey(t => new { t.TickerID, t.Id });

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
