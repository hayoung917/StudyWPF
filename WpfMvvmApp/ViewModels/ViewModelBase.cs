using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged //값이 바뀌면 알려주는 것
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            //이렇게 바꿔쓸 수 있음
            //if (PropertyChanged != null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
        }
    }
}
