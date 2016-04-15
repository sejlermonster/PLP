using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Graphikos.Scheme;
using Graphikos.ViewModels;

namespace Graphikos
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
            base.OnStartup(sender, e);
        }

        protected override void Configure()
        {
            _container.Instance<IWindowManager>(new WindowManager());
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.PerRequest<ShellViewModel>();
            _container.PerRequest<ISchemeHandler, SchemeHandler>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
