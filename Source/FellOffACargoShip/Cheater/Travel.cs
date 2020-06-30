using System;
using BattleTech;
using HBS;

namespace FellOffACargoShip.Cheater
{
    internal static class Travel
    {
        public static void To(string param)
        {
            SimGameState simGameState = SceneSingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;

            if (param == "help")
            {
                string help = "";
                help += "• This command will let you travel to a system instantly";
                help += Environment.NewLine;
                help += "• Params: the desired system name";
                help += Environment.NewLine;
                help += "• Example: '/travel Victoria'";
                PopupHelper.Info(help);

                return;
            }



            string message = "";

            if (string.Equals(simGameState.CurSystem.Name, param))
            {
                message = $"Already orbiting {param}";
                Logger.Debug($"[Cheater_Travel_To] {message}");
                PopupHelper.Info(message);

                return;
            }

            foreach (StarSystem starSystem in simGameState.StarSystems)
            {
                if (string.Equals(starSystem.Name, param, StringComparison.OrdinalIgnoreCase))
                {
                    simGameState.SetCurrentSystem(starSystem, true, false);
                    simGameState.CameraController.spaceController.Orbit(SimGameTravelStatus.IN_SYSTEM, false);

                    message = $"Arrived at {param}";
                    Logger.Debug($"[Cheater_Travel_To] {message}");
                    PopupHelper.Info(message);

                    return;
                }
            }

            message = $"Couldn't find {param} in available star systems";
            Logger.Debug($"[Cheater_Travel_To] {message}");
            PopupHelper.Info(message);
        }
    }
}
