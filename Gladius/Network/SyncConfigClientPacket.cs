using ProtoBuf;

namespace Gladius.Network
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SyncConfigClientPacket
    {
        public double GladiusCopperDamage;
        public double GladiusBismuthBronzeDamage;
        public double GladiusTinBronzeDamage;
    }
}