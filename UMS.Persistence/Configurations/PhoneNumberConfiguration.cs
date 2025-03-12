using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UMS.Domain.ValueObjects;

namespace UMS.Persistence.Configurations;

public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        builder.HasKey(u => new { u.UserId, u.Number });

        builder.HasOne(p => p.User)
            .WithMany(u => u.PhoneNumber)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.Type)
            .HasConversion<string>();

        builder.Property(u => u.Number)
            .HasMaxLength(50);
        
        builder.HasIndex(u => u.Number);
    }
}