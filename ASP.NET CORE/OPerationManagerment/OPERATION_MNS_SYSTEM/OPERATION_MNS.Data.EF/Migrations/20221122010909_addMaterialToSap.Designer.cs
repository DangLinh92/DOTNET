﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OPERATION_MNS.Data.EF;

namespace OPERATION_MNS.Data.EF.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20221122010909_addMaterialToSap")]
    partial class addMaterialToSap
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("APP_ROLE_CLAIM");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("APP_USER_CLAIM");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("APP_USER_LOGIN");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "UserId");

                    b.ToTable("APP_USER_ROLE");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("APP_USER_TOKEN");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.ACTUAL_DAILY_VIEW", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DateActual")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("DateCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("DateModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Material_MES")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Model_GOC")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<float>("Qty_PostOperationShipping")
                        .HasColumnType("real");

                    b.Property<float>("Qty_ShippingWait")
                        .HasColumnType("real");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("ACTUAL_DAILY_VIEW");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.APP_ROLE", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("APP_ROLE");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.APP_USER", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("DateModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShowPass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("APP_USER");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.FUNCTION", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("DateCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("DateModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("IconCss")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("UserCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("FUNCTION");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.GOC_PLAN", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DateCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("DateModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("DatePlan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Division")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Material")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Module")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("MonthPlan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("QuantityActual")
                        .HasColumnType("real");

                    b.Property<float>("QuantityGap")
                        .HasColumnType("real");

                    b.Property<float>("QuantityPlan")
                        .HasColumnType("real");

                    b.Property<float>("StandardQtyForMonth")
                        .HasColumnType("real");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("GOC_PLAN");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.GOC_STANDAR_QTY", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DateCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("DateModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Division")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Material")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Module")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("MonthBegin")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<float>("StandardQtyForMonth")
                        .HasColumnType("real");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("GOC_STANDAR_QTY");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.INVENTORY_ACTUAL", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("After_Plate_Develop_Visual_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("After_Roof_Develop_Visual_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("After_Roof_Lami_Visual_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("Before_Plate_PR_Wafer_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("Before_Roof_Lami_Wafer_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("CassetteInputStock_Pre")
                        .HasColumnType("real");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<float>("Cu_Sn_Plating")
                        .HasColumnType("real");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("DateCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("DateModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<float>("EBR")
                        .HasColumnType("real");

                    b.Property<float>("Input_wafer_inspection")
                        .HasColumnType("real");

                    b.Property<string>("Material")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<float>("Nd_Plate_Visual_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("PR_Strip_Cu_Etching")
                        .HasColumnType("real");

                    b.Property<float>("Plate_BST")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Inspection_Wait")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Measure")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Patterning_Develop")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Patterning_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Patterning_Mask_Cleaning")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Patterning_Measure")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Patterning_PR")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Patterning_PR_Ashing")
                        .HasColumnType("real");

                    b.Property<float>("Plate_Patterning_Photo")
                        .HasColumnType("real");

                    b.Property<float>("Plating_Input_Waiting")
                        .HasColumnType("real");

                    b.Property<float>("Post_Operation_Shipping")
                        .HasColumnType("real");

                    b.Property<float>("PreOperationWaiting")
                        .HasColumnType("real");

                    b.Property<float>("Probe_Waite")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Ashing")
                        .HasColumnType("real");

                    b.Property<float>("Roof_BST")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Develop")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Hardening")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Laminating")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Mask_Cleaning")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Measure")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Oven")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Oven_PEB")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Photo")
                        .HasColumnType("real");

                    b.Property<float>("Roof_QDR")
                        .HasColumnType("real");

                    b.Property<float>("Roof_Remover")
                        .HasColumnType("real");

                    b.Property<float>("SN_Plate_Measure")
                        .HasColumnType("real");

                    b.Property<float>("Seed_Deposition")
                        .HasColumnType("real");

                    b.Property<string>("Series")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<float>("Shipping_Wait")
                        .HasColumnType("real");

                    b.Property<float>("St_Plate_Visual_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("Ti_ething")
                        .HasColumnType("real");

                    b.Property<float>("Total")
                        .HasColumnType("real");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserCreated")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserModified")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<float>("Wafer_Probe_IR")
                        .HasColumnType("real");

                    b.Property<float>("Wafer_Probe_RF")
                        .HasColumnType("real");

                    b.Property<float>("Wafer_Sorting")
                        .HasColumnType("real");

                    b.Property<float>("WaitMarkingIDCHK")
                        .HasColumnType("real");

                    b.Property<float>("Wall_Ashing")
                        .HasColumnType("real");

                    b.Property<float>("Wall_Ashing_Waiting")
                        .HasColumnType("real");

                    b.Property<float>("Wall_Develop")
                        .HasColumnType("real");

                    b.Property<float>("Wall_Inspection")
                        .HasColumnType("real");

                    b.Property<float>("Wall_Mask_Cleaning")
                        .HasColumnType("real");

                    b.Property<float>("Wall_Oven")
                        .HasColumnType("real");

                    b.Property<float>("Wall_PR")
                        .HasColumnType("real");

                    b.Property<float>("Wall_PR_Measure")
                        .HasColumnType("real");

                    b.Property<float>("Wall_PR_Wafer_inspection")
                        .HasColumnType("real");

                    b.Property<float>("Wall_Photo")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("INVENTORY_ACTUAL");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.LANGUAGE", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Resources")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LANGUAGE");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.MATERIAL_TO_SAP", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Material")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("SAP_Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("MATERIAL_TO_SAP");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.PERMISSION", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ApproveL1")
                        .HasColumnType("bit");

                    b.Property<bool>("ApproveL2")
                        .HasColumnType("bit");

                    b.Property<bool>("ApproveL3")
                        .HasColumnType("bit");

                    b.Property<bool>("CanCreate")
                        .HasColumnType("bit");

                    b.Property<bool>("CanDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("CanRead")
                        .HasColumnType("bit");

                    b.Property<bool>("CanUpdate")
                        .HasColumnType("bit");

                    b.Property<string>("FunctionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FunctionId");

                    b.HasIndex("RoleId");

                    b.ToTable("PERMISSION");
                });

            modelBuilder.Entity("OPERATION_MNS.Data.Entities.PERMISSION", b =>
                {
                    b.HasOne("OPERATION_MNS.Data.Entities.FUNCTION", "Function")
                        .WithMany()
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OPERATION_MNS.Data.Entities.APP_ROLE", "AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
