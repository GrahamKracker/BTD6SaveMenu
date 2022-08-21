namespace BTD6ModdedSaveHelper;

using BTD_Mod_Helper.Extensions;

public class SavePanel
{
    private static ModHelperPanel panel;
    private static ModHelperButton button;
    private static ModHelperImage image;

    private static void opensavemenu()
    {
        ModGameMenu.Open<SaveMenu>();
    }

    private static void CreatePanel(GameObject screen)
    {
        panel = screen.AddModHelperPanel(new Info("ModSavePanel")
        {
            Anchor = new Vector2(1, 0), Pivot = new Vector2(1, 0)
        });
        var animator = panel.AddComponent<Animator>();
        animator.runtimeAnimatorController = Animations.PopupAnim;

        button = panel.AddButton(new Info("SaveChangerLong", -2325, 440, 900, 280, new Vector2(1, 0), new Vector2(0.5f, 0)), VanillaSprites.GreenBtnLong, new Action(opensavemenu));
        image = button.AddImage(new Info("ModSavePanelImage", 140, 45, 200, 200, new Vector2(0, 0), new Vector2(0.5f, 0)), VanillaSprites.SaveGameIcon);
        button.AddText(new Info("Text", 50, 0, 1000, 200), "Saves List", 90f);
    }

    private static void Init()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var ModSavePanel = screen.FindChild("ModSavePanel");
        if (ModSavePanel == null)
            CreatePanel(screen.gameObject);
    }

    private static void RevealButton()
    {
        panel.GetComponent<Animator>().Play("PopupSlideIn");
        panel.SetActive(true);
    }

    private static void HideButton()
    {
        panel.GetComponent<Animator>().Play("PopupSlideOut");
        panel.SetActive(false);
    }

    public static void Show()
    {
        Init();
        RevealButton();
    }

    public static void Hide()
    {
        if (panel != null) HideButton();
    }
}
