namespace BTD6SaveMenu;

using static BTD6SaveMenu.Main;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.UI_New;
using Assets.Scripts.Unity.UI_New.InGame.RightMenu.Powers;
using Assets.Scripts.Unity.UI_New.LevelUp;
using Assets.Scripts.Unity.UI_New.Main.PowersSelect;
using Assets.Scripts.Unity.UI_New.Settings;
using Assets.Scripts.Utils;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using TMPro;

public class PreviewMenu : ModGameMenu<PowersSelectScreen>
{
    private static ModHelperImage previewimage;
    private const int towermultiplierx = 15;
    private const int towermultipliery = -12;

    public override bool OnMenuOpened(Object data)
    {
        var save = (MapSaveDataModel) data;
        globalselectedsave = save;
        var gameObject = GameMenu.gameObject;
        gameObject.DestroyAllChildren();
        CommonForegroundScreen.instance.Hide();
        CommonForegroundScreen.instance.Show(true, "", false, false, false, false, false, false);
        CommonForegroundScreen.instance.heading.gameObject.SetActive(false);


        var previewpanel = gameObject.AddModHelperPanel(new Info("PreviewPanel", InfoPreset.FillParent));
        previewimage = previewpanel.AddImage(new Info("PreviewImage", InfoPreset.FillParent), VanillaSprites.UISprite);
        foreach (var map in GameData.Instance.mapSet.maps)
        {
            if (save.mapName == map.id)
            {
                previewimage.Image.SetSprite(map.mapSprite);
                //previewimage.Image.SetSprite(map.mapSprite.guidRef);
                previewimage.SetActive(true);
            }
        }

        string cash = "Error";
        foreach (var (sim, player) in save.players)
        {
            if (player != null)
                cash = String.Format("{0:#,###0}", player.cash);
        }

        //VanillaSprites.SaveGameIcon
        previewimage.AddImage(new Info("CashIcon", -1850, 1150, 200, 200), VanillaSprites.StartingCash);
        previewimage.AddText(new Info("Cash", -1475, 1170, 500, 100), cash, 90f).Text.alignment = TextAlignmentOptions.Left;
        previewimage.AddText(new Info("Round", 1950, 1175, 500, 100), "Round", 90f);
        previewimage.AddText(new Info("Round Number", 1800, 1100, 500, 100), String.Format("{0:#,###0}", save.round), 90f).Text.alignment = TextAlignmentOptions.BottomRight;

        foreach (var tower in save.placedTowers)
        {
            string sprite = GetSpriteReference<Main>("ModdedTower").guidRef;

            if (tower != null)
            {
                if (standardtowers.Contains(tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier) || nonstandardtowers.Contains(tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier))
                {
                    //MelonLogger.Msg("Placed Tower: " + tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier);
                    sprite = sprites[tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier].portrait.guidRef;
                    previewimage.AddImage(new Info($"{tower.baseId}-{tower.pathOneTier}{tower.pathTwoTier}{tower.pathThreeTier}", (float) Math.Round(tower.position.x) * towermultiplierx, (float) Math.Round(tower.position.y) * towermultipliery, 200, 200, new Vector2(.5f, .5f)), sprite);
                }
                else if (standardheroes.Contains(tower.baseId + "-" + tower.pathOneTier))
                {
                    //MelonLogger.Msg("Placed Hero: " + tower.baseId + "-" + tower.pathOneTier);
                    sprite = sprites[tower.baseId + "-" + tower.pathOneTier].portrait.guidRef;
                    previewimage.AddImage(new Info($"{tower.baseId}-{tower.pathOneTier}", (float) Math.Round(tower.position.x) * towermultiplierx, (float) Math.Round(tower.position.y) * towermultipliery, 200, 200, new Vector2(.5f, .5f)), sprite);
                }
                else
                {
                    if (Game.instance.model.GetTowerFromId(tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier) != null)
                    {
                        //MelonLogger.Msg("Placed Modded Tower: " + tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier);
                        sprite = Game.instance.model.GetTowerFromId(tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier).portrait.guidRef;
                        previewimage.AddImage(new Info($"{tower.baseId}-{tower.pathOneTier}{tower.pathTwoTier}{tower.pathThreeTier}", (float) Math.Round(tower.position.x) * towermultiplierx, (float) Math.Round(tower.position.y) * towermultipliery, 200, 200, new Vector2(.5f, .5f)), sprite);
                    }
                    else if (Game.instance.model.GetTowerFromId(tower.baseId) != null)
                    {
                        //MelonLogger.Msg("Placed Modded Tower: " + tower.baseId);
                        sprite = Game.instance.model.GetTowerFromId(tower.baseId).portrait.guidRef;
                        previewimage.AddImage(new Info($"{tower.baseId}-{tower.pathOneTier}{tower.pathTwoTier}{tower.pathThreeTier}", (float) Math.Round(tower.position.x) * towermultiplierx, (float) Math.Round(tower.position.y) * towermultipliery, 200, 200, new Vector2(.5f, .5f)), sprite);
                    }
                    else
                    {
                        MelonLogger.Msg("Errored Tower: " + tower.baseId + "-" + tower.pathOneTier + tower.pathTwoTier + tower.pathThreeTier);
                        previewimage.AddImage(new Info($"{tower.baseId}-{tower.pathOneTier}{tower.pathTwoTier}{tower.pathThreeTier}", (float) Math.Round(tower.position.x) * towermultiplierx, (float) Math.Round(tower.position.y) * towermultipliery, 300, 200, new Vector2(.5f, .5f)), sprite);
                    }
                }
            }
        }

        return false;
    }
}
