using BepInEx.Configuration;
using HarmonyLib;

namespace LetsGoDeeper.Patches;

[HarmonyPatch(typeof(HUDManager))]
public class HUDManagerPatch {
    [HarmonyPatch("HoldInteractionFill")]
    [HarmonyPostfix]
    private static void DestroyEntranceExitAbility(ref bool __result) {
        if (!__result)
            return;

        var localPlayer = HUDManager.Instance.playersManager.localPlayerController;

        var interactTrigger = localPlayer.hoveringOverTrigger;

        if (interactTrigger == null)
            return;

        var entranceTeleport = interactTrigger.gameObject.GetComponent<EntranceTeleport>();

        if (entranceTeleport == null)
            return;

        if (entranceTeleport.entranceId == 0) {
            if (entranceTeleport.isEntranceToBuilding)
                return;

            if (LetsGoDeeper.configManager.allowMainEntranceExit.Value)
                return;

            if (ExitChecker.CanExit() && LetsGoDeeper.configManager.allowExitIfLastOneAlive.Value)
                return;

            __result = false;
            interactTrigger.currentCooldownValue = 1F;

            HUDManager.Instance.DisplayTip("Entrance Door", "I'm an entrance, you fool!");
            return;
        }

        if (!entranceTeleport.isEntranceToBuilding)
            return;

        if (LetsGoDeeper.configManager.allowFireExitEnter.Value)
            return;

        __result = false;
        interactTrigger.currentCooldownValue = 1F;
        HUDManager.Instance.DisplayTip("Fire Exit Door",
            "Fire? Where? Oh wait, you're trying to enter through an exit...");
    }

    [HarmonyPatch("DisplayNewScrapFound")]
    [HarmonyPostfix]
    public static void AfterDisplayNewScrapFound() {
        if (HUDManager.Instance.itemsToBeDisplayed.Count <= 0)
            return;

        foreach (var grabbableObject in HUDManager.Instance.itemsToBeDisplayed) {
            if (grabbableObject is not RagdollGrabbableObject ragdollGrabbableObject)
                continue;

            if (ragdollGrabbableObject.GetComponent<FakeBody>() != null)
                continue;

            var player = ragdollGrabbableObject.ragdoll.playerScript;

            if (player is null)
                continue;

            var clientId = player.playerClientId;

            if (ExitChecker.FoundBodiesList.Contains(clientId))
                continue;

            ExitChecker.FoundBodiesList.Add(clientId);
            ExitChecker.foundBodies += 1;
        }
    }
}