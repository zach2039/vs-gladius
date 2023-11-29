using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

using Gladius.Network;
using Gladius.Configs;

namespace Gladius
{
    public class ModTemplateModSystem : ModSystem
    {
       private IServerNetworkChannel serverChannel;
        private ICoreAPI api;

        public override void StartPre(ICoreAPI api)
        {
            string cfgFileName = "Gladius.json";

            try 
            {
                GladiusConfig cfgFromDisk;
                if ((cfgFromDisk = api.LoadModConfig<GladiusConfig>(cfgFileName)) == null)
                {
                    api.StoreModConfig(GladiusConfig.Loaded, cfgFileName);
                }
                else
                {
                    GladiusConfig.Loaded = cfgFromDisk;
                }
            } 
            catch 
            {
                api.StoreModConfig(GladiusConfig.Loaded, cfgFileName);
            }

            base.StartPre(api);
        }

        public override void Start(ICoreAPI api)
        {
            this.api = api;
            base.Start(api);

            api.Logger.Notification("Loaded Gladius!");
        }

        private void OnPlayerJoin(IServerPlayer player)
        {
            // Send connecting players config settings
            this.serverChannel.SendPacket(
                new SyncConfigClientPacket {
                    GladiusCopperDamage = GladiusConfig.Loaded.GladiusCopperDamage,
                    GladiusTinBronzeDamage = GladiusConfig.Loaded.GladiusTinBronzeDamage,
                    GladiusBismuthBronzeDamage = GladiusConfig.Loaded.GladiusBismuthBronzeDamage
                }, player);
        }

        public override void StartServerSide(ICoreServerAPI sapi)
        {
            sapi.Event.PlayerJoin += this.OnPlayerJoin; 
            
            // Create server channel for config data sync
            this.serverChannel = sapi.Network.RegisterChannel("gladius")
                .RegisterMessageType<SyncConfigClientPacket>()
                .SetMessageHandler<SyncConfigClientPacket>((player, packet) => {});
        }

        public override void StartClientSide(ICoreClientAPI capi)
        {
            // Sync config settings with clients
            capi.Network.RegisterChannel("gladius")
                .RegisterMessageType<SyncConfigClientPacket>()
                .SetMessageHandler<SyncConfigClientPacket>(p => {
                    this.Mod.Logger.Event("Received config settings from server");
                    GladiusConfig.Loaded.GladiusCopperDamage = p.GladiusCopperDamage;
                    GladiusConfig.Loaded.GladiusTinBronzeDamage = p.GladiusTinBronzeDamage;
                    GladiusConfig.Loaded.GladiusBismuthBronzeDamage = p.GladiusBismuthBronzeDamage;
                });
        }
    }
}
