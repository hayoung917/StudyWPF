using Caliburn.Micro;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLiveChartApp.ViewModels
{
    
    public class LineChartViewModel : Conductor<object>
    {
        //public ChartValues<double> LineValues { get; set; }
        ChartValues<double> lineValues;
        public ChartValues<double> LineValues
        {
            get => lineValues;
            set
            {
                lineValues = value;
                NotifyOfPropertyChange(() => LineValues);
            }
        }

        public LineChartViewModel()
        {
            IntializeChartData();
        }

        private void IntializeChartData()
        {
            LineValues = new ChartValues<double> { 3, 5, 6, 7, 12.4, 0, 7, 9, 4.5 };
        }
    }
}
