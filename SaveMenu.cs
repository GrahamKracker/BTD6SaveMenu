namespace BTD6ModdedSaveHelper;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Unity.UI_New.Popups;
using BTD_Mod_Helper.Extensions;
using TMPro;

public class SaveMenu : ModGameMenu<ExtraSettingsScreen>
{
    private static Dictionary<MapSaveDataModel, SaveBrowserFile> savePanels = new();
    private static Dictionary<SaveBrowserFile, ModHelperButton> infoButtons = new();

    private static ModHelperScrollPanel savesList;
    private static ModHelperScrollPanel infoList;
    public static ModHelperScrollPanel descriptionPanel;
    private static ModHelperPanel InfoPanel;
    private static ModHelperButton towersbutton;
    private static ModHelperButton heroesbutton;
    private static ModHelperButton historybutton;
    public static ModHelperImage previewimage;


    private static ModHelperPanel selectedSavePanel;
    private static ModHelperText selectedSaveName;
    private static SaveBrowserFile saveTemplate;
    internal static MapSaveDataModel selectedSave;
    public static ModHelperText selectedSaveDescription;

    public override bool OnMenuOpened(Object data)
    {
        CommonForegroundHeader.SetText("Saves List");

        var panelTransform = GameMenu.gameObject.GetComponentInChildrenByName<RectTransform>("Panel");
        var panel = panelTransform.gameObject;
        panel.DestroyAllChildren();
        var SaveMenu = panel.AddModHelperPanel(new Info("SaveMenu", MenuWidth, MenuHeight));

        CreateLeftMenu(SaveMenu);
        CreateMiddleMenu(SaveMenu);
        CreateRightMenu(SaveMenu);

        selectedSave = savePanels.First().Key;
        SetSelectedSave(selectedSave);

        updatemiddlemenu(selectedSave);
        MelonCoroutines.Start(CreateSavePanels());
        return false;
    }

    private static void updatemiddlemenu(MapSaveDataModel save)
    {
        try
        {
            towersbutton.gameObject.Destroy();
            heroesbutton.gameObject.Destroy();
            historybutton.gameObject.Destroy();
        }
        catch
        {
        }

        towersbutton = InfoButtons.CreateTowersButton(save);
        towersbutton.Image.SetSprite(VanillaSprites.GreenBtnLong);
        InfoButtons.selectedbutton = towersbutton;
        infoList.AddScrollContent(towersbutton);
        selectedSaveDescription.SetText(GetDescription(save));

        heroesbutton = InfoButtons.CreateHeroesButton(save);
        infoList.AddScrollContent(heroesbutton);
        historybutton = InfoButtons.CreateHistoryButton(save);
        infoList.AddScrollContent(historybutton);
    }

    private static IEnumerator CreateSavePanels()
    {
        var keys = savePanels.Keys.ToList();
        var i = 0;
        foreach (var key in keys)
        {
            var panel = savePanels[key] = saveTemplate.Duplicate(key.mapName);
            if (i > 4) yield return null;
            panel.SetSave(key);
            if (i > 4) yield return null;
            i++;
        }

        yield return null;
    }

    private void CreateMiddleMenu(ModHelperPanel SaveMenu)
    {
        var middleMenu = SaveMenu.AddPanel(new Info("MiddleMenu", 0, 0, MiddleMenuWidth, MenuHeight), VanillaSprites.MainBGPanelBlue, RectTransform.Axis.Vertical, Padding, Padding);
        var firstRow = middleMenu.AddPanel(new Info("FirstRow")
        {
            Height = 150, FlexWidth = 1
        }, null, RectTransform.Axis.Horizontal, 50);

        var saveTitlePanel = firstRow.AddPanel(new Info("SaveTitlePanel")
        {
            Flex = 1
        }, VanillaSprites.BlueInsertPanelRound);
        selectedSaveName = saveTitlePanel.AddText(new Info("SaveTitle", InfoPreset.FillParent)
        {
            X = 0,
            Y = 0,
            Width = Padding * -2,
            Height = -Padding
        }, "", FontLarge, TextAlignmentOptions.Left);
        selectedSaveName.Text.enableAutoSizing = true;
        firstRow.AddButton(new Info("Edit") {Size = ModNameHeight}, VanillaSprites.EditBtn, new Action(() => { }));
        firstRow.AddButton(new Info("Delete")
        {
            Size = ModNameHeight
        }, VanillaSprites.CloseBtn, new Action(() =>
        {
            PopupScreen.instance.SafelyQueue(screen => screen.ShowPopup(PopupScreen.Placement.menuCenter, "Confirm Delete Save?", "This will permanently delete the specified save file with no recovery", new Action(() =>
            {
                GameExt.GetPlayerProfile(Game.instance).savedMaps.Remove(selectedSave.mapName);
                savePanels[selectedSave].gameObject.Destroy();
                savePanels.Remove(selectedSave);
                SetSelectedSave(savePanels.First().Key);
            }), "Yes", null, "No", Popup.TransitionAnim.Scale));
        }));
        infoList = middleMenu.AddScrollPanel(new Info("MapListScroll", InfoPreset.Flex), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound, Padding, Padding);
        previewimage = middleMenu.AddImage(new Info("PreviewImage", InfoPreset.Flex)
        {
            X = 0,
            Y = 0,
            Width = 500,
            Height = 400
        }, VanillaSprites.MainBGPanelGrey);
        previewimage.SetActive(false);
        //                string TowerName=__instance.objectId.guidRef.Split('-')[0];
        //guidRef="Ui[Tempest-Portrait]"
    }

    private static void CreateLeftMenu(ModHelperPanel SaveMenu)
    {
        var leftMenu = SaveMenu.AddPanel(new Info("LeftMenu", (MenuWidth - LeftMenuWidth) / -2f, 0, LeftMenuWidth, MenuHeight), VanillaSprites.MainBGPanelBlue, RectTransform.Axis.Vertical, Padding, Padding);
        var topRow = leftMenu.AddPanel(new Info("TopRow")
        {
            Height = ModNameHeight,
            FlexWidth = 1
        }, null, RectTransform.Axis.Horizontal, Padding);
        topRow.AddPanel(new Info("Filler", InfoPreset.Flex));

        savesList = leftMenu.AddScrollPanel(new Info("MapListScroll", InfoPreset.Flex), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound, Padding, Padding);

        savePanels = new Dictionary<MapSaveDataModel, SaveBrowserFile>();

        saveTemplate = SaveBrowserFile.CreateTemplate();
        savesList.AddScrollContent(saveTemplate);
        foreach (var (name, map) in Game.instance.GetBtd6Player().Data.savedMaps)
        {
            if (map.gameType == GameType.Standard)
                savePanels[map] = null;
        }
    }

    internal static void SetSelectedSave(MapSaveDataModel mapSave)
    {
        selectedSave = mapSave;
        if (mapSave.mapName == "Tutorial")
            selectedSaveName.SetText("MonkeyMeadow");
        else
            selectedSaveName.SetText(mapSave.mapName);
        updatemiddlemenu(mapSave);
    }

    public static string GetDescription(MapSaveDataModel mapSave, bool hero = false, bool history = false)
    {
        int[] tiers = {0, 0, 0};
        List<string> description = new();
        List<string> standardtowers = new();
        List<string> standardheroes = new();
        standardtowers.Clear();
        description.Clear();
        foreach (var towermodel in Game.instance.model.towers)
        {
            if (towermodel.IsHero())
            {
                if (!standardheroes.Contains(towermodel.baseId))
                {
                    standardheroes.Add(towermodel.baseId);
                }
            }

            if (!towermodel.isGeraldoItem && !towermodel.isPowerTower && !towermodel.isSubTower && towermodel.IsStandardTower() && towermodel.IsBaseTower && !towermodel.IsHero() && !standardheroes.Contains(towermodel.baseId))
            {
                standardtowers.Add(towermodel.baseId);
            }
        }

        description.Clear();
        var errorcount = 0;
        if (!history)
        {
            foreach (var tower in mapSave.placedTowers)
            {
                if (tower != null)
                {
                    if (string.IsNullOrEmpty(tower.heroId) && !hero && standardtowers.Contains(tower.baseId))
                    {
                        if (tower.pathOneTier == 6)
                            tiers = new[] {5, 5, 5};
                        else
                            tiers = new[] {tower.pathOneTier, tower.pathTwoTier, tower.pathThreeTier};
                        description.Add(tower.baseId + " - " + string.Join(", ", tiers));
                    }
                    else if (hero && !string.IsNullOrEmpty(tower.heroId))
                    {
                        var herotier = "Level " + tower.pathOneTier;
                        description.Add(tower.baseId + " - " + herotier);
                    }
                }
                else
                {
                    errorcount++;
                    description.Add("Error: Tower details not found");
                }
            }
        }
        else
        {
            foreach (var tower in mapSave.towerHistory)
            {
                if (tower != null && tower.destroyed)
                {
                    if (standardheroes.Contains(tower.baseId))
                    {
                        var herotier = "Level " + tower.tiers[0];
                        description.Add(tower.baseId + " - " + herotier);
                    }
                    else
                    {
                        description.Add(tower.baseId + " - " + string.Join(", ", tower.tiers));
                    }
                }
            }
        }

        var combined = string.Join("\n", description);
        if (errorcount <= mapSave.placedTowers.Count)
            return combined;
        else
            return "Error";
    }

    public override void OnMenuClosed()
    {
        savePanels.Clear();
    }

    private static void CreateRightMenu(ModHelperPanel SaveMenu)
    {
        selectedSavePanel = SaveMenu.AddPanel(new Info("SaveInfo", (MenuWidth - RightMenuWidth) / 2f, 0, RightMenuWidth, MenuHeight), VanillaSprites.MainBGPanelBlue, RectTransform.Axis.Vertical, Padding, Padding);
        var topRow = selectedSavePanel.AddPanel(new Info("TopRow")
        {
            Height = ModNameHeight,
            FlexWidth = 1
        }, null, RectTransform.Axis.Horizontal, Padding);

        descriptionPanel = selectedSavePanel.AddScrollPanel(new Info("DescriptionPanel", InfoPreset.Flex), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound, 50, 50);

        selectedSaveDescription = ModHelperText.Create(new Info("DescriptionText")
        {
            Width = RightMenuWidth - Padding * 4
        }, "", FontSmall, TextAlignmentOptions.TopLeft);
        descriptionPanel.AddScrollContent(selectedSaveDescription);
        selectedSaveDescription.LayoutElement.preferredHeight = -1;
        selectedSaveDescription.Text.enableAutoSizing = true;
        selectedSaveDescription.Text.lineSpacing = Padding / 2f;
        selectedSaveDescription.Text.font = Fonts.Btd6FontBody;
    }

    #region Constants

    internal const int Padding = 50;

    internal const int MenuWidth = 4400;
    internal const int MenuHeight = 1900;

    internal const int LeftMenuWidth = 1400;
    internal const int MiddleMenuWidth = 1400;
    internal const int RightMenuWidth = 1400;

    internal const int ModNameWidth = 1000;

    internal const int ModNameHeight = 150;

    internal const int FontSmall = 52;
    internal const int FontMedium = 69;
    internal const int FontLarge = 80;

    #endregion
}
