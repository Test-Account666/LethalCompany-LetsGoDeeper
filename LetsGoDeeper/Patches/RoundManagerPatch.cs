using System.Linq;
using HarmonyLib;

namespace LetsGoDeeper.Patches;

[HarmonyPatch(typeof(RoundManager))]
public static class RoundManagerPatch {
    [HarmonyPatch("GenerateNewLevelClientRpc")]
    [HarmonyPostfix]
    public static void AfterGenerateNewLevelClientRpc() {
        ExitChecker.FoundBodiesList.Clear();
        ExitChecker.foundBodies = 0;

        ExitChecker.requiredBodies = StartOfRound.Instance.allPlayerScripts.Count(playerControllerB =>
            playerControllerB != null && playerControllerB.isPlayerControlled) - 1;

        if (!LetsGoDeeper.configManager.needsToFindAllBodiesFirst.Value)
            ExitChecker.requiredBodies = 0;
    }
}