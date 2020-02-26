using System;

namespace FellOffACargoShip.Info
{
    class Help
    {
        public static void Show()
        {
            string message = "";
            message += "• Commands: '/list', '/mech', '/comp', '/funds', '/xp', '/upgr', '/ronin', '/rep'";
            message += Environment.NewLine;
            message += "• All commands require at least one parameter separated by space";
            message += Environment.NewLine;
            message += "Providing 'help' as first parameter will explain what that command will do and provide an example";
            PopupHelper.Info(message);
        }
    }
}
