using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UMS.Domain.ValueObjects;

namespace UMS.Persistence.Configurations;

public class UserRelationshipConfiguration : IEntityTypeConfiguration<UserRelationship>
{
    public void Configure(EntityTypeBuilder<UserRelationship> builder)
    {
        builder.HasKey(u => new { u.UserId, u.RelatedUserId });

        builder.HasOne(u => u.User)
            .WithMany(u => u.RelatedUsers)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.RelatedUser)
            .WithMany(u => u.RelatedByUsers)
            .HasForeignKey(u => u.RelatedUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(u => u.RelationshipType)
            .HasConversion<string>();
    }
}