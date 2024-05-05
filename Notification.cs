using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace DayCounter
{
    public class Notification : IInGameNotification
    {
        public bool ShouldBeRemoved => timeLeft <= 0;

        public Asset<Texture2D> iconTexture = ModContent.Request<Texture2D>("DayCounter/Assets/Sun");

        private int timeLeft;
        private bool locked = false;

        public static Config config
        {
            get
            {
                return ModContent.GetInstance<Config>();
            }
        }

        private float Scale
        {
            get
            {
                if (timeLeft < 30)
                {
                    return MathHelper.Lerp(0f, 1f, timeLeft / 30f);
                }

                if (timeLeft > 285)
                {
                    return MathHelper.Lerp(1f, 0f, (timeLeft - 285) / 15f);
                }

                return 1f;
            }
        }

        private float Opacity
        {
            get
            {
                if (Scale <= 0.5f)
                {
                    return 0f;
                }

                return (Scale - 0.5f) / 0.5f;
            }
        }

        public void OnCreation()
        {
            SoundStyle sound = (int)config.NotificationSound switch
            {
                1 => SoundID.Item4,
                2 => SoundID.Coins,
                3 => SoundID.Tink,
                4 => SoundID.Pixie,
                5 => SoundID.Meowmere,
                6 => SoundID.AchievementComplete,
                _ => SoundID.Item35,
            };

            if (config.PlaySound)
            SoundEngine.PlaySound(sound);
            timeLeft = ModContent.GetInstance<Config>().NotificationTime;
        }

        public void Update()
        {
            if (!locked)
            {
                OnCreation();
                locked = true;
            }

            timeLeft--;

            if (timeLeft < 0)
            {
                timeLeft = 0;
            }
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
        {
            if (Opacity <= 0f)
            {
                return;
            }

            

            float effectiveScale = Scale * 1.1f;
            Vector2 size = (FontAssets.ItemStack.Value.MeasureString($"Day { DayCounterSystem.Day}") + new Vector2(58f, 10f)) * effectiveScale;
            Rectangle panelSize = Terraria.Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);

            bool hovering = panelSize.Contains(Main.MouseScreen.ToPoint());

            Terraria.Utils.DrawInvBG(spriteBatch, panelSize, config.BackgroundColor * (hovering ? config.HoverAlpha : config.DefaultAlpha));
            float iconScale = effectiveScale * 0.7f;
            Vector2 vector = panelSize.Right() - Vector2.UnitX * effectiveScale * (12f + iconScale * iconTexture.Width());
            spriteBatch.Draw(iconTexture.Value, vector, null, Color.White * Opacity, 0f, new Vector2(0f, iconTexture.Width() / 2f), iconScale, SpriteEffects.None, 0f);
            Terraria.Utils.DrawBorderString(color: new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor / 5, Main.mouseTextColor) * Opacity, sb: spriteBatch, text: $"Day {DayCounterSystem.Day}", pos: vector - Vector2.UnitX * 10f, scale: effectiveScale * 0.9f, anchorx: 1f, anchory: 0.4f);

            if (hovering)
            {
                if (!PlayerInput.IgnoreMouseInterface)
                {
                    OnMouseOver();
                }
            }
        }

        public virtual void OnClick()
        {
            Main.LocalPlayer.mouseInterface = true;
            Main.mouseLeftRelease = false;

            if (timeLeft > 30)
                timeLeft = 30;
        }

        private void OnMouseOver()
        {
           switch((int)config.MouseInteractionType)
           {
                case 1:
                    if (timeLeft > 30)
                        timeLeft = 30;
                    break;
                case 2:
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                        OnClick();
                    break;
            }
        }

        public void PushAnchor(ref Vector2 positionAnchorBottom) => positionAnchorBottom.Y -= 50f * Opacity;

    }
}
