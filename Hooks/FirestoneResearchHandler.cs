using HarmonyLib;
using Il2Cpp;
using Il2CppCodeStage.AntiCheat.ObscuredTypes;
using MelonLoader;

namespace FireCheetos.Hooks
{
    [HarmonyPatch(typeof(BattleMainStageManager), "WavesIncreasement")]
    public static class WavesIncreasementPatch
    {
        public static bool Prefix(BattleMainStageManager __instance)
        {
            MelonDebug.Msg("Interceptado: WavesIncreasement => totalChanceToSkipWave: " +
                           __instance.totalChanceToSkipWave);

            __instance.totalChanceToSkipWave = new ObscuredDouble(0.9);
            return true;
        }
    }

    [HarmonyPatch(typeof(BattleMainStageManager), "StagesIncreasement")]
    public static class StagesIncreasementPatch
    {
        public static bool Prefix(BattleMainStageManager __instance)
        {
            MelonDebug.Msg("Interceptado: StagesIncreasement => totalChanceToSkipStage: " +
                           __instance.totalChanceToSkipStage + " | maxStageNumber: " + __instance.chanceToSkipStage);

            // __instance.totalChanceToSkipStage = new ObscuredDouble(0.6);
            return true;
        }
    }
}