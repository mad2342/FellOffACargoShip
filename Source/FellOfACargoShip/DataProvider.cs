using System.Collections.Generic;
using BattleTech;
using HBS;
using HBS.Data;

namespace FellOfACargoShip
{
    internal class DataProvider
    {
        private static SimGameState simGameState = SceneSingletonBehavior<UnityGameInstance>.Instance.Game.Simulation;

        private IDataItemStore<string, ChassisDef> ChassisDefs = simGameState.DataManager.ChassisDefs;
        private IDataItemStore<string, WeaponDef> WeaponDefs = simGameState.DataManager.WeaponDefs;
        private IDataItemStore<string, UpgradeDef> UpgradeDefs = simGameState.DataManager.UpgradeDefs;
        private IDataItemStore<string, HeatSinkDef> HeatSinkDefs = simGameState.DataManager.HeatSinkDefs;
        private IDataItemStore<string, AmmunitionBoxDef> AmmoBoxDefs = simGameState.DataManager.AmmoBoxDefs;

        private List<string> chassisDefBlacklist = new List<string> {
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

        private List<string> weaponDefBlacklist = new List<string> {
            "Weapon_MeleeAttack",
            "Weapon_DFAAttack",
            "Weapon_Laser_AI_Imaginary",
            "Weapon_Mortar_Thumper",
            "Weapon_Mortar_MechMortar",
            "Weapon_Autocannon_AC20_SPECIAL-Victoria",
            "Weapon_Flamer_Flamer_SPECIAL-Victoria"
        };

        private List<string> upgradeDefBlacklist = new List<string> {
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

        private List<string> heatSinkDefBlacklist = new List<string>
        {

        };

        private List<string> ammoBoxDefBlacklist = new List<string> {
            "AmmunitionBox_Flamer",
            "Ammo_AmmunitionBox_Generic_Flamer",
            "Ammo_AmmunitionBox_Generic_SRMInferno",
            "Ammo_AmmunitionBox_Generic_Narc"
        };

        public List<string> ArgoUpgradeIds = new List<string>()
        {
            "argoUpgrade_drive0",
            "argoUpgrade_drive1",
            "argoUpgrade_drive2",
            "argoUpgrade_mechBay_automation1",
            "argoUpgrade_mechBay_automation2",
            "argoUpgrade_mechBay_machineShop",
            "argoUpgrade_mechBay_refitHarness",
            "argoUpgrade_mechBay_scaffolding",
            "argoUpgrade_mechBay1",
            "argoUpgrade_mechBay2",
            "argoUpgrade_mechBay3",
            "argoUpgrade_medBay_hospital",
            "argoUpgrade_medBay1",
            "argoUpgrade_medBay2",
            "argoUpgrade_medBay3",
            "argoUpgrade_pod1",
            "argoUpgrade_pod2",
            "argoUpgrade_pod3",
            "argoUpgrade_power1",
            "argoUpgrade_power2",
            "argoUpgrade_power3",
            "argoUpgrade_rec_arcade",
            "argoUpgrade_rec_gym",
            "argoUpgrade_rec_hydroponics",
            "argoUpgrade_rec_library1",
            "argoUpgrade_rec_library2",
            "argoUpgrade_rec_lounge1",
            "argoUpgrade_rec_lounge2",
            "argoUpgrade_rec_pool",
            "argoUpgrade_structure0",
            "argoUpgrade_structure1",
            "argoUpgrade_structure2",
            "argoUpgrade_trainingModule1",
            "argoUpgrade_trainingModule2",
            "argoUpgrade_trainingModule3"
        };
        public List<string> MechDefIds = new List<string>();
        public List<string> WeaponDefIds = new List<string>();
        public List<string> UpgradeDefIds = new List<string>();
        public List<string> HeatSinkDefIds = new List<string>();
        public List<string> AmmoBoxDefIds = new List<string>();

        public DataProvider()
        {
            // Collect Mechs
            Logger.Debug("[DataProvider] Collecting all valid Mechs");
            foreach (string chassisDefId in ChassisDefs.Keys)
            {
                if (!chassisDefBlacklist.Contains(chassisDefId))
                {
                    string id = chassisDefId.Replace("chassisdef", "mechdef");
                    MechDefIds.Add(id);
                }
            }
            MechDefIds.Sort();

            // Collect Weapons
            Logger.Debug("[DataProvider] Collecting all valid Weapons");
            foreach (string id in WeaponDefs.Keys)
            {
                if (!weaponDefBlacklist.Contains(id))
                {
                    WeaponDefIds.Add(id);
                }
            }
            WeaponDefIds.Sort();

            // Collect Upgrades
            Logger.Debug("[DataProvider] Collecting all valid Upgrades");
            foreach (string id in UpgradeDefs.Keys)
            {
                if (!upgradeDefBlacklist.Contains(id))
                {
                    UpgradeDefIds.Add(id);
                }
            }
            UpgradeDefIds.Sort();

            // Collect Heatsinks
            Logger.Debug("[DataProvider] Collecting all valid Heatsinks");
            foreach (string id in HeatSinkDefs.Keys)
            {
                if (!heatSinkDefBlacklist.Contains(id))
                {
                    HeatSinkDefIds.Add(id);
                }
            }
            HeatSinkDefIds.Sort();

            // Collect Ammunition
            Logger.Debug("[DataProvider] Collecting all valid AmmunitionBoxes");
            foreach (string id in AmmoBoxDefs.Keys)
            {
                if (!ammoBoxDefBlacklist.Contains(id))
                {
                    AmmoBoxDefIds.Add(id);
                }
            }
            AmmoBoxDefIds.Sort();
        }

        public void ListArgoUpgrades()
        {
            Logger.Debug("--- ArgoUpgradeIds");
            foreach (string id in ArgoUpgradeIds)
            {
                Logger.Debug("\"" + id + "\",", false);
            }
            Logger.Debug("---");
        }

        public void ListMechs()
        {
            Logger.Debug("--- MechDefIds");
            foreach (string id in MechDefIds)
            {
                Logger.Debug("\"" + id + "\",", false);
            }
            Logger.Debug("---");
        }

        public void ListWeapons()
        {
            Logger.Debug("--- WeaponDefIds");
            foreach (string id in WeaponDefIds)
            {
                Logger.Debug("\"" + id + "\",", false);
            }
            Logger.Debug("---");
        }

        public void ListUpgrades()
        {
            Logger.Debug("--- UpgradeDefIds");
            foreach (string id in UpgradeDefIds)
            {
                Logger.Debug("\"" + id + "\",", false);
            }
            Logger.Debug("---");
        }

        public void ListHeatsinks()
        {
            Logger.Debug("--- HeatSinkDefIds");
            foreach (string id in HeatSinkDefIds)
            {
                Logger.Debug("\"" + id + "\",", false);
            }
            Logger.Debug("---");
        }

        public void ListAmmoBoxes()
        {
            Logger.Debug("--- AmmoBoxDefIds");
            foreach (string id in AmmoBoxDefIds)
            {
                Logger.Debug("\"" + id + "\",", false);
            }
            Logger.Debug("---");
        }

        public void ListAll()
        {
            Logger.Debug("------------------------------------------------------------------------------------------------------------------------");
            Logger.Debug("[DataProvider_ListAll] Generate clean, json-ready list of all valid ArgoUpgrades, Mechs and Components.");
            Logger.Debug("------------------------------");

            // ArgoUpgrades
            ListArgoUpgrades();

            // Mechs
            ListMechs();

            // Weapons
            ListWeapons();

            // Upgrades
            ListUpgrades();

            // Heatsinks
            ListHeatsinks();

            // AmmoBoxes
            ListAmmoBoxes();

            Logger.Debug("------------------------------");
            Logger.Debug("------------------------------------------------------------------------------------------------------------------------");
        }
    }
}
