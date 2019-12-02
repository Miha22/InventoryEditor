using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;

namespace ItemRestrictorAdvanced
{
    public class CommandGetVirtual : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "incloud";
        public string Help => "Shows your virtual inventory using UI";
        public string Syntax => "/incloud or /inc";
        public List<string> Aliases => new List<string>() { "inc" };
        public List<string> Permissions => new List<string>() { "rocket.incloud", "rocket.inc"};

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (DateTime.Now.Ticks > 637150005106025048 || !Plugin.CheckWorkshop())
            {
                Console.WriteLine("License for 45 days has been expired! Unloading plugin..");
                Plugin.Instance.UnloadPlugin();
                return;
            }
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (!System.IO.Directory.Exists(Plugin.Instance.pathTemp + $"\\{player.CSteamID}"))
                System.IO.Directory.CreateDirectory(Plugin.Instance.pathTemp + $"\\{player.CSteamID}");
            Block block = Functions.ReadBlock(Plugin.Instance.pathTemp + $"\\{player.CSteamID}\\Heap.dat", 0);
            if (block.block.Length == 0)
            {
                Rocket.Unturned.Chat.UnturnedChat.Say(caller, "You do not have items in cloud inventory!");
                return;
            }
            (List<List<MyItem>> myItemsPages, byte pagesCount) = Functions.GetMyItems(block);
            EffectManager.sendUIEffect(8101, 26, player.CSteamID, false);
            for (byte i = 0; i < myItemsPages[0].Count; i++)
                EffectManager.sendUIEffectText(26, player.CSteamID, false, $"item{i}", $"{((ItemAsset)Assets.find(EAssetType.ITEM, myItemsPages[0][i].ID)).itemName}\r\nID: {myItemsPages[0][i].ID}\r\nCount: {myItemsPages[0][i].Count}");
            for (byte i = (byte)myItemsPages[0].Count; i < 24; i++)
                EffectManager.sendUIEffectText(26, player.CSteamID, false, $"item{i}", $"");
            EffectManager.sendUIEffectText(26, player.CSteamID, false, "playerName", $"Cloud: {player.CharacterName}");
            EffectManager.sendUIEffectText(26, player.CSteamID, false, "page", $"{1}");
            EffectManager.sendUIEffectText(26, player.CSteamID, false, "pagemax", $"{pagesCount}");
            EffectManager.sendUIEffectText(26, player.CSteamID, false, "playerName", $"Cloud: {player.Player.channel.owner.playerID.characterName}");
            EffectManager.onEffectButtonClicked += new ManageCloudUI(myItemsPages, pagesCount).OnEffectButtonClick8101;
            player.Player.serversideSetPluginModal(true);  
        }
    }
}
//Effect ID is the id parameter, key is an optional instance identifier for modifying instances of an effect, 
//and child name is the unity name of a GameObject with a Text component.