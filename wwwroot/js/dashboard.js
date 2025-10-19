async function fetchJSON(url) {
    const res = await fetch(url);
    return await res.json();
  }
  
  const metricEl = document.getElementById("metricSelect");
  const providerEl = document.getElementById("providerSelect");
  const refreshBtn = document.getElementById("refreshBtn");
  
  let trendChart, summaryChart;
  
  async function loadTrend() {
    const metric = metricEl.value;
    const provider = providerEl.value;
  
    const data = await fetchJSON(`/api/series?metric=${encodeURIComponent(metric)}&provider=${encodeURIComponent(provider)}`);
    const labels = data.map(d => new Date(d.date).toLocaleDateString());
    const values = data.map(d => d.value);
  
    if (trendChart) trendChart.destroy();
    trendChart = new Chart(document.getElementById('trendChart'), {
      type: 'line',
      data: { labels, datasets: [{ label: `${provider} ${metric}`, data: values, tension: 0.3 }] },
      options: { responsive: true, maintainAspectRatio: false, scales: { y: { beginAtZero: false } } }
    });
  }
  
  async function loadSummary() {
    const metric = metricEl.value;
    const data = await fetchJSON(`/api/summary?metric=${encodeURIComponent(metric)}`);
    const labels = data.map(d => d.provider);
    const values = data.map(d => d.total);
  
    if (summaryChart) summaryChart.destroy();
    summaryChart = new Chart(document.getElementById('summaryChart'), {
      type: 'doughnut',
      data: { labels, datasets: [{ label: `${metric} total`, data: values }] },
      options: { responsive: true, maintainAspectRatio: false }
    });
  }
  
  async function refreshAll() {
    await loadTrend();
    await loadSummary();
  }
  
  refreshBtn.addEventListener('click', refreshAll);
  window.addEventListener('DOMContentLoaded', refreshAll);
  