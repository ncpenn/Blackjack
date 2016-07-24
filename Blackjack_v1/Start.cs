using System.Runtime.CompilerServices;
using Blackjack.Actors;
using Blackjack.Interfaces;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

[assembly: InternalsVisibleTo("BlackjackTests")]
namespace Blackjack
{
   
    public static class Start
    {
        public static void Go()
        {
            // Registering
            var container = new WindsorContainer();
            container.Register(Component.For<IPlayer>().ImplementedBy<Player>());
            // Resolving
            //var logger = container.Resolve<IPlayer>();
        }
    }
}
