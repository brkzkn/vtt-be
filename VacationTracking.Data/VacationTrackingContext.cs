using Microsoft.EntityFrameworkCore;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data
{
    public class VacationTrackingContext : DbContext
    {
        public VacationTrackingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayTeam> HolidayTeam { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vacation> Vacations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyId)
                    .HasName("companies_pkey");

                entity.ToTable("companies");

                entity.HasIndex(e => e.CreatedBy)
                    .HasName("fki_fk_companies_createdby_users_id");

                entity.HasIndex(e => e.UpdatedBy)
                    .HasName("fki_fk_companies_updatedby_users_id");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address1)
                    .HasColumnName("address_1")
                    .HasMaxLength(100);

                entity.Property(e => e.Address2)
                    .HasColumnName("address_2")
                    .HasMaxLength(100);

                entity.Property(e => e.CompanyName)
                    .HasColumnName("company_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Logo).HasColumnName("logo");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            
            modelBuilder.Entity<HolidayTeam>(entity =>
            {
                entity.HasKey(e => new { e.HolidayId, e.TeamId })
                    .HasName("holiday_team_pkey");

                entity.ToTable("holiday_team");

                entity.Property(e => e.HolidayId).HasColumnName("holiday_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.HasOne(d => d.Holiday)
                    .WithMany(p => p.HolidayTeams)
                    .HasForeignKey(d => d.HolidayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_holidayteam_holidayid_holidays_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.HolidayTeams)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_holidayteam_teamid_teams_id");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.HasKey(e => e.HolidayId)
                    .HasName("holidays_pkey");

                entity.ToTable("holidays");

                entity.Property(e => e.HolidayId)
                    .HasColumnName("holiday_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.IsFullDay).HasColumnName("is_full_day");

                entity.Property(e => e.HolidayName)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Holidays)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_holidays_companies_companyid");
            });

            modelBuilder.Entity<LeaveType>(entity =>
            {
                entity.HasKey(e => e.LeaveTypeId)
                    .HasName("leave_types_pkey");

                entity.ToTable("leave_types");

                entity.Property(e => e.LeaveTypeId)
                    .HasColumnName("leave_type_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ColorCode)
                    .HasColumnName("color_code")
                    .HasMaxLength(10);

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.DefaultDaysPerYear).HasColumnName("default_days_per_year");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsAllowNegativeBalance).HasColumnName("is_allow_negative_balance");

                entity.Property(e => e.IsApproverRequired).HasColumnName("is_approver_required");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.IsHalfDaysActivated).HasColumnName("is_half_days_activated");

                entity.Property(e => e.IsHideLeaveTypeName).HasColumnName("is_hide_leave_type_name");

                entity.Property(e => e.IsReasonRequired).HasColumnName("is_reason_required");

                entity.Property(e => e.IsUnlimited).HasColumnName("is_unlimited");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnName("type_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.LeaveTypes)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_leavetype_companyid_companies_id");
            });


            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.TeamId })
                    .HasName("team_members_pkey");

                entity.ToTable("team_members");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.Property(e => e.IsApprover).HasColumnName("is_approver");

                entity.Property(e => e.IsMember).HasColumnName("is_member");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamMembers)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teammember_teamid_teams_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teammember_userid_users_id");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TeamId)
                    .HasName("teams_pkey");

                entity.ToTable("teams");

                entity.HasIndex(e => e.CreatedBy)
                    .HasName("fki_fk_teams_createdby_users_id");

                entity.HasIndex(e => e.UpdatedBy)
                    .HasName("fki_fk_teams_updatedby_users_id");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.TeamName)
                    .HasColumnName("team_name")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_company_id");
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("users_pkey");

                entity.ToTable("users");

                entity.HasIndex(e => e.CreatedBy)
                    .HasName("fki_fk_users_createdby_user_id");

                entity.HasIndex(e => e.UpdatedBy)
                    .HasName("fki_fk_users_updatedby_user_id");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountType)
                    .HasColumnName("account_type")
                    .HasMaxLength(50);

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(200);

                entity.Property(e => e.EmployeeSince)
                    .HasColumnName("employee_since")
                    .HasColumnType("date");

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            });

            modelBuilder.Entity<Vacation>(entity =>
            {
                entity.HasKey(e => e.VacationId)
                    .HasName("vacations_pkey");

                entity.ToTable("vacations");

                entity.Property(e => e.VacationId)
                    .HasColumnName("vacation_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApproverId).HasColumnName("approver_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.IsHalfDay).HasColumnName("is_half_day");

                entity.Property(e => e.LeaveTypeId).HasColumnName("leave_type_id");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(200);

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasMaxLength(200);

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.VacationStatus)
                    .IsRequired()
                    .HasColumnName("vacation_status")
                    .HasMaxLength(20);

                entity.HasOne(d => d.Approver)
                    .WithMany(p => p.VacationsApprover)
                    .HasForeignKey(d => d.ApproverId)
                    .HasConstraintName("fk_vacations_approverid_users_id");

                
                entity.HasOne(d => d.LeaveType)
                    .WithMany(p => p.Vacations)
                    .HasForeignKey(d => d.LeaveTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_vacations_leavetypeid_leavetypes_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VacationsUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_vacations_userid_users_id");
            });



        }


    }
}
