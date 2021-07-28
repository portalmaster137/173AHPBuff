using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _173AHPBuff
{
    using System;
    using Exiled;
    using System.Collections.Generic;
    using Exiled.API.Features;
    using MEC;
    using Player = Exiled.Events.Handlers.Player;
    using Server = Exiled.Events.Handlers.Server;
    public class Main : Plugin<Config>
    {
        public override string Name { get; } = "173 AHP Mod";
        public override string Author { get; } = "Porta";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 0, 10);
        public override string Prefix { get; } = "173 AHP Mod";

        public EventHandlers eventHandler;

        public override void OnEnabled()
        {
            eventHandler = new EventHandlers(this);
            if (Config.IsEnabled)
            {
                Player.ChangedRole += eventHandler.Changed;
                Player.Hurting += eventHandler.Hurt;
                Server.RoundStarted += eventHandler.Start;
                Server.EndingRound += eventHandler.End;
            }
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            if (Config.IsEnabled)
            {   
                Player.ChangedRole -= eventHandler.Changed;
                Player.Hurting -= eventHandler.Hurt;
                Server.RoundStarted -= eventHandler.Start;
                Server.EndingRound -= eventHandler.End;
            }
        }
    }
}
