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
        public Dictionary<string, long> damages = new Dictionary<string, long>(); // key: boss name, value: dmg. for if multiple bosses are spawned, or bosses minions
        public Dictionary<int, long> PlayerDamageLaps = new Dictionary<int, long>(); //storing the individual laps damage
        public DamageTrackerMod dt = DamageTrackerMod.Instance;

        public override void ProcessTriggers(TriggersSet triggersSet) {
            if (DamageTrackerMod.SpawnKingSlimeHotkey.JustPressed) {
                int slimeID = NPCID.KingSlime;
                NPC.NewNPC((int)player.Bottom.X, (int)player.Bottom.Y, slimeID);
            }
            if (DamageTrackerMod.ToggleUIHotkey.JustPressed) {
                if (dt.dtInterface.CurrentState == null) { 
                    dt.ShowMyUI();
                }
                else
                {
                    dt.HideMyUI();
                }
            }
                base.ProcessTriggers(triggersSet);
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) {
            if (target != null && target.boss) {  //target is valid and is boss
                int realDamage = NegativeDamageFix(target, damage);
                LapDamage += realDamage;
                PlayerTotalDamage += realDamage;
                if (damages.ContainsKey(target.FullName)) { //if you have done damage to the boss
                    damages[target.FullName] += realDamage;
                } else //if you havent done dmg to the boss yet
                  {
                    damages.Add(target.FullName, realDamage);
                }
                AddLapDamage(realDamage);
                dt.dtUIState.changeText(LapDamageString());

            }
            base.OnHitNPC(item, target, damage, knockback, crit);
        }

        public string LapDamageString() {
            string concat = "";
            for (int i = 0; i < LapNumber; i++) {
                concat = string.Format("Damage Lap {0}: {1}", i + 1, PlayerDamageLaps[i] + "\n--------\n");
            }

            return concat;
        }
        public int NegativeDamageFix(NPC npc, int damage) {
            if(npc.life - damage < 0) {
                return damage - (npc.life - damage);
            }
            return damage;

        }
        public void AddLapDamage(long dmg) {
            if (PlayerDamageLaps.ContainsKey(LapNumber)) {
                PlayerDamageLaps[LapNumber] += dmg;
            } else {
                PlayerDamageLaps.Add(LapNumber, dmg); //if the lap hasnt been started yet
            }
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