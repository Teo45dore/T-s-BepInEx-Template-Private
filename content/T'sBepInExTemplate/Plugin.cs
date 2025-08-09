using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TemplateName;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)] //Tells the BepInEx Chainloader to load this class.
public class Plugin : BaseUnityPlugin //BaseUnityPlugin inherits from MonoBehaviour so we can use Awake, Update, etc.
{
    public static Plugin Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony Harmony { get; set; } = null!;

    internal const string PLUGIN_GUID = "{pluginGUID}";
    internal const string PLUGIN_NAME = "TemplateName";
    internal const string PLUGIN_VERSION = "1.0.0"; //Remember to change this when developing new versions!

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        new ModConfig(base.Config);

        Patch();

        Logger.LogInfo($"{PLUGIN_GUID} v{PLUGIN_VERSION} has loaded!");
    }

    internal static void Patch()
    {
        Harmony ??= new Harmony(PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}
