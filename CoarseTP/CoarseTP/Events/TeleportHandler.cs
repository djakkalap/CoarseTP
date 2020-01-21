using System;
using System.Collections.Generic;
using System.Linq;
using MEC;

using EXILED;

namespace CoarseTP {
    public class TeleportHandler {
        private readonly CTPPlugin plugin;
        private readonly Random rand;

        // Add plugin instance to the handler.
        public TeleportHandler(CTPPlugin plugin) { 
            this.plugin = plugin;

            this.rand = new Random();
        }

        // Handle the event.   
        public void OnSCP914Activate(ref SCP914UpgradeEvent ev) {
            if (ev.KnobSetting == Scp914.Scp914Knob.Coarse)
            {
                // List<ReferenceHub> players = GetPlayersFromInputs(ev.Players);
                // List<Smod2.API.Player> players = GetPlayers(ev.Players);

                if (ev.Players.Count > 0)
                {
                    Smod2.API.Vector randomPos = GetRandomTeleportPos();

                    // Teleport players to the position vector.
                    foreach (ReferenceHub rHub in ev.Players) 
                    {
                        //player.Teleport(randomPos);
                        Timing.RunCoroutine(_Teleport(rHub, randomPos));
                    }
                }
            }
        }
        
        private List<Smod2.API.Player> GetPlayers(List<ReferenceHub> inputs) {
            List<Smod2.API.Player> players = new List<Smod2.API.Player>();

            foreach (ReferenceHub rHub in inputs)
            {
                players.Add(new ServerMod2.API.SmodPlayer(rHub.gameObject));
            }

            return players;
        }

        // This function is horrible, I'll change it when I need to.
        private Smod2.API.Vector GetRandomTeleportPos() {
            double num = rand.NextDouble();

            Plugin.Info("randiadjs: " + num);

            if (num < 0.1f)
            {
                return plugin.allowedPositions.ElementAt(plugin.posZones.FindIndex(zone => zone.Equals(Smod2.API.ZoneType.HCZ)));
            } else
            {
                return plugin.allowedPositions.ElementAt(plugin.posZones.FindIndex(zone => zone.Equals(Smod2.API.ZoneType.LCZ)));
            }
        }

        private IEnumerator<float> _Teleport(ReferenceHub player, Smod2.API.Vector pos) {
            yield return Timing.WaitForSeconds(.1f);

            player.plyMovementSync.OverridePosition(new UnityEngine.Vector3(pos.x, pos.y, pos.z), 0f, false);
        }
    }
}
