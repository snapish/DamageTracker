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
        internal UserInterface dtInterface;
        public DamageTrackerUI dtUIState;
        private GameTime _lastUpdateUiGameTime;
        public static ModHotKey ToggleUIHotkey;
        public static ModHotKey SpawnKingSlimeHotkey;
        public ToupinPlayer tp;
        public static DamageTrackerMod Instance => ModContent.GetInstance<DamageTrackerMod>();
        public Mod mod;


        public override void Load()
        {
            ToggleUIHotkey = RegisterHotKey("Open Main Interface", "P");
            SpawnKingSlimeHotkey = RegisterHotKey("DEBUG: Spawn King Slime", "G");
            // this makes sure that the UI doesn't get opened on the server
            // the server can't see UI, can it? it's just a command prompt
            if (!Main.dedServ)
            {   
                Logger.Info("Loading mod");
                dtInterface = new UserInterface();
                dtUIState = new DamageTrackerUI();
                dtUIState.Activate(); //calls initialize, then all child initializations
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (dtInterface?.CurrentState != null) {
                dtInterface.Update(gameTime);
             }
            base.UpdateUI(gameTime);
        }
        public override void Unload()
        {
            dtUIState = null;
            ToggleUIHotkey = null;
            SpawnKingSlimeHotkey = null;
            base.Unload();
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
            foreach(NPC npc in Main.npc)
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
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "MyMod: MyInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && dtInterface?.CurrentState != null)
                        {
                            dtInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));
            }
            base.ModifyInterfaceLayers(layers);
        }

        internal void ShowMyUI()
        {
            
            NPC mynpc = new NPC();
            Logger.Info("Showing UI");
            dtInterface?.SetState(dtUIState);
        }

        internal void HideMyUI()
        {
            Logger.Info("Hiding UI");
            dtInterface?.SetState(null);
        }

    }

}