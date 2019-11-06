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

namespace CoarseTP {
    [PluginDetails(
        author = "djakkalap",
        name = "Coarse TP",
        description = "This plugin allows players to be teleported to a random room on the 'coarse' setting in SCP-914.",
        id = "djakkalap.ctpplugin",
        configPrefix = "ctp",
        langFile = "ctpplugin",
        version = "0.2",
        SmodMajor = 3,
        SmodMinor = 5,
        SmodRevision = 0
        )]

    public class CTPPlugin : Plugin {
        internal List<Vector> allowedPositions;
        internal List<ZoneType> posZones;

        public override void OnDisable() {
            Info(Details.name + " has been disabled.");
        }

        public override void OnEnable() {
            Info(Details.name + " has been enabled.");
        }

        // Register the parts of the plugin.
        public override void Register() {
            AddConfig(new ConfigSetting("ctp_damage_disable", true, true, "Whether to damage the player when teleported."));
            AddConfig(new ConfigSetting("ctp_health_multiplier", 0.5f, true, "Percentage of the max HP of the player's role that gets deducted upon teleporting."));
            AddConfig(new ConfigSetting("ctp_hcz_chance", 0.2f, true, "Determines the chance of teleporting to HCZ."));
            AddConfig(new ConfigSetting("ctp_disable", false, true, "Whether to disable CoarseTP."));

            AddEventHandler(typeof(IEventHandlerWaitingForPlayers), new LoadPluginHandler(this));
            AddEventHandler(typeof(IEventHandlerSCP914Activate), new TeleportHandler(this));

            AddCommand("ctpdisable", new CTPDisableCommand(this));
        }
    }
}
