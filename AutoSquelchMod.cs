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
            harmony.PatchAll(typeof(SquelchPatcher));

        }
    }

    public static class SquelchPatcher
    {
        [HarmonyPatch(typeof(EnemyEmoteHandler), "IsSquelched", new Type[] { typeof(int) })]
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result)
        {
            AutoSquelchMod.SharedLogger.Msg($"Patching IsSquelched: {__result}");
            __result = true;
            AutoSquelchMod.SharedLogger.Msg($"Returning instead: {__result}");
            return false;
        }
    }
}
