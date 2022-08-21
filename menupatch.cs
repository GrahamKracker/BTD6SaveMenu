namespace BTD6ModdedSaveHelper;

using Assets.Scripts.Unity.UI_New.Main.DifficultySelect;
using Assets.Scripts.Unity.UI_New.Main.MapSelect;

//[HarmonyPatch(typeof(Assets.Scripts.Unity.UI_New.Main.MapSelect.MapButton), nameof(MapButton.mapImage.OnEnable))]

public partial class Main
{
    public override void OnMainMenu()
    {
        base.OnMainMenu();
        //MelonLogger.Msg("latest: " + MapSaveLoader.LatestVersion);
        SaveButton.Hide();
    }
}

[HarmonyPatch(typeof(MenuManager), nameof(MenuManager.OpenMenu))]
internal static class MenuManager_OpenMenu
{
    [HarmonyPostfix]
    private static void Postfix(MenuManager __instance, string menuName)
    {
        var currentMenu = __instance.GetCurrentMenuName();
        if (menuName == "DifficultySelectUI")
        {
            SaveButton.Show();
        }
        else
        {
            if (menuName == "MapSelectScreen")
                SavePanel.Show();

            SaveButton.Hide();
        }
    }
}

[HarmonyPatch(typeof(DifficultySelectScreen), nameof(DifficultySelectScreen.Open))]
internal static class DifficultySelectScreen_Open
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        SaveButton.Show();
        //MelonLogger.Msg("data2: " + MenuManager.instance.GetCurrentMenuName());
        //MelonLogger.Msg("data24: " + MenuManager.instance.currMenu.Item3.);
        //MelonLogger.Msg("data: " + MenuManager.instance.GetMenuData("DifficultySelectUI"));
        SavePanel.Hide();
    }
}

[HarmonyPatch(typeof(DifficultySelectScreen), nameof(DifficultySelectScreen.OpenModeSelectUi))]
internal static class DifficultySelectScreen_OpenModeSelectUi
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        SaveButton.Hide();
        SavePanel.Hide();
    }
}

[HarmonyPatch(typeof(ContinueGamePanel), nameof(ContinueGamePanel.ContinueClicked))]
internal static class ContinueGamePanel_ContinueClicked
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        SaveButton.Hide();
        SavePanel.Hide();
    }
}

[HarmonyPatch(typeof(MapSelectScreen), nameof(MapSelectScreen.Open))]
internal static class DifficultySelectScreen_Clo
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        SaveButton.Hide();
        SavePanel.Show();
    }
}

[HarmonyPatch(typeof(MapSelectScreen), nameof(MapSelectScreen.Close))]
internal static class DifficultySelectScreen_Cl
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        SaveButton.Hide();
        SavePanel.Hide();
    }
}
