using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TweetSharp;

namespace TweetApp.Installers
{
    public class TwitterServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().Pick()
                                .If(Component.IsInSameNamespaceAs<TwitterService>())
                                .LifestyleTransient()
                                .WithService.DefaultInterfaces());
        } 
    }
}