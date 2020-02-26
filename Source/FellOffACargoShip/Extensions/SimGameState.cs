using System.Collections.Generic;
using BattleTech;

namespace FellOffACargoShip.Extensions
{
    internal static class SimGameStateExtensions
    {
        public static string GetChassisIdFromVariantName(this SimGameState simGameState, string variant)
        {
            foreach (KeyValuePair<string, ChassisDef> chassisDefs in simGameState.DataManager.ChassisDefs)
            {
                string chassisId = chassisDefs.Key;
                ChassisDef chassisDef = chassisDefs.Value;

                if (chassisDef.VariantName == variant || chassisDef.VariantName.ToUpper() == variant)
                {
                    return chassisDef.Description.Id;
                }
            }
            return null;
        }
    }
}
