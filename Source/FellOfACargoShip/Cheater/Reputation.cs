using System;
using BattleTech;
using HBS;

namespace FellOfACargoShip.Cheater
{
    internal static class Reputation
    {
        // Factions: Davion, Liao, Kurita, Marik, Steiner, TaurianConcordat, MagistracyOfCanopus, AuriganPirates, MercenaryReviewBoard
        private static SimGameState simGameState = SceneSingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;

        public static void Add(string param)
        {
            if (param == "help")
            {
                string help = "";
                help += "• This command will add reputation for some faction";
                help += Environment.NewLine;
                help += "• Params: faction name '+' the desired amount";
                help += Environment.NewLine;
                help += "• Example: '/rep AuriganPirates+100'";
                help += Environment.NewLine;
                help += "Valid factions are Davion, Liao, Kurita, Marik, Steiner, TaurianConcordat, MagistracyOfCanopus, AuriganPirates and MercenaryReviewBoard";
                PopupHelper.Info(help);

                return;
            }



            //@ToDo: Allow to add reputation for all factions at once (ie: "/rep all+100")

            string message = "";

            FactionValue faction = FactionEnumeration.GetInvalidUnsetFactionValue();
            string[] array = param.Split(new char[] {'+'});

            if (array.Length != 2)
            {
                message = $"Param should be faction+number. Type '/rep help' to get an example.";
                Logger.Debug($"[Cheater_Reputation_Add] {message}");
                PopupHelper.Info(message);

                return;
            }

            string factionName = array[0];
            int.TryParse(array[1], out int val);

            try
            {
                faction = FactionEnumeration.GetFactionByName(factionName);
            }
            catch (ArgumentException ex)
            {
                message = $"Unable to convert to a faction from provided string: {factionName}. Type '/rep help' to get a list of valid factions.";
                Logger.Debug($"[Cheater_Reputation_Add] {message}");
                Logger.Error(ex);
                PopupHelper.Info(message);
            }

            if (faction.IsInvalidUnset)
            {
                message = $"Invalid faction passed: {factionName}. Type '/rep help' to get a list of valid factions.";
                Logger.Debug($"[Cheater_Reputation_Add] {message}");
                PopupHelper.Info(message);

                return;
            }

            simGameState.AddReputation(faction, val, false, null);

            message = $"Added {val} reputation for faction {factionName}.";
            Logger.Debug($"[Cheater_Reputation_Add] {message}");
            PopupHelper.Info(message);
        }
    }
}
