using CloudDashboard.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core (SQLite)
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages();

var app = builder.Build();

// seed on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await Seed.EnsureSeedAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

// minimal JSON API for charts
app.MapGet("/api/series", async (AppDbContext db, string metric = "Cost", string provider = "Azure") =>
{
    var rows = await db.DataRecords
        .Where(r => r.Metric == metric && r.Provider == provider)
        .OrderBy(r => r.Date)
        .Select(r => new { date = r.Date, value = r.Value })
        .ToListAsync();

    return Results.Ok(rows);
});

app.MapGet("/api/summary", async (AppDbContext db, string metric = "Cost") =>
{
    var summary = await db.DataRecords
        .Where(r => r.Metric == metric)
        .GroupBy(r => r.Provider)
        .Select(g => new { provider = g.Key, total = g.Sum(x => x.Value) })
        .OrderByDescending(x => x.total)
        .ToListAsync();

    return Results.Ok(summary);
});

app.Run();
