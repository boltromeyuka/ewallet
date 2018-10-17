using EWallet.data;
using Ninject.Modules;

namespace EWallet.bl
{
    /// <summary>
    /// Module Ninject for kernel in 
    /// view layer
    /// </summary>
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionString);            
        }
    }
}
