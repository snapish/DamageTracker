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
            if (npc.boss) {
                Main.NewText("Boss just died woah");
            } else {
                Main.NewText("Non boss died sadge");
            }
            base.NPCLoot(npc);
        }

    }
}
