using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HSEPractice2.Areas.Identity.Data;

public class WaterparkContext : IdentityDbContext<ApplicationUser>
{
    public WaterparkContext(DbContextOptions<WaterparkContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    //public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    //public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    //public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    //public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    //public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Attraction> Attractions { get; set; }

    public virtual DbSet<Benefit> Benefits { get; set; }

    public virtual DbSet<Bracelet> Bracelets { get; set; }

	public virtual DbSet<Checking> Checkings { get; set; }

	//public virtual DbSet<CheckingAttraction> CheckingAttractions { get; set; }

    public virtual DbSet<CurrentShift> CurrentShifts { get; set; }

    public virtual DbSet<CurrentWorkout> CurrentWorkouts { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<OperationsHistory> OperationsHistories { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Vedomost> Vedomosts { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    public List<Staff> GetStaffListWithPostNavigation()
    {
        return Staff.Include(s => s.PostNavigation).ToList();
    }

    public List<Attraction> GetAttractionListWithZoneNavigation()
    {
        return Attractions.Include(s => s.ZoneLocationNavigation).ToList();
    }

    //public List<CheckingAttraction> GetChecks()
    //{
    //    return CheckingAttractions.Include(s => s.Attraction).Include(s => s.Employee).ToList();
    //}

	public List<Checking> GetCheckings()
	{
		return Checkings.Include(s => s.Attraction).Include(s => s.Employee).ToList();
	}

	public List<CurrentShift> GetShifts()
    {
        return CurrentShifts.Include(s => s.EmployeeNavigation).Include(s => s.ShiftNavigation).ToList();
    }
    public List<OperationsHistory> GetHistoryList()
    {
        return OperationsHistories.Include(s => s.ServiceNameNavigation).Include(s => s.DiscountPercentageNavigation).ToList();
    }
    public List<CurrentWorkout> GetWorkouts()
    {
        return CurrentWorkouts.Include(s=>s.TrainerNavigation).Include(s=>s.WorkoutDateNavigation).ToList();
    }

    public List<Event> GetEvents()
    {
        return Events.Include(s => s.Employee).ToList();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=waterpark;Username=postgres;Password=AnnaPostgrePassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration());


        //modelBuilder.Entity<AspNetRole>(entity =>
        //{
        //    entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

        //    entity.Property(e => e.Name).HasMaxLength(256);
        //    entity.Property(e => e.NormalizedName).HasMaxLength(256);
        //});

        //modelBuilder.Entity<AspNetRoleClaim>(entity =>
        //{
        //    entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

        //    entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        //});

        //modelBuilder.Entity<AspNetUser>(entity =>
        //{
        //    entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

        //    entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

        //    entity.Property(e => e.Email).HasMaxLength(256);
        //    entity.Property(e => e.FirstName).HasMaxLength(255);
        //    entity.Property(e => e.LastName).HasMaxLength(255);
        //    entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
        //    entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
        //    entity.Property(e => e.UserName).HasMaxLength(256);

        //    entity.HasMany(d => d.Roles).WithMany(p => p.Users)
        //        .UsingEntity<Dictionary<string, object>>(
        //            "AspNetUserRole",
        //            r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
        //            l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
        //            j =>
        //            {
        //                j.HasKey("UserId", "RoleId");
        //                j.ToTable("AspNetUserRoles");
        //                j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
        //            });
        //});

        //modelBuilder.Entity<AspNetUserClaim>(entity =>
        //{
        //    entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

        //    entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        //});

        //modelBuilder.Entity<AspNetUserLogin>(entity =>
        //{
        //    entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

        //    entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

        //    entity.Property(e => e.LoginProvider).HasMaxLength(128);
        //    entity.Property(e => e.ProviderKey).HasMaxLength(128);

        //    entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        //});

        //modelBuilder.Entity<AspNetUserToken>(entity =>
        //{
        //    entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

        //    entity.Property(e => e.LoginProvider).HasMaxLength(128);
        //    entity.Property(e => e.Name).HasMaxLength(128);

        //    entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        //});

        modelBuilder.Entity<Attraction>(entity =>
        {
            entity.HasKey(e => e.AttractionId).HasName("pk_attraction_id");

            entity.ToTable("attractions");

            entity.HasIndex(e => e.AttractionName, "attractions_attraction_name_key").IsUnique();

            entity.Property(e => e.AttractionId).HasColumnName("attraction_id");
            entity.Property(e => e.AttractionDescription).HasColumnName("attraction_description");
            entity.Property(e => e.AttractionName)
                .HasMaxLength(64)
                .HasColumnName("attraction_name");
            entity.Property(e => e.DescentTime).HasColumnName("descent_time");
            entity.Property(e => e.GrowthRestrictions).HasColumnName("growth_restrictions");
            entity.Property(e => e.SlideHeight).HasColumnName("slide_height");
            entity.Property(e => e.WeightRestrictions).HasColumnName("weight_restrictions");
            entity.Property(e => e.ZoneLocation).HasColumnName("zone_location");

            entity.HasOne(d => d.ZoneLocationNavigation).WithMany(p => p.Attractions)
                .HasForeignKey(d => d.ZoneLocation)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_zone_location");
        });

        modelBuilder.Entity<Benefit>(entity =>
        {
            entity.HasKey(e => e.BenefitId).HasName("pk_benefit_id");

            entity.ToTable("benefits");

            entity.HasIndex(e => e.BenefitName, "benefits_benefit_name_key").IsUnique();

            entity.Property(e => e.BenefitId).HasColumnName("benefit_id");
            entity.Property(e => e.BenefitName)
                .HasMaxLength(128)
                .HasColumnName("benefit_name");
            entity.Property(e => e.ConditionForReceivingBenefit)
                .HasMaxLength(256)
                .HasColumnName("condition_for_receiving_benefit");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
        });

        modelBuilder.Entity<Bracelet>(entity =>
        {
            entity.HasKey(e => e.BraceletId).HasName("pk_bracelet_id");

            entity.ToTable("bracelets");

            entity.Property(e => e.BraceletId)
                .ValueGeneratedNever()
                .HasColumnName("bracelet_id");
            entity.Property(e => e.CashBalance).HasColumnName("cash_balance");
            entity.Property(e => e.LockerNumber).HasColumnName("locker_number");
            entity.Property(e => e.RemainingTime).HasColumnName("remaining_time");
        });

		modelBuilder.Entity<Checking>(entity =>
		{
			entity.HasKey(e => e.CheckingId).HasName("pk_checking_attractions2");

			entity.ToTable("checking");

			entity.Property(e => e.CheckingId)
				.HasDefaultValueSql("nextval('checking_attractions2_checking_id_seq'::regclass)")
				.HasColumnName("checking_id");
			entity.Property(e => e.AdmissionLaunch)
				.HasMaxLength(10)
				.HasDefaultValueSql("'да'::character varying")
				.HasColumnName("admission_launch");
			entity.Property(e => e.AttractionId).HasColumnName("attraction_id");
			entity.Property(e => e.CheckingDate)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("checking_date");
			entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
			entity.Property(e => e.Note).HasColumnName("note");

			entity.HasOne(d => d.Attraction).WithMany(p => p.Checkings)
				.HasForeignKey(d => d.AttractionId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("fk_attraction_id2");

			entity.HasOne(d => d.Employee).WithMany(p => p.Checkings)
				.HasForeignKey(d => d.EmployeeId)
				.OnDelete(DeleteBehavior.SetNull)
				.HasConstraintName("fk_employee_id2");
		});

		//modelBuilder.Entity<CheckingAttraction>(entity =>
  //      {
  //          entity.HasKey(e => new { e.CheckingDate, e.AttractionId }).HasName("pk_checking_attractions");

  //          entity.ToTable("checking_attractions");

  //          entity.Property(e => e.CheckingDate)
  //              .HasColumnName("checking_date")
  //              .HasColumnType("timestamp");
		//	entity.Property(e => e.AttractionId).HasColumnName("attraction_id");
  //          entity.Property(e => e.AdmissionLaunch)
  //              .HasMaxLength(10)
  //              .HasDefaultValueSql("'да'::character varying")
  //              .HasColumnName("admission_launch");
  //          entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
  //          entity.Property(e => e.Note).HasColumnName("note");

  //          entity.HasOne(d => d.Attraction).WithMany(p => p.CheckingAttractions)
  //              .HasForeignKey(d => d.AttractionId)
  //              .OnDelete(DeleteBehavior.Cascade)
  //              .HasConstraintName("fk_attraction_id");

  //          entity.HasOne(d => d.Employee).WithMany(p => p.CheckingAttractions)
  //              .HasForeignKey(d => d.EmployeeId)
  //              .OnDelete(DeleteBehavior.SetNull)
  //              .HasConstraintName("fk_employee_id");
  //      });

        modelBuilder.Entity<CurrentShift>(entity =>
        {
            entity.HasKey(e => e.CurrentShiftId).HasName("pk_current_shift_id");

            entity.ToTable("current_shift");

            entity.Property(e => e.CurrentShiftId).HasColumnName("current_shift_id");
            entity.Property(e => e.Employee).HasColumnName("employee");
            entity.Property(e => e.Shift).HasColumnName("shift");

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.CurrentShifts)
                .HasForeignKey(d => d.Employee)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_employee");

            entity.HasOne(d => d.ShiftNavigation).WithMany(p => p.CurrentShifts)
                .HasForeignKey(d => d.Shift)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_shift");
        });

        modelBuilder.Entity<CurrentWorkout>(entity =>
        {
            entity.HasKey(e => e.CurrentWorkoutId).HasName("pk_current_workout_id");

            entity.ToTable("current_workout");

            entity.Property(e => e.CurrentWorkoutId).HasColumnName("current_workout_id");
            entity.Property(e => e.Trainer).HasColumnName("trainer");
            entity.Property(e => e.WorkoutDate).HasColumnName("workout_date");

            entity.HasOne(d => d.TrainerNavigation).WithMany(p => p.CurrentWorkouts)
                .HasForeignKey(d => d.Trainer)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_trainer");

            entity.HasOne(d => d.WorkoutDateNavigation).WithMany(p => p.CurrentWorkouts)
                .HasForeignKey(d => d.WorkoutDate)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_workout_date");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("pk_event_id");

            entity.ToTable("events");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("date_time");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Duration)
                .HasDefaultValueSql("'02:00:00'::time without time zone")
                .HasColumnName("duration");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EventName)
                .HasMaxLength(64)
                .HasColumnName("event_name");

            entity.HasOne(d => d.Employee).WithMany(p => p.Events)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_employee_id");
        });

        modelBuilder.Entity<OperationsHistory>(entity =>
        {
            entity.HasKey(e => new { e.DateTimePurchase, e.BraceletId }).HasName("pk_operations_history");

            entity.ToTable("operations_history");

            entity.Property(e => e.DateTimePurchase)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time_purchase");
            entity.Property(e => e.BraceletId).HasColumnName("bracelet_id");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
            entity.Property(e => e.ServiceCount).HasColumnName("service_count");
            entity.Property(e => e.ServiceName).HasColumnName("service_name");

            entity.HasOne(d => d.Bracelet).WithMany(p => p.OperationsHistories)
                .HasForeignKey(d => d.BraceletId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_bracelet_id");

            entity.HasOne(d => d.DiscountPercentageNavigation).WithMany(p => p.OperationsHistories)
                .HasForeignKey(d => d.DiscountPercentage)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_discount_percentage");

            entity.HasOne(d => d.ServiceNameNavigation).WithMany(p => p.OperationsHistories)
                .HasForeignKey(d => d.ServiceName)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_service_name");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("pk_post_id");

            entity.ToTable("posts");

            entity.HasIndex(e => e.Post1, "posts_post_key").IsUnique();

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Post1)
                .HasMaxLength(64)
                .HasColumnName("post");
            entity.Property(e => e.SalaryAmount).HasColumnName("salary_amount");
        });

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Staff)
            .WithOne(s => s.PostNavigation)
            .HasForeignKey(s => s.Post)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Result>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("results");

            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
            entity.Property(e => e.Workday).HasColumnName("workday");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("pk_service_id");

            entity.ToTable("services");

            entity.HasIndex(e => e.ServiceName, "services_service_name_key").IsUnique();

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(128)
                .HasColumnName("service_name");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("pk_shift_id");

            entity.ToTable("shifts");

            entity.Property(e => e.ShiftId).HasColumnName("shift_id");
            entity.Property(e => e.ShiftDate)
                .HasColumnName("shift_date");
            entity.Property(e => e.WorkShedule).HasColumnName("work_shedule");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("pk_employee_id");

            entity.ToTable("staff");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.DateOfHiring)
                .HasColumnName("date_of_hiring")
                .HasColumnType("timestamp");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(64)
                .HasColumnName("employee_name");
            entity.Property(e => e.EmployeePatronymic)
                .HasMaxLength(64)
                .HasColumnName("employee_patronymic");
            entity.Property(e => e.EmployeeSurname)
                .HasMaxLength(64)
                .HasColumnName("employee_surname");
            entity.Property(e => e.Post).HasColumnName("post");
            entity.Property(e => e.Phone)
                .HasColumnName("phone");

            entity.HasOne(d => d.PostNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Post)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_post_id");
        });

        modelBuilder.Entity<Vedomost>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vedomost");

            entity.Property(e => e.BraceletId).HasColumnName("bracelet_id");
            entity.Property(e => e.DateTimePurchase)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time_purchase");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ServiceCount).HasColumnName("service_count");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(128)
                .HasColumnName("service_name");
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("pk_workout_id");

            entity.ToTable("workouts");

            entity.Property(e => e.WorkoutId).HasColumnName("workout_id");
            entity.Property(e => e.DateOfWorkout)
                .HasColumnName("date_of_workout");
            entity.Property(e => e.Duration)
                .HasColumnName("duration");
            entity.Property(e => e.WorkoutType)
                .HasMaxLength(64)
                .HasColumnName("workout_type");
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(e => e.ZoneId).HasName("pk_zone_id");

            entity.ToTable("zones");

            entity.HasIndex(e => e.ZoneName, "zones_zone_name_key").IsUnique();

            entity.Property(e => e.ZoneId).HasColumnName("zone_id");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(64)
                .HasColumnName("zone_name");

            entity.HasMany(e => e.Attractions)
                .WithOne(e => e.ZoneLocationNavigation)
                .HasForeignKey(e => e.ZoneLocation)
                .OnDelete(DeleteBehavior.Cascade);
        });

        //OnModelCreatingPartial(modelBuilder);

        //modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(255);
        builder.Property(u => u.LastName).HasMaxLength(255);
    }
}