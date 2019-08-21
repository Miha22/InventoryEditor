﻿using Rocket.API;
using System.Collections.Generic;
using Rocket.Unturned;
using SDG.Unturned;
using Rocket.Unturned.Player;
using UnityEngine;
using System.IO;
using Steamworks;

namespace ItemRestrictorAdvanced
{
    public class CommandBoxUp : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "sendbox";
        public string Help => "watch on special loot box and execute /sendbox or /sendbox <prefered box name>";
        public string Syntax => "/sendbox or /sendbox <name of your box>";
        public List<string> Aliases => new List<string>() { "db" };
        public List<string> Permissions => new List<string>() { "rocket.sendbox", "rocket.sb" };
        //string path = Plugin.Instance.pathTemp;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length > 1)
            {
                Rocket.Unturned.Chat.UnturnedChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                throw new WrongUsageOfCommandException(caller, this);
            }
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out RaycastHit hit, 5, RayMasks.BARRICADE_INTERACT))
            {
                if (BarricadeManager.tryGetInfo(hit.transform, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion r))
                {
                    BarricadeData bdata = r.barricades[index];
                    if (bdata.barricade.id != 3280 || bdata.owner != player.CSteamID.m_SteamID)
                    {
                        Rocket.Unturned.Chat.UnturnedChat.Say(caller, $"Error occured: this barricade is not a virtual inventory box or box is not yours.", Color.red);
                        Rocket.Unturned.Chat.UnturnedChat.Say(caller, $"Owner steamID: {bdata.owner}\r\nYour steamID: {player.CSteamID.ToString()}");
                        return;
                    }
                    StateToBlock(bdata.barricade, player.CSteamID, (command.Length == 0) ? SetBoxName(Plugin.Instance.pathTemp + "\\" + player.CSteamID.ToString()) : command[0]);
                }
            }
        }

        private static void StateToBlock(Barricade barricade, CSteamID steamID, string boxName)
        {
            Block block = new Block();
            block.writeUInt16(barricade.id);
            block.writeUInt16(barricade.health);
            Plugin.Instance.WriteSpell(block);
            block.writeByteArray(barricade.state);
            Functions.WriteBlock(Plugin.Instance.pathTemp + "\\" + steamID.ToString() + "\\" + boxName, block, false);
        }

        private string SetBoxName(string path)
        {
            DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories();
            if (directories == null)
                return "box_0";

            //Directory.CreateDirectory(path + $@"\box_{directories.Length - 1}");
            return $"box_{directories.Length - 1}";
        }

        private void UploadItems(List<MyItem> items, string playerSteamID)
        {
            string path = Plugin.Instance.pathTemp + $"\\{playerSteamID}";
            List<List<MyItem>> boxes = CreateBoxes(items);
            foreach (List<MyItem> box in boxes)
            {
                Block block = new Block();
                block.writeUInt16(3280);
                block.writeUInt16(600);
                Plugin.Instance.WriteSpell(block);
                block.writeUInt16((ushort)box.Count);
                foreach (var myItem in box)
                    block.writeByteArray(myItem.State);
                Functions.WriteBlock(path + $"\\{SetBoxName(path)}", block, false);
            }
        }

        private List<List<MyItem>> CreateBoxes(List<MyItem> myItems)
        {
            List<List<MyItem>> boxes = new List<List<MyItem>>();
            do
            {
                List<MyItem> selectedItems = new List<MyItem>();
                bool[,] page = Plugin.Instance.FillPage(10, 6);
                foreach (var item in myItems)
                {
                    if ((item.Size_x > 10 && item.Size_y > 6) || (item.Size_x > 6 && item.Size_y > 10))
                        continue;
                    if (Plugin.Instance.FindPlace(ref page, 10, 6, item.Size_x, item.Size_y, out byte x, out byte y))
                    {
                        item.X = x;
                        item.Y = y;
                        selectedItems.Add(item);
                        myItems.Remove(item);
                    }
                    else
                    {
                        if (Plugin.Instance.FindPlace(ref page, 6, 10, item.Size_y, item.Size_x, out byte newX, out byte newY))
                        {
                            item.X = newX;
                            item.Y = newY;
                            item.Rot = 1;
                            selectedItems.Add(item);
                            myItems.Remove(item);
                        }
                    }
                }
                boxes.Add(selectedItems);
            } while (myItems.Count != 0);

            return boxes;
        }
    }
}
//Effect ID is the id parameter, key is an optional instance identifier for modifying instances of an effect, 
//and child name is the unity name of a GameObject with a Text component.