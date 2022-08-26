namespace BTD6SaveMenu;

using BTD_Mod_Helper.Extensions;

public class SaveButton
{
    private static ModHelperPanel buttonPanel;
    private static ModHelperButton button;

    private static void opensavemenu()
    {
        ModGameMenu.Open<SaveMenu>();
    }

    private static void CreatePanel(GameObject screen)
    {
        buttonPanel = screen.AddModHelperPanel(new Info("SaveChangerPanel")
        {
            Anchor = new Vector2(1, 0), Pivot = new Vector2(1, 0)
        });

        var animator = buttonPanel.AddComponent<Animator>();
        animator.speed = .75f;
        animator.runtimeAnimatorController = Animations.PopupAnim;
        button = buttonPanel.AddButton(new Info("SaveChanger", -1410, 130, 350, 350, new Vector2(1, 0), new Vector2(0.5f, 0)), ModContent.GetSpriteReference<Main>("ModsBtn").guidRef, new Action(opensavemenu));
        button.AddText(new Info("Text", 0, -175, 500, 100), "Saves List", 60f);
    }

    private static void Init()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var SaveChangerPanel = screen.FindChild("SaveChangerPanel");
        if (SaveChangerPanel == null)
            CreatePanel(screen.gameObject);
    }

    private static void RevealButton()
    {
        buttonPanel.GetComponent<Animator>().Play("PopupSlideIn");
        buttonPanel.SetActive(true);
    }

    private static void HideButton()
    {
        buttonPanel.GetComponent<Animator>().Play("PopupSlideOut");
        buttonPanel.SetActive(false);
    }

    public static void Show()
    {
        Init();
        RevealButton();
    }

    public static void Hide()
    {
        if (buttonPanel != null) HideButton();
    }
}