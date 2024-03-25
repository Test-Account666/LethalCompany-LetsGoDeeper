using BepInEx.Configuration;

namespace LetsGoDeeper;

public class ConfigManager {
    internal ConfigEntry<bool> allowExitIfLastOneAlive = null!;
    internal ConfigEntry<bool> needsToFindAllBodiesFirst = null!;
    internal ConfigEntry<bool> allowFireExitEnter = null!;
    internal ConfigEntry<bool> allowMainEntranceExit = null!;

    internal void Setup(ConfigFile configFile) {
        allowExitIfLastOneAlive = configFile.Bind("General", "1. Allow exit if last one alive", false,
            "If true, will allow using any way to exit is possible, if you're the last one alive.");
        needsToFindAllBodiesFirst = configFile.Bind("General", "2. Only if all bodies found", false,
            "If true, will only allow the above option, if all bodies have been discovered. (If a player died without a body, it will count as seen)");

        allowFireExitEnter = configFile.Bind("General", "3. Allow Fire Exit Enter", false,
            "If true, will allow you to enter through fire exits");

        allowMainEntranceExit = configFile.Bind("General", "4. Allow Main Entrance Exit", false,
            "If true, will allow you to leave through the main entrance");
    }
}