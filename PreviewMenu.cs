namespace BTD6SaveMenu;

using System.Collections;
using Assets.Scripts.Data;
using Assets.Scripts.Unity.UI_New;
using Assets.Scripts.Unity.UI_New.InGame.RightMenu.Powers;
using Assets.Scripts.Unity.UI_New.LevelUp;
using Assets.Scripts.Unity.UI_New.Main.PowersSelect;
using Assets.Scripts.Unity.UI_New.Settings;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;

//public class PreviewMenu : ModGameMenu<LevelUpScreen>
public class PreviewMenu : ModGameMenu<PowersSelectScreen>
{
    private ModHelperPanel previewpanel;
    private ModHelperImage previewimage;

    public override bool OnMenuOpened(Object data)
    {
        var save = (MapSaveDataModel) data;
        var gameObject = GameMenu.gameObject;
        gameObject.DestroyAllChildren();
        CommonForegroundScreen.instance.Hide();
        CommonForegroundScreen.instance.Show(true, "", false, false, false, false, false, false);
        CommonForegroundScreen.instance.heading.gameObject.SetActive(false);
        previewpanel = gameObject.AddModHelperPanel(new Info("PreviewPanel", InfoPreset.FillParent));
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
        return false;
    }

    private static IEnumerator CreatePreviewTowers(MapSaveDataModel save)
    {
        
        return null;
    }
    
    // public override void OnMenuClosed()
    // {
    //     previewimage.SetActive(false);
    //     previewpanel.SetActive(false);
    //     previewimage.gameObject.Destroy();
    //     previewpanel.gameObject.Destroy();
    // }
}
