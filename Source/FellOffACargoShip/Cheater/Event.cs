using System;
using BattleTech;
using HBS;

namespace FellOffACargoShip.Cheater
{
    internal static class Event
    {
        public static void Force(string param)
        {
            SimGameState simGameState = SceneSingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;

            if (param == "help")
            {
                string help = "";
                help += "• This command will let you force an event";
                help += Environment.NewLine;
                help += "• Params: the desired event id";
                help += Environment.NewLine;
                help += "• Example: '/event event_co_littleAccident'";
                help += Environment.NewLine;
                help += Environment.NewLine;
                help += "Currently only events with scope 'Company' are supported";
                PopupHelper.Info(help);

                return;
            }



            string message = "";

            Pilot plt = null;
            SimGameForcedEvent evt = new SimGameForcedEvent
            {
                Scope = EventScope.Company,
                EventID = param,
                MinDaysWait = 0,
                MaxDaysWait = 0,
                Probability = 100,
                RetainPilot = false
            };
            simGameState.AddSpecialEvent(evt, plt);



            message = $"Forced event '{param}' to occur the next day";
            Logger.Debug($"[Cheater_Event_Force] {message}");
            PopupHelper.Info(message);
        }
    }
}
