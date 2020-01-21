using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.EventHandlers;
using EXILED;

namespace CoarseTP {
    [PluginDetails(
        author = "djakkalap",
        name = "Coarse TP (EXILED)",
        description = "This plugin allows players to be teleported to a random room on the 'coarse' setting in SCP-914.",
        id = "djakkalap.ctpplugin",
        configPrefix = "ctp",
        langFile = "ctpplugin",
        version = "0.2",
        SmodMajor = 3,
        SmodMinor = 6,
        SmodRevision = 0
        )]

    public class CTPPlugin : EXILED.Plugin {
        internal List<Vector> allowedPositions;
        internal List<ZoneType> posZones;

        public override string getName => "CoarseTP";
        public LoadPluginHandler loadPluginHandler;
        public TeleportHandler teleportHandler;  

        public override void OnDisable() {
            Info(getName + " has been disabled.");
        }

        public override void OnEnable() {
            loadPluginHandler = new LoadPluginHandler(this);
            teleportHandler = new TeleportHandler(this);

            Events.WaitingForPlayersEvent += loadPluginHandler.OnWaitingForPlayers;
            Events.Scp914UpgradeEvent += teleportHandler.OnSCP914Activate; 

            Info(getName + " has been enabled.");

        }

        public override void OnReload() {
            throw new NotImplementedException();
        }
    }
}
