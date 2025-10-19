namespace CloudDashboard.Models;

public class DataRecord
{
    public int Id { get; set; }
    public string Provider { get; set; } = "Azure";   // Azure/AWS/GCP
    public string Metric { get; set; } = "Cost";      // Cost/CPU/Storage/Network
    public double Value { get; set; }
    public DateTime Date { get; set; }
}
