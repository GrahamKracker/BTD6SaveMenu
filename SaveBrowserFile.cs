namespace BTD6SaveMenu;

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
    public static void SetSave(this SaveBrowserFile save, MapSaveDataModel map)
    {
        save.MainButton.Button.SetOnClick(() => { SaveMenu.SetSelectedSave(map); });
        save.Name.SetText(map.mapName);
        if (map.mapName == "Tutorial") save.Name.SetText("MonkeyMeadow");
        save.GameVersion.SetText(map.gameVersion);
        foreach (var mapSetMap in GameData.Instance.mapSet.maps)
        {
            if (map.mapName == mapSetMap.id)
            {
                save.ModdedIcon.SetActive(false);
                save.Icon.SetActive(true);
                save.Icon.Image.SetSprite(mapSetMap.mapSprite);
            }
            else if (!GameData._instance.mapSet.Maps.items.Any(x => x.id == map.mapName))
            {
                save.Icon.SetActive(false);
                save.ModdedIcon.SetActive(true);
                save.ModdedIcon.Image.SetSprite(ModContent.GetSpriteReference<Main>("ModsBtn").guidRef);
            }
        }

        save.SetActive(true);
    }
}
