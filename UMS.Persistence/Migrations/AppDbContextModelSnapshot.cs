﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UMS.Persistence.Context;

#nullable disable

namespace UMS.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UMS.Domain.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("UMS.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SocialNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.HasIndex("CityId")
                        .IsUnique();

                    b.ToTable("Users", t =>
                        {
                            t.HasCheckConstraint("CHK_PhoneNumber_Length", "LEN(SocialNumber) = 11");

                            t.HasCheckConstraint("CHK_User_Age_18", "DATEDIFF(YEAR, DateOfBirth, GETDATE()) >= 18");

                            t.HasCheckConstraint("CHK_User_Name", "(Firstname NOT LIKE '%[^a-zA-Z]%' AND Lastname NOT LIKE '%[^a-zA-Z]%')");
                        });
                });

            modelBuilder.Entity("UMS.Domain.ValueObjects.PhoneNumber", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Number")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "Number");

                    b.HasIndex("Number");

                    b.ToTable("PhoneNumbers");
                });

            modelBuilder.Entity("UMS.Domain.ValueObjects.UserRelationship", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RelatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("RelationshipType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "RelatedUserId");

                    b.HasIndex("RelatedUserId");

                    b.ToTable("UserRelationships");
                });

            modelBuilder.Entity("UMS.Domain.Entities.User", b =>
                {
                    b.HasOne("UMS.Domain.Entities.City", "City")
                        .WithOne()
                        .HasForeignKey("UMS.Domain.Entities.User", "CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("UMS.Domain.ValueObjects.PhoneNumber", b =>
                {
                    b.HasOne("UMS.Domain.Entities.User", "User")
                        .WithMany("PhoneNumber")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UMS.Domain.ValueObjects.UserRelationship", b =>
                {
                    b.HasOne("UMS.Domain.Entities.User", "RelatedUser")
                        .WithMany()
                        .HasForeignKey("RelatedUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("UMS.Domain.Entities.User", "User")
                        .WithMany("Relationships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("RelatedUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UMS.Domain.Entities.User", b =>
                {
                    b.Navigation("PhoneNumber");

                    b.Navigation("Relationships");
                });
#pragma warning restore 612, 618
        }
    }
}
