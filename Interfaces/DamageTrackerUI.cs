using Interfaces;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace DamageTracker.UI
{
    public class DamageTrackerUI : UIState
    {
        
        private DragableUIPanel mainPanel;
        public static bool visible;
        public float oldScale;
        public override void OnInitialize()
        {
            visible = true;
            mainPanel = new DragableUIPanel();
            
            
            mainPanel.Left.Set(800, 0); //this makes the distance between the left of the screen and the left of the panel 800 pixels (somewhere by the middle).
            mainPanel.Top.Set(100, 0); //this is the distance between the top of the screen and the top of the panel
            mainPanel.Width.Set(100, 0);
            mainPanel.Height.Set(100, 0);
            //Texture2D pauseButton = ModContent.GetTexture("Terraria/UI/ButtonPause");
            
            Append(mainPanel); //appends the panel to the UIState
          //  Append(mainPanel.displayText);
        }

        public override void Update(GameTime gameTime)
        {
            if (oldScale != Main.inventoryScale)
            {
                oldScale = Main.inventoryScale;
                Recalculate();
            }
            base.Update(gameTime);
        }
        public void changeText(string newText) {

            mainPanel.displayText.SetText(newText);
            Append(mainPanel.displayText);
        }

    }



    public class damageDisplay :UIElement {
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            

            base.DrawSelf(spriteBatch);
        }
    }
}