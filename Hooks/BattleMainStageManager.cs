using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace FireCheetos.Hooks
{
    [HarmonyPatch(typeof(BattleMainStageManager), "CalculateWaveGold")]
    public static class CalculateWaveGoldPatch
    {
        public static bool Prefix(BattleMainStageManager __instance, ref int CurrentStageNumber, int CurrentWaveNumber)
        {
            MelonDebug.Msg("Interceptado: CalculateWaveGold => CurrentStageNumber: " + CurrentStageNumber +
                           " | CurrentWaveNumber: " + CurrentWaveNumber);

            if (!Main.feature6Enabled) return true;

            CurrentStageNumber += 10;
            return true;
        }
    }
}