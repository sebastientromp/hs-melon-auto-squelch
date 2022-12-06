using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System;

namespace AutoSquelch
{
    public class AutoSquelchMod : MelonMod
    {
        public static MelonLogger.Instance SharedLogger;

        public override void OnInitializeMelon()
        {
            AutoSquelchMod.SharedLogger = LoggerInstance;
            var harmony = this.HarmonyInstance;
            MethodInfo isSquelchedMethod = typeof(EnemyEmoteHandler).GetMethod("IsSquelched", new Type[] { typeof(int) });
            MethodInfo squelchPrefix = AccessTools.Method(typeof(SquelchPatcher), "Prefix");
            harmony.Patch(isSquelchedMethod, new HarmonyMethod(squelchPrefix));

        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
        }
    }

    public static class SquelchPatcher
    {
        public static bool Prefix(ref bool __result)
        {
            AutoSquelchMod.SharedLogger.Msg($"Patching IsSquelched: {__result}");
            __result = true;
            AutoSquelchMod.SharedLogger.Msg($"Returning instead: {__result}");
            return false;
        }
    }
}
