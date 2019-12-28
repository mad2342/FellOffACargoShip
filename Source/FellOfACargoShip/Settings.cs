using System.Collections.Generic;

namespace FellOfACargoShip
{
    internal class Settings
    {

        public bool LogComponentLists = true;

        public bool AddInventory = true;
        public bool AddInventoryMechs = false;
        public bool AddInventoryMechsSilently = true;
        public List<string> AddInventoryMechsList = new List<string>();
        public bool AddInventoryComponents = true;
        public int AddInventoryComponentCount = 10;

        public int AddFunds = 15000000;
        public int AddXP = 15000;

        public bool AddArgoUpgrades = true;
        public List<string> AddArgoUpgradesList = new List<string>();
        public List<string> AllArgoUpgrades = new List<string>()
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

        public bool AddRoninPilots = true;
        public List<string> AddRoninPilotsList = new List<string>();
    }
}
