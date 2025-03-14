using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UMS.Domain.Entities;

namespace UMS.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .IsRequired()
            .UseIdentityColumn(1, 1);

        builder.Property(u => u.Firstname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Lastname)
            .IsRequired()
            .HasMaxLength(50);

        builder.ToTable(tb =>
            tb.HasCheckConstraint(
                "CHK_User_Name",
                "(Firstname NOT LIKE '%[^a-zA-Z]%' AND Lastname NOT LIKE '%[^a-zA-Z]%')"));

        builder.Property(u => u.Gender)
            .HasConversion<string>();

        builder.Property(u => u.SocialNumber)
            .IsRequired()
            .HasMaxLength(11);

        builder.ToTable(tb =>
            tb.HasCheckConstraint(
                "CHK_PhoneNumber_Length",
                "LEN(SocialNumber) = 11"));

        builder.Property(u => u.DateOfBirth)
            .IsRequired();

        builder.ToTable(tb =>
            tb.HasCheckConstraint(
                "CHK_User_Age_18",
                "DATEDIFF(YEAR, DateOfBirth, GETDATE()) >= 18"
            ));
        
        builder.HasOne<City>(u => u.City)
            .WithMany()
            .HasForeignKey(u => u.CityId);
    }
}