

export class SetupChart {
    private chartData: any;
    private options: any;

    public RenderBarChart<T1, T2>(chartLable: string, chartText: string, seriesNames: T1, seriesData: T2): any {
         this.chartData = {
            labels: seriesNames,
            datasets: [
                {
                    label:chartLable,
                    backgroundColor: [
                        "#00b33c",
                        "#ffbb33",
                        "#FF0080",
                        "#590649",
                        "#DF013A"
                      ],
                    borderColor: '#1E88E5',
                    data: seriesData
                }

            ]

        }
        this.options = {
            title: {
                display: true,
                text: chartText,
                fontSize: 10
            },
            legend: {
                position: 'bottom'
            },
            responsive: false,
            maintainAspectRatio: false,
            barWidth: 1,
            scales: {
                yAxes: [{
                    ticks: {
                        stepSize: 5,
                        beginAtZero: true
                    }
                }]
            }
        };
        return this.chartData;
    }

    
    public RenderPieChart<T1, T2>(chartText: string="", seriesNames: T1, seriesData: T2): any {
    this.chartData = {
      labels: seriesNames,
      datasets: [
        {
          data: seriesData,
          backgroundColor: [
            "#00b33c",
            "#ffbb33",
            "#FF0080",
            "#590649",
            "#FF8000"
          ],
          borderColor: [
            "#FBFBEF"
          ]
        }]
    };

    this.options = {
      title: {
        display: true,
        text: chartText,
        fontSize: 10
      },
      legend: {
        position: 'bottom'
      },
      responsive: false,
      maintainAspectRatio: false,
    };
       return this.chartData;
   }
}