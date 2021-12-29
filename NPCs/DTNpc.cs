using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace DamageTracker.NPCs
{
    public class DTNpc: ModNPC{
        public override string Texture => base.Texture;


        public override void BossLoot(ref string name, ref int potionType) {
            Main.NewText("Boss just died woah");
            base.BossLoot(ref name, ref potionType);
        }

        public override int SpawnNPC(int tileX, int tileY) {
            foreach(var npc in Main.npc) {
                if (npc.boss) {
                    Main.NewText("Spawned " + npc.FullName);
                }
            }
            return base.SpawnNPC(tileX, tileY);
        }
    }
}
