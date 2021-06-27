using System;
using System.Collections.Generic;
using System.Linq;
using BattleTech;
using Harmony;
using HBS;

namespace FellOffACargoShip.Cheater
{
    internal static class Ronin
    {
        public static void Add(string param)
        {
            SimGameState simGameState = SceneSingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;

            if (param == "help")
            {
                string help = "";
                help += "• This command will add ronin pilots to your roster";
                help += Environment.NewLine;
                help += "• Params: the id or the callsign of the desired ronin";
                help += Environment.NewLine;
                help += "• Example: '/ronin pilot_backer_Eck'";
                help += Environment.NewLine;
                help += "• Example: '/ronin Eck'";
                PopupHelper.Info(help);

                return;
            }



            int pilotsToAddCount = simGameState.GetMaxMechWarriors() - simGameState.PilotRoster.Count;

            if (pilotsToAddCount <= 0)
            {
                string message = $"No space left in roster.";
                Logger.Debug($"[Cheater_Ronin_Add] {message}");
                PopupHelper.Info(message);

                return;
            }

            if (param == "All" || param == "all")
            {
                for (int i = 0; i < pilotsToAddCount; i++)
                {
                    PilotDef pilotDef = simGameState.GetUnusedRonin();
                    simGameState.AddPilotToRoster(pilotDef, true, true);

                    string message = $"Added {pilotDef.Description.Callsign} to roster.";
                    Logger.Debug($"[Cheater_Ronin_Add] {message}");
                    PopupHelper.Info(message);
                }
            }
            else
            {
                // Allow to add by id or callsign (ie: "/ronin pilot_backer_Eck" or "/ronin Eck")
                PilotDef pilotDef = simGameState.DataManager.PilotDefs.FirstOrDefault(kvp => kvp.Key == param || kvp.Value.Description.Callsign == param || kvp.Value.Description.Callsign?.ToLower() == param).Value;
                if (pilotDef != null) 
                //if (simGameState.DataManager.PilotDefs.TryGet(param, out PilotDef pilotDef))
                {
                    List<string> ___usedRoninIDs = (List<string>)AccessTools.Field(typeof(SimGameState), "usedRoninIDs").GetValue(simGameState);

                    if (!___usedRoninIDs.Contains(pilotDef.Description.Id) && simGameState.IsRoninWhitelisted(pilotDef))
                    {
                        simGameState.AddPilotToRoster(pilotDef, true, true);

                        string message = $"Added {pilotDef.Description.Callsign} to roster.";
                        Logger.Debug($"[Cheater_Ronin_Add] {message}");
                        PopupHelper.Info(message);
                    }
                    else
                    {
                        string message = $"{pilotDef.Description.Callsign} is blacklisted or already part of your roster.";
                        Logger.Debug($"[Cheater_Ronin_Add] {message}");
                        PopupHelper.Info(message);
                    }
                }
                else
                {
                    string message = $"Couldn't find ronin with id or callsign: {param}";
                    Logger.Debug($"[Cheater_Ronin_Add] {message}");
                    PopupHelper.Info(message);
                }
            }
        }
    }
}
