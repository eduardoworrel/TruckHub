import Grid from '@mui/material/Unstable_Grid2';
import Typography from '@mui/material/Typography';
import { CircularProgress } from '@mui/material';
import { useEffect, useState } from 'react';
import { DashboardInfoResponse } from 'src/interfaces/truck';

import { _tasks, _posts, _timeline } from 'src/_mock';
import { DashboardContent } from 'src/layouts/dashboard';

import { AnalyticsCurrentVisits } from '../analytics-current-visits';
import { AnalyticsWebsiteVisits } from '../analytics-website-visits';
import { AnalyticsWidgetSummary } from '../analytics-widget-summary';

export function OverviewAnalyticsView() {
  const [data, setData] = useState<DashboardInfoResponse | undefined>();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const handle = async () => {
      const request = await fetch('http://localhost:7006/api/trucks/dashboard');
      const result = await request.json();
      setLoading(false);
      setData(result);
    };
    if (loading) {
      
      handle();
    }
  }, [loading]);

  const now = new Date();
  const times: string[] = [];
  for (let i = 0; i < 6; i += 1) {
    const date = new Date(now);
    date.setMinutes(0);
    date.setHours(date.getHours() - i);
    times.unshift(date.toTimeString().slice(0, 5));
  }

  const hourlyData: { [key: string]: number[] } = {};

  (data?.detailedHourCounts ?? []).forEach((d) => {
    if (!hourlyData[d.modelName]) {
      hourlyData[d.modelName] = Array(6).fill(0);
    }
    const timeIndex = times.indexOf(d.time);
    if (timeIndex !== -1) {
      hourlyData[d.modelName][timeIndex] = d.count;
    }
  });

  const hourlySeries = Object.keys(hourlyData).map((team) => ({
    name: team,
    data: hourlyData[team],
  }));

  return (
    <DashboardContent maxWidth="xl">
      <Typography variant="h4" sx={{ mb: { xs: 3, md: 5 } }}>
      Olá, Bem vindo de volta {loading ? <CircularProgress size={22} /> : "👋"}
      </Typography>
      
      
      <Grid container spacing={3}>
        
            <Grid xs={12} sm={12} md={12}>
              <AnalyticsWidgetSummary
                title="Total de caminhões"
                percent={2.6}
                total={data?.total ?? 0}
                icon={<img alt="icon" src="/assets/icons/glass/ic-glass-bag.svg" />}
                chart={{
                  categories: data?.hourCounts.map((e) => e.time).reverse() ?? [],
                  series: data?.hourCounts.map((e) => e.count).reverse() ?? [],
                }}
              />
            </Grid>
            <Grid xs={12} md={6} lg={8}>
              <AnalyticsWebsiteVisits
                title="Modelos adicionados nas últimas 6 horas"
                chart={{
                  categories: times,
                  series: hourlySeries ?? [],
                }}
              />
            </Grid>
            <Grid xs={12} md={6} lg={4}>
              <AnalyticsCurrentVisits
                title="Distribuição de Planta"
                chart={{
                  series:
                    data?.plantCounts.map((e) => ({ label: e.country, value: e.count })) ?? [],
                }}
              />
            </Grid>
       

      </Grid>
    </DashboardContent>
  );
}
