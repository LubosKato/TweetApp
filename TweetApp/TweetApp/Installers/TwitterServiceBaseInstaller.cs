using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TweetApp.TweetService;

namespace TweetApp.Installers
{
    public class TwitterServiceBaseInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().Pick()
                                .If(Component.IsInSameNamespaceAs<TwitterServiceBase>())
                                .LifestyleTransient()
                                .WithService.DefaultInterfaces());
        }
    }
}