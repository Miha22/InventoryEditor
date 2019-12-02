using System;
using Rocket.Core.Plugins;
using System.IO;
using System.Collections.Generic;
using Logger = Rocket.Core.Logging.Logger;

namespace ItemRestrictorAdvanced
{
    public class Plugin : RocketPlugin<PluginConfiguration>
    {
        internal static Plugin Instance;
        //internal CancellationTokenSource cts;
        //internal CancellationToken token;
        string path;
        string pathPages;
        internal string pathTemp;
        //public const int ProductID = 207; // Read #how-to-publish to see how you obtain a Product ID
        //public Version ProductVersion = new Version("1.0.3"); //Keep it the same when uploading to website!

    protected override void Load()
        {

            Instance = this;
            //_ = new Refresh();
           
            // Provider.onServerShutdown += OnServerShutdown;

            //cts = new CancellationTokenSource();
            //token = cts.Token;

            path = $@"Plugins\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}\Inventories\{SDG.Unturned.Provider.map}";
            pathPages = $@"Plugins\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}\Data\{SDG.Unturned.Provider.map}";
            pathTemp = pathPages + @"\Temp";

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Attributes == FileAttributes.ReadOnly)
                directory.Attributes &= ~FileAttributes.ReadOnly;

            if (!System.IO.Directory.Exists(pathPages))
                System.IO.Directory.CreateDirectory(pathPages);
            DirectoryInfo directoryPages = new DirectoryInfo(pathPages);
            if (directory.Attributes == FileAttributes.ReadOnly)
                directory.Attributes &= ~FileAttributes.ReadOnly;

            if (!System.IO.Directory.Exists(pathTemp))
                System.IO.Directory.CreateDirectory(pathTemp);
            DirectoryInfo directoryTemp = new DirectoryInfo(pathTemp);
            if (directory.Attributes == FileAttributes.ReadOnly)
                directory.Attributes &= ~FileAttributes.ReadOnly;

            //LoadInventoryTo(path, pathPages);
           //WatcherAsync(token, path, pathPages, pathTemp);
            Logger.Log("InventoryEditor by M22 loaded!", ConsoleColor.Cyan);
        }
        protected override void Unload()
        {
            //cts.Cancel();
            //if(Refresh.Refreshes != null)
                //for (byte i = 0; i < Refresh.Refreshes.Length; i++)
                    //Refresh.Refreshes[i].TurnOff(i);
            //ManageUI.UnLoad();
        }
        //[RocketCommand("inventory", "", "", AllowedCaller.Both)]
        //[RocketCommandAlias("inv")]
        //public void Execute(IRocketPlayer caller, string[] command)
        //{
        //    foreach (var steamPlayer in Provider.clients)
        //    {
        //        UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(steamPlayer);
        //        for (byte i = 0; i < 8; i++)
        //        {
        //            for (byte j = 0; j < player.Inventory.getItemCount(i); j++)
        //            {
        //                ItemJar item = player.Inventory.getItem(i, j);
        //                Console.WriteLine($"id: {item.item.id}, x:{item.x}, y:{item.y}  size x: {item.size_x}, size y: {item.size_y}, rot: {item.rot}");
        //            }
        //        }
        //    }
        //}
        //async void WatcherAsync(CancellationToken token, string path, string pathPages, string pathTemp)
        //{
        //    //Console.WriteLine("Начало метода FactorialAsync"); // выполняется синхронно
        //    if (token.IsCancellationRequested)
        //        return;
        //    await Task.Run(() => new Watcher(pathPages + @"\Pages", pathTemp).Run(path, token)); // выполняется асинхронно
        //    //Console.WriteLine("Конец метода FactorialAsync");  // выполняется синхронно
        //}

        //public void OnServerShutdown()
        //{
        //    //System.Diagnostics.Process.Start(@"C:\Deniel\Desktop\прочее\file.txt");
        //    cts.Cancel();
        //    Provider.onServerShutdown -= OnServerShutdown;
        //}

        //[RocketCommand("ShutdownServer", "", "", AllowedCaller.Both)]
        //[RocketCommandAlias("ss")]
        //[RocketCommandAlias("sss")]
        //public void Execute(IRocketPlayer caller, string[] command)
        //{
        //    Application.Quit();
        //    //for (byte i = 0; i < 8; i++)
        //    //{
        //    //    for (byte j = 0; j < ((UnturnedPlayer)caller).Inventory.getItemCount(i); j++)
        //    //    {
        //    //        Console.WriteLine($"Sorted items: {((UnturnedPlayer)caller).Inventory.getItem(i, j).item.id}, size x: {((UnturnedPlayer)caller).Inventory.getItem(i, j).size_x}, size y: {((UnturnedPlayer)caller).Inventory.getItem(i, j).size_y}, rot: {((UnturnedPlayer)caller).Inventory.getItem(i, j).rot}, x: {((UnturnedPlayer)caller).Inventory.getItem(i, j).x}, y: {((UnturnedPlayer)caller).Inventory.getItem(i, j).y}");
        //    //    }
        //    //}
        //    //foreach (var steamPlayer in Provider.clients)
        //    //{
        //    //    Console.WriteLine("----------------------------");
        //    //    Console.WriteLine($"character name: {steamPlayer.playerID.characterName}");
        //    //    Console.WriteLine($"nickname name: {steamPlayer.playerID.nickName}");
        //    //    Console.WriteLine($"playerName name: {steamPlayer.playerID.playerName}");
        //    //    Console.WriteLine($"steamID: {steamPlayer.playerID.steamID.ToString()}");
        //    //    Console.WriteLine("----------------------------");
        //    //}
        //}

        //internal void LoadInventoryTo(string path, string path2)
        //{
        //    foreach (DirectoryInfo directory in new DirectoryInfo("../Players").GetDirectories())
        //    {
        //        string folderForPages = path2 + $@"\Pages";

        //        if (!System.IO.Directory.Exists(folderForPages))
        //            System.IO.Directory.CreateDirectory(folderForPages);
        //        DirectoryInfo dir = new DirectoryInfo(folderForPages);
        //        if (directory.Attributes == FileAttributes.ReadOnly)
        //            directory.Attributes &= ~FileAttributes.ReadOnly;

        //        //string pathPlayer = path + $@"\{directory.Name.Split('_')[0]}.json";
        //        //string pathPages = folderForPages + $@"\{directory.Name.Split('_')[0]}.json";
        //        (List<MyItem> myItems, List<Page> pages) = GetPlayerItems(directory.Name);
        //        if (myItems == null || pages == null)
        //            break;
        //        using (StreamWriter streamWriter = new StreamWriter(path + $@"\{directory.Name.Split('_')[0]}.json"))
        //        {
        //            string json = JsonConvert.SerializeObject((object)myItems, Formatting.Indented);
        //            streamWriter.Write(json);
        //        }
        //        using (StreamWriter streamWriter = new StreamWriter(folderForPages + $@"\{directory.Name.Split('_')[0]}.json"))
        //        {
        //            JsonWriter jsonWriter = (JsonWriter)new JsonTextWriterFormatted((TextWriter)streamWriter);
        //            new JsonSerializer().Serialize(jsonWriter, (object)pages);
        //            jsonWriter.Flush();
        //        }
        //    }
        //}

        //private (List<MyItem>, List<Page>) GetPlayerItems(string steamIdstr, EIgnore ignore = EIgnore.None)//look up a call of GetPlayerItems for "str" for more info
        //{
        //    List<MyItem> myItems = new List<MyItem>();
        //    List<Page> pages = new List<Page>();
        //    Block block = ServerSavedata.readBlock("/Players/" + steamIdstr + "/" + Provider.map + "/Player/Inventory.dat", 0);
        //    if (block == null)
        //        return (null, null);
        //    byte num1 = block.readByte();//BUFFER_SIZE
        //    for (byte index1 = 0, counter = 0; counter < PlayerInventory.PAGES - 1; index1++, counter++)
        //    {
        //        byte width = block.readByte();
        //        byte height = block.readByte();
        //        //block.readByte();
        //        //block.readByte();
        //        byte itemCount = block.readByte();
        //        pages.Add(new Page(index1, width, height));
        //        //Console.WriteLine($"Page: {index1}, width: {width}, height: {height}, items on page: {itemCount}");
        //        //if (ignore == EIgnore.MyItems)
        //        //{
        //        //    for (byte index = 0; index < itemCount; index++)
        //        //    {
        //        //        block.readByte();
        //        //        block.readByte();
        //        //        block.readByte();
        //        //        block.readUInt16();
        //        //        block.readByte();
        //        //        block.readByte();
        //        //        block.readByteArray();
        //        //    }
        //        //    continue;
        //        //}

        //        for (byte index = 0; index < itemCount; index++)
        //        {
        //            byte x = block.readByte();
        //            byte y = block.readByte();
        //            //byte rot = 0;
        //            //if (block.readByte() % 2 != 0)
        //            //    rot = 1;
        //            //else
        //                block.readByte();

        //            ushort newID = block.readUInt16();
        //            byte newAmount = block.readByte();
        //            byte newQuality = block.readByte();
        //            byte[] newState = block.readByteArray();
        //            MyItem myItem = new MyItem(newID, newAmount, newQuality, newState);/*, newState, rot, x, y, index1, width, height);*/
        //            //Console.WriteLine($"item: id: {newID}, amt: {newAmount}, qlt: {newQuality}, Page: {counter}");
        //            if (HasItem(myItem, myItems))
        //                continue;
        //            else
        //                myItems.Add(myItem);
        //        }
        //    }
        //    return (myItems, pages);
        //}
        //internal (bool, List<MyItem>) TryAddItems(string writepath, string readpath, string readpath2)
        //{
        //    Block block = new Block();
        //    block.writeByte(GetPagesCount(readpath2));//how many pages will be
        //    List<MyItem> myItems;
        //    using (StreamReader streamReader = new StreamReader(readpath))//SDG.Framework.IO.Deserialization
        //    {
        //        JsonReader reader = (JsonReader)new JsonTextReader((TextReader)streamReader);
        //        myItems = new JsonSerializer().Deserialize<List<MyItem>>(reader);
        //    }
        //    if (myItems == null)
        //    {
        //        Logger.LogError($"Player has no items in path: {readpath}");
        //        return (false, null);
        //    }
        //    //else
        //    //{
        //    //    Console.WriteLine($"items count: {myItems.Count}");
        //    //}
        //    byte len = (byte)myItems.Count;
        //    for (byte i = 0; i < len; i++)
        //    {
        //        ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, myItems[i].ID);
        //        myItems[i].Size_x = itemAsset.size_x;
        //        myItems[i].Size_y = itemAsset.size_y;
        //        myItems[i].Rot = 0;
        //        if (myItems[i].Count > 1)
        //            for (byte j = 0; j < myItems[i].Count - 1; j++)
        //                myItems.Add(new MyItem(myItems[i].ID, myItems[i].x, myItems[i].Quality, 0, myItems[i].Size_x, myItems[i].Size_y, myItems[i].State));
        //    }
        //    myItems.Sort(new MyItemComparer());
        //    //return (true, null);
        //    byte pages = GetPagesCount(readpath2);
        //    for (byte i = 0; i < pages; i++)
        //    {
        //        byte width, height, itemsCount;
        //        (width, height) = GetPageSize(readpath2, i);
        //        if (width == 0 && height == 0)
        //        {
        //            //Console.WriteLine($"Page: {i} has width and height ZERO");
        //            block.writeByte(0);
        //            block.writeByte(0);
        //            block.writeByte(0);
        //            continue;
        //        }

        //        //Console.WriteLine("-------------------");
        //        //Console.WriteLine($"Operation on PAGE: {i}, width: {width}, height: {height}");
        //        (List<MyItem> selectedItems, List<MyItem> unSelectedItems) = SelectItems(width, height, myItems);
        //        //Console.WriteLine($"myItems count: {myItems.Count}, selectedItems count: {selectedItems.Count}, UNselectedItems count: {unSelectedItems.Count}");
        //        myItems = unSelectedItems;
        //        //Console.WriteLine($"selectedItems = null? {selectedItems == null}, unSelectedItems = null? {unSelectedItems == null}");
        //        itemsCount = (byte)selectedItems.Count;
        //        block.writeByte(width);
        //        block.writeByte(height);
        //        block.writeByte(itemsCount);
        //        //Console.WriteLine($"For Page: {i}, items count: {itemsCount}");
        //        for (byte j = 0; j < itemsCount; j++)
        //        {
        //            ItemJar itemJar = new ItemJar(selectedItems[j].X, selectedItems[j].Y, selectedItems[j].Rot, new Item(selectedItems[j].ID, selectedItems[j].x, selectedItems[j].Quality, selectedItems[j].State));
        //            block.writeByte(itemJar.x);
        //            block.writeByte(itemJar.y);
        //            block.writeByte(itemJar.rot);
        //            block.writeUInt16(itemJar.item.id);
        //            block.writeByte(itemJar.item.amount);
        //            block.writeByte(itemJar.item.quality);
        //            block.writeByteArray(itemJar.item.state);
        //            //block.writeByte(itemJar == null ? (byte)0 : itemJar.x);
        //            //block.writeByte(itemJar == null ? (byte)0 : itemJar.y);
        //            //block.writeByte(itemJar == null ? (byte)0 : itemJar.rot);
        //            //block.writeUInt16(itemJar == null ? (ushort)0 : itemJar.item.id);
        //            //block.writeByte(itemJar == null ? (byte)0 : itemJar.item.amount);
        //            //block.writeByte(itemJar == null ? (byte)0 : itemJar.item.quality);
        //            //block.writeByteArray(itemJar == null ? new byte[0] : itemJar.item.state);
        //        }
        //    }
        //    Functions.WriteBlock(writepath, block, false);
        //    return (true, myItems);
        //}
        //private (List<MyItem>, List<MyItem>) SelectItems(byte width, byte height, List<MyItem> myItems)// for page
        //{
        //    List<MyItem> selectedItems = new List<MyItem>();
        //    List<MyItem> unSelectedItems = new List<MyItem>();
        //    bool[,] page = FillPage(width, height);
        //    foreach (var item in myItems)
        //    {
        //        if (FindPlace(ref page, height, width, item.Size_x, item.Size_y, out byte x, out byte y))
        //        {
        //            //Console.WriteLine("found place");
        //            //Console.WriteLine("------------------------------------");
        //            item.X = x;
        //            item.Y = y;
        //            //Console.WriteLine($"item.X: {item.X}, item.Y: {item.Y}");
        //            //Console.WriteLine("------------------------------------");
        //            selectedItems.Add(item);
        //            //myItems.Remove(item);
        //            //for (int i = 0; i < height; i++)
        //            //{
        //            //    for (int j = 0; j < width; j++)
        //            //    {
        //            //        Console.Write($"{page[i, j]} ");
        //            //    }
        //            //    Console.WriteLine();
        //            //}
        //        }
        //        else
        //        {
        //            if (FindPlace(ref page, height, width, item.Size_y, item.Size_x, out byte nx, out byte ny))
        //            {
        //                item.X = nx;
        //                item.Y = ny;
        //                //byte temp = item.Size_x;
        //                //item.Size_x = item.Size_y;
        //                //item.Size_y = temp;
        //                item.Rot = 1;
        //                selectedItems.Add(item);
        //            }
        //            else
        //                unSelectedItems.Add(item);
        //            //byte temp = item.Size_x;
        //            //item.Size_x = item.Size_y;
        //            //item.Size_y
        //            //Console.WriteLine("not found place");
        //            //myItems.Remove(item);

        //            //unSelectedItems.Add(item);

        //            //Console.WriteLine("not found finished");
        //        }
        //        //Console.WriteLine($"width:{width}, height: {height}, item id: {item.ID}, size x: {item.Size_x}, size y: {item.Size_y}");
        //    }
        //    //Console.WriteLine("point 12");
        //    return (selectedItems, unSelectedItems);
        //}
        //public bool FindPlace(ref bool[,] page, byte pageHeight, byte pageWidth, byte reqWidth, byte reqHeight, out byte x, out byte y)//request > 1
        //{
        //    //Console.WriteLine();
        //    //Console.WriteLine();
        //    //Console.WriteLine("0. FindPlace()/Starting new item");
        //    //Console.WriteLine("-----------------------------");
        //    //for (int i = 0; i < pageHeight; i++)
        //    //{
        //    //    for (int j = 0; j < pageWidth; j++)
        //    //    {
        //    //        Console.Write($"{page[i, j]} ");
        //    //    }
        //    //    Console.WriteLine();
        //    //}
        //    //Console.WriteLine("-----------------------------");
        //    for (byte i = 0; i < pageHeight; i++)
        //    {
        //        for (byte j = 0; j < pageWidth; j++)
        //        {
        //            //Console.WriteLine($"1. FindTrues()  i = {i} j = {j}");
        //            (bool found, EFailure failure) = FindTrues(ref page, pageHeight, pageWidth, i, j, reqWidth, reqHeight, out byte temp_x, out byte temp_y);
        //            if (found)
        //            {
        //                //Console.WriteLine("trues found");
        //                x = temp_x;
        //                y = temp_y;
        //                FillPageCells(ref page, i, j, reqWidth, reqHeight);

        //                return true;
        //            }
        //            else if (failure == EFailure.Width)
        //                break;
        //            else if (failure == EFailure.Height)
        //            {
        //                x = 0;
        //                y = 0;
        //                return false;
        //            }
        //        }
        //    }
        //    x = 0;
        //    y = 0;

        //    return false;//place not found
        //}
        //public void FillPageCells(ref bool[,] page, byte startRowindex, byte startIndex, byte reqWidth, byte reqHeight)
        //{
        //    Console.WriteLine("------------------------");
        //    Console.WriteLine("3. FillPageCells() before");
        //    for (byte i = startRowindex; i < (reqHeight + startRowindex); i++)
        //    {
        //        for (byte j = startIndex; j < (reqWidth + startIndex); j++)
        //        {
        //            Console.Write(page[i, j]);
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine("------------------------");
        //    Console.WriteLine();
        //    for (byte i = startRowindex; i < (reqHeight + startRowindex); i++)
        //    {
        //        for (byte j = startIndex; j < (reqWidth + startIndex); j++)
        //        {
        //            page[i, j] = false;
        //        }
        //    }
        //    Console.WriteLine("------------------------");
        //    Console.WriteLine("3. FillPageCells() after");
        //    for (byte i = startRowindex; i < (reqHeight + startRowindex); i++)
        //    {
        //        for (byte j = startIndex; j < (reqWidth + startIndex); j++)
        //        {
        //            Console.Write(page[i, j]);
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine("------------------------");
        //}
        //public (bool found, EFailure failure) FindTrues(ref bool[,] page, byte pageHeight, byte pageWidth, byte startRowindex, byte startIndex, byte reqWidth, byte reqHeight, out byte temp_x, out byte temp_y)
        //{
        ////    Console.WriteLine("2. FindTrues()");
        ////    Console.WriteLine($"pageWidth: {pageWidth}, startIndex: {startIndex}, reqWidth: {reqWidth}");
        ////    Console.WriteLine($"pageHeight: {pageWidth}, startIndex: {startRowindex}, reqWidth: {reqHeight}");
        //    if ((pageWidth - startIndex) < reqWidth)
        //    {
        //        temp_x = 0;
        //        temp_y = 0;
        //        //Console.WriteLine("Width failure!");
        //        return (false, EFailure.Width);
        //    }
        //    if ((pageHeight - startRowindex) < reqHeight)
        //    {
        //        temp_x = 0;
        //        temp_y = 0;
        //        //Console.WriteLine("Height failure!");
        //        return (false, EFailure.Height);
        //    }

        //    for (byte i = startRowindex; i < (reqHeight + startRowindex); i++)
        //    {
        //        for (byte j = startIndex; j < (reqWidth + startIndex); j++)
        //        {
        //            if (page[i, j] == false)
        //            {
        //                temp_x = 0;
        //                temp_y = 0;
        //                //Console.WriteLine("false found");
        //                return (false, EFailure.Occupied);
        //            }
        //        }
        //    }
        //    //Console.WriteLine("------------------------------------");
        //    temp_y = startRowindex;
        //    temp_x = startIndex;

        //    return (true, 0);
        //}
        //public bool[,] FillPage(byte width, byte height)
        //{
        //    bool[,] page = new bool[height, width];
        //    for (byte i = 0; i < height; i++)
        //    {
        //        for (byte j = 0; j < width; j++)
        //        {
        //            page[i, j] = true;
        //        }
        //    }

        //    return page;
        //}
        //private (byte, byte) GetPageSize(string readpath, byte pageIndex)
        //{
        //    List<Page> pages;
        //    using (StreamReader streamReader = new StreamReader(readpath))//SDG.Framework.IO.Deserialization
        //    {
        //        JsonReader reader = (JsonReader)new JsonTextReader((TextReader)streamReader);
        //        pages = new JsonSerializer().Deserialize<List<Page>>(reader);
        //    }
        //    foreach (var page in pages)
        //    {
        //        if (page.Number == pageIndex)
        //            return (page.Width, page.Height);
        //    }

        //    return (0, 0);
        //}
        //private byte GetPagesCount(string readpath)
        //{
        //    List<Page> pages;
        //    using (StreamReader streamReader = new StreamReader(readpath))//SDG.Framework.IO.Deserialization
        //    {
        //        JsonReader reader = (JsonReader)new JsonTextReader((TextReader)streamReader);
        //        pages = new JsonSerializer().Deserialize<List<Page>>(reader);
        //    }

        //    return (byte)pages.Count;
        //}
        internal bool HasItem(MyItem item, List<MyItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Equals(item))
                {
                    items[i].Count++;
                    return true;
                }
            }
            return false;
        }
        //internal void WriteSpell(Block block)
        //{
        //    block.writeByte(229);
        //    block.writeByte(208);
        //    block.writeByte(19);
        //    block.writeByte(9);
        //    block.writeByte(1);
        //    block.writeByte(0);
        //    block.writeByte(16);
        //    block.writeByte(1);
        //    block.writeByte(237);
        //    block.writeByte(149);
        //    block.writeByte(137);
        //    block.writeByte(1);
        //    block.writeByte(0);
        //    block.writeByte(0);
        //    block.writeByte(112);
        //    block.writeByte(1);
        //}
    }
}
