using HarmonyLib;
using UnityEngine;

namespace LetsGoDeeper.Patches;

[HarmonyPatch(typeof(RagdollGrabbableObject))]
public static class RagdollPatch {
    [HarmonyPatch("Update")]
    [HarmonyPostfix]
    public static void AfterUpdate(RagdollGrabbableObject __instance) {
        if (__instance.gameObject.GetComponent<FakeBody>() is not null)
            return;

        var bodyScanNode = __instance.transform.Find("ScanNode");
        if (bodyScanNode is not null)
            if (bodyScanNode.gameObject.activeSelf)
                return;

        var bodyMapDot = __instance.transform.Find("MapDot");
        if (bodyMapDot is not null)
            if (bodyMapDot.gameObject.activeSelf)
                return;

        var bodyBoxCollider = __instance.GetComponent<BoxCollider>();
        if (bodyBoxCollider is not null)
            if (bodyBoxCollider.enabled)
                return;

        __instance.gameObject.AddComponent<FakeBody>();
    }
}