
namespace Gladius.Configs
{
    public class GladiusConfig 
    {
        public static GladiusConfig Loaded { get; set; } = new GladiusConfig();

        public double GladiusCopperDamage { get; set; } = 2.5;
        public double GladiusBismuthBronzeDamage { get; set; } = 2.8;
        public double GladiusTinBronzeDamage { get; set; } = 2.8;
    }
}