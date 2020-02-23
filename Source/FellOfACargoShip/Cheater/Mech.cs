using System;
using BattleTech;
using FellOfACargoShip.Extensions;
using HBS;

namespace FellOfACargoShip.Cheater
{
    internal static class Mech
    {
        private static SimGameState simGameState = SceneSingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;
        private static DataProvider dataProvider = new DataProvider();

        public static void Add(string param)
        {
            if (param == "help")
            {
                string help = "";
                help += "• This command will add mechs to your inventory";
                help += Environment.NewLine;
                help += "• Params: the variant name of the desired mech";
                help += Environment.NewLine;
                help += "• Example: '/mech WHM-6R'";
                PopupHelper.Info(help);

                return;
            }

            // Special interest
            if (param == "bourbon")
            {
                foreach (string id in dataProvider.BourbonCustomMechIds)
                {
                    AddMech(id, true);
                }
                return;
            }



            // BEN: Note that the CHASSIS.ID is needed for this to function correctly.
            // Even in SimGameState.AddMech this would be called with a MechDef (which doesn't work)
            // Currently all calls to SimGameState.AddMech seem to use params that won't get to that place
            // In SimGameState.UnreadyMech it's called with a MechDef.Chassis.Description.Id and works correctly
            string chassisId = simGameState.GetChassisIdFromVariantName(param.ToUpper());

            if (chassisId == null)
            {
                string message = $"No mech found for variant: {param}";
                Logger.Debug($"[Cheater_Mech_Add] {message}");
                PopupHelper.Info(message);

                return;
            }

            AddMech(chassisId);

            




            // Local helper
            void AddMech(string id, bool force = false)
            {
                string mechId = id.Replace("chassisdef", "mechdef");
                if (!force && !dataProvider.MechDefIds.Contains(mechId))
                {
                    string message = $"{mechId} is blacklisted.";
                    Logger.Debug($"[Cheater_Mech_Add] {message}");
                    PopupHelper.Info(message);

                    return;
                }

                if (FellOfACargoShip.Settings.AddMechsSilently)
                {
                    simGameState.AddItemStat(id, typeof(MechDef), false);

                    string message = $"{id} added to inventory.";
                    Logger.Debug($"[Cheater_Mech_Add] {message}");
                    PopupHelper.Info(message);
                }
                else
                {
                    simGameState.AddMechByID(mechId, true);
                    Fields.IsCustomPopup = false;
                }
            }
        }
    }
}
