global using BTD_Mod_Helper;
global using MelonLoader;
global using BTD6SaveMenu;
using Main = BTD6SaveMenu.Main;

[assembly: MelonInfo(typeof(Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BTD6SaveMenu;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers;
using UnhollowerRuntimeLib;

public class Main : BloonsTD6Mod
{
    public static ProfileModel profile;
    public static MapSaveDataModel globalselectedsave = null;
    public static List<string> standardtowers = new();
    public static List<string> nonstandardtowers = new();
    public static List<string> standardheroes = new();
    public static Dictionary<string, TowerModel> sprites = new();

    public override void OnProfileLoaded(ProfileModel result)
    {
        base.OnProfileLoaded(result);
        profile = result;
    }

    public override void OnGameModelLoaded(GameModel model)
    {
        base.OnGameModelLoaded(model);
        nonstandardtowers.Clear();
        standardtowers.Clear();
        standardheroes.Clear();
        foreach (var tower in model.towers)
        {
            if (tower.IsHero() && !standardheroes.Contains(tower.baseId + "-" + tower.tiers[0]))
            {
                //MelonLogger.Msg("Saved Hero: " + tower.baseId + "-" + tower.tiers[0]);
                standardheroes.Add(tower.baseId + "-" + tower.tiers[0]);
                sprites[tower.baseId + "-" + tower.tiers[0]] = tower;
                continue;
            }

            if (!tower.isGeraldoItem && !tower.isSubTower && !tower.IsHero() && !standardheroes.Contains(tower.baseId + "-" + tower.tiers[0] + tower.tiers[1] + tower.tiers[2]))
            {
                //MelonLogger.Msg("Saved Tower: " + tower.baseId + "-" + tower.tiers[0] + tower.tiers[1] + tower.tiers[2]);
                standardtowers.Add(tower.baseId + "-" + tower.tiers[0] + tower.tiers[1] + tower.tiers[2]);
                sprites[tower.baseId + "-" + tower.tiers[0] + tower.tiers[1] + tower.tiers[2]] = tower;
            }
        }

        foreach (var tower in model.powers.Select(power => power.tower))
        {
            if (tower != null && tower.portrait.guidRef != null)
            {
                //MelonLogger.Msg("Saved Power: " + power.tower.baseId + "-" + power.tower.tiers[0] + power.tower.tiers[1] + power.tower.tiers[2]);
                nonstandardtowers.Add(tower.baseId + "-" + tower.tiers[0] + tower.tiers[1] + tower.tiers[2]);
                sprites[tower.baseId + "-" + tower.tiers[0] + tower.tiers[1] + tower.tiers[2]] = tower;
            }
        }
    }

    public override void OnApplicationStart()
    {
        base.OnApplicationStart();
        MelonLogger.Msg("BTD6SaveMenu Has Loaded");
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
