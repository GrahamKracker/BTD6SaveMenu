global using BTD_Mod_Helper;
global using MelonLoader;
global using BTD6ModdedSaveHelper;
using Main = BTD6ModdedSaveHelper.Main;

[assembly: MelonInfo(typeof(Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BTD6ModdedSaveHelper;

using UnhollowerRuntimeLib;

public partial class Main : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        base.OnApplicationStart();
        MelonLogger.Msg("BTD6ModdedSaveHelper Has Loaded");
    }

    internal static T Create<T>(Info info) where T : ModHelperComponent
    {
        var newGameObject = new GameObject(info.Name, new[] {Il2CppType.Of<RectTransform>()});
        var modHelperComponent = newGameObject.AddComponent<T>();
        modHelperComponent.initialInfo = info;
        info.Apply(modHelperComponent);
        return modHelperComponent;
    }
}