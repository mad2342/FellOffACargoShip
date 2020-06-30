using System;
using System.Collections.Generic;
using System.Linq;
using BattleTech;
using HBS;

namespace FellOffACargoShip.Cheater
{
    internal static class Experience
    {
        public static void Add(string param)
        {
            SimGameState simGameState = SceneSingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;

            if (param == "help")
            {
                string help = "";
                help += "• This command will add experience for pilots in your roster";
                help += Environment.NewLine;
                help += "• Params: the desired amount of xp";
                help += Environment.NewLine;
                help += "• Example: '/xp 30000'";
                PopupHelper.Info(help);

                return;
            }



            //@ToDo: Allow XP for only one pilot of current Roster (ie: "Behemoth+5000")

            string message = "";

            if (!int.TryParse(param, out int xp) || xp <= 0)
            {
                message = $"Param is not a positive number.";
                Logger.Debug($"[Cheater_Experience_Add] {message}");
                PopupHelper.Info(message);

                return;
            }

            List<string> pilotCallsigns = new List<string>();

            simGameState.Commander.AddExperience(0, "FellOffACargoShip.AddXP", xp);
            pilotCallsigns.Add(simGameState.Commander.Callsign);

            foreach (var item in simGameState.PilotRoster.Select((value, i) => new { i, value }))
            {
                Pilot pilot = item.value;
                int index = item.i;

                pilot.AddExperience(index, "FellOffACargoShip.AddXP", xp);
                pilotCallsigns.Add(pilot.Callsign);
            }

            string allAffectedPilots = String.Join(", ", pilotCallsigns);

            message = $"Added {xp} XP to {allAffectedPilots}.";
            Logger.Debug($"[Cheater_Experience_Add] {message}");
            PopupHelper.Info(message);
        }
    }
}
