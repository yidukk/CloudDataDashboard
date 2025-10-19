using Microsoft.EntityFrameworkCore;
using CloudDashboard.Models;

namespace CloudDashboard.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<DataRecord> DataRecords => Set<DataRecord>();
}
