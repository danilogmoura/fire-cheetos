using HarmonyLib;
using Il2Cpp;

namespace FireCheetos.Hooks
{
    [HarmonyPatch(typeof(TempleOfEternalsMech), "ActivatePrestige")]
    public static class TempleOfEternalsMechPatch
    {
        private static bool Prefix(ref Firestones.PrestigeType PrestigeType)
        {
            if (!Main.feature4Enabled) return true;

            PrestigeType = Firestones.PrestigeType.Epic;
            return true;
        }
    }
}