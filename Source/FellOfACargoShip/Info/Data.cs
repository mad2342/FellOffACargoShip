using System;

namespace FellOfACargoShip.Info
{
    internal static class Data
    {
        private static DataProvider dataProvider = new DataProvider();

        public static void List(string param)
        {
            if (param == "help")
            {
                string help = "";
                help += "• This command will output json ready lists of data in logfile";
                help += Environment.NewLine;
                help += "• Params: 'all', 'argo', 'mechs', 'weapons' 'upgrades', 'heatsinks', 'ammo'";
                help += Environment.NewLine;
                help += "• Example: '/list weapons'";
                PopupHelper.Info(help);

                return;
            }



            string message = "";

            if (param == "all")
            {
                dataProvider.ListAll();
                message = $"Generated a clean list of all valid ArgoUpgradeIds, MechDefIds and ComponentDefIds in /Mods/FellOfACargoShip/FellOfACargoShip.log";
            }
            else if (param == "argo")
            {
                dataProvider.ListArgoUpgrades();
                message = $"Generated a clean list of all valid ArgoUpgradeIds in /Mods/FellOfACargoShip/FellOfACargoShip.log";
            }
            else if (param == "mechs")
            {
                dataProvider.ListMechs();
                message = $"Generated a clean list of all valid MechDefIds in /Mods/FellOfACargoShip/FellOfACargoShip.log";
            }
            else if (param == "weapons")
            {
                dataProvider.ListWeapons();
                message = $"Generated a clean list of all valid WeaponDefIds in /Mods/FellOfACargoShip/FellOfACargoShip.log";
            }
            else if (param == "upgrades")
            {
                dataProvider.ListUpgrades();
                message = $"Generated a clean list of all valid UpgradeDefIds in /Mods/FellOfACargoShip/FellOfACargoShip.log";
            }
            else if (param == "heatsinks")
            {
                dataProvider.ListHeatsinks();
                message = $"Generated a clean list of all valid HeatsinkDefIds in /Mods/FellOfACargoShip/FellOfACargoShip.log";
            }
            else if (param == "ammo")
            {
                dataProvider.ListAmmoBoxes();
                message = $"Generated a clean list of all valid AmmoBoxDefIds in /Mods/FellOfACargoShip/FellOfACargoShip.log";
            }
            else
            {
                message = $"No action defined for param: {param}";
            }
            Logger.Debug($"[Info_List] {message}");
            PopupHelper.Info(message);
        }
    }
}
