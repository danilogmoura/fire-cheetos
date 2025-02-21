using HarmonyLib;
using Il2CppCodeStage.AntiCheat.Detectors;
using MelonLoader;

namespace FireCheetos.Hooks
{
    [HarmonyPatch(typeof(SpeedHackDetector), "Update")]
    public class Patch_SpeedHackDetector_Update
    {
        private static bool Prefix()
        {
            MelonDebug.Msg("SpeedHackDetector.Update() called");
            return false;
        }
    }
}