using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;

namespace _173AHPBuff
{
    public class EventHandlers
    {
        private readonly Main plugin;
        private bool isSpawning;
        private Player scp173;
        private Random random;
        private int shotCooldown;

        public CoroutineHandle runningCo;

        public EventHandlers(Main plugin) 
        {
            this.random = new Random();
            this.isSpawning = false;
            this.plugin = plugin;
            shotCooldown = 0;
            
        }

        internal void Changed(ChangedRoleEventArgs ev)
        {
            if (ev.Player.Role == RoleType.Scp173)
            {
                this.scp173 = ev.Player;
                scp173.ArtificialHealthDecay = 0;
                scp173.ArtificialHealth = plugin.Config.MaxAHP;
                Log.Debug(scp173.Nickname + " Is 173");
            }
        }

        internal void Shot(ShotEventArgs ev)
        {
            if (ev.Target == scp173.GameObject)
            {
                shotCooldown += 1;
                Log.Debug("Shot cooldown is now " + shotCooldown);
                Timing.CallDelayed(plugin.Config.DelayTime, () =>
                {
                    shotCooldown -= 1;
                    Log.Debug("Shot cooldown is now " + shotCooldown);
                });
            }
        }

        internal void End(EndingRoundEventArgs ev)
        {
            Timing.KillCoroutines(shotCooldown);
            
        }

        public IEnumerator<float> shotCoRoutine()
        {
            while (true)
            {
                Log.Debug("Called");
                if (shotCooldown <= 0)
                {
                    Log.Debug("Shot cooldown is 0");
                    if (scp173.ArtificialHealth < plugin.Config.MaxAHP)
                    {
                        scp173.ArtificialHealth += 1;
                    }
                    if (scp173.ArtificialHealth < plugin.Config.MaxAHP)
                    {
                        scp173.ArtificialHealth += 1;
                    }
                }
                yield return Timing.WaitForSeconds(0.2f);
            }
        }

        internal void Start()
        {
            Timing.RunCoroutine(shotCoRoutine());
            Log.Debug("Coroutine Started");
        }
    }
}