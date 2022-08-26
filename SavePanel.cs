namespace BTD6SaveMenu;

using BTD_Mod_Helper.Extensions;

public static class SavePanel
{
    private static ModHelperPanel panel;

    private static void opensavemenu()
    {
        ModGameMenu.Open<SaveMenu>();
    }

    private static void CreatePanel(GameObject screen)
    {
        panel = screen.AddModHelperPanel(new Info("SavePanel")
        {
            Anchor = new Vector2(1, 0), Pivot = new Vector2(1, 0)
        });
        var button = panel.AddButton(new Info("SavePanelButton", -2325, 440, 900, 280, new Vector2(1, 0), new Vector2(0.5f, 0)), VanillaSprites.GreenBtnLong, new Action(opensavemenu));
        button.AddImage(new Info("SavePanelImage", 140, 45, 200, 200, new Vector2(0, 0), new Vector2(0.5f, 0)), VanillaSprites.SaveGameIcon);
        button.AddText(new Info("Text", 50, 0, 1000, 200), "Saves List", 90f);
    }

    private static void Init()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var ModSavePanel = screen.FindChild("SavePanel");
        if (ModSavePanel == null)
            CreatePanel(screen.gameObject);
    }


    private static void HideButton()
    {
        panel.SetActive(false);
    }

    public static void Show()
    {
        Init();
        panel.SetActive(true);
    }

    public static void Hide()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var ModSavePanel = screen.FindChild("SavePanel");
        if (ModSavePanel != null)
            HideButton();
    }
}