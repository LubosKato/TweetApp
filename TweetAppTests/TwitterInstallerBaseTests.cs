using Castle.Windsor;
using NUnit.Framework;
using TweetApp.Installers;

namespace TweetAppTests
{
    [TestFixture]
    public class TwitterInstallerBaseTests
    {
        private readonly IWindsorContainer containerWithControllers;

        public TwitterInstallerBaseTests()
        {
            containerWithControllers = new WindsorContainer()
                .Install(new ControllersInstaller());
        }
    }
}