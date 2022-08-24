namespace BTD6SaveMenu;

using BTD_Mod_Helper.Extensions;

public static class InfoButton
{
    public static ModHelperButton selectedbutton;

    public static ModHelperButton CreateButton(MapSaveDataModel save, string name,Action onclick=null,string sprite=null,bool towers=false, bool hero=false, bool history=false, bool misc=false)
    {
        ModHelperButton button;      
        var description = SaveMenu.GetDescription(save, towers,hero, history, misc);     
        button = null;
        if (onclick == null)
            {
                
                onclick = () =>
                {
                    selectedbutton.Image.SetSprite(VanillaSprites.BlueBtnLong);
                    button.Image.SetSprite(VanillaSprites.GreenBtnLong);
                    SaveMenu.selectedSaveDescription.SetText(description);
                    selectedbutton = button;
                };
                if (description.Equals("Error"))
                { 
                    button = ModHelperButton.Create(new Info(name + "Button", width: 562, height: 200), VanillaSprites.RedBtnLong, null);
                    button.AddText(new Info("Text", 0, 0, 500, 100), name, 60f);
                    return button;
                }
            }
        if (sprite == null)
        {
            button = ModHelperButton.Create(new Info(name + "Button", width: 562, height: 200), VanillaSprites.BlueBtnLong, onclick);
            button.AddText(new Info(name + "Button", 0, 0, 500, 100), name, 60f);
            return button;
        }
        button = ModHelperButton.Create(new Info(name + "Button", width: 562, height: 200), sprite, onclick);
        button.AddText(new Info(name + "Button", 0, 0, 500, 100), name, 60f);
        return button;
        
    }
}
