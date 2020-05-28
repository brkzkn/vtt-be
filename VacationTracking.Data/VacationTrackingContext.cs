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
			modelBuilder.Entity<Holiday>(entity =>
			{
				entity.HasOne(h => h.Company)
					  .WithMany(c => c.Holidays)
					  .HasForeignKey(h => h.CompanyId)
					  .OnDelete(DeleteBehavior.ClientSetNull);
			});

			modelBuilder.Entity<HolidayTeam>(entity =>
			{
				entity.HasOne(ht => ht.Holiday)
					.WithMany(h => h.HolidayTeams)
					.HasForeignKey(d => d.HolidayId)
					.OnDelete(DeleteBehavior.ClientSetNull);

				entity.HasOne(ht => ht.Team)
					.WithMany(t => t.HolidayTeams)
					.HasForeignKey(d => d.TeamId)
					.OnDelete(DeleteBehavior.ClientSetNull);
			});

			modelBuilder.Entity<LeaveType>(entity =>
			{
				entity.HasOne(lt => lt.Company)
					.WithMany(c => c.LeaveTypes)
					.HasForeignKey(d => d.CompanyId)
					.OnDelete(DeleteBehavior.ClientSetNull);
			});

			modelBuilder.Entity<Team>(entity =>
			{
				entity.HasOne(t => t.Company)
					.WithMany(c => c.Teams)
					.HasForeignKey(d => d.CompanyId)
					.OnDelete(DeleteBehavior.ClientSetNull);
			});

			modelBuilder.Entity<TeamMember>(entity =>
			{
				entity.HasOne(tm => tm.Team)
					.WithMany(t => t.TeamMembers)
					.HasForeignKey(d => d.TeamId)
					.OnDelete(DeleteBehavior.ClientSetNull);
			});

			modelBuilder.Entity<Vacation>(entity =>
			{
				entity.HasOne(v => v.Approver)
					.WithMany(u => u.VacationsApprover)
					.HasForeignKey(d => d.ApproverId);

				entity.HasOne(v => v.LeaveType)
					.WithMany(lt => lt.Vacations)
					.HasForeignKey(v => v.LeaveTypeId)
					.OnDelete(DeleteBehavior.ClientSetNull);

				entity.HasOne(v => v.User)
					.WithMany(u => u.VacationsUser)
					.HasForeignKey(v => v.UserId)
					.OnDelete(DeleteBehavior.ClientSetNull);
			});


		}


	}
}
