namespace BTD6SaveMenu;

using Assets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;

[RegisterTypeInIl2Cpp(false)]
public class SaveBrowserFile : ModHelperComponent
{
    public SaveBrowserFile(IntPtr ptr) : base(ptr)
    {
    }

    public ModHelperButton MainButton => GetDescendent<ModHelperButton>("MainButton");
    public ModHelperImage Icon => GetDescendent<ModHelperImage>("Icon");
    public ModHelperImage ModdedIcon => GetDescendent<ModHelperImage>("ModdedIcon");

    public ModHelperText Name => GetDescendent<ModHelperText>("Name");
    public ModHelperText GameVersion => GetDescendent<ModHelperText>("GameVersion");

    public static SaveBrowserFile CreateTemplate()
    {
        var save = Main.Create<SaveBrowserFile>(new Info("SaveTemplate")
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
            Width = 290,
            Height = 290,
            Anchor = new Vector2(0, 0.5f)
        }, ModContent.GetSpriteReference<Main>("ModsBtn").guidRef);
        var modname = panel.AddText(new Info("Name", 150f, 50f, 500, 150), "Name", SaveMenu.FontMedium);
        modname.Text.enableAutoSizing = true;

        panel.AddText(new Info("GameVersion", -150f, -50, 200f, 150, new Vector2(1, 0.5f)), "Version", SaveMenu.FontSmall);

        save.SetActive(false);
        return save;
    }
}

internal static class SaveMenuModExt
{
    public static void SetupIcons(this SaveBrowserFile save, MapSaveDataModel map)
    {
        try
        {
            foreach (var mapSetMap in GameData.Instance.mapSet.maps)
            {
                if (mapSetMap.mapSprite != null && map.mapName == mapSetMap.id)
                {
                    save.Icon.Image.SetSprite(mapSetMap.mapSprite);
                    save.Icon.SetActive(true);
                }
                else if (!GameData._instance.mapSet.Maps.items.Any(x => x.id == map.mapName))
                {
                    save.ModdedIcon.Image.SetSprite(ModContent.GetSpriteReference<Main>("ModsBtn").guidRef);
                    save.ModdedIcon.SetActive(true);
                }
            }
        }
        catch (NullReferenceException) {}
}

    public static void SetSave(this SaveBrowserFile save, MapSaveDataModel map)
    {
        save.MainButton.Button.SetOnClick(() => { SaveMenu.SetSelectedSave(map); });
        var name = save.Name;
        name.SetText(map.mapName);
        if (map.mapName == "Tutorial") save.Name.SetText("MonkeyMeadow");
        
        
        var gameVersion = save.GameVersion;
        gameVersion.SetText(map.gameVersion);

        if (int.Parse(map.gameVersion.Split('.')[0]) < Main.profile.savedByGameVersion.Major || map.version < Assets.Scripts.Simulation.Utils.MapSaveLoader.LatestVersion)
        {
            gameVersion.Text.color = Color.red;
            name.Text.color = Color.red;
        }
        else if (int.Parse(map.gameVersion.Split('.')[1]) < Main.profile.savedByGameVersion.Minor)
        {
            gameVersion.Text.color = Color.yellow;
            name.Text.color = Color.yellow;
        }        
        else if (map.gameVersion == Main.profile.savedByGameVersion.Major+"."+Main.profile.savedByGameVersion.Minor)
        {
            gameVersion.Text.color = Color.green;
            name.Text.color = Color.green;
        }
        
        save.ModdedIcon.SetActive(false);
        save.Icon.SetActive(false);
        foreach (var mapSetMap in GameData.Instance.mapSet.maps)
        {
            if (map.mapName == mapSetMap.id)
            {
                save.Icon.Image.SetSprite(mapSetMap.mapSprite);     
                save.Icon.SetActive(true);

            }
            else if (!GameData._instance.mapSet.Maps.items.Any(x => x.id == map.mapName))
            {               
                save.ModdedIcon.Image.SetSprite(ModContent.GetSpriteReference<Main>("ModsBtn").guidRef); 
                save.ModdedIcon.SetActive(true);
            }
        }
        save.SetActive(true);
    }
}
