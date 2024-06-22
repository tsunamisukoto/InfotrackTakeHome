using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TakeHomeAssignmentEntities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<SettlementBooking> Settlements { get; set; }
}

public class SettlementBooking
{

}