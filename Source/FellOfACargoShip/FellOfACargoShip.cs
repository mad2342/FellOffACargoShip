using Harmony;
using System.Reflection;
using System;
using Newtonsoft.Json;
using System.IO;
using BattleTech;
using System.Collections.Generic;
using HBS.Data;
using System.Linq;
using BattleTech.UI;
using HBS.Collections;

namespace FellOfACargoShip
{
    public static class FellOfACargoShip
    {
        internal static string LogPath;
        internal static string ModDirectory;
        internal static Settings Settings;
        // BEN: DebugLevel (0: nothing, 1: error, 2: debug, 3: info)
        internal static int DebugLevel = 1;

        public static void Init(string directory, string settings)
        {
            ModDirectory = directory;
            LogPath = Path.Combine(ModDirectory, "FellOfACargoShip.log");

            Logger.Initialize(LogPath, DebugLevel, ModDirectory, nameof(FellOfACargoShip));

            try
            {
                Settings = JsonConvert.DeserializeObject<Settings>(settings);
            }
            catch (Exception e)
            {
                Settings = new Settings();
                Logger.Error(e);
            }

            // Harmony calls need to go last here because their Prepare() methods directly check Settings...
            HarmonyInstance harmony = HarmonyInstance.Create("de.mad.FellOfACargoShip");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(SimGameState), "_OnAttachUXComplete")]
    public static class SimGameState__OnAttachUXComplete_AddInventory
    {
        public static bool Prepare()
        {
            return FellOfACargoShip.Settings.LogComponentLists || FellOfACargoShip.Settings.AddInventory;
        }

        public static void Postfix(SimGameState __instance)
        {
            try
            {
                IDataItemStore<string, ChassisDef> ChassisDefs = __instance.DataManager.ChassisDefs;
                IDataItemStore<string, WeaponDef> WeaponDefs = __instance.DataManager.WeaponDefs;
                IDataItemStore<string, UpgradeDef> UpgradeDefs = __instance.DataManager.UpgradeDefs;
                IDataItemStore<string, HeatSinkDef> HeatSinkDefs = __instance.DataManager.HeatSinkDefs;
                IDataItemStore<string, AmmunitionBoxDef> AmmoBoxDefs = __instance.DataManager.AmmoBoxDefs;

                List<string> mechDefIds = new List<string>();
                List<string> weaponDefIds = new List<string>();
                List<string> upgradeDefIds = new List<string>();
                List<string> heatSinkDefIds = new List<string>();
                List<string> ammoBoxDefIds = new List<string>();

                List<string> chassisDefBlacklist = new List<string> {
                    "chassisdef_atlas_AS7-GG",
                    "chassisdef_centurion_TARGETDUMMY",
                    "chassisdef_marauder_MAD-BH",
                    "chassisdef_marauder_MAD-CM",
                    "chassisdef_panther_TARGETDUMMY",
                    "chassisdef_urbanmech_TESTDUMMY",
                    "chassisdef_warhammer_WHM-BW",
                    "chassisdef_crab__fp_gladiator_BSC-27",
                    "chassisdef_archer_ARC-XO",
                    "chassisdef_archer_ARC-LS",
                    "chassisdef_annihilator_ANH-JH",
                    "chassisdef_bullshark_BSK-M3",
                    "chassisdef_bullshark_BSK-MAZ",
                    "chassisdef_rifleman_RFL-RIP",
                    "chassisdef_griffin_GRF-1N_DECOMMISSIONED"
                };
                List<string> weaponDefBlacklist = new List<string> {
                    "Weapon_MeleeAttack",
                    "Weapon_DFAAttack",
                    "Weapon_Laser_AI_Imaginary",
                    "Weapon_Mortar_Thumper",
                    "Weapon_Mortar_MechMortar",
                    "Weapon_Autocannon_AC20_SPECIAL-Victoria",
                    "Weapon_Flamer_Flamer_SPECIAL-Victoria"
                };
                List<string> upgradeDefBlacklist = new List<string> {
                    "Gear_General_Enhanced_Missilery_System",
                    "Gear_Actuator_Prototype_Hatchet",
                    "Gear_General_Targeting_Baffle",
                    "Gear_General_Rangefinder_Suite",
                    "Gear_General_Intercept_System",
                    "Gear_General_GM_Ballistic_Siege_Compensators",
                    "Gear_General_Close_Quarters_Combat_Suite",
                    "Gear_Sensor_Prototype_ECM",
                    "Gear_General_Optimized_Capacitors",
                    "Gear_Sensor_Prototype_ActiveProbe",
                    "Gear_Sensor_Prototype_EWE",
                    "Gear_Mortar_Thumper",
                    "Gear_Mortar_MechMortar",
                    "TargetDummyMod",
                    "Gear_Cockpit_Tacticon_B2000_Battle_Computer",
                    "Gear_General_Company_Command_Module",
                    "Gear_General_Lance_Command_Module",
                    "Gear_General_Robinson_TG120_Drive_Train"
                };
                List<string> heatSinkDefBlacklist = new List<string>
                {

                };
                List<string> ammoBoxDefBlacklist = new List<string> {
                    "AmmunitionBox_Flamer",
                    "Ammo_AmmunitionBox_Generic_Flamer",
                    "Ammo_AmmunitionBox_Generic_SRMInferno",
                    "Ammo_AmmunitionBox_Generic_Narc"
                };



                // Collect Mechs
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Collecting all valid Mechs");
                foreach (string chassisDefId in ChassisDefs.Keys)
                {
                    if (!chassisDefBlacklist.Contains(chassisDefId))
                    {
                        string id = chassisDefId.Replace("chassisdef", "mechdef");
                        mechDefIds.Add(id);
                    }
                }
                mechDefIds.Sort();

                // Collect Weapons
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Collecting all valid Weapons");
                foreach (string id in WeaponDefs.Keys)
                {
                    if (!weaponDefBlacklist.Contains(id))
                    {
                        weaponDefIds.Add(id);
                    }
                }
                weaponDefIds.Sort();

                // Collect Upgrades
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Collecting all valid Upgrades");
                foreach (string id in UpgradeDefs.Keys)
                {
                    if (!upgradeDefBlacklist.Contains(id))
                    {
                        upgradeDefIds.Add(id);
                    }
                }
                upgradeDefIds.Sort();

                // Collect Heatsinks
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Collecting all valid Heatsinks");
                foreach (string id in HeatSinkDefs.Keys)
                {
                    if (!heatSinkDefBlacklist.Contains(id))
                    {
                        heatSinkDefIds.Add(id);
                    }
                }
                heatSinkDefIds.Sort();

                // Collect Ammunition
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Collecting all valid AmmunitionBoxes");
                foreach (string id in AmmoBoxDefs.Keys)
                {
                    if (!ammoBoxDefBlacklist.Contains(id))
                    {
                        ammoBoxDefIds.Add(id);
                    }
                }
                ammoBoxDefIds.Sort();



                // Add Inventory
                if (FellOfACargoShip.Settings.AddInventory)
                {
                    // Add Mechs
                    if (FellOfACargoShip.Settings.AddInventoryMechs)
                    {
                        // Custom list given?
                        if (FellOfACargoShip.Settings.AddInventoryMechsList.Any())
                        {
                            foreach (string id in FellOfACargoShip.Settings.AddInventoryMechsList)
                            {
                                if (FellOfACargoShip.Settings.AddInventoryMechsSilently)
                                {
                                    // BEN: Note that the CHASSIS.ID is needed for this to function correctly.
                                    // Even in SimGameState.AddMech this would be called with a MechDef (which doesn't work)
                                    // Currently all calls to SimGameState.AddMech seem to use params that won't get to that place
                                    // In SimGameState.UnreadyMech it's called with a MechDef.Chassis.Description.Id and works correctly
                                    string chassisID = id.Replace("mechdef", "chassisdef");
                                    __instance.AddItemStat(chassisID, typeof(MechDef), false);
                                }
                                else
                                {
                                    __instance.AddMechByID(id, true);
                                }
                                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + id + " to inventory.");
                            }
                        }
                        else
                        {
                            foreach (string id in mechDefIds)
                            {
                                if (FellOfACargoShip.Settings.AddInventoryMechsSilently)
                                {
                                    // BEN: Note that the CHASSIS.ID is needed for this to function correctly.
                                    // Even in SimGameState.AddMech this would be called with a MechDef (which doesn't work)
                                    // Currently all calls to SimGameState.AddMech seem to use params that won't get to that place
                                    // In SimGameState.UnreadyMech it's called with a MechDef.Chassis.Description.Id and works correctly
                                    string chassisID = id.Replace("mechdef", "chassisdef");
                                    __instance.AddItemStat(chassisID, typeof(MechDef), false);
                                }
                                else
                                {
                                    __instance.AddMechByID(id, true);
                                }
                                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + id + " to inventory.");
                            }
                        }
                    }

                    // Add Weapons
                    if (FellOfACargoShip.Settings.AddInventoryComponents)
                    {
                        foreach (string id in weaponDefIds)
                        {
                            int i = 0;
                            while (i < FellOfACargoShip.Settings.AddInventoryComponentCount)
                            {
                                __instance.AddItemStat(id, typeof(WeaponDef), false);
                                i++;
                            }
                            Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + id + "(" + FellOfACargoShip.Settings.AddInventoryComponentCount + ") to inventory.");
                        }
                    }

                    // Add upgrades
                    if (FellOfACargoShip.Settings.AddInventoryComponents)
                    {
                        foreach (string id in upgradeDefIds)
                        {
                            int i = 0;
                            while (i < FellOfACargoShip.Settings.AddInventoryComponentCount)
                            {
                                __instance.AddItemStat(id, typeof(UpgradeDef), false);
                                i++;
                            }
                            Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + id + "(" + FellOfACargoShip.Settings.AddInventoryComponentCount + ") to inventory.");
                        }
                    }

                    //Add Heatsinks
                    if (FellOfACargoShip.Settings.AddInventoryComponents)
                    {
                        foreach (string id in heatSinkDefIds)
                        {

                            int i = 0;
                            while (i < FellOfACargoShip.Settings.AddInventoryComponentCount)
                            {
                                __instance.AddItemStat(id, typeof(HeatSinkDef), false);
                                i++;
                            }
                            Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + id + "(" + FellOfACargoShip.Settings.AddInventoryComponentCount + ") to inventory.");
                        }
                    }

                    // Add Ammunition
                    if (FellOfACargoShip.Settings.AddInventoryComponents)
                    {
                        foreach (string id in ammoBoxDefIds)
                        {

                            int i = 0;
                            while (i < FellOfACargoShip.Settings.AddInventoryComponentCount)
                            {
                                __instance.AddItemStat(id, typeof(AmmunitionBoxDef), false);
                                i++;
                            }
                            Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + id + "(" + FellOfACargoShip.Settings.AddInventoryComponentCount + ") to inventory.");
                        }
                    }
                }



                // Clean list
                if (FellOfACargoShip.Settings.LogComponentLists)
                {
                    Logger.Debug("------------------------------------------------------------------------------------------------------------------------");
                    Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Generate clean, json-ready list of all valid Mechs and Components.");
                    Logger.Debug("------------------------------");
                    // Mechs
                    foreach (string id in mechDefIds)
                    {
                        Logger.Debug("\"" + id + "\",", false);
                    }
                    // Weapons
                    foreach (string id in weaponDefIds)
                    {
                        Logger.Debug("\"" + id + "\",", false);
                    }
                    // Upgrades
                    foreach (string id in upgradeDefIds)
                    {
                        Logger.Debug("\"" + id + "\",", false);
                    }
                    // Heatsinks
                    foreach (string id in heatSinkDefIds)
                    {
                        Logger.Debug("\"" + id + "\",", false);

                    }
                    // Ammunition
                    foreach (string id in ammoBoxDefIds)
                    {
                        Logger.Debug("\"" + id + "\",", false);
                    }
                    Logger.Debug("------------------------------");
                    Logger.Debug("------------------------------------------------------------------------------------------------------------------------");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    [HarmonyPatch(typeof(SimGameState), "_OnAttachUXComplete")]
    public static class SimGameState__OnAttachUXComplete_AddArgoUpgrades
    {
        public static bool Prepare()
        {
            return FellOfACargoShip.Settings.AddArgoUpgrades;
        }

        public static void Postfix(SimGameState __instance, List<ShipModuleUpgrade> ___shipUpgrades, List<string> ___purchasedArgoUpgrades, TagSet ___companyTags)
        {
            try
            {
                string[] startingArgoUpgrades;
                switch (__instance.SimGameMode)
                {
                    case SimGameState.SimGameType.KAMEA_CAMPAIGN:
                        startingArgoUpgrades = __instance.Constants.Story.StartingArgoUpgrades;
                        goto IL_100;
                    case SimGameState.SimGameType.CAREER:
                        startingArgoUpgrades = __instance.Constants.CareerMode.StartingArgoUpgrades;
                        goto IL_100;
                }
                startingArgoUpgrades = __instance.Constants.Debug.StartingArgoUpgrades;

                IL_100:

                List<string> argoUpgradesToAdd = new List<string>();
                if (FellOfACargoShip.Settings.AddArgoUpgradesList.Count > 0)
                {
                    argoUpgradesToAdd = FellOfACargoShip.Settings.AddArgoUpgradesList;
                }
                else {
                    argoUpgradesToAdd = FellOfACargoShip.Settings.AllArgoUpgrades;
                }

                if (startingArgoUpgrades != null)
                {
                    foreach (string id in startingArgoUpgrades)
                    {
                        argoUpgradesToAdd.Remove(id);
                    }
                }

                if(argoUpgradesToAdd.Count > 0)
                {
                    foreach (string id in argoUpgradesToAdd)
                    {
                        ShipModuleUpgrade upgrade = __instance.DataManager.ShipUpgradeDefs.Get(id);

                        //__instance.AddArgoUpgrade(upgrade);

                        // BEN: Custom AddArgoUpgrade (No timeline refresh, this will be applied later ONE TIME ONLY)
                        ___shipUpgrades.Add(upgrade);
                        ___purchasedArgoUpgrades.Add(upgrade.Description.Id);
                        if (__instance.CurDropship == DropshipType.Argo)
                        {
                            if (upgrade.Tags != null && !upgrade.Tags.IsEmpty)
                            {
                                ___companyTags.AddRange(upgrade.Tags);
                            }
                            foreach (SimGameStat companyStat in upgrade.Stats)
                            {
                                __instance.SetCompanyStat(companyStat);
                            }
                            if (upgrade.Actions != null)
                            {
                                SimGameResultAction[] actions = upgrade.Actions;
                                for (int i = 0; i < actions.Length; i++)
                                {
                                    SimGameState.ApplyEventAction(actions[i], null);
                                }
                            }
                        }
                        Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added Upgrade (" + id + ") to Argo.");
                    }
                    // Refresh timeline ONCE
                    __instance.RoomManager.RefreshTimeline();


                    // BEN: Refresh MechBayPanel (if an upgrade.Action possibly adds a Mech...)
                    // Probably add this to a postfix of SimGameState.AddMech()
                    /*
                    MechBayPanel mechBay = (MechBayPanel)AccessTools.Field(typeof(SGRoomController_MechBay), "mechBay").GetValue(__instance.RoomManager.MechBayRoom);
                    MechBayRowGroupWidget bayGroupWidget = (MechBayRowGroupWidget)AccessTools.Field(typeof(MechBayPanel), "bayGroupWidget").GetValue(mechBay);
                    bayGroupWidget.SetData(mechBay, __instance);
                    */
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    [HarmonyPatch(typeof(SimGameState), "_OnAttachUXComplete")]
    public static class SimGameState__OnAttachUXComplete_AddFunds
    {
        public static bool Prepare()
        {
            return FellOfACargoShip.Settings.AddFunds > 0;
        }

        public static void Postfix(SimGameState __instance)
        {
            try
            {
                __instance.AddFunds(FellOfACargoShip.Settings.AddFunds, null, true);
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + FellOfACargoShip.Settings.AddFunds + " C-Bills to inventory.");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }



    [HarmonyPatch(typeof(SimGameState), "_OnAttachUXComplete")]
    public static class SimGameState__OnAttachUXComplete_AddRoninPilots
    {
        public static bool Prepare()
        {
            return FellOfACargoShip.Settings.AddRoninPilots;
        }

        public static void Postfix(SimGameState __instance)
        {
            try
            {
                bool pilotsWillFit = __instance.PilotRoster.Count + FellOfACargoShip.Settings.AddRoninPilotsList.Count <= __instance.GetMaxMechWarriors();
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] __instance.PilotRoster.Count: " + __instance.PilotRoster.Count);
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] FellOfACargoShip.Settings.AddRoninPilotsList.Count: " + FellOfACargoShip.Settings.AddRoninPilotsList.Count);
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] __instance.GetMaxMechWarriors: " + __instance.GetMaxMechWarriors());

                if (pilotsWillFit && FellOfACargoShip.Settings.AddRoninPilotsList.Count > 0)
                {
                    foreach (string roninID in FellOfACargoShip.Settings.AddRoninPilotsList)
                    {
                        Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] roninID: " + roninID);
                        PilotDef pilotDef = __instance.DataManager.PilotDefs.Get(roninID);

                        if (pilotDef != null)
                        {
                            __instance.AddPilotToRoster(pilotDef, true, true);
                            Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + pilotDef.Description.Callsign + " to Roster");
                        }
                    }
                }
                else
                {
                    Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Failed to add Ronin: No space or List emtpy");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }



    [HarmonyPatch(typeof(SimGameState), "_OnAttachUXComplete")]
    public static class SimGameState__OnAttachUXComplete_AddXP
    {
        public static bool Prepare()
        {
            return FellOfACargoShip.Settings.AddXP > 0;
        }

        public static void Postfix(SimGameState __instance)
        {
            try
            {
                // Won't work on START_GAME as _OnAttachUXComplete was called before the Commander has finished character creation
                __instance.Commander.AddExperience(0, "LittleThings.AddXP", FellOfACargoShip.Settings.AddXP);
                Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + FellOfACargoShip.Settings.AddXP + " XP to " + __instance.Commander.Callsign + ".");

                foreach (var item in __instance.PilotRoster.Select((value, i) => new { i, value }))
                {
                    Pilot pilot = item.value;
                    int index = item.i;

                    pilot.AddExperience(index, "LittleThings.AddXP", FellOfACargoShip.Settings.AddXP);
                    Logger.Debug("[SimGameState__OnAttachUXComplete_POSTFIX] Added " + FellOfACargoShip.Settings.AddXP + " XP to " + pilot.Callsign + ".");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    [HarmonyPatch(typeof(SimGameState), "OnCharacterCreationComplete")]
    public static class SimGameState_OnCharacterCreationComplete_AddCommanderXP
    {
        public static bool Prepare()
        {
            return FellOfACargoShip.Settings.AddXP > 0;
        }

        public static void Postfix(SimGameState __instance)
        {
            try
            {
                __instance.Commander.AddExperience(0, "LittleThings.AddXP", FellOfACargoShip.Settings.AddXP);
                Logger.Debug("[SimGameState_OnCharacterCreationComplete_POSTFIX] Added " + FellOfACargoShip.Settings.AddXP + " XP to " + __instance.Commander.Callsign + ".");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    [HarmonyPatch(typeof(SimGameState), "OnCareerModeCharacterCreationComplete")]
    public static class SimGameState_OnCareerModeCharacterCreationComplete_AddCommanderXP
    {
        public static bool Prepare()
        {
            return FellOfACargoShip.Settings.AddXP > 0;
        }

        public static void Postfix(SimGameState __instance)
        {
            try
            {
                __instance.Commander.AddExperience(0, "LittleThings.AddXP", FellOfACargoShip.Settings.AddXP);
                Logger.Debug("[SimGameState_OnCareerModeCharacterCreationComplete_POSTFIX] Added " + FellOfACargoShip.Settings.AddXP + " XP to " + __instance.Commander.Callsign + ".");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
