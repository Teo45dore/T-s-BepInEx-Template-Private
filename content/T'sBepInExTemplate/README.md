# T's BepInEx Template


Thanks for using my BepInEx plugin template!

I tried to make this template understandable for beginners, and convenient for more experienced modders.

Below are descriptions of the main files/folders in this template and some tips, happy modding!


## FILES:


Plugin.cs: Contains basic initialization code for BepInEx plugins,
you can read more about how it works at the [BepInEx documentation](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/2_plugin_start.html)


Patches.cs: Contains 2 example classes, one is a basic class that inherits from MonoBehaviour,
and the other is a class that [patches](https://harmony.pardeike.net/articles/patching.html) the Awake method from the first class to change to change some logic.


To learn more about patching with Harmony some good resources are the [HarmonyX wiki](https://github.com/BepInEx/HarmonyX/wiki) and the [Harmony documentation](https://harmony.pardeike.net/articles/intro.html)


Config.cs: Contains a config setup that allows a user to change the value of a [ConfigEntry](https://docs.bepinex.dev/api/BepInEx.Configuration.ConfigEntry-1.html), and for it to automatically refresh during runtime whenever the user edits the .cfg file.
[Learn more about BepInEx's config system](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/4_configuration.html)


## Folders:


game libs: Automatically references any assemblies put inside of it, as the name suggests it's useful for referencing any game assemblies you need that aren't on [NuGet](https://www.nuget.org)


## Tips:


**Versioning**


BepInEx's versioning format is [semantic versioning](https://semver.org) so keep that in mind when updating your mods version.


To change your plugin's version change the Plugin.PLUGIN_VERSION string in Plugin.cs and modify the TemplateName.csproj file:



```xml
	<!--Replace with current version of project-->
	<Version>1.0.0</Version>
```



**Logging**


It's useful to see whats happening during runtime and you can use the provided Plugin.Logger to achieve this.

First enable the [Console.Logging] section in the BepInEx.cfg file:



```toml
[Logging.Console]

## Enables showing a console for log output.
# Setting type: Boolean
# Default value: false
Enabled = true
```



now when you launch the game a console window will appear, this is where you will see your messages.


In your code you can call Plugin.Logger.LogDebug() to print something to the console window, you can think of it as Console.WriteLine().



***When logging please use Plugin.Logger.LogDebug whenever possible***



This is because if we frequently print things this can have a noticable performence impact for users,

the debug logging channel is disabled by default so users wont see the logs and wont get their game lagged.


Enable the Debug logging channel in the BepInEx.cfg file:


```toml
[Logging.Console]

...

## Which log levels to show in the console output.
# Setting type: LogLevel
# Default value: Fatal, Error, Warning, Message, Info
# Acceptable values: None, Fatal, Error, Warning, Message, Info, Debug, All
# Multiple values can be set at the same time by separating them with , (e.g. Debug, Warning)
LogLevels = All
```