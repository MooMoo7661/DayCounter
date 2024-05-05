using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Microsoft.Xna.Framework;

namespace DayCounter
{
    public class DayCounterSystem : ModSystem
    {
        public static int Day = 1;

        public override void Load()
        {
            On_Main.UpdateTime_StartDay += On_Main_UpdateTime_StartDay;
        }

        private void On_Main_UpdateTime_StartDay(On_Main.orig_UpdateTime_StartDay orig, ref bool stopEvents)
        {
            orig(ref stopEvents);
            Day++;
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["DayCounter:Day"] = Day;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            Day = tag.GetInt("DayCounter:Day");
        }

        public override void ClearWorld()
        {
            Day = 0;
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Day);
        }

        public override void NetReceive(BinaryReader reader)
        {
            Day = reader.Read();
        }
    }

    public class GetDayCommand : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "day";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[c/32FF82:It is day {DayCounterSystem.Day}.]"), Color.White);
        }
    }

    public class ResetDayCommand : ModCommand
    {
        public override CommandType Type
            => CommandType.Server;

        public override string Command
            => "resetday";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            DayCounterSystem.Day = 1;
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[c/32FF82:Reset the day counter.]"), Color.White);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }
    }

    public class SetDayCommand : ModCommand
    {
        public override CommandType Type
            => CommandType.Server;

        public override string Command
            => "setday";

        public override string Description => "Sets the day. Only accepts whole numbers.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            DayCounterSystem.Day = int.Parse(args[0]);
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[c/32FF82:Set the day counter to {DayCounterSystem.Day}.]"), Color.White);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }
    }

    public class PreviewCommand : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "preview";

        public override string Description => "Previews the notification";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (caller.Player.whoAmI == Main.myPlayer)
            {
                InGameNotificationsTracker.AddNotification(new Notification());
            }
        }
    }

}
