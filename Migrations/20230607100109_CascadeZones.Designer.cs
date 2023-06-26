﻿// <auto-generated />
using System;
using HSEPractice2.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HSEPractice2.Migrations
{
    [DbContext(typeof(WaterparkContext))]
    [Migration("20230607100109_CascadeZones")]
    partial class CascadeZones
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HSEPractice2.Areas.Identity.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Attraction", b =>
                {
                    b.Property<int>("AttractionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("attraction_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AttractionId"));

                    b.Property<string>("AttractionDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("attraction_description");

                    b.Property<string>("AttractionName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("attraction_name");

                    b.Property<decimal?>("DescentTime")
                        .HasColumnType("numeric")
                        .HasColumnName("descent_time");

                    b.Property<int?>("GrowthRestrictions")
                        .HasColumnType("integer")
                        .HasColumnName("growth_restrictions");

                    b.Property<decimal?>("SlideHeight")
                        .HasColumnType("numeric")
                        .HasColumnName("slide_height");

                    b.Property<int?>("WeightRestrictions")
                        .HasColumnType("integer")
                        .HasColumnName("weight_restrictions");

                    b.Property<int>("ZoneLocation")
                        .HasColumnType("integer")
                        .HasColumnName("zone_location");

                    b.HasKey("AttractionId")
                        .HasName("pk_attraction_id");

                    b.HasIndex("ZoneLocation");

                    b.HasIndex(new[] { "AttractionName" }, "attractions_attraction_name_key")
                        .IsUnique();

                    b.ToTable("attractions", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Benefit", b =>
                {
                    b.Property<int>("BenefitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("benefit_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BenefitId"));

                    b.Property<string>("BenefitName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("benefit_name");

                    b.Property<string>("ConditionForReceivingBenefit")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("condition_for_receiving_benefit");

                    b.Property<int>("DiscountPercentage")
                        .HasColumnType("integer")
                        .HasColumnName("discount_percentage");

                    b.HasKey("BenefitId")
                        .HasName("pk_benefit_id");

                    b.HasIndex(new[] { "BenefitName" }, "benefits_benefit_name_key")
                        .IsUnique();

                    b.ToTable("benefits", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Bracelet", b =>
                {
                    b.Property<int>("BraceletId")
                        .HasColumnType("integer")
                        .HasColumnName("bracelet_id");

                    b.Property<decimal>("CashBalance")
                        .HasColumnType("numeric")
                        .HasColumnName("cash_balance");

                    b.Property<int?>("LockerNumber")
                        .HasColumnType("integer")
                        .HasColumnName("locker_number");

                    b.Property<TimeOnly>("RemainingTime")
                        .HasColumnType("time without time zone")
                        .HasColumnName("remaining_time");

                    b.HasKey("BraceletId")
                        .HasName("pk_bracelet_id");

                    b.ToTable("bracelets", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.CheckingAttraction", b =>
                {
                    b.Property<DateOnly>("CheckingDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("checking_date")
                        .HasDefaultValueSql("CURRENT_DATE");

                    b.Property<int>("AttractionId")
                        .HasColumnType("integer")
                        .HasColumnName("attraction_id");

                    b.Property<string>("AdmissionLaunch")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("admission_launch")
                        .HasDefaultValueSql("'да'::character varying");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer")
                        .HasColumnName("employee_id");

                    b.Property<string>("Note")
                        .HasColumnType("text")
                        .HasColumnName("note");

                    b.HasKey("CheckingDate", "AttractionId")
                        .HasName("pk_checking_attractions");

                    b.HasIndex("AttractionId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("checking_attractions", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.CurrentShift", b =>
                {
                    b.Property<int>("CurrentShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("current_shift_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CurrentShiftId"));

                    b.Property<int>("Employee")
                        .HasColumnType("integer")
                        .HasColumnName("employee");

                    b.Property<int>("Shift")
                        .HasColumnType("integer")
                        .HasColumnName("shift");

                    b.HasKey("CurrentShiftId")
                        .HasName("pk_current_shift_id");

                    b.HasIndex("Employee");

                    b.HasIndex("Shift");

                    b.ToTable("current_shift", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.CurrentWorkout", b =>
                {
                    b.Property<int>("CurrentWorkoutId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("current_workout_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CurrentWorkoutId"));

                    b.Property<int>("Trainer")
                        .HasColumnType("integer")
                        .HasColumnName("trainer");

                    b.Property<int>("WorkoutDate")
                        .HasColumnType("integer")
                        .HasColumnName("workout_date");

                    b.HasKey("CurrentWorkoutId")
                        .HasName("pk_current_workout_id");

                    b.HasIndex("Trainer");

                    b.HasIndex("WorkoutDate");

                    b.ToTable("current_workout", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("event_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EventId"));

                    b.Property<DateOnly>("DateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("date_time")
                        .HasDefaultValueSql("CURRENT_DATE");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.Property<TimeOnly>("Duration")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("time without time zone")
                        .HasColumnName("duration")
                        .HasDefaultValueSql("'02:00:00'::time without time zone");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("integer")
                        .HasColumnName("employee_id");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("event_name");

                    b.HasKey("EventId")
                        .HasName("pk_event_id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("events", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.OperationsHistory", b =>
                {
                    b.Property<DateTime>("DateTimePurchase")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_time_purchase");

                    b.Property<int>("BraceletId")
                        .HasColumnType("integer")
                        .HasColumnName("bracelet_id");

                    b.Property<int>("DiscountPercentage")
                        .HasColumnType("integer")
                        .HasColumnName("discount_percentage");

                    b.Property<int>("ServiceCount")
                        .HasColumnType("integer")
                        .HasColumnName("service_count");

                    b.Property<int>("ServiceName")
                        .HasColumnType("integer")
                        .HasColumnName("service_name");

                    b.HasKey("DateTimePurchase", "BraceletId")
                        .HasName("pk_operations_history");

                    b.HasIndex("BraceletId");

                    b.HasIndex("DiscountPercentage");

                    b.HasIndex("ServiceName");

                    b.ToTable("operations_history", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("post_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PostId"));

                    b.Property<string>("Post1")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("post");

                    b.Property<int>("SalaryAmount")
                        .HasColumnType("integer")
                        .HasColumnName("salary_amount");

                    b.HasKey("PostId")
                        .HasName("pk_post_id");

                    b.HasIndex(new[] { "Post1" }, "posts_post_key")
                        .IsUnique();

                    b.ToTable("posts", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Result", b =>
                {
                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("total_amount");

                    b.Property<DateTimeOffset>("Workday")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("workday");

                    b.ToTable((string)null);

                    b.ToView("results", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Service", b =>
                {
                    b.Property<int>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("service_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ServiceId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<int>("Price")
                        .HasColumnType("integer")
                        .HasColumnName("price");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("service_name");

                    b.HasKey("ServiceId")
                        .HasName("pk_service_id");

                    b.HasIndex(new[] { "ServiceName" }, "services_service_name_key")
                        .IsUnique();

                    b.ToTable("services", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Shift", b =>
                {
                    b.Property<int>("ShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("shift_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ShiftId"));

                    b.Property<DateOnly>("ShiftDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("shift_date")
                        .HasDefaultValueSql("CURRENT_DATE");

                    b.Property<int>("WorkShedule")
                        .HasColumnType("integer")
                        .HasColumnName("work_shedule");

                    b.HasKey("ShiftId")
                        .HasName("pk_shift_id");

                    b.ToTable("shifts", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Staff", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("employee_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmployeeId"));

                    b.Property<DateTime>("DateOfHiring")
                        .HasColumnType("timestamp")
                        .HasColumnName("date_of_hiring");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("employee_name");

                    b.Property<string>("EmployeePatronymic")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("employee_patronymic");

                    b.Property<string>("EmployeeSurname")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("employee_surname");

                    b.Property<long>("Phone")
                        .HasColumnType("bigint")
                        .HasColumnName("phone");

                    b.Property<int>("Post")
                        .HasColumnType("integer")
                        .HasColumnName("post");

                    b.HasKey("EmployeeId")
                        .HasName("pk_employee_id");

                    b.HasIndex("Post");

                    b.ToTable("staff", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Vedomost", b =>
                {
                    b.Property<int?>("BraceletId")
                        .HasColumnType("integer")
                        .HasColumnName("bracelet_id");

                    b.Property<DateTime?>("DateTimePurchase")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_time_purchase");

                    b.Property<int?>("DiscountPercentage")
                        .HasColumnType("integer")
                        .HasColumnName("discount_percentage");

                    b.Property<decimal?>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int?>("ServiceCount")
                        .HasColumnType("integer")
                        .HasColumnName("service_count");

                    b.Property<string>("ServiceName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("service_name");

                    b.ToTable((string)null);

                    b.ToView("vedomost", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Workout", b =>
                {
                    b.Property<int>("WorkoutId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("workout_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WorkoutId"));

                    b.Property<DateOnly>("DateOfWorkout")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("date_of_workout")
                        .HasDefaultValueSql("CURRENT_DATE");

                    b.Property<TimeOnly>("Duration")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("time without time zone")
                        .HasColumnName("duration")
                        .HasDefaultValueSql("'01:00:00'::time without time zone");

                    b.Property<string>("WorkoutType")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("workout_type");

                    b.HasKey("WorkoutId")
                        .HasName("pk_workout_id");

                    b.ToTable("workouts", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Zone", b =>
                {
                    b.Property<int>("ZoneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("zone_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ZoneId"));

                    b.Property<string>("ZoneName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("zone_name");

                    b.HasKey("ZoneId")
                        .HasName("pk_zone_id");

                    b.HasIndex(new[] { "ZoneName" }, "zones_zone_name_key")
                        .IsUnique();

                    b.ToTable("zones", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("HSEPractice2.Models.Attraction", b =>
                {
                    b.HasOne("HSEPractice2.Models.Zone", "ZoneLocationNavigation")
                        .WithMany("Attractions")
                        .HasForeignKey("ZoneLocation")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_zone_location");

                    b.Navigation("ZoneLocationNavigation");
                });

            modelBuilder.Entity("HSEPractice2.Models.CheckingAttraction", b =>
                {
                    b.HasOne("HSEPractice2.Models.Attraction", "Attraction")
                        .WithMany("CheckingAttractions")
                        .HasForeignKey("AttractionId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_attraction_id");

                    b.HasOne("HSEPractice2.Models.Staff", "Employee")
                        .WithMany("CheckingAttractions")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_employee_id");

                    b.Navigation("Attraction");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HSEPractice2.Models.CurrentShift", b =>
                {
                    b.HasOne("HSEPractice2.Models.Staff", "EmployeeNavigation")
                        .WithMany("CurrentShifts")
                        .HasForeignKey("Employee")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_employee");

                    b.HasOne("HSEPractice2.Models.Shift", "ShiftNavigation")
                        .WithMany("CurrentShifts")
                        .HasForeignKey("Shift")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_shift");

                    b.Navigation("EmployeeNavigation");

                    b.Navigation("ShiftNavigation");
                });

            modelBuilder.Entity("HSEPractice2.Models.CurrentWorkout", b =>
                {
                    b.HasOne("HSEPractice2.Models.Staff", "TrainerNavigation")
                        .WithMany("CurrentWorkouts")
                        .HasForeignKey("Trainer")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_trainer");

                    b.HasOne("HSEPractice2.Models.Workout", "WorkoutDateNavigation")
                        .WithMany("CurrentWorkouts")
                        .HasForeignKey("WorkoutDate")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_workout_date");

                    b.Navigation("TrainerNavigation");

                    b.Navigation("WorkoutDateNavigation");
                });

            modelBuilder.Entity("HSEPractice2.Models.Event", b =>
                {
                    b.HasOne("HSEPractice2.Models.Staff", "Employee")
                        .WithMany("Events")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_employee_id");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HSEPractice2.Models.OperationsHistory", b =>
                {
                    b.HasOne("HSEPractice2.Models.Bracelet", "Bracelet")
                        .WithMany("OperationsHistories")
                        .HasForeignKey("BraceletId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_bracelet_id");

                    b.HasOne("HSEPractice2.Models.Benefit", "DiscountPercentageNavigation")
                        .WithMany("OperationsHistories")
                        .HasForeignKey("DiscountPercentage")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_discount_percentage");

                    b.HasOne("HSEPractice2.Models.Service", "ServiceNameNavigation")
                        .WithMany("OperationsHistories")
                        .HasForeignKey("ServiceName")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_service_name");

                    b.Navigation("Bracelet");

                    b.Navigation("DiscountPercentageNavigation");

                    b.Navigation("ServiceNameNavigation");
                });

            modelBuilder.Entity("HSEPractice2.Models.Staff", b =>
                {
                    b.HasOne("HSEPractice2.Models.Post", "PostNavigation")
                        .WithMany("Staff")
                        .HasForeignKey("Post")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_post_id");

                    b.Navigation("PostNavigation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("HSEPractice2.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HSEPractice2.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HSEPractice2.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("HSEPractice2.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HSEPractice2.Models.Attraction", b =>
                {
                    b.Navigation("CheckingAttractions");
                });

            modelBuilder.Entity("HSEPractice2.Models.Benefit", b =>
                {
                    b.Navigation("OperationsHistories");
                });

            modelBuilder.Entity("HSEPractice2.Models.Bracelet", b =>
                {
                    b.Navigation("OperationsHistories");
                });

            modelBuilder.Entity("HSEPractice2.Models.Post", b =>
                {
                    b.Navigation("Staff");
                });

            modelBuilder.Entity("HSEPractice2.Models.Service", b =>
                {
                    b.Navigation("OperationsHistories");
                });

            modelBuilder.Entity("HSEPractice2.Models.Shift", b =>
                {
                    b.Navigation("CurrentShifts");
                });

            modelBuilder.Entity("HSEPractice2.Models.Staff", b =>
                {
                    b.Navigation("CheckingAttractions");

                    b.Navigation("CurrentShifts");

                    b.Navigation("CurrentWorkouts");

                    b.Navigation("Events");
                });

            modelBuilder.Entity("HSEPractice2.Models.Workout", b =>
                {
                    b.Navigation("CurrentWorkouts");
                });

            modelBuilder.Entity("HSEPractice2.Models.Zone", b =>
                {
                    b.Navigation("Attractions");
                });
#pragma warning restore 612, 618
        }
    }
}
