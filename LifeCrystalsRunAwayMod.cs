using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework;

namespace LifeCrystalsRunAwayMod
{
    public class LifeCrystalsRunAwayMod : Mod
    {
    }

    public class LifeCrystalTile : GlobalTile
    {
        public static bool hasSpawned = false;
        public override bool CanDrop(int i, int j, int type)
        {
            return false;
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (type == TileID.Heart && !hasSpawned)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Main.tile[i, j].ClearTile();
                    NPC.NewNPC(NPC.GetSource_None(), i * 16, j * 16, ModContent.NPCType<LifeCrystal>());
                    hasSpawned = true;
                }

                Main.NewText("Something appeared...", 255, 50, 50);
            }
        }
    }

    public class LifeCrystal : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.width = 130;
            NPC.height = 122;
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.defense = 1;
            NPC.lifeMax = 75;
            NPC.knockBackResist = 0.5f;
            NPC.value = 100f;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.scale = 0.33f;

            AnimationType = AnimationID.FakeChestOpening;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.LifeCrystal, 1));
        }
        private int jumpTimer = 0;
        public override void AI()
        {
            if (jumpTimer > 0)
            {
                jumpTimer--;
            }
            else
            {
                if (NPC.collideY)
                {
                    NPC.velocity.Y = -8f;
                }
                jumpTimer = 90;
            }
            if (NPC.position.X < Main.LocalPlayer.position.X) //if at left
            {
                NPC.velocity.X = -6f;
            }
            else
            {
                NPC.velocity.X = +6f;
            }
            int timer = 20;
            if (timer > 0)
            {
                timer--;
            }
            else
            {
                LifeCrystalTile.hasSpawned = false;
            }
        }
    }
}