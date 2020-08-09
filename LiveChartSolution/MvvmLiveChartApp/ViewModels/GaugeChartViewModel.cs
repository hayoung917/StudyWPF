using Caliburn.Micro;
using System;
using System.Threading;

namespace MvvmLiveChartApp.ViewModels
{
    public class GaugeChartViewModel : Conductor<object>
    {
        //값 사용하고 버림
        double angValue;
        public double AngValue
        {
            get => angValue;
            set
            {
                angValue = value;
                NotifyOfPropertyChange(() => AngValue);
            }
        }
        public Timer CustomTimer { get; set; }
        public GaugeChartViewModel()
        {
            CustomTimer = new Timer(TickCallBack);
            CustomTimer.Change(1000, 1000);
        }

        private void TickCallBack(object state)
        {
            AngValue = new Random().Next(20, 300);
        }
    }
}