using Smod2.Commands;

namespace CoarseTP {
    class CTPDisableCommand : ICommandHandler {
        private readonly CTPPlugin plugin;

        public CTPDisableCommand(CTPPlugin plugin) => this.plugin = plugin;

        public string GetCommandDescription() {
            return "Disables the CTP plugin.";
        }

        public string GetUsage() {
            return "CTPDISABLE";
        }

        public string[] OnCall(ICommandSender sender, string[] args) {
            plugin.Info("Disabling " + plugin.Details.name + "...");
            plugin.PluginManager.DisablePlugin(plugin);

            return new string[] { GetUsage() + " called, disabling CoarseTP." };
        }
    }
}
