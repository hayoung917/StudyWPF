﻿using Caliburn.Micro;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThirdCaliburnApp.Helpers;
using ThirdCaliburnApp.ViewModels;

namespace ThirdCaliburnApp
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container;
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();
            //디자인패턴 singleton 여러곳에서 사용해야 할 경우 하나만 만들어서 사용하겠다.
            container.Singleton<IWindowManager, WindowManager>();
            container.RegisterInstance(typeof(IDialogService),null,new DialogService(dialogTypeLocator: new DialogTypeLocator()));
            container.PerRequest<MainViewModel>();
        }
        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
