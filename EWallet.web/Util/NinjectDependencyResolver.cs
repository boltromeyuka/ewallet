using EWallet.bl;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EWallet.web.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<ICategoryService>().To<CategoryService>();
            kernel.Bind<ICurrencyService>().To<CurrencyService>();
            kernel.Bind<IOperationService>().To<OperationService>();
            kernel.Bind<IDynamicsService>().To<DynamicsService>();
        }
    }
}