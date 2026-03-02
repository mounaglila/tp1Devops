using Microsoft.EntityFrameworkCore;

namespace Incidentapi_mounaa.Models
{
    public class IncidentsDbContext : DbContext
    {
        public IncidentsDbContext(DbContextOptions<IncidentsDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Incident> Incidents { get; set; }
    }
}

