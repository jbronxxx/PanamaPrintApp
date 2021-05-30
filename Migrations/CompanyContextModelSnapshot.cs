﻿// <auto-generated />
using System;
using PanamaPrintApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PanamaPrintApp.Migrations
{
    [DbContext(typeof(CompanyContext))]
    partial class CompanyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CompanyData.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("INN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("CompanyData.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Consumables")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("EquipmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CompanyData.Models.Price", b =>
                {
                    b.Property<int>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServicePrice")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("CompanyOrder", b =>
                {
                    b.Property<int>("CompaniesCompanyId")
                        .HasColumnType("int");

                    b.Property<int>("OrdersOrderId")
                        .HasColumnType("int");

                    b.HasKey("CompaniesCompanyId", "OrdersOrderId");

                    b.HasIndex("OrdersOrderId");

                    b.ToTable("CompanyOrder");
                });

            modelBuilder.Entity("CompanyOrder", b =>
                {
                    b.HasOne("CompanyData.Models.Company", null)
                        .WithMany()
                        .HasForeignKey("CompaniesCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyData.Models.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
