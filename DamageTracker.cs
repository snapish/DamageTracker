using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using DamageTracker.UI;
using Terraria.GameInput;

namespace DamageTracker
{
    public class DamageTrackerMod : Mod
    {
 
        internal DamageTrackerUI damageTrackerUI;
        public UserInterface myInterface;
        private GameTime _lastUpdateUiGameTime;
        public static ModHotKey ToggleUIHotkey;
        public static DamageTrackerMod Instance => ModContent.GetInstance<DamageTrackerMod>();
        public Mod mod;


        public override void Load()
        {
            ToggleUIHotkey = RegisterHotKey("Open Main Interface", "P");
            // this makes sure that the UI doesn't get opened on the server
            // the server can't see UI, can it? it's just a command prompt
            if (!Main.dedServ)
            {   
                myInterface = new UserInterface(); // the thing that actually gets passed to terraria
                damageTrackerUI = new DamageTrackerUI(); //what you modify and feed to myInterface 
                damageTrackerUI.Activate(); //initialize the UI element and all its children 
            }
        }


        public override void Unload()
        {
            ToggleUIHotkey = null;
            damageTrackerUI = null;
            base.Unload();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            // it will only draw if the player is not on the main menu
            if (myInterface?.CurrentState != null && !Main.gameMenu && DamageTrackerUI.visible)
            {
                myInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("DamageTracker: myInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && myInterface?.CurrentState != null)
                        {
                            myInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));
            }
            
        }

        public static bool IsBossAlive()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].boss)
                    return true;  //it's a boss and it's in the world somwhere
            }
            return false;  //none found
        }   
        public static Dictionary<string, long> CumululativeBossHP()
        {
            long cumulitiveHp = 0;
            var TotalHP = new Dictionary<string, long>();
            foreach(Terraria.NPC npc in Main.npc)
            {
                if(npc.active && npc.boss)
                {
                    string name = npc.GivenName;
                    long hp = npc.lifeMax;
                    cumulitiveHp += hp;
                    TotalHP.Add(name, hp);
                }
            }
            if(TotalHP.Count > 1) //if theres more than one boss...
            {
                TotalHP.Add("Total", cumulitiveHp); //... add a final, separate key for the grand total so we don't have to do it somewhere else 
            }
            return TotalHP;
        }


        public string ShowMyUI()
        {
            Main.NewText("attempting to show ui");
            myInterface.IsVisible = true;
            return "showing ui";
        }

        public string HideMyUI()
        {

            myInterface.IsVisible = false;
            return "hiding ui";
        }

    }
}