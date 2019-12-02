using SDG.Unturned;
using System.Collections.Generic;

namespace ItemRestrictorAdvanced
{
    class ManageCloudUI
    {
        byte itemIndex;
        byte pagesCount;
        byte currentPage;
        List<List<MyItem>> MyItemsPages;

        public ManageCloudUI(List<List<MyItem>> myItemsPages, byte pagesCount)
        {
            currentPage = 1;
            MyItemsPages = myItemsPages;
            this.pagesCount = pagesCount;
        }

        public void OnEffectButtonClick8101(Player callerPlayer, string buttonName)
        {
            if (buttonName.Substring(0, 4) == "item")
            {
                byte.TryParse(buttonName.Substring(4), out itemIndex);
                //itemIndex += (byte)((currentPage - 1) * 24);
                buttonName = "item";
            }

            switch (buttonName)
            {
                case "item":
                    if((itemIndex + 1) <= MyItemsPages[currentPage - 1].Count)
                    {
                        MyItem myItem = MyItemsPages[currentPage - 1][itemIndex];
                        if (callerPlayer.inventory.tryAddItemAuto(new Item(myItem.ID, myItem.X, myItem.Quality, myItem.State), false, false, false, false))
                        {
                            //MyItemsPages[currentPage - 1][itemIndex].Count--;
                            if (--MyItemsPages[currentPage - 1][itemIndex].Count == 0)
                                MyItemsPages[currentPage - 1].RemoveAt(itemIndex);
                            //ReturnLoad(MyItemsPages, callerPlayer.channel.owner.playerID.steamID.ToString());
                        }
                        else
                            Rocket.Unturned.Chat.UnturnedChat.Say(Rocket.Unturned.Player.UnturnedPlayer.FromPlayer(callerPlayer), "You inventory is full, remove something");
                        EffectManager.askEffectClearByID(26, callerPlayer.channel.owner.playerID.steamID);
                        ShowItemsUI(callerPlayer, currentPage);
                    }                  
                    break;

                case "ButtonNext":
                    if (currentPage == pagesCount)
                        currentPage = 1;
                    else
                        currentPage++;
                    EffectManager.askEffectClearByID(8101, callerPlayer.channel.owner.playerID.steamID);
                    ShowItemsUI(callerPlayer, currentPage);
                    break;

                case "ButtonPrev":
                    if (currentPage == 1)
                        currentPage = pagesCount;
                    else
                        currentPage--;
                    EffectManager.askEffectClearByID(8101, callerPlayer.channel.owner.playerID.steamID);
                    ShowItemsUI(callerPlayer, currentPage);
                    break;

                case "MainPage":
                    EffectManager.onEffectButtonClicked -= this.OnEffectButtonClick8101;
                    ReturnLoad(MyItemsPages, callerPlayer.channel.owner.playerID.steamID.ToString());
                    QuitUI(callerPlayer, 8101);
                    break;
                case "ButtonExit":
                    EffectManager.onEffectButtonClicked -= this.OnEffectButtonClick8101;
                    ReturnLoad(MyItemsPages, callerPlayer.channel.owner.playerID.steamID.ToString());
                    QuitUI(callerPlayer, 8101);
                    break;
                default://non button click
                    return;
            }
        }
        private void ShowItemsUI(Player callPlayer, byte page)//target player idnex in provider.clients
        {
            try
            {
                EffectManager.sendUIEffect(8101, 26, callPlayer.channel.owner.playerID.steamID, true);
                if (MyItemsPages[page - 1].Count != 0)
                    for (byte i = 0; i < MyItemsPages[page - 1].Count; i++)
                        EffectManager.sendUIEffectText(26, callPlayer.channel.owner.playerID.steamID, true, $"item{i}", $"{((ItemAsset)Assets.find(EAssetType.ITEM, MyItemsPages[pagesCount - 1][i].ID)).itemName}\r\nID: {MyItemsPages[pagesCount - 1][i].ID}\r\nCount: {MyItemsPages[pagesCount - 1][i].Count}");
                for (byte i = (byte)MyItemsPages[0].Count; i < 24; i++)
                    EffectManager.sendUIEffectText(26, callPlayer.channel.owner.playerID.steamID, true, $"item{i}", $"");
                EffectManager.sendUIEffectText(26, callPlayer.channel.owner.playerID.steamID, true, "page", $"{page}");
                EffectManager.sendUIEffectText(26, callPlayer.channel.owner.playerID.steamID, true, "pagemax", $"{pagesCount}");
                EffectManager.sendUIEffectText(26, callPlayer.channel.owner.playerID.steamID, true, "playerName", $"Cloud: {callPlayer.channel.owner.playerID.characterName}");
            }
            catch (System.Exception e)
            {
                Rocket.Core.Logging.Logger.LogException(e, "Exception in ManageCloudUI.ShowItemsUI(Player, byte)");
                QuitUI(callPlayer, 8101);
                return;
            }
        }

        private void ReturnLoad(List<List<MyItem>> myItems, string CSteamID)
        {
            Block block = new Block(0);
            ushort itemsCount = 0;
            foreach (List<MyItem> page in myItems)
            {
                foreach (MyItem item in page)
                    itemsCount += item.Count;
            }
            byte multiplier = (byte)System.Math.Floor(itemsCount / 256.0);
            block.writeByte((byte)itemsCount);
            block.writeByte(multiplier);
            if (!System.IO.Directory.Exists(Plugin.Instance.pathTemp + $"\\{CSteamID}"))
                System.IO.Directory.CreateDirectory(Plugin.Instance.pathTemp + $"\\{CSteamID}");
            if (itemsCount == 0)
            {
                Functions.WriteBlock(Plugin.Instance.pathTemp + $"\\{CSteamID}\\Heap.dat", block, false);
                return;
            }


            foreach (List<MyItem> page in myItems)
            {
                foreach (MyItem item in page)
                {
                    //itemsCount += item.Count;
                    for (ushort i = 0; i < item.Count; i++)
                    {
                        block.writeUInt16(item.ID);
                        block.writeByte(item.X);
                        block.writeByte(item.Quality);
                        block.writeUInt16((ushort)item.State.Length);
                        foreach (byte bite in item.State)
                            block.writeByte(bite);
                    }
                    
                    //block.writeUInt16(item.id);
                    //block.writeByte(item.amount);
                    //block.writeByte(item.quality);
                    //block.writeUInt16((ushort)item.state.Length);
                    //foreach (byte bite in item.state)
                    //    block.writeByte(bite);
                }
            }

            Functions.WriteBlock(Plugin.Instance.pathTemp + $"\\{CSteamID}\\Heap.dat", block, false);
        }

        private void QuitUI(Player callerPlayer, ushort effectId)
        {
            EffectManager.askEffectClearByID(effectId, callerPlayer.channel.owner.playerID.steamID);
            callerPlayer.serversideSetPluginModal(false);
            //ManageUI.UICallers.Remove(callerPlayer);
            MyItemsPages.Clear();
        }
    }
}
//Effect ID is the id parameter, key is an optional instance identifier for modifying instances of an effect, 
//and child name is the unity name of a GameObject with a Text component.