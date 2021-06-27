using System;
using BattleTech;
using FellOffACargoShip.Extensions;
using HBS;

namespace FellOffACargoShip.Cheater
{
    internal static class Mech
    {
        public static void Add(string param)
        {
            SimGameState simGameState = SceneSingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;
            DataProvider dataProvider = new DataProvider();

            if (param == "help")
            {
                string help = "";
                help += "• This command will add mechs to your inventory";
                help += Environment.NewLine;
                help += "• Params: the variant name of the desired mech";
                help += Environment.NewLine;
                help += "• Example: '/mech warhammer_WHM-6R'";
                PopupHelper.Info(help);

                return;
            }

            // Special interests
            if (param == "bourbon")
            {
                foreach (string id in dataProvider.BourbonCustomMechIds)
                {
                    AddMech(id, true);
                }
                return;
            }
            if (param == "special")
            {
                foreach (string id in dataProvider.SpecialGearMechIds)
                {
                    AddMech(id, true);
                }
                return;
            }



            // Shortcut if param already is a proper mechdef id
            if (simGameState.DataManager.MechDefs.Exists(param))
            {
                simGameState.AddMechByID(param, true);
                return;
            }



            // BEN: Note that the CHASSIS.ID is needed for this to function correctly.
            // Even in SimGameState.AddMech this would be called with a MechDef (which doesn't work)
            // Currently all calls to SimGameState.AddMech seem to use params that won't get to that place
            // In SimGameState.UnreadyMech it's called with a MechDef.Chassis.Description.Id and works correctly

            //string chassisId = simGameState.GetChassisIdFromVariantName(param.ToUpper());
            string chassisId = simGameState.GetChassisIdFromVariantName(param);

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

                if (FellOffACargoShip.Settings.AddMechsSilently)
                {
                    simGameState.AddItemStat(id, typeof(MechDef), false);

                    string message = $"{id} added to inventory.";
                    Logger.Debug($"[Cheater_Mech_Add] {message}");
                    PopupHelper.Info(message);
                }
                else
                {
                    simGameState.AddMechByID(mechId, true);
                }
            }
        }
    }
}
