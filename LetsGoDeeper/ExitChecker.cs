using System.Collections.Generic;
using System.Linq;

namespace LetsGoDeeper;

public static class ExitChecker {
    internal static readonly List<ulong> FoundBodiesList = [];
    internal static int foundBodies = 0;
    internal static int requiredBodies = 0;

    public static bool CanExit() {
        var alivePeople = StartOfRound.Instance.allPlayerScripts.Count(playerScript =>
            !playerScript.isPlayerDead && playerScript.isPlayerControlled);

        if (alivePeople > 1)
            return false;

        return foundBodies >= requiredBodies;
    }
}