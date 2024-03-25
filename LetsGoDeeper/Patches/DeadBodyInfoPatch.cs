using HarmonyLib;

namespace LetsGoDeeper.Patches;

[HarmonyPatch(typeof(DeadBodyInfo))]
public static class DeadBodyInfoPatch {
    [HarmonyPatch("DetectIfSeenByLocalPlayer")]
    [HarmonyPrefix]
    public static void BeforeDetectIfSeenByLocalPlayer(DeadBodyInfo __instance, out bool __state) {
        __state = false;

        if (__instance.seenByLocalPlayer)
            return;

        if (__instance.playerScript is null)
            return;

        if (__instance.grabBodyObject.GetComponent<FakeBody>() != null)
            return;

        if (ExitChecker.FoundBodiesList.Contains(__instance.playerScript.playerClientId))
            return;

        __state = true;
    }

    [HarmonyPatch("DetectIfSeenByLocalPlayer")]
    [HarmonyPostfix]
    public static void BeforeDetectIfSeenByLocalPlayer(DeadBodyInfo __instance, bool __state) {
        if (!__state)
            return;

        if (!__instance.seenByLocalPlayer)
            return;

        if (__instance.playerScript is null)
            return;
        
        if (__instance.grabBodyObject.GetComponent<FakeBody>() != null)
            return;

        if (ExitChecker.FoundBodiesList.Contains(__instance.playerScript.playerClientId))
            return;

        ExitChecker.FoundBodiesList.Add(__instance.playerScript.playerClientId);
        ExitChecker.foundBodies += 1;
    }
}