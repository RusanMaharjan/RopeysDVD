using Coursework.Areas.Identity.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Actor> Actors { get; set; }
    public DbSet<DVDCategory> DvdCategories { get; set; }
    public DbSet<Studio> Studios { get; set; }
    public DbSet<Producer> Producers { get; set; }
    public DbSet<LoanType> LoanTypes { get; set; }
    public DbSet<MembershipCategory> MembershipCategories { get; set; }
    public DbSet<DVDTitle> DvdTitles { get; set; }
    public DbSet<DVDCopy> DvdCopies { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<CastMember> CastMembers { get; set; }
}