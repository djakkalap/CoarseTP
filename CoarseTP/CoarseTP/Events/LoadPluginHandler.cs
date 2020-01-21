using System;
using System.Collections.Generic;
using System.Linq;

using Smod2;
using Smod2.API;
using Smod2.Config;
using Smod2.EventHandlers;
using Smod2.Events;

namespace CoarseTP {
    public class LoadPluginHandler  {
        private readonly CTPPlugin plugin;

        public LoadPluginHandler(CTPPlugin plugin) => this.plugin = plugin;

        public void OnWaitingForPlayers() {
            List<Vector> allowedPositions = new List<Vector>();
            List<ZoneType> posZones = new List<ZoneType>();
            string[] AllowedRIDs = { "LC_CAFE", "HC_079_HALL" };
            // This gets all the rooms with a roomID
            UnityEngine.GameObject[] allRooms = UnityEngine.GameObject.FindGameObjectsWithTag("RoomID");

            foreach (UnityEngine.GameObject room in allRooms)
            {
                string rid = room.GetComponent<Rid>().id;

                if (AllowedRIDs.Contains(rid))
                {
                    UnityEngine.Vector3 roomPos = room.transform.position;

                    if (rid == "HC_079_HALL")
                    {
                        if (roomPos.y > -1004)
                        {
                            allowedPositions.Add(new Vector(roomPos.x, roomPos.y + 2.0f, roomPos.z));
                            posZones.Add(ZoneType.HCZ);
                        }
                    }
                    else
                    {
                        // We add 1 unit to the y component, to prevent getting stuck or other weird glitches.
                        allowedPositions.Add(new Vector(roomPos.x, roomPos.y + 2.0f, roomPos.z));
                        posZones.Add(ZoneType.LCZ);
                    }
                }
            }

            plugin.allowedPositions = allowedPositions;
            plugin.posZones = posZones;
        }
    }
}
