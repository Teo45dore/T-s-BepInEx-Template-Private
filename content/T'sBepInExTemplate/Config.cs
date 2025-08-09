using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace TemplateName;

//This class uses BepInEx's default config system to allow the user to customize aspects of your mod that you pick.
//e.g. In a mod that changes the damage value of a weapon to a config value that the user can adjust!

//If your mod doesn't need a config then go ahead and delete this file (and the line in Plugin.cs instantiating the config!).
internal class ModConfig : IDisposable
{
    private ConfigFile cfg;
    private FileSystemWatcher watcher;
    private bool isDisposing;
    public static ModConfig Instance { get; set; } = null!;
    public static ConfigEntry<float> exampleFloatConfig { get; set; } = null!;

    public ModConfig(ConfigFile cfg)
    {
        Instance = this;
        this.cfg = cfg;

        this.cfg.SaveOnConfigSet = false; //We disable SaveOnConfigSet when binding because its generally not very performant.

        //Initialize the config entry. Repeat for any additional ConfigEntrys you might have adjusting the parameters as necessary.
        exampleFloatConfig = this.cfg.Bind(
            "General", //The section of the config option.
            "FloatConfigExample", //The name of the config option.
            1f, //The default value (must be the same type as the ConfigEntry).
            new ConfigDescription("<Desciption>", new AcceptableValueRange<float>(0f, 100f)) //Or just put a string here if you don't need a limited range.
        );

        this.RemoveOrphanedEntries();

        this.cfg.Save(); //We use Save() here because we disabled SaveOnConfigSet.

        this.cfg.SaveOnConfigSet = true; //We re-enable SaveOnConfigSet after binding.

        //Sets up a FileSystemWatcher to be used for checking if the user has made any changes to the config file.
        this.watcher = new FileSystemWatcher(Paths.ConfigPath) {
            Filter = Plugin.PLUGIN_GUID + ".cfg",
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };

        //Subscribe the ReloadConfig method to the watcher's Changed event, firing whenever the user edits the config.
        this.watcher.Changed += this.ReloadConfig;
    }

    public void ReloadConfig(object o, FileSystemEventArgs args)
    {
        this.cfg.Reload(); //Reload the config so it can see the changes made.

        this.cfg.SaveOnConfigSet = false;

        //Use the Bind method again to set the ConfigEntry's value to the value found in the reloaded config file.
        exampleFloatConfig.Value = this.cfg.Bind(exampleFloatConfig.Definition.Section,
             exampleFloatConfig.Definition.Key, (float)exampleFloatConfig.DefaultValue, //Make sure to cast the default value to the ConfigEntry's value type.
             exampleFloatConfig.Description).Value;

        //We don't use cfg.Save() here because we aren't writing anything to the file here, the user has already made the changes and we don't want to create a loop of watcher events.

        this.cfg.SaveOnConfigSet = true;

        Plugin.Logger.LogDebug($"New value = {exampleFloatConfig.Value}!");
    }

    void RemoveOrphanedEntries() //Uses reflection to remove any ConfigEntrys in the ConfigFile that are no longer in use.
    {
        PropertyInfo orphanedEntriesProperty = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
        Dictionary<ConfigDefinition, string> orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProperty.GetValue(this.cfg);
        orphanedEntries.Clear();
    }

    //IDisposable pattern since we have a FileSystemWatcher.
    public void Dispose()
    {
        this.Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.isDisposing) return;

        if (disposing) {
            this.watcher.Changed -= this.ReloadConfig;
            this.watcher.Dispose();
        }

        this.isDisposing = true;
    }
}
