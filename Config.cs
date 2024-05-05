using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.UI;

namespace DayCounter
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        // getting 𝓯𝓻𝓮𝓪𝓴𝔂

        public enum MouseInteraction
        {
            None,
            Hover,
            Click
        }

        public enum NotificationSoundType
        {
            Bell,
            ManaCrystal,
            Coins,
            Tink,
            Pixie,
            Meowmere,
            AchievementComplete
        }

        [Header("General")]
        [DefaultValue(300)]
        [Slider]
        [Range(30, 600)]
        [Increment(30)]
        public int NotificationTime {  get; set; }

        [DefaultValue(MouseInteraction.Click)]
        public MouseInteraction MouseInteractionType { get; set; }

        [DefaultValue(NotificationSoundType.Bell)]
        public NotificationSoundType NotificationSound { get; set; }

        [DefaultValue(true)]
        public bool PlaySound { get; set; }

        [Header("Colors")]
       
        [DefaultValue(typeof(Color), "0, 0, 0, 255")]
        public Color TextColor { get; set; }

        [DefaultValue(typeof(Color), "26, 34, 63, 204")]
        public Color BackgroundColor { get; set; }

        [BackgroundColor(250, 235, 215)]
        [SliderColor(181, 87, 92)]
        [DefaultValue(0.75f)]
        [Slider]
        [Range(0f, 1f)]
        [Increment(0.1f)]
        [DrawTicks]
        public float HoverAlpha { get; set; }

        [BackgroundColor(250, 235, 215)]
        [SliderColor(181, 87, 92)]
        [DefaultValue(0.5f)]
        [Slider]
        [Range(0f, 1f)]
        [Increment(0.1f)]
        [DrawTicks]
        public float DefaultAlpha { get; set; }
    }
}
