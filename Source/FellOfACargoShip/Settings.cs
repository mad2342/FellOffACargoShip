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
        public int AddInventoryFunds = 3000000;

        public bool LogComponentLists = true;
    }
}
