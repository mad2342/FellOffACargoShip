using Harmony;
using System.Reflection;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using UnityEngine;
using TMPro;
using HBS;

namespace FellOffACargoShip
{
    public static class FellOffACargoShip
    {
        internal static string LogPath;
        internal static string ModDirectory;
        internal static Settings Settings;
        // BEN: DebugLevel (0: nothing, 1: error, 2: debug, 3: info)
        internal static int DebugLevel = 1;

        public static void Init(string directory, string settings)
        {
            ModDirectory = directory;
            LogPath = Path.Combine(ModDirectory, "FellOffACargoShip.log");

            Logger.Initialize(LogPath, DebugLevel, ModDirectory, nameof(FellOffACargoShip));

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
            HarmonyInstance harmony = HarmonyInstance.Create("de.mad.FellOffACargoShip");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }



    [HarmonyPatch(typeof(HeraldryNameWidget), "OnNameInputEndEdit")]
    public static class HeraldryNameWidget_OnNameInputEndEdit_Patch
    {
        public static void Postfix(HeraldryNameWidget __instance)
        {
            try
            {
                HBS_InputField nameInput = (HBS_InputField)AccessTools.Field(typeof(HeraldryNameWidget), "nameInput").GetValue(__instance);
                string currentHeraldryName = nameInput.text;

                if (string.IsNullOrEmpty(currentHeraldryName))
                {
                    Logger.Debug("[HeraldryNameWidget_OnNameInputEndEdit_POSTFIX] No input");
                    return;
                }
                if (currentHeraldryName.Length > 0 && currentHeraldryName.Substring(0, 1) != "/")
                {
                    Logger.Debug("[HeraldryNameWidget_OnNameInputEndEdit_POSTFIX] No command");
                    return;
                }

                List<string> validCommands = new List<string>() { "/help", "/list", "/mech", "/comp", "/funds", "/xp", "/upgr", "/ronin", "/rep", "/travel" };
                string command = validCommands.FirstOrDefault(c => currentHeraldryName.Contains(c));

                if (command != null)
                {
                    Logger.Debug("[HeraldryNameWidget_OnNameInputEndEdit_POSTFIX] Command: " + command);

                    string param = currentHeraldryName.Replace(command, "").Trim();

                    if (command != "/help" && String.IsNullOrEmpty(param))
                    {
                        Logger.Debug("[HeraldryNameWidget_OnNameInputEndEdit_POSTFIX] Recognized a command but no param was given");
                        PopupHelper.Info("Recognized a command but no param was given");
                        return;
                    }
                        
                    switch(command)
                    {
                        case "/help":

                            Info.Help.Show();

                            return;

                        case "/list":

                            Info.Data.List(param);

                            return;

                        case "/mech":

                            Cheater.Mech.Add(param);

                            return;

                        case "/comp":

                            Cheater.Component.Add(param);

                            return;

                        case "/rep":

                            Cheater.Reputation.Add(param);

                            return;

                        case "/funds":

                            Cheater.Funds.Add(param);

                            return;

                        case "/xp":

                            Cheater.Experience.Add(param);

                            return;

                        case "/upgr":

                            Cheater.Upgrade.Add(param);

                            return;

                        case "/ronin":

                            Cheater.Ronin.Add(param);

                            return;

                        case "/travel":

                            Cheater.Travel.To(param);

                            return;

                        default:
                            return;
                    }
                }
                else
                {
                    Logger.Debug("[HeraldryNameWidget_OnNameInputEndEdit_POSTFIX] Unknown command");
                    PopupHelper.Info("Unknown command");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }



    [HarmonyPatch(typeof(HeraldryNameWidget), "SetData")]
    public static class HeraldryNameWidget_SetData_Patch
    {
        public static void Prefix(HeraldryNameWidget __instance)
        {
            try
            {
                Logger.Debug("[HeraldryNameWidget_SetData_PREFIX] Disable text validation, expand character limit");

                HBS_InputField nameInput = (HBS_InputField)AccessTools.Field(typeof(HeraldryNameWidget), "nameInput").GetValue(__instance);
                nameInput.characterLimit = 80;
                nameInput.contentType = HBS_InputField.ContentType.Standard;
                nameInput.characterValidation = HBS_InputField.CharacterValidation.None;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }



    [HarmonyPatch(typeof(GenericPopup), "HandleEnterKeypress")]
    public static class GenericPopup_HandleEnterKeypress_Patch
    {
        public static bool Prefix(GenericPopup __instance)
        {
            Logger.Debug("[GenericPopup_HandleEnterKeypress_PREFIX] Fields.IsCustomPopup: " + Fields.IsCustomPopup);

            if (Fields.IsCustomPopup)
            {
                Logger.Debug("[GenericPopup_HandleEnterKeypress_PREFIX] Disable EnterKeyPress");
                return false;
            }
            return true;
        }
    }



    [HarmonyPatch(typeof(HeraldryCreatorPanel), "HandleEnterKeypress")]
    public static class HeraldryCreatorPanel_HandleEnterKeypress_Patch
    {
        public static bool Prefix(HeraldryCreatorPanel __instance)
        {
            Logger.Debug("[HeraldryCreatorPanel_HandleEnterKeypress_PREFIX] Disable EnterKeyPress");
            return false;
        }
    }
}
