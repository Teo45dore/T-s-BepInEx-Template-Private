using HarmonyLib;
using UnityEngine;

namespace TemplateName;

[HarmonyPatch(typeof(ExampleClass))] //Specifies the type to patch.
internal class ExamplePatch
{
    [HarmonyPatch("Awake")] //Specifies the method to patch, only works with declared methods.
    [HarmonyPostfix] //Will run right before the original method returns.
    private static void AwakePostfix(ExampleClass __instance) //__instance parameter passes the ExampleClass instance.
    {
        __instance.SetBind(KeyCode.UpArrow);
    }
}

internal class ExampleClass : MonoBehaviour
{
    private KeyCode walkForward;

    private void Awake()
    {
        this.SetBind(KeyCode.S);
        this.SetBind(KeyCode.W);

        //ExamplePatch.AwakePostfix(this) <-- Will be inserted by our patch.

        return;
    }

    private void Start()
    {
        if (this.walkForward == KeyCode.W) {
            Plugin.Logger.LogDebug("Was not patched.");
        } else {
            Plugin.Logger.LogDebug("Was patched!");
        }

        //Prints: Was patched!
    }

    public void SetBind(KeyCode code)
    {
        this.walkForward = code;
    }
}