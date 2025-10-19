using CloudDashboard.Models;

namespace CloudDashboard.Data;

public static class Seed
{
    static readonly string[] Providers = ["Azure", "AWS", "GCP"];
    static readonly string[] Metrics   = ["Cost", "CPU", "Storage", "Network"];

    public static async Task EnsureSeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();
        if (db.DataRecords.Any()) return;

        var rand = new Random(123);
        var start = DateTime.UtcNow.Date.AddDays(-29); // last 30 days

        foreach (var day in Enumerable.Range(0, 30))
        {
            var date = start.AddDays(day);
            foreach (var p in Providers)
            foreach (var m in Metrics)
            {
                var baseVal = m switch
                {
                    "Cost"    => rand.Next(80, 200),
                    "CPU"     => rand.Next(20, 90),
                    "Storage" => rand.Next(200, 800),
                    _         => rand.Next(100, 500) // Network
                };
                db.DataRecords.Add(new DataRecord
                {
                    Provider = p,
                    Metric   = m,
                    Value    = baseVal + rand.NextDouble(),
                    Date     = date
                });
            }
        }
        await db.SaveChangesAsync();
    }
}