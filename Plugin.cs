﻿using System;
using Rocket.Core.Plugins;
using Rocket.API;
using Logger = Rocket.Core.Logging.Logger;
using System.IO;
using System.Threading.Tasks;
using Rocket.Core.Commands;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Unturned.Player;
using Newtonsoft.Json;
using SDG.Framework.IO.Serialization;

namespace ItemRestrictorAdvanced
{
    class ItemRestrictor : RocketPlugin<PluginConfiguration>
    {
        static string path = $@"Plugins\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}\Inventories\{SDG.Unturned.Provider.map}";
        internal static ItemRestrictor Instance;
        public static System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
        System.Threading.CancellationToken token = cts.Token;

        public ItemRestrictor()
        {
            Provider.onServerShutdown += OnServerShutdown;
        }

        protected override void Load()
        {
            if (Configuration.Instance.Enabled)
            {
                Instance = this;
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                DirectoryInfo directory = new DirectoryInfo(path);
                if (directory.Attributes == FileAttributes.ReadOnly)
                    directory.Attributes &= ~FileAttributes.ReadOnly;

                try
                {
                    LoadInventoryTo(path);
                    WatcherAsync(token);
                    Logger.Log("ItemRestrictorAdvanced by M22 loaded!", ConsoleColor.Yellow);
                }
                catch (Exception e)
                {
                    Logger.LogException(e, $"EXCEPTION MESSAGE: {e.Message} \n EXCEPTION TargetSite: {e.TargetSite} \n EXCEPTION StackTrace {e.StackTrace}");
                    cts.Cancel();
                    Console.WriteLine();
                }
                //create json files for each player from inventory.dat..
            }
            else
            {
                cts.Cancel();
                Logger.Log("Plugin is turned off in Configuration, unloading...", ConsoleColor.Cyan);
                UnloadPlugin();
            }
        }
        [RocketCommand("inventory", "", "", AllowedCaller.Both)]
        [RocketCommandAlias("inv")]
        public void Execute(IRocketPlayer caller, string[] command)
        {
            foreach (var steamPlayer in Provider.clients)
            {
                UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(steamPlayer);
                for (byte i = 0; i < 8; i++)
                {
                    for (byte j = 0; j < player.Inventory.getItemCount(i); j++)
                    {
                        ItemJar item = player.Inventory.getItem(i, j);
                        Console.WriteLine($"id: {item.item.id}, x:{item.x}, y:{item.y}  size x: {item.size_x}, size y: {item.size_y}, rot: {item.rot}");
                    }
                }
            }
        }
        static async void WatcherAsync(System.Threading.CancellationToken token)
        {
            //Console.WriteLine("Начало метода FactorialAsync"); // выполняется синхронно
            if (token.IsCancellationRequested)
                return;
            await Task.Run(()=>new Watcher().Run(path, token));                            // выполняется асинхронно
            //Console.WriteLine("Конец метода FactorialAsync");  // выполняется синхронно
        }
        public static void OnServerShutdown()
        {
            cts.Cancel();
            Provider.onServerShutdown -= OnServerShutdown;
        }

        //[RocketCommand("ss", "", "", AllowedCaller.Console)]
        //[RocketCommandAlias("ss")]

        //public void CheckTotalVehicles()
        //{
        //    Dictionary<SteamPlayer, ushort> carOwners = new Dictionary<SteamPlayer, ushort>();
        //    foreach (var steamPlayer in Provider.clients)
        //    {
        //    }
        //    foreach (var carOwner in carOwners)
        //    {
        //        Console.WriteLine($"Owner: {carOwner.Key}, cars: {carOwner.Value}");
        //    }

        //}
        //private ushort VehiclesCounter(SteamPlayer steamPlayer)
        //{
        //    ushort counter = 0;
        //    foreach (var veh in VehicleManager.vehicles)
        //    {
        //        if (veh.isLocked && veh.lockedOwner == steamPlayer.playerID.steamID)
        //            counter++;
        //    }
        //    return counter;
        //}
        public void LoadInventoryTo(string path)
        {
            foreach (DirectoryInfo directory in new DirectoryInfo("../Players").GetDirectories())
            {
                //string path = $@"..\Players\{directory.Name}\{Provider.map}\Player\Inventory.json";
                //string path = $@"Plugins\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}\Inventories\{directory.Name.Split('_')[0]}.json";
                //string newPath = path + $@"\{directory.Name.Split('_')[0]}_{PlayerName(directory.Name.Split('_')[0])}.json";
                string folderForPages = path + $@"\PagesData_DoNotTouch";
                //DirectoryInfo dir = new DirectoryInfo(folderForPages);
                //if (!dir.Exists)
                //    dir.Create();

                if (!System.IO.Directory.Exists(folderForPages))
                    System.IO.Directory.CreateDirectory(folderForPages);
                DirectoryInfo dir = new DirectoryInfo(folderForPages);
                if (directory.Attributes == FileAttributes.ReadOnly)
                    directory.Attributes &= ~FileAttributes.ReadOnly;

                string pathPlayer = path + $@"\{directory.Name.Split('_')[0]}.json";
                string pathPages = path + $@"\{dir.Name}\PagesData_{directory.Name.Split('_')[0]}.json";

                //if (!File.Exists(newPath))
                //    File.Create(newPath);
                //FileInfo file = new FileInfo(pathPlayer);
                //if (!file.Exists)
                //    file.Create();
                //if (file.Attributes == FileAttributes.ReadOnly)
                //    file.Attributes &= ~FileAttributes.ReadOnly;
                //file = null;
                //FileInfo file2 = new FileInfo(pathPages);
                //if (!file2.Exists)
                //    file2.Create();
                //if (file2.Attributes == FileAttributes.ReadOnly)
                //    file2.Attributes &= ~FileAttributes.ReadOnly;
                //file2 = null;

                (List<MyItem> myItems, List<Page> pages) = GetPlayerItems(directory.Name);
                //new JSONSerializer().serialize<List<MyItem>>(myItems, newPath, false);
                using (StreamWriter streamWriter = new StreamWriter(pathPlayer))//SDG.Framework.IO.Serialization
                {
                    JsonWriter jsonWriter = (JsonWriter)new JsonTextWriterFormatted((TextWriter)streamWriter);
                    new JsonSerializer().Serialize(jsonWriter, (object)myItems);
                    jsonWriter.Flush();
                }
                using (StreamWriter streamWriter = new StreamWriter(pathPages))//SDG.Framework.IO.Serialization
                {
                    JsonWriter jsonWriter = (JsonWriter)new JsonTextWriterFormatted((TextWriter)streamWriter);
                    new JsonSerializer().Serialize(jsonWriter, (object)pages);
                    jsonWriter.Flush();
                }
                //DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<MyItem>));
                //using (FileStream fs = new FileStream(newPath, FileMode.OpenOrCreate))
                //{
                //    jsonFormatter.WriteObject(fs, myItems);
                //}

                //using (FileStream fs = new FileStream("people.json", FileMode.OpenOrCreate))
                //{
                //    Person[] newpeople = (Person[])jsonFormatter.ReadObject(fs);

                //    foreach (Person p in newpeople)
                //    {
                //        Console.WriteLine("Имя: {0} --- Возраст: {1}", p.Name, p.Age);
                //    }
                //}

                //try
                //{
                //    string path = $@"..\Players\{directory.Name}\{Provider.map}\Player\Inventory.txt";
                //    string path2 = $@"..\Players\{directory.Name}\{Provider.map}\Inventory.txt";
                //    if (!File.Exists(path))
                //        File.Create(path);
                //    if (directory.Attributes == FileAttributes.ReadOnly)
                //        directory.Attributes &= ~FileAttributes.ReadOnly;
                //    using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                //    {
                //        //string[] playerID = directory.Name.Split('_');
                //        List<Item> playerItems = GetPlayerItems(directory.Name);
                //        foreach (Item item in playerItems)
                //        {
                //            //sw.WriteLine($"ID: {item.id}\n Amount: {item.amount}\n Quality: {item.quality}");
                //            sw.WriteLine($"ID: {item.id}");
                //            sw.WriteLine($"Amount: {item.state[0]}");
                //            sw.WriteLine($"x{item.amount}");
                //            sw.WriteLine($"Quality: {item.quality}");
                //            sw.WriteLine("-------");
                //        }
                //    }
                //}
                //catch (Exception e)
                //{
                //    Logger.LogError($"{e.Message}\n{e.TargetSite}");
                //}
            }
        }
        private (List<MyItem>, List<Page>) GetPlayerItems(string steamIdstr)//look up a call of GetPlayerItems for "str" for more info
        {
            List<MyItem> myItems = new List<MyItem>();
            List<Page> pages = new List<Page>();
            Block block = ServerSavedata.readBlock("/Players/" + steamIdstr + "/" + Provider.map + "/Player/Inventory.dat", 0);
            if (block == null)
                System.Console.WriteLine("Player has no items");
            else
                System.Console.WriteLine("Player has items");
            byte num1 = block.readByte();//BUFFER_SIZE
            for (byte index1 = 0, counter = 0; counter < PlayerInventory.PAGES - 1; index1++, counter++)
            {
                byte width = block.readByte();
                byte height = block.readByte();
                //block.readByte();
                //block.readByte();
                byte itemCount = block.readByte();
                if (width == 0 && height == 0)
                {
                    index1--;
                    continue;
                }
                pages.Add(new Page(index1, width, height));
                Console.WriteLine($"Page: {index1}, width: {width}, height: {height}, items on page: {itemCount}");
                for (byte index = 0; index < itemCount; index++)
                {
                    byte x = block.readByte();
                    byte y = block.readByte();
                    byte rot = 0;
                    if (num1 > 4)
                        rot = block.readByte();
                    else
                        block.readByte();

                    ushort newID = block.readUInt16();
                    byte newAmount = block.readByte();
                    byte newQuality = block.readByte();
                    byte[] newState = block.readByteArray();
                    MyItem myItem = new MyItem(newID, newAmount, newQuality);/*, newState, rot, x, y, index1, width, height);*/
                    if (HasItem(myItem, myItems))
                        continue;
                    else
                        myItems.Add(myItem);
                }
            }
            return (myItems, pages);
        }
        internal static void OnInventoryAdded()
        {

        }
        public (bool, List<MyItem>) TryAddItems(string writepath, string readpath, string readpath2)
        {
            Block block = new Block();
            block.writeByte(PlayerInventory.SAVEDATA_VERSION);
            List<MyItem> myItems;
            using (StreamReader streamReader = new StreamReader(readpath))//SDG.Framework.IO.Deserialization
            {
                JsonReader reader = (JsonReader)new JsonTextReader((TextReader)streamReader);
                myItems = new JsonSerializer().Deserialize<List<MyItem>>(reader);
            }
            if (myItems == null)
            {
                Console.WriteLine("Items is null");
                return (false, null);
            }
            else
            {
                Console.WriteLine($"items count: {myItems.Count}");
            }
            byte len = (byte)myItems.Count;
            for (byte i = 0; i < len; i++)
            {
                ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, myItems[i].ID);
                myItems[i].Size_x = itemAsset.size_x;
                myItems[i].Size_y = itemAsset.size_y;
                myItems[i].Rot = 0;
                if (myItems[i].Count > 1)
                {
                    for (byte j = 0; j < myItems[i].Count - 1; j++)
                    {
                        myItems.Add(myItems[i]);
                    }
                }
            }
            myItems.Sort(new MyItemComparer());
            //foreach (var item in myItems)
            //{
            //    Console.WriteLine($"Sorted items: {item.ID}, size x: {item.Size_x}, size y: {item.Size_y}");
            //}
            byte pages = GetPagesCount(readpath2);
            for (byte i = 0; i < pages; i++)
            {
                byte width, height, itemsCount;
                (width, height) = GetPageSize(readpath2, i);
                if ((width == 0 && height == 0))
                {
                    block.writeByte(1);
                    block.writeByte(1);
                    block.writeByte(1);
                    block.writeByte(0);
                    block.writeByte(0);
                    block.writeByte(0);
                    block.writeUInt16(0);
                    block.writeByte(0);
                    block.writeByte(0);
                    block.writeByteArray(new byte[0]);
                    continue;
                }
                //Console.WriteLine("-------------------");
                //Console.WriteLine($"Operation on PAGE: {i}, width: {width}, height: {height}");
                (List<MyItem> selectedItems, List<MyItem> unSelectedItems) = SelectItems(width, height, myItems);
                //Console.WriteLine($"myItems count: {myItems.Count}, selectedItems count: {selectedItems.Count}, UNselectedItems count: {unSelectedItems.Count}");
                myItems = unSelectedItems;
                //Console.WriteLine($"selectedItems = null? {selectedItems == null}, unSelectedItems = null? {unSelectedItems == null}");
                itemsCount = (byte)selectedItems.Count;
                block.writeByte(width);
                block.writeByte(height);
                block.writeByte(itemsCount);
                //Console.WriteLine($"For Page: {i}, items count: {itemsCount}");
                for (byte j = 0; j < itemsCount; j++)
                {
                    ItemJar itemJar = new ItemJar(selectedItems[j].X, selectedItems[j].Y, selectedItems[j].Rot, new Item(selectedItems[j].ID, selectedItems[j].x, selectedItems[j].Quality));
                    block.writeByte(itemJar == null ? (byte)0 : itemJar.x);
                    block.writeByte(itemJar == null ? (byte)0 : itemJar.y);
                    block.writeByte(itemJar == null ? (byte)0 : itemJar.rot);
                    block.writeUInt16(itemJar == null ? (ushort)0 : itemJar.item.id);
                    block.writeByte(itemJar == null ? (byte)0 : itemJar.item.amount);
                    block.writeByte(itemJar == null ? (byte)0 : itemJar.item.quality);
                    block.writeByteArray(itemJar == null ? new byte[0] : itemJar.item.state);
                }
            }
            Functions.WriteBlock(writepath, block);
            return (true, myItems);
        }
        private (List<MyItem>, List<MyItem>) SelectItems(byte width, byte height, List<MyItem> myItems)// for page
        {
            List<MyItem> selectedItems = new List<MyItem>();
            List<MyItem> unSelectedItems = new List<MyItem>();
            bool[,] page = FillPage(width, height);
            foreach (var item in myItems)
            {
                if (FindPlace(ref page, height, width, item.Size_x, item.Size_y, out byte x, out byte y))
                {
                    //Console.WriteLine("found place");
                    //Console.WriteLine("------------------------------------");
                    item.X = x;
                    item.Y = y;
                    //Console.WriteLine($"item.X: {item.X}, item.Y: {item.Y}");
                    //Console.WriteLine("------------------------------------");
                    selectedItems.Add(item);
                    //myItems.Remove(item);
                    //for (int i = 0; i < height; i++)
                    //{
                    //    for (int j = 0; j < width; j++)
                    //    {
                    //        Console.Write($"{page[i, j]} ");
                    //    }
                    //    Console.WriteLine();
                    //}
                }
                else
                {
                    if (FindPlace(ref page, height, width, item.Size_y, item.Size_x, out x, out y))
                    {
                        item.X = x;
                        item.Y = y;
                        byte temp = item.Size_x;
                        item.Size_x = item.Size_y;
                        item.Size_y = temp;
                        item.Rot = 1;
                        selectedItems.Add(item);
                    }
                    else
                        unSelectedItems.Add(item);
                    //byte temp = item.Size_x;
                    //item.Size_x = item.Size_y;
                    //item.Size_y
                    //Console.WriteLine("not found place");
                    //myItems.Remove(item);

                    //unSelectedItems.Add(item);

                    //Console.WriteLine("not found finished");
                }
                //Console.WriteLine($"width:{width}, height: {height}, item id: {item.ID}, size x: {item.Size_x}, size y: {item.Size_y}");
            }
            //Console.WriteLine("point 12");
            return (selectedItems, unSelectedItems);
        }
        private bool FindPlace(ref bool[,] page, byte pageHeight, byte pageWidth, byte reqWidth, byte reqHeight, out byte x, out byte y)//request > 1
        {
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine("0. FindPlace()/Starting new item");
            //Console.WriteLine("-----------------------------");
            //for (int i = 0; i < pageHeight; i++)
            //{
            //    for (int j = 0; j < pageWidth; j++)
            //    {
            //        Console.Write($"{page[i, j]} ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine("-----------------------------");
            for (byte i = 0; i < pageHeight; i++)
            {
                for (byte j = 0; j < pageWidth; j++)
                {
                    //Console.WriteLine($"1. FindTrues()  i = {i} j = {j}");
                    (bool found, Failure failure) = FindTrues(ref page, pageHeight, pageWidth, i, j, reqWidth, reqHeight, out byte temp_x, out byte temp_y);
                    if (found)
                    {
                        //Console.WriteLine("trues found");
                        x = temp_x;
                        y = temp_y;
                        FillPageCells(ref page, i, j, reqWidth, reqHeight);

                        return true;
                    }
                    else if (failure == Failure.Width)
                        break;
                    else if (failure == Failure.Height)
                    {
                        x = 0;
                        y = 0;
                        return false;
                    }
                }
            }
            x = 0;
            y = 0;
            Console.WriteLine("Unexpected error");
            return false;
        }
        private void FillPageCells(ref bool[,] page, byte startRowindex, byte startIndex, byte reqWidth, byte reqHeight)
        {
            //Console.WriteLine("3. FillPageCells()");
            for (byte i = startRowindex; i < (reqHeight + startRowindex); i++)
            {
                for (byte j = startIndex; j < (reqWidth + startIndex); j++)
                {
                    page[i, j] = false;
                }
            }
        }
        private (bool found, Failure failure) FindTrues(ref bool[,] page, byte pageHeight, byte pageWidth, byte startRowindex, byte startIndex, byte reqWidth, byte reqHeight, out byte temp_x, out byte temp_y)
        {
        //    Console.WriteLine("2. FindTrues()");
        //    Console.WriteLine($"pageWidth: {pageWidth}, startIndex: {startIndex}, reqWidth: {reqWidth}");
        //    Console.WriteLine($"pageHeight: {pageWidth}, startIndex: {startRowindex}, reqWidth: {reqHeight}");
            if ((pageWidth - startIndex) < reqWidth)
            {
                temp_x = 0;
                temp_y = 0;
                //Console.WriteLine("Width failure!");
                return (false, Failure.Width);
            }
            if ((pageHeight - startRowindex) < reqHeight)
            {
                temp_x = 0;
                temp_y = 0;
                //Console.WriteLine("Height failure!");
                return (false, Failure.Height);
            }

            for (byte i = startRowindex; i < (reqHeight + startRowindex); i++)
            {
                for (byte j = startIndex; j < (reqWidth + startIndex); j++)
                {
                    if (page[i, j] == false)
                    {
                        temp_x = 0;
                        temp_y = 0;
                        //Console.WriteLine("false found");
                        return (false, Failure.Occupied);
                    }
                }
            }
            //Console.WriteLine("------------------------------------");
            temp_y = startRowindex;
            temp_x = startIndex;
            return (true, 0);
        }
        private bool[,] FillPage(byte width, byte height)
        {
            bool[,] page = new bool[height, width];
            for (byte i = 0; i < height; i++)
            {
                for (byte j = 0; j < width; j++)
                {
                    page[i, j] = true;
                }
            }

            return page;
        }
        private (byte, byte) GetPageSize(string readpath, byte pageIndex)
        {
            List<Page> pages;
            using (StreamReader streamReader = new StreamReader(readpath))//SDG.Framework.IO.Deserialization
            {
                JsonReader reader = (JsonReader)new JsonTextReader((TextReader)streamReader);
                pages = new JsonSerializer().Deserialize<List<Page>>(reader);
            }
            foreach (var page in pages)
            {
                if (page.Number == pageIndex)
                    return (page.Width, page.Height);
            }

            return (0, 0);
        }
        private byte GetPagesCount(string readpath)
        {
            List<Page> pages;
            using (StreamReader streamReader = new StreamReader(readpath))//SDG.Framework.IO.Deserialization
            {
                JsonReader reader = (JsonReader)new JsonTextReader((TextReader)streamReader);
                pages = new JsonSerializer().Deserialize<List<Page>>(reader);
            }

            return (byte)pages.Count;
        }
        private bool HasItem(MyItem item, List<MyItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == item.ID && items[i].Quality == item.Quality && items[i].x == item.x)
                {
                    items[i].Count++;
                    return true;
                }
            }
            return false;
        }
    }
}
