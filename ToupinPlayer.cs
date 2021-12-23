using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using DamageTracker;
using Terraria.Chat;

namespace ToupinPlayer
{
    public class ToupinPlayer : ModPlayer
    {
        public long PlayerTotalDamage;
        public int LapNumber;
        public long LapDamage;
        public Dictionary<string, long> damages = new Dictionary<string, long>();
        public long[] PlayerDamageLaps;
        public DamageTrackerMod dt = DamageTrackerMod.Instance;


        public override void ProcessTriggers(TriggersSet triggersSet) {
            if (DamageTrackerMod.ToggleUIHotkey.JustPressed) {
                dt.myInterface.IsVisible = !dt.myInterface.IsVisible;
                Main.NewText("flippy dippy woo");
                    //Main.NewText(dt.ShowMyUI());
            }
                base.ProcessTriggers(triggersSet);
        }
        public override void ModifyHitNPCWithProj(Projectile proj, Terraria.NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target != null && target.boss) //target is valid and is boss
            {
                LapDamage += damage; 
                PlayerTotalDamage += damage;
                if (damages.ContainsKey(target.FullName)) { //if you have done damage to the boss
                    damages[target.FullName] += damage;
                }
                else //if you havent done dmg to the boss yet
                {
                    damages.Add(target.FullName, damage);
                }
                PlayerDamageLaps[LapNumber] += damage;
            }
                base.ModifyHitNPCWithProj(proj, target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

     
        //on respawn things : 
        //create a "lap" of their damage, like a stopwatch lap
        //if no bosses alive set ui state to hidden
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (LapDamage > 1000 && !pvp){ //u didnt die asap, and the death wasnt pvp related

                LapDamage = 0;
                LapNumber++;
            }
            base.Kill(damage, hitDirection, pvp, damageSource);

        }
        
    }
}