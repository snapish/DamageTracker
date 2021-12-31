using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace DamageTracker.NPCs
{
    public class DTModNpc: ModNPC{
        public override string Texture => base.Texture;

        public override int SpawnNPC(int tileX, int tileY) {
            foreach(var npc in Main.npc) {
                if (npc.boss) {
                    Main.NewText("Spawned " + npc.FullName);
                }
            }
            return base.SpawnNPC(tileX, tileY);
        }
    }
    public class DTNpc : GlobalNPC {

        public override void NPCLoot(NPC npc) {

            if (npc.boss && ToupinPlayer.damages.ContainsKey(npc.FullName)) {
                Main.NewText("You did " + string.Format("{0:n0}", ToupinPlayer.damages[npc.FullName]) + " to " + npc.FullName);
                ToupinPlayer.damages[npc.FullName] = 0;
            } 
            base.NPCLoot(npc);
        }

    }
}
