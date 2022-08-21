namespace BTD6ModdedSaveHelper;

using BTD_Mod_Helper.Extensions;

public static class InfoButtons
{
    public static ModHelperButton selectedbutton;

    public static ModHelperButton CreateTowersButton(MapSaveDataModel save)
    {
        ModHelperButton towersbutton;
        var description = SaveMenu.GetDescription(save);
        if (description.Equals("Error"))
        {
            towersbutton = ModHelperButton.Create(new Info("TowersButton", width: 562, height: 200), VanillaSprites.MainBGPanelGrey, null);
            towersbutton.AddText(new Info("Text", 0, 0, 500, 100), "Towers", 60f);
            return towersbutton;
        }

        towersbutton = null;
        towersbutton = ModHelperButton.Create(new Info("TowersButton", width: 562, height: 200), VanillaSprites.BlueBtnLong, new Action(() =>
        {
            selectedbutton.Image.SetSprite(VanillaSprites.BlueBtnLong);
            towersbutton.Image.SetSprite(VanillaSprites.GreenBtnLong);
            SaveMenu.selectedSaveDescription.SetText(description);
            selectedbutton = towersbutton;
            SaveMenu.previewimage.SetActive(false);
        }));
        towersbutton.AddText(new Info("Text", 0, 0, 500, 100), "Towers", 60f);
        return towersbutton;
    }

    public static ModHelperButton CreateHeroesButton(MapSaveDataModel save)
    {
        ModHelperButton heroesbutton = null;
        var description = SaveMenu.GetDescription(save, true);
        if (description.Equals("Error"))
        {
            heroesbutton = ModHelperButton.Create(new Info("HeroesButton", width: 562, height: 200), VanillaSprites.MainBGPanelGrey, null);
            heroesbutton.AddText(new Info("Text", 0, 0, 500, 100), "Heroes", 60f);
            return heroesbutton;
        }

        heroesbutton = ModHelperButton.Create(new Info("HeroesButton", width: 562, height: 200), VanillaSprites.BlueBtnLong, new Action(() =>
        {
            selectedbutton.Image.SetSprite(VanillaSprites.BlueBtnLong);
            heroesbutton.Image.SetSprite(VanillaSprites.GreenBtnLong);
            SaveMenu.selectedSaveDescription.SetText(description);
            selectedbutton = heroesbutton;
            SaveMenu.previewimage.SetActive(false);
        }));
        heroesbutton.AddText(new Info("Text", 0, 0, 500, 100), "Heroes", 60f);
        return heroesbutton;
    }


    public static ModHelperButton CreateHistoryButton(MapSaveDataModel save)
    {
        ModHelperButton historybutton = null;
        var description = SaveMenu.GetDescription(save, false, true);
        if (description.Equals("Error"))
        {
            historybutton = ModHelperButton.Create(new Info("HistoryButton", width: 562, height: 200), VanillaSprites.MainBGPanelGrey, null);
            historybutton.AddText(new Info("Text", 0, 0, 500, 100), "Sold", 60f);
            return historybutton;
        }

        historybutton = ModHelperButton.Create(new Info("HistoryButton", width: 562, height: 200), VanillaSprites.BlueBtnLong, new Action(() =>
        {
            selectedbutton.Image.SetSprite(VanillaSprites.BlueBtnLong);
            historybutton.Image.SetSprite(VanillaSprites.GreenBtnLong);
            SaveMenu.selectedSaveDescription.SetText(description);
            selectedbutton = historybutton;
            SaveMenu.previewimage.SetActive(false);
        }));
        historybutton.AddText(new Info("Text", 0, 0, 500, 100), "Sold", 60f);
        return historybutton;
    }

    public static ModHelperButton CreatePreviewButton(MapSaveDataModel save)
    {
        ModHelperButton previewbutton = null;
        if (true)
        {
            previewbutton = ModHelperButton.Create(new Info("HistoryButton", width: 562, height: 200), VanillaSprites.MainBGPanelGrey, null);
            previewbutton.AddText(new Info("Text", 0, 0, 500, 100), "Preview", 60f);
            return previewbutton;
        }

        previewbutton = ModHelperButton.Create(new Info("PreviewButton", width: 562, height: 200), VanillaSprites.BlueBtnLong, new Action(() =>
        {
            selectedbutton.Image.SetSprite(VanillaSprites.BlueBtnLong);
            previewbutton.Image.SetSprite(VanillaSprites.GreenBtnLong);
            selectedbutton = previewbutton;
            SaveMenu.previewimage.SetActive(false);
        }));
        previewbutton.AddText(new Info("Text", 0, 0, 500, 100), "Sold", 60f);
        return previewbutton;
    }

    public static ModHelperImage CreatePreviewImage(MapSaveDataModel save)
    {
        foreach (var mapSetMap in GameData.Instance.mapSet.maps)
        {
            if (save.savedMapsId == mapSetMap.id)
            {
                var image = ModHelperImage.Create(new Info("Image", width: 562, height: 200), mapSetMap.mapSprite.guidRef);
                return image;
            }
        }

        return null;
    }
}
