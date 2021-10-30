using CtrlInvest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Infra.Context.Mapping
{
    public class TicketMap : EntityConfigurationBase<Ticket>
    {
        protected override void Initialize(EntityTypeBuilder<Ticket> builder)
        {
            base.Initialize(builder);

            builder.ToTable("Tickets");
            builder.HasKey(b => b.Id).HasName("Ticket");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Exchange)
                     .IsRequired()
                     .HasMaxLength(50);

            builder.Property(e => e.Country)
                     .IsRequired()
                     .HasMaxLength(50);

            builder.Property(e => e.Currency)                     
                     .HasMaxLength(50);
        }
    }
}