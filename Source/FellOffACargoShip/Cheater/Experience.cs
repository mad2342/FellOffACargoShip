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
                help += "• Params: 'All', 'all' or the callsign of some pilot '+' the desired xp";
                help += Environment.NewLine;
                help += "• Example: '/xp Behemoth+50000'";
                help += Environment.NewLine;
                help += "• Example: '/xp All+10000'";
                PopupHelper.Info(help);

                return;
            }



            string message = "";

            string[] array = param.Split(new char[] { '+' });
            if (array.Length != 2)
            {
                message = $"Param should be id+number. Type '/ronin help' to get some examples.";
                Logger.Debug($"[Cheater_Experience_Add] {message}");
                PopupHelper.Info(message);

                return;
            }

            string callsign = array[0];
            int.TryParse(array[1], out int xp);

            if (xp <= 0)
            {
                message = $"XP is not a positive number.";
                Logger.Debug($"[Cheater_Experience_Add] {message}");
                PopupHelper.Info(message);

                return;
            }


            if (callsign == "All" || callsign == "all")
            {
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
            else
            {
                bool success = false;

                if (callsign == simGameState.Commander.Callsign || callsign == simGameState.Commander.Callsign.ToLower())
                {
                    simGameState.Commander.AddExperience(0, "FellOffACargoShip.AddXP", xp);

                    success = true;
                    message = $"Added {xp} XP to {simGameState.Commander.Callsign}.";
                }
                else
                {
                    foreach (var item in simGameState.PilotRoster.Select((value, i) => new { i, value }))
                    {
                        Pilot pilot = item.value;
                        int index = item.i;

                        if (callsign == pilot.Callsign || callsign == pilot.Callsign.ToLower())
                        {
                            pilot.AddExperience(index, "FellOffACargoShip.AddXP", xp);

                            success = true;
                            message = $"Added {xp} XP to {pilot.Callsign}.";
                        }
                    }
                }

                if (success)
                {
                    Logger.Debug($"[Cheater_Experience_Add] {message}");
                    PopupHelper.Info(message);
                }
                else
                {
                    message = $"Couldn't find pilot with callsign: {callsign}";
                    Logger.Debug($"[Cheater_Ronin_Add] {message}");
                    PopupHelper.Info(message);
                }
            }
        }
    }
}
