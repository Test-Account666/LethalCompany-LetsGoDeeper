using System.Collections.Generic;
using JetBrains.Annotations;

namespace LetsGoDeeper;

public static class ExitChecker {
    internal static readonly List<ulong> FoundBodiesList = [];
    internal static int foundBodies = 0;
    internal static int requiredBodies = 0;

    public static bool CanExit() {
        return foundBodies >= requiredBodies;
    }
}