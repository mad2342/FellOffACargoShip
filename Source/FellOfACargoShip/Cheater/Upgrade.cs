using System;
using System.Collections.Generic;
using BattleTech;
using Harmony;
using HBS;
using HBS.Collections;

namespace FellOfACargoShip.Cheater
{
    internal static class Upgrade
    {
        private static SimGameState simGameState = LazySingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;
        private static DataProvider dataProvider = new DataProvider();

        public static void Add(string param)
        {
            if (param == "help")
            {
                string help = "";
                help += "• This command will add modules to your dropship";
                help += Environment.NewLine;
                help += "• Params: 'All', 'all' or the id of some module";
                help += Environment.NewLine;
                help += "• Example: '/upgr argoUpgrade_power2'";
                help += Environment.NewLine;
                help += "• Example: '/upgr all'";
                PopupHelper.Info(help);

                return;
            }



            if (simGameState.CurDropship != DropshipType.Argo)
            {
                string message = $"You need a better dropship first.";
                Logger.Debug($"[Cheater_Upgrade_Add] {message}");
                PopupHelper.Info(message);

                return;
            }

            List<ShipModuleUpgrade> ___shipUpgrades = (List<ShipModuleUpgrade>)AccessTools.Field(typeof(SimGameState), "shipUpgrades").GetValue(simGameState);
            List<string> ___purchasedArgoUpgrades = (List<string>)AccessTools.Field(typeof(SimGameState), "purchasedArgoUpgrades").GetValue(simGameState);
            TagSet ___companyTags = (TagSet)AccessTools.Field(typeof(SimGameState), "companyTags").GetValue(simGameState);
            List<string> argoUpgradesToAdd = dataProvider.ArgoUpgradeIds;

            foreach (string id in ___purchasedArgoUpgrades)
            {
                argoUpgradesToAdd.Remove(id);
            }

            if (argoUpgradesToAdd.Count <= 0)
            {
                string message = $"No upgrades left to build.";
                Logger.Debug($"[Cheater_Upgrade_Add] {message}");
                PopupHelper.Info(message);

                return;
            }


            if (param == "All" || param == "all")
            {
                foreach (string id in argoUpgradesToAdd)
                {
                    ShipModuleUpgrade upgrade = simGameState.DataManager.ShipUpgradeDefs.Get(id);
                    //__instance.AddArgoUpgrade(upgrade);

                    // BEN: Custom AddArgoUpgrade (No timeline refresh, this will be applied later ONE TIME ONLY)
                    ___shipUpgrades.Add(upgrade);
                    ___purchasedArgoUpgrades.Add(upgrade.Description.Id);
                    if (simGameState.CurDropship == DropshipType.Argo)
                    {
                        if (upgrade.Tags != null && !upgrade.Tags.IsEmpty)
                        {
                            ___companyTags.AddRange(upgrade.Tags);
                        }
                        foreach (SimGameStat companyStat in upgrade.Stats)
                        {
                            simGameState.SetCompanyStat(companyStat);
                        }
                        // Apply Actions?
                        /*
                        if (upgrade.Actions != null)
                        {
                            SimGameResultAction[] actions = upgrade.Actions;
                            for (int i = 0; i < actions.Length; i++)
                            {
                                SimGameState.ApplyEventAction(actions[i], null);
                            }
                        }
                        */
                    }
                    string message = $"Added upgrade {id} to the Argo.";
                    Logger.Debug($"[Cheater_Upgrade_Add] {message}");
                    PopupHelper.Info(message);
                }
                // Refresh timeline ONCE
                simGameState.RoomManager.RefreshTimeline();
            }
            else
            {
                if (argoUpgradesToAdd.Contains(param))
                {
                    ShipModuleUpgrade upgrade = simGameState.DataManager.ShipUpgradeDefs.Get(param);
                    simGameState.AddArgoUpgrade(upgrade);

                    string message = $"Added upgrade {param} to the Argo.";
                    Logger.Debug($"[Cheater_Upgrade_Add] {message}");
                    PopupHelper.Info(message);
                }
                else
                {
                    string message = $"Upgrade is unknown or already built: {param}";
                    Logger.Debug($"[Cheater_Upgrade_Add] {message}");
                    PopupHelper.Info(message);
                }
            }
        }
    }
}
