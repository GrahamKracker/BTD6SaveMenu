namespace BTD6SaveMenu;

using System.Linq;
using Assets.Scripts.Simulation.Utils;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;
using static Main;

[RegisterTypeInIl2Cpp(false)]
public class SaveMenuPanel : ModHelperComponent
{
    public SaveMenuPanel(IntPtr ptr) : base(ptr)
    {
    }

    public ModHelperButton MainButton => GetDescendent<ModHelperButton>("MainButton");
    public ModHelperImage Icon => GetDescendent<ModHelperImage>("Icon");
    public ModHelperImage ModdedIcon => GetDescendent<ModHelperImage>("ModdedIcon");

    public ModHelperText Name => GetDescendent<ModHelperText>("Name");
    public ModHelperText GameVersion => GetDescendent<ModHelperText>("GameVersion");

    public static SaveMenuPanel CreateTemplate()
    {
        var save = Create<SaveMenuPanel>(new Info("SaveTemplate")
        {
            Height = 400,
            FlexWidth = 1
        });
        var panel = save.AddButton(new Info("MainButton", InfoPreset.FillParent), VanillaSprites.MainBGPanelBlue, null);
        panel.AddImage(new Info("Icon")
        {
            X = 250,
            Y = 0,
            Width = 390,
            Height = 290,
            Anchor = new Vector2(0, 0.5f)
        }, VanillaSprites.UISprite);
        panel.AddImage(new Info("ModdedIcon")
        {
            X = 185,
            Y = 0,
            Width = 420,
            Height = 320,
            Anchor = new Vector2(0, 0.5f)
        }, VanillaSprites.UISprite);
        var modname = panel.AddText(new Info("Name", 150f, 50f, 500, 150), "Name", SaveMenu.FontMedium);
        modname.Text.enableAutoSizing = true;

        panel.AddText(new Info("GameVersion", -150f, -50, 200f, 150, new Vector2(1, 0.5f)), "Version", SaveMenu.FontSmall);

        save.SetActive(false);
        return save;
    }
}

internal static class SaveMenuModExt
{
    public static void SetupIcons(this SaveMenuPanel save, MapSaveDataModel map)
    {
        foreach (var mapSetMap in GameData.Instance.mapSet.maps)
        {
            if (GameData._instance.mapSet.Maps.items.Any(x => x.id == map.mapName))
            {
                if (mapSetMap.mapSprite is not null && map.mapName == mapSetMap.id && mapSetMap.mapSprite.GUID is not null && mapSetMap.mapSprite.guidRef is not null && mapSetMap.mapSprite.guidRef != "" && mapSetMap.mapSprite.GUID != "" && mapSetMap.mapSprite.GetGUID() is not null && mapSetMap.mapSprite.GetGUID() != "")
                    try
                    {
                        save.Icon.Image.SetSprite(mapSetMap.mapSprite);
                        save.Icon.SetActive(true);
                    }
                    catch (Exception)
                    {
                        save.ModdedIcon.Image.SetSprite(ModContent.GetSpriteReference<Main>("UnknownMap").guidRef);
                        save.Name.Text.color = Color.red;
                        save.ModdedIcon.SetActive(true);
                        save.Icon.SetActive(false);
                    }
            }
            else
            {
                save.ModdedIcon.Image.SetSprite(ModContent.GetSpriteReference<Main>("UnknownMap").guidRef);
                save.Name.Text.color = Color.red;
                save.ModdedIcon.SetActive(true);
                save.Icon.SetActive(false);
            }
        }
    }

    public static void SetSave(this SaveMenuPanel save, MapSaveDataModel map)
    {
        save.MainButton.Button.SetOnClick(() => { SaveMenu.SetSelectedSave(map); });
        var name = save.Name;
        name.SetText(map.mapName);
        if (map.mapName == "Tutorial") save.Name.SetText("MonkeyMeadow");


        var gameVersion = save.GameVersion;
        gameVersion.SetText(map.gameVersion);

        if (int.Parse(map.gameVersion.Split('.')[0]) != profile.savedByGameVersion.Major || map.version != MapSaveLoader.LatestVersion)
            gameVersion.Text.color = Color.red;
        else if (map.gameVersion.Split('.')[1] != profile.savedByGameVersion.ToString().Split('.')[1])
            gameVersion.Text.color = Color.yellow;
        else if (map.gameVersion == profile.savedByGameVersion.Major + "." + profile.savedByGameVersion.Minor) gameVersion.Text.color = Color.green;

        var iscompatible = true;
        foreach (var tower in map.placedTowers.Where(tower => tower is not null))
        {
            
            if (Game.instance.model.GetTowerFromId(tower.baseId) != null)
                //MelonLogger.Msg("Placed Modded Tower: " + tower.baseId);
                continue;
            try
            {
                if (standardheroes.Contains(tower.baseId + "-" + tower.pathOneTier) || standardtowers.Contains(tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier) || nonstandardtowers.Contains(tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier))
                    //MelonLogger.Msg("Placed Tower: " + tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier);
                    continue;
            }
            catch
            {
            }

            try
            {
                if (Game.instance.model.GetTowerFromId(tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier) != null)
                    //MelonLogger.Msg("Placed Modded Tower: " + tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier);
                    continue;
            }
            catch
            {
            }

            bool IsValidCrosspath(int[] tiers)
            {
                var sorted = tiers.OrderByDescending(num => num).ToArray();
                return sorted[0] <= 5 && sorted[1] <= 2 && sorted[2] == 0;
            }
            bool checkucrosspathingcompat()
            {        
                int[] tiers2 = new int[2];
                //MelonLogger.Msg("Crosspath Has Mod: " + ModContent.HasMod("UltimateCrosspathing"));
                if (!ModContent.HasMod("UltimateCrosspathing"))
                    for (var i = 0; i <= tower.pathOneTier; i++) for (var j = 0; j <= tower.pathTwoTier; j++) for (var k = 0; k <= tower.pathThreeTier; k++)
                    {
                        tiers2 = new[] {i, j, k};
                        if (!IsValidCrosspath(tiers2))
                        {
                            MelonLogger.Msg("Invalid Crosspath: " + tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier);
                            return false;
                        }
                    }
                else 
                {
                    for (var i = 0; i <= tower.pathOneTier; i++) for (var j = 0; j <= tower.pathTwoTier; j++) for (var k = 0; k <= tower.pathThreeTier; k++)
                    {
                        tiers2 = new[] {i, j, k};
                        //MelonLogger.Msg("Has Crosspathing: "+ string.Join(". ", tiers2)+"\\ "+ tiers2.Sum()+", "+ ModContent.GetMod("UltimateCrosspathing").ModSettings["MaxTiers"].GetValue().ToString());
                        if (tiers2.Sum()< int.Parse(ModContent.GetMod("UltimateCrosspathing").ModSettings["MaxTiers"].GetValue().ToString()))
                        {
                            MelonLogger.Msg("Invalid Crosspath because of the MaxTiers ModSetting: " + tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier);
                            return false;
                        }
                    }
                }
                return true;
            }

            if (checkucrosspathingcompat() == false)
            {
                iscompatible = false;
                break;
            }


            if (Game.instance.model.GetTowerFromId(tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier) == null)
            {
                MelonLogger.Msg("Incompatible Tower: " + tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier);
                iscompatible = false;
                break;
            }
        }

        if (save.GameVersion.Text.color == Color.green) name.Text.color = Color.green;

        if (save.GameVersion.Text.color == Color.yellow) name.Text.color = Color.yellow;

        if (!iscompatible || save.GameVersion.Text.color == Color.red) name.Text.color = Color.red;

        save.ModdedIcon.SetActive(false);
        save.Icon.SetActive(false);
        SetupIcons(save, map);

        save.SetActive(true);
    }
}
