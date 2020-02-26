using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using Harmony;
using HBS;

namespace FellOffACargoShip
{
    internal static class PopupHelper
    {
        private static string title = "Fell Off A Cargo Ship";

        public static void Info(string message)
        {
            Fields.IsCustomPopup = true;

            GenericPopup popup = GenericPopupBuilder
                .Create(title, message)
                .AddButton("Ok", null, true, null)
                .CancelOnEscape()
                .AddFader(new UIColorRef?(LazySingletonBehavior<UIManager>.Instance.UILookAndColorConstants.PopupBackfill), 0.5f, true)
                .SetAlwaysOnTop()
                .SetOnClose(delegate
                {
                    Fields.IsCustomPopup = false;
                })
                .Render();

            LocalizableText ___contentText = (LocalizableText)AccessTools.Field(typeof(GenericPopup), "_contentText").GetValue(popup);
            ___contentText.alignment = TMPro.TextAlignmentOptions.TopLeft;
        }
    }
}
