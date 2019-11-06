using System;
using System.Collections.Generic;
using System.Linq;
using MEC;

using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.EventHandlers;
using Smod2.Events;

namespace CoarseTP {
    class TeleportHandler : IEventHandlerSCP914Activate {
        private readonly CTPPlugin plugin;

        // Add plugin instance to the handler.
        public TeleportHandler(CTPPlugin plugin) => this.plugin = plugin;

        // Handle the event.   
        public void OnSCP914Activate(SCP914ActivateEvent ev) {
            if (ev.KnobSetting == KnobSetting.COARSE)
            {
                List<Player> players = GetPlayersFromInputs(ev.Inputs);
                
                if (players.Count > 0)
                {
                    Vector randomPos = GetRandomTeleportPos();

                    // Teleport players to the position vector.
                    foreach (Player player in players)
                    {
                        // So why do I teleport someone using a coroutine? I don't know, but the normal way didn't work.
                        // It might have interfered with the way SCP:SL does the teleport to the outtake.
                        Timing.RunCoroutine(_Teleport(player, randomPos));
                        

                        if (!plugin.GetConfigBool("ctp_damage_disable"))
                        {
                            AlterHealth(player);
                        }
                    }
                }
            }
        }
        
        // This method changes the player's health after using SCP-914.
        private void AlterHealth(Player player) {
            if ((player.TeamRole.Team != Smod2.API.Team.TUTORIAL) && (player.TeamRole.Team != Smod2.API.Team.SCP))
            {
                player.SetHealth((int) (player.GetHealth() - (player.TeamRole.MaxHP * plugin.GetConfigFloat("ctp_health_multiplier"))), DamageType.RAGDOLLLESS);
            }
        }

        private List<Player> GetPlayersFromInputs(object[] inputs) {
            List<Player> players = new List<Player>();

            foreach (UnityEngine.Collider collider in inputs)
            {
                // This signifies that the collider is a player.
                // NOTE!
                // There is something really sketchy here with the GameObject and stuff. I used the UnityEngine.CoreModule.dll
                // from SCP:SL and the UnityEngine.dll from Unity itself, it was the only way I got this to work.
                if (collider.name == "Player")
                {
                    players.Add(new ServerMod2.API.SmodPlayer(collider.gameObject));
                }
            }

            return players;
        }

        // This function is horrible, I'll change it when I need to.
        private Vector GetRandomTeleportPos() {
            Random rand = new Random();

            if (rand.NextDouble() < plugin.GetConfigFloat("ctp_hcz_chance"))
            {
                return plugin.allowedPositions.ElementAt(plugin.posZones.FindIndex(zone => zone.Equals(ZoneType.HCZ)));
            } else
            {
                return plugin.allowedPositions.ElementAt(plugin.posZones.FindIndex(zone => zone.Equals(ZoneType.LCZ)));
            }
        }

        private IEnumerator<float> _Teleport(Player player, Vector pos) {
            yield return Timing.WaitForSeconds(0.1f);

            player.Teleport(pos, true);
        }
    }
}
