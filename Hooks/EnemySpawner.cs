using HarmonyLib;
using Il2Cpp;

namespace FireCheetos.Hooks
{
    [HarmonyPatch(typeof(EnemySpawner), "GetEnemy")]
    public static class EnemySpawnerPatch
    {
        private static void Postfix(EnemySpawner __instance)
        {
            if (!Main.feature3Enabled) return;

            foreach (var instanceBattleEnemy in __instance.battleEnemies)
            {
                instanceBattleEnemy.currentHealth = 1;
                instanceBattleEnemy.damageTotal = 1;
                instanceBattleEnemy.armorTotal = 1;
            }
        }
    }
}