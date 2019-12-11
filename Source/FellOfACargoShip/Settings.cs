using System.Collections.Generic;

namespace FellOfACargoShip
{
    internal class Settings
    {
        public bool AddInventory = false;
        public bool AddInventoryMechs = false;
        public List<string> AddInventoryMechsList = new List<string>();
        public bool AddInventoryComponents = true;
        public int AddInventoryComponentCount = 10;

        public int AddFunds = 15000000;
        public int AddXP = 15000;

        public bool LogComponentLists = true;
    }
}
