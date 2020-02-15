using System;
using BattleTech;
using HBS;

namespace FellOfACargoShip.Cheater
{
    internal static class Component
    {
        private static SimGameState simGameState = LazySingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;
        private static DataProvider dataProvider = new DataProvider();

        public static void Add(string param)
        {
            if (param == "help")
            {
                string help = "";
                help += "• This command will add components to your inventory";
                help += Environment.NewLine;
                help += "• Params: 'All', 'all' or the id of some component '+' the desired amount";
                help += Environment.NewLine;
                help += "• Example: '/comp Weapon_Gauss_Gauss_0-STOCK+5'";
                help += Environment.NewLine;
                help += "• Example: '/comp Gear_HeatSink_Generic_Double+20'";
                help += Environment.NewLine;
                help += "• Example: '/comp All+10'";
                PopupHelper.Info(help);

                return;
            }



            string message = "";

            string[] array = param.Split(new char[] { '+' });
            if (array.Length != 2)
            {
                message = $"Param should be id+number. Type '/comp help' to get some examples.";
                Logger.Debug($"[Cheater_Reputation_Add] {message}");
                PopupHelper.Info(message);

                return;
            }

            string componentDefId = array[0];
            int.TryParse(array[1], out int count);

            if (count <= 0)
            {

                message = $"Amount is not a positive number.";
                Logger.Debug($"[Cheater_Component_Add] {message}");
                PopupHelper.Info(message);

                return;
            }

            if(componentDefId == "All" || componentDefId == "all")
            {
                // Add Weapons
                foreach (string id in dataProvider.WeaponDefIds)
                {
                    int i = 0;
                    while (i < count)
                    {
                        simGameState.AddItemStat(id, typeof(WeaponDef), false);
                        i++;
                    }
                    Logger.Debug($"[Cheater_Component_Add] Added {id}({count}) to inventory.");
                }

                // Add upgrades
                foreach (string id in dataProvider.UpgradeDefIds)
                {
                    int i = 0;
                    while (i < count)
                    {
                        simGameState.AddItemStat(id, typeof(UpgradeDef), false);
                        i++;
                    }
                    Logger.Debug($"[Cheater_Component_Add] Added {id}({count}) to inventory.");
                }

                // Add Heatsinks
                foreach (string id in dataProvider.HeatSinkDefIds)
                {
                    int i = 0;
                    while (i < count)
                    {
                        simGameState.AddItemStat(id, typeof(HeatSinkDef), false);
                        i++;
                    }
                    Logger.Debug($"[Cheater_Component_Add] Added {id}({count}) to inventory.");
                }

                // Add AmmoBoxes
                foreach (string id in dataProvider.AmmoBoxDefIds)
                {
                    int i = 0;
                    while (i < count)
                    {
                        simGameState.AddItemStat(id, typeof(AmmunitionBoxDef), false);
                        i++;
                    }
                    Logger.Debug($"[Cheater_Component_Add] Added {id}({count}) to inventory.");
                }

                message = $"Added {count} pieces of all known components to inventory.";
                Logger.Debug($"[Cheater_Component_Add] {message}");
                PopupHelper.Info(message);
            }           
            else
            {
                // Try to find a valid component
                Type componentType;

                // Weapon
                if (dataProvider.WeaponDefIds.Contains(componentDefId))
                {
                    componentType = typeof(WeaponDef);
                }
                // Upgrade
                else if (dataProvider.UpgradeDefIds.Contains(componentDefId))
                {
                    componentType = typeof(UpgradeDef);
                }
                // Heatsink
                else if(dataProvider.HeatSinkDefIds.Contains(componentDefId)) {
                    componentType = typeof(HeatSinkDef);
                }
                // AmmoBox
                else if (dataProvider.AmmoBoxDefIds.Contains(componentDefId))
                {
                    componentType = typeof(AmmunitionBoxDef);
                }
                else
                {
                    message = $"{componentDefId} is unknown or blacklisted.";
                    Logger.Debug($"[Cheater_Component_Add] {message}");
                    PopupHelper.Info(message);

                    return;
                }

                // Add
                int i = 0;
                while (i < count)
                {
                    simGameState.AddItemStat(componentDefId, componentType, false);
                    i++;
                }

                message = $"Added {count} pieces of {componentDefId} to inventory.";
                Logger.Debug($"[Cheater_Component_Add] {message}");
                PopupHelper.Info(message);
            }
        }
    }
}
