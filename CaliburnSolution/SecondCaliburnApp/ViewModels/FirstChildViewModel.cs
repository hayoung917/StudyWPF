﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SecondCaliburnApp.ViewModels
{
    public class FirstChildViewModel : Screen    //상속이 없으면 notifypropertychange못씀 
    {
        public void FirstClicked()
        {
            MessageBox.Show("Hello World :)");
        }
    }
}
