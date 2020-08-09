using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLiveChartApp.ViewModels
{
    internal class MainViewModel : Conductor<object>
    {
        public void LoadLineChart()
        {
            ActivateItem(new LineChartViewModel());
        }

        public void LoadGaugeChart()
        {
            ActivateItem(new GaugeChartViewModel());
        }
        public void ExitProgram()
        {
            //0은 오류 없이 끝내는것 나머지는 오류로 끝남
            Environment.Exit(0);
        }

    }
}
