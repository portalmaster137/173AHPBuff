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
            this.scp173 = null;
            this.runningCo = new CoroutineHandle();
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



        internal void Hurt(HurtingEventArgs ev)
        {
            if(ev.Target == scp173)
            {
                shotCooldown += 1;
                //Log.Debug("Shot cooldown is now " + shotCooldown);
                Timing.CallDelayed(plugin.Config.DelayTime, () =>
                {
                    shotCooldown -= 1;
                    //Log.Debug("Shot cooldown is now " + shotCooldown);
                });
            }
            if (plugin.Config.AllDmgToAHP && ev.Target.Role == RoleType.Scp173 && ev.Target.ArtificialHealth > 0)
            {
                ev.IsAllowed = false;
                scp173.ArtificialHealth -= ev.Amount;
            } else
            {
                ev.IsAllowed = true;
            }
        }

   
        internal void End(EndingRoundEventArgs ev)
        {
            Timing.KillCoroutines(runningCo);
            
        }

        public IEnumerator<float> ShotCoRoutine()
        {
            while (true)
            {
                //Log.Debug("Called");
                if (shotCooldown <= 0 && scp173 != null)
                {
                    //Log.Debug("Shot cooldown is 0");

                    for (int i = 0; i < plugin.Config.HealthToHealPerTick; i++)
                    {
                        if (scp173.ArtificialHealth < plugin.Config.MaxAHP)
                        {
                            scp173.ArtificialHealth += 1;
                        }
                    }
                }
                yield return Timing.WaitForSeconds(plugin.Config.TimeToWaitPerTick);
            }
        }

        internal void Start()
        {
            runningCo = Timing.RunCoroutine(ShotCoRoutine());
            //Log.Debug("Coroutine Started");
        }
    }
}