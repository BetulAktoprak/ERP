import { AfterViewInit, Component } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
declare const Chart: any;

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements AfterViewInit {
  ngAfterViewInit(): void {
    this.showChart(chart1Config, 'myChart1');
    this.showChart(chart2Config, 'myChart2');
    this.showChart(chart3Config, 'myChart3');
    this.showChart(chart4Config, 'myChart4');
    this.showChart(chart5Config, 'myChart5');
  }

  showChart(config: any, name: string) {
    const ctx = document.getElementById(name);

    new Chart(ctx, config);
  }
}

function transparentize(color: string, opacity: number): string {
  const alpha = Math.round(opacity * 255);
  return color.replace('rgb', 'rgba').replace(')', `, ${alpha / 255})`);
}

// Rastgele sayılar üreten fonksiyon
function numbers({ count, min, max }: { count: number, min: number, max: number }): number[] {
  const data = [];
  for (let i = 0; i < count; i++) {
    data.push(Math.floor(Math.random() * (max - min + 1)) + min);
  }
  return data;
}

function months({ count }: { count: number }): string[] {
  const monthNames = [
    'January', 'February', 'March', 'April', 'May', 'June', 'July',
    'August', 'September', 'October', 'November', 'December'
  ];

  return monthNames.slice(0, count);
}

const CHART_COLORS = {
  red: 'rgb(255, 99, 132)',
  blue: "blue",
  orange: "orange",
  yellow: "yellow",
  green: "green"
};

const chart1Config = {
  type: 'bar',
  data: {
    labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
    datasets: [{
      label: '# of Votes',
      data: [12, 19, 3, 5, 2, 3],
      borderWidth: 1
    }]
  },
  options: {
    scales: {
      y: {
        beginAtZero: true
      }
    }
  }
}

const chart2Config = {
  type: 'line',
  data: {
    labels: ['Day 1', 'Day 2', 'Day 3', 'Day 4', 'Day 5', 'Day 6'],
    datasets: [
      {
        label: 'Dataset',
        data: numbers({ count: 6, min: -100, max: 100 }),
        borderColor: CHART_COLORS.red,
        backgroundColor: transparentize(CHART_COLORS.red, 0.5),
        pointStyle: 'circle',
        pointRadius: 10,
        pointHoverRadius: 15
      }
    ]
  },
  options: {
    responsive: true,
    plugins: {
      title: {
        display: true,
        text: (ctx: any) => 'Point Style: ' + ctx.chart.data.datasets[0].pointStyle,
      }
    }
  }
}

const NUMBER_CFG = {count: 7, min: 0, max: 100};
const labels = months({count: 7});
const dataFirstSkip = numbers(NUMBER_CFG);
const dataMiddleSkip = numbers(NUMBER_CFG);
const dataLastSkip = numbers(NUMBER_CFG);

const chart3Config = {
  type: 'radar',
  data: {
    labels: labels,
    datasets: [
      {
        label: 'Skip first dataset',
        data: dataFirstSkip,
        borderColor: CHART_COLORS.red,
        backgroundColor: transparentize(CHART_COLORS.red, 0.5),
      },
      {
        label: 'Skip mid dataset',
        data: dataMiddleSkip,
        borderColor: CHART_COLORS.blue,
        backgroundColor: transparentize(CHART_COLORS.blue, 0.5),
      },
      {
        label: 'Skip last dataset',
        data: dataLastSkip,
        borderColor: CHART_COLORS.green,
        backgroundColor: transparentize(CHART_COLORS.green, 0.5),
      }
    ]
  },
  options: {
    responsive: true,
    plugins: {
      title: {
        display: true,
        text: 'Chart.js Radar Skip Points Chart'
      }
    }
  },
}

const chart4Config =  {
  type: 'pie',
  data: {
    labels: ['Red', 'Orange', 'Yellow', 'Green', 'Blue'],
    datasets: [
      {
        label: 'Dataset 1',
        data: numbers(NUMBER_CFG),
        backgroundColor: Object.values(CHART_COLORS),
      }
    ]
  },
  options: {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
      },
      title: {
        display: true,
        text: 'Chart.js Pie Chart'
      }
    }
  },
}

const chart5Config = {
  type: 'polarArea',
  data: {
    labels: ['Red', 'Orange', 'Yellow', 'Green', 'Blue'],
    datasets: [
      {
        label: 'Dataset 1',
        data: numbers(NUMBER_CFG),
        backgroundColor: [
          transparentize(CHART_COLORS.red, 0.5),
          transparentize(CHART_COLORS.orange, 0.5),
          transparentize(CHART_COLORS.yellow, 0.5),
          transparentize(CHART_COLORS.green, 0.5),
          transparentize(CHART_COLORS.blue, 0.5),
        ]
      }
    ]
  },
  options: {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
      },
      title: {
        display: true,
        text: 'Chart.js Polar Area Chart'
      }
    }
  },
}