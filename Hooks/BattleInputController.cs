using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace FireCheetos.Hooks
{
    [HarmonyPatch(typeof(BattleInputController), "UpdateKeyboardInput")]
    public static class BattleGuardianManager_UpdateKeyboardInput
    {
        public static bool Prefix(BattleInputController __instance)
        {
            if (!Main.feature1Enabled) return true;

            if (!__instance.battleGuardianManager.GuardianProjectileManager.IsGuardianProjectileAvailable())
                return false;

            var leaderTarget = __instance.battleController.GetLeaderTarget();
            if (leaderTarget != null)
            {
                __instance.currentTouchPos.Set(leaderTarget.position.x, leaderTarget.position.y, 0f);
                __instance.SpawnGuardianProjectile();
                return false;
            }

            var num = (float)RandomNumGenerator.NewRandomInt(0, Screen.width - 1);
            var num2 = (float)RandomNumGenerator.NewRandomInt(0, Screen.height - 1);
            __instance.currentTouchPos.Set(num, num2, 0f);
            __instance.SpawnGuardianProjectile();

            return false;
        }
    }
}