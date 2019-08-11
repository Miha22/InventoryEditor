﻿using System;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Unturned.Player;
using System.Globalization;
using Logger = Rocket.Core.Logging.Logger;
using System.Threading;

namespace ItemRestrictorAdvanced
{
    class ManageUI
    {
        internal static List<ManageUI> Instances;
        internal static List<Player> UICallers;
        internal static byte PagesCount { get; set; }
        private static CancellationTokenSource cts;
        private static CancellationToken token;
        internal byte PagesCountInv { get; set; }
        private byte playerIndex;
        private byte itemIndex;
        private byte currentPage;
        private Player targetPlayer;
        private readonly Player callerPlayer;
        private List<List<MyItem>> UIitemsPages;
        private MyItem selectedItem;

        static ManageUI()
        {
            Instances = new List<ManageUI>();
            UICallers = new List<Player>();
            cts = new CancellationTokenSource();
            token = cts.Token;
        }

        public ManageUI(byte pagesCount, Player caller)
        {
            currentPage = 1;
            callerPlayer = caller;
            ManageUI.PagesCount = pagesCount;// !
            UIitemsPages = new List<List<MyItem>>();
            Instances.Add(this);
        }

        internal static void UnLoad()
        {
            if (ManageUI.Instances == null)
                return;
            cts.Cancel();
            if (ManageUI.Instances.Count != 0)
            {
                foreach (ManageUI manageUI in ManageUI.Instances)
                    manageUI.UnLoadEvents();
            }
            
            foreach (Player caller in UICallers)
            {
                EffectManager.askEffectClearByID(8100, caller.channel.owner.playerID.steamID);
                EffectManager.askEffectClearByID(8101, caller.channel.owner.playerID.steamID);
                EffectManager.askEffectClearByID(8102, caller.channel.owner.playerID.steamID);
                caller.serversideSetPluginModal(false);
            }
        }

        private void UnLoadEvents()
        {
            EffectManager.onEffectButtonClicked -= this.OnEffectButtonClick;
            EffectManager.onEffectButtonClicked -= this.OnEffectButtonClick8101;
            EffectManager.onEffectButtonClicked -= this.OnEffectButtonClick8102;
        }

        private void SetCurrentPage(byte page, ulong mSteamID)
        {
            for (byte i = 0; i < Refresh.Refreshes.Length; i++)
            {
                if (Refresh.Refreshes[i].CallerSteamID.m_SteamID == mSteamID)
                {
                    Refresh.Refreshes[i].CurrentPage = page;
                    return;
                }
            }
        }

        private void SetPlayersList(byte CurrentPage, Steamworks.CSteamID CallerSteamID)
        {
            byte multiplier = (byte)((CurrentPage - 1) * 24);
            for (byte i = multiplier; (i < (24 + multiplier)) && (i < (byte)Provider.clients.Count); i++)
                EffectManager.sendUIEffectText(22, CallerSteamID, false, $"text{i}", $"{Provider.clients[i].playerID.characterName}");
        }

        public void OnEffectButtonClick(Player callerPlayer, string buttonName)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"text[0-9]", System.Text.RegularExpressions.RegexOptions.Compiled);
            byte.TryParse(buttonName.Substring(4), out playerIndex);
            if (regex.IsMatch(buttonName))
            {
                if (Provider.clients.Count < ((playerIndex + 1) * PagesCount))
                    return;
                playerIndex = (byte)((currentPage - 1) * 24);
                currentPage = 1;
                try
                {
                    targetPlayer = Provider.clients[playerIndex].player;
                }
                catch (Exception)
                {
                    Logger.Log($"Player not found: player has just left the server. UI call from: {callerPlayer.channel.owner.playerID.characterName}");
                    return;
                }
                targetPlayer.inventory.onInventoryAdded += OnInventoryChange;
                targetPlayer.inventory.onInventoryRemoved += OnInventoryChange;
                EffectManager.askEffectClearByID(8100, callerPlayer.channel.owner.playerID.steamID);
                GetTargetItems();
                ShowItemsUI(callerPlayer, currentPage);

                EffectManager.onEffectButtonClicked -= OnEffectButtonClick;
                EffectManager.onEffectButtonClicked += OnEffectButtonClick8101;
                for (byte i = 0; i < Refresh.Refreshes.Length; i++)
                {
                    if (Refresh.Refreshes[i].CallerSteamID.m_SteamID == callerPlayer.channel.owner.playerID.steamID.m_SteamID)
                    {
                        Refresh.Refreshes[i].TurnOff(i);
                        break;
                    }
                }
            }
            else if (buttonName == "ButtonNext")
            {
                if (currentPage == PagesCount)
                {
                    EffectManager.sendUIEffectText(22, callerPlayer.channel.owner.playerID.steamID, false, "page", $"{PagesCount}");
                    SetCurrentPage(PagesCount, callerPlayer.channel.owner.playerID.steamID.m_SteamID);
                    SetPlayersList(PagesCount, callerPlayer.channel.owner.playerID.steamID);
                    currentPage = 1;
                }
                else
                {
                    EffectManager.sendUIEffectText(22, callerPlayer.channel.owner.playerID.steamID, false, "page", $"{currentPage}");
                    SetCurrentPage(currentPage, callerPlayer.channel.owner.playerID.steamID.m_SteamID);
                    SetPlayersList(currentPage, callerPlayer.channel.owner.playerID.steamID);
                    currentPage++;
                    //sends current page
                }
            }
            else if (buttonName == "ButtonPrev")
            {
                if (currentPage == 1)
                {
                    EffectManager.sendUIEffectText(22, callerPlayer.channel.owner.playerID.steamID, false, "page", $"1");
                    SetCurrentPage(1, callerPlayer.channel.owner.playerID.steamID.m_SteamID);
                    SetPlayersList(1, callerPlayer.channel.owner.playerID.steamID);
                    currentPage = PagesCount;
                    //sends 1st page
                }
                else
                {
                    EffectManager.sendUIEffectText(22, callerPlayer.channel.owner.playerID.steamID, false, "page", $"{currentPage}");
                    SetCurrentPage(currentPage, callerPlayer.channel.owner.playerID.steamID.m_SteamID);
                    SetPlayersList(currentPage, callerPlayer.channel.owner.playerID.steamID);
                    currentPage--;
                    //sends current page
                }
            }
            else if (buttonName == "ButtonExit")
            {
                EffectManager.onEffectButtonClicked -= OnEffectButtonClick;
                QuitUI(callerPlayer, 8100);
            }
            else//non button click
                return;
            //Logger.LogException(new MissingMethodException("Internal exception: Missing Method: a method is missing in Dictionary or button name mismatched. \n"));
        }

        public void OnEffectButtonClick8101(Player callerPlayer, string buttonName)
        {
            if(buttonName.Substring(0, 4) == "item")
            {
                byte.TryParse(buttonName.Substring(4), out itemIndex);
                //itemIndex += (byte)((currentPage - 1) * 24); 
                buttonName = "item";
            }
                
            switch (buttonName)
            {
                case "item":
                    //show 8102
                    //EffectManager.askEffectClearByID(8101, callerPlayer.channel.owner.playerID.steamID);
                    //if (UIitemsPages[currentPage - 1].Count >= (itemIndex + 1))
                    //    selectedItem = UIitemsPages[currentPage - 1][itemIndex];
                    //else
                    //    return;    
                    //selectedItem = (UIitemsPages[currentPage - 1].Count >= (itemIndex + 1))?(selectedItem = UIitemsPages[currentPage - 1][itemIndex]):(selectedItem = null);
                    if (UIitemsPages[currentPage - 1].Count >= (itemIndex + 1))
                        selectedItem = UIitemsPages[currentPage - 1][itemIndex];
                    else
                        selectedItem = new MyItem();
                    EffectManager.onEffectButtonClicked -= OnEffectButtonClick8101;
                    EffectManager.onEffectButtonClicked += OnEffectButtonClick8102;
                    EffectManager.onEffectTextCommitted += OnTextCommited;
                    EffectManager.sendUIEffect(8102, 24, callerPlayer.channel.owner.playerID.steamID, false);
                    break;

                case "SaveExit":
                    SaveExitAddItem(callerPlayer);
                    break;

                case "ButtonNext":
                    if (currentPage == PagesCountInv)
                        currentPage = 1;
                    else
                        currentPage++;
                    GetTargetItems();
                    EffectManager.askEffectClearByID(8101, callerPlayer.channel.owner.playerID.steamID);
                    ShowItemsUI(callerPlayer, currentPage);
                    break;

                case "ButtonPrev":
                    if (currentPage == 1)
                        currentPage = PagesCountInv;
                    else
                        currentPage--;
                    GetTargetItems();
                    EffectManager.askEffectClearByID(8101, callerPlayer.channel.owner.playerID.steamID);
                    ShowItemsUI(callerPlayer, currentPage);
                    break;

                case "MainPage":
                    if (targetPlayer != null)
                    {
                        targetPlayer.inventory.onInventoryAdded -= OnInventoryChange;
                        targetPlayer.inventory.onInventoryRemoved -= OnInventoryChange;
                    }
                    EffectManager.askEffectClearByID(8101, UnturnedPlayer.FromPlayer(callerPlayer).CSteamID);
                    EffectManager.onEffectButtonClicked -= OnEffectButtonClick8101;
                    CommandGetInventory.Instance.Execute(UnturnedPlayer.FromPlayer(callerPlayer), null);
                    break;

                case "ButtonExit":
                    if (targetPlayer != null)
                    {
                        targetPlayer.inventory.onInventoryAdded -= OnInventoryChange;
                        targetPlayer.inventory.onInventoryRemoved -= OnInventoryChange;
                    }
                    EffectManager.onEffectButtonClicked -= OnEffectButtonClick8101;
                    QuitUI(callerPlayer, 8101);
                    break;
                default://non button click
                    return;
            }
        }

        private void OnTextCommited(Player player, string button, string text)
        {
            ushort id = 0;
            byte amount = 0;//make globals
            if (button == "ID")
                ushort.TryParse(text, out id);
            else
                byte.TryParse(text, out amount);
            if (selectedItem != null)
            {
                selectedItem.ID = (id != 0) ? (id) : (selectedItem.ID);
                selectedItem.Count = (amount != 0) ? (amount) : (selectedItem.Count);
            }
            else
            {
                selectedItem = new MyItem();
                selectedItem.ID = (id != 0) ? (id) : (selectedItem.ID);
                selectedItem.Count = (amount != 0) ? (amount) : (selectedItem.Count);
            }
                
            //if some input field is empty then load existing count
           // newMyItem = new MyItem(id, amount, )
        }

        public void OnEffectButtonClick8102(Player callerPlayer, string buttonName)
        {
            if (buttonName == "SaveExit")
            {
                EffectManager.onEffectButtonClicked -= OnEffectButtonClick8102;
                EffectManager.onEffectButtonClicked += OnEffectButtonClick8101;
                EffectManager.askEffectClearByID(8102, callerPlayer.channel.owner.playerID.steamID);
                EffectManager.onEffectTextCommitted -= OnTextCommited;
            }
        }

        private async void OnInventoryChange(byte page, byte index, ItemJar item)
        {
            if (token.IsCancellationRequested)
                return;
            //EffectManager.askEffectClearByID(8101, this.callerPlayer.channel.owner.playerID.steamID);
            GetTargetItems();
            await System.Threading.Tasks.Task.Run(()=> ShowItemsUI(this.callerPlayer, currentPage));
            //ShowItemsUI(this.callerPlayer, currentPage);
        }

        private void ShowItemsUI(Player callerPlayer, byte page)//target player idnex in provider.clients
        {
            try
            {
                EffectManager.sendUIEffect(8101, 23, callerPlayer.channel.owner.playerID.steamID, false);
                if(UIitemsPages[page - 1].Count != 0)
                    for (byte i = 0; i < UIitemsPages[page - 1].Count; i++)
                        EffectManager.sendUIEffectText(23, callerPlayer.channel.owner.playerID.steamID, false, $"item{i}", $"{((ItemAsset)Assets.find(EAssetType.ITEM, UIitemsPages[page - 1][i].ID)).itemName} \r\n Count: {UIitemsPages[page - 1][i].Count}");
                EffectManager.sendUIEffectText(23, callerPlayer.channel.owner.playerID.steamID, false, "page", $"{page}");
                EffectManager.sendUIEffectText(23, callerPlayer.channel.owner.playerID.steamID, false, "pagemax", $"{PagesCountInv}");
            }
            catch (Exception)
            {
                Console.WriteLine("exception in show ui");
                QuitUI(callerPlayer, 8101);
                return;
            }
        }

        private void GetTargetItems()
        {
            UIitemsPages.Clear();
            Items[] pages;
            try
            {
                pages = targetPlayer.inventory.items;// array (reference type in stack => by value)
            }
            catch (Exception)
            {
                // load to Inventory.dat and maybe to virtual inventory
                Console.WriteLine("exception in target items");
                QuitUI(callerPlayer, 8101);
                return;
            }
            List<MyItem> items = new List<MyItem>();

            foreach (var page in pages)
            {
                if (page == null)
                    continue;
                foreach (ItemJar item in page.items)
                {
                    MyItem myItem = new MyItem(item.item.id, item.item.amount, item.item.quality, item.item.state);
                    if (ItemRestrictor.Instance.HasItem(myItem, items))
                        continue;
                    else
                        items.Add(myItem);
                }
            }

            if (items.Count == 0)
            {
                PagesCountInv = 1;
                UIitemsPages.Add(items);
                return;
            }
                
            ushort counter = 0;
            for (byte i = 0; i < (byte)Math.Ceiling(items.Count / 24.0); i++)
            {
                List<MyItem> myPage = new List<MyItem>();
                for (ushort j = 0; (j < 24) && (counter < (ushort)items.Count); j++, counter++)
                    myPage.Add(items[j + (24 * i)]);
                UIitemsPages.Add(myPage);
            }
            PagesCountInv = (byte)UIitemsPages.Count;
            Console.WriteLine("in get target items");
            Console.WriteLine($"UIitemsPages.Count: {UIitemsPages.Count}");
            Console.WriteLine($"PagesCountInv: {PagesCountInv}");
        }

        private void QuitUI(Player callerPlayer, ushort effectId)
        {
            EffectManager.askEffectClearByID(effectId, UnturnedPlayer.FromPlayer(callerPlayer).CSteamID);
            callerPlayer.serversideSetPluginModal(false);
            ManageUI.UICallers.Remove(callerPlayer);
            Console.WriteLine($"caller: {callerPlayer.channel.owner.playerID.characterName} removed from list!");
            UIitemsPages.Clear();
        }

        private void SaveExitAddItem(Player callerPlayer)
        {
            string id = "";
            string x = "";
            TextInfo text = CultureInfo.CurrentCulture.TextInfo;
            EffectManager.sendEffectTextCommitted("ID", id);
            EffectManager.sendEffectTextCommitted("x", x);
            Console.WriteLine();
            Console.WriteLine($"ID: {id}, x: {x}");
            Console.WriteLine();
        }
    }
    class AddManager
    {

    }
}
