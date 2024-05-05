using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;
using System.Drawing;

namespace DayCounter
{
    public class NotifPlayer : ModPlayer
    {
        private bool locked = false;
        public override void PostUpdate()
        {
            if (Utils.GetDayTimeAs24FloatStartingFromMidnight() == 4.5f) 
            {
                if (Player.whoAmI == Main.myPlayer && !locked)
                {
                    locked = true;
                    InGameNotificationsTracker.AddNotification(new Notification());
                }
            }
            else if (Utils.GetDayTimeAs24FloatStartingFromMidnight() >= 5f)
            {
                locked = false;
            }
        }
    }
}
