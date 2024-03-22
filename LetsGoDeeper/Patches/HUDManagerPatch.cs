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

            __result = false;
            interactTrigger.currentCooldownValue = 1F;

            HUDManager.Instance.DisplayTip("Entrance Door", "I'm an entrance, you fool!");
            return;
        }

        if (!entranceTeleport.isEntranceToBuilding)
            return;

        __result = false;
        interactTrigger.currentCooldownValue = 1F;
        HUDManager.Instance.DisplayTip("Fire Exit Door",
            "Fire? Where? Oh wait, you're trying to enter through an exit...");
    }
}