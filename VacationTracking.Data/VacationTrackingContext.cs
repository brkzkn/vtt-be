using Microsoft.EntityFrameworkCore;
using VacationTracking.Domain.Models;

namespace VacationTracking.Data
{
    public class VacationTrackingContext : DbContext
    {
        public VacationTrackingContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanySetting> CompanySettings { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<HolidayTeam> HolidayTeams { get; set; }
        public virtual DbSet<LeaveType> LeaveTypes { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamMember> TeamMembers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<Vacation> Vacations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanySetting>(entity =>
            {
                entity.HasKey(e => new { e.CompanyId, e.SettingId });

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanySettings)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanySetting_Company");

                entity.HasOne(d => d.Setting)
                    .WithMany(p => p.CompanySettings)
                    .HasForeignKey(d => d.SettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanySetting_Setting");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Holidays)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Holiday_Company");
            });

            modelBuilder.Entity<HolidayTeam>(entity =>
            {
                entity.HasKey(e => new { e.HolidayId, e.TeamId });

                entity.HasOne(d => d.Holiday)
                    .WithMany(p => p.HolidayTeam)
                    .HasForeignKey(d => d.HolidayId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HolidayTeam_Holiday");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.HolidaysTeam)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HolidayTeam_Team");
            });

            modelBuilder.Entity<LeaveType>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.LeaveTypes)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeaveType_Company");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Team_Company");
            });

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(tm => new { tm.TeamId, tm.UserId });

                entity.HasOne(tm => tm.Team)
                    .WithMany(t => t.TeamMembers)
                    .HasForeignKey(tm => tm.TeamId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TeamMember_Team");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamMember_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.AccountType)
                      .HasConversion<string>();

                entity.Property(u => u.Status)
                      .HasConversion<string>();

                entity.HasOne(u => u.Company)
                    .WithMany(c => c.Users)
                    .HasForeignKey(u => u.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Company");
            });


            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(u => u.SettingType)
                      .HasConversion<string>();
            });

            modelBuilder.Entity<UserSetting>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.SettingId });

                entity.HasOne(d => d.Setting)
                    .WithMany(p => p.UserSettings)
                    .HasForeignKey(d => d.SettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSetting_Setting");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSettings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSetting_User");
            });

            modelBuilder.Entity<Vacation>(entity =>
            {

                entity.HasOne(d => d.Approver)
                    .WithMany(p => p.VacationApprovers)
                    .HasForeignKey(d => d.ApproverId)
                    .HasConstraintName("FK_Vacation_User1");

                entity.HasOne(d => d.LeaveType)
                    .WithMany(p => p.Vacations)
                    .HasForeignKey(d => d.LeaveTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacation_LeaveType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Vacations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacation_User");
            });
        }
    }
}
