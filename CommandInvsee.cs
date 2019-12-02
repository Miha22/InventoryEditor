using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;

namespace ItemRestrictorAdvanced
{

    public class CommandGetInventory : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "invsee";
        public string Help => "Shows you someone's inventory using UI that you can edit";
        public string Syntax => "/invsee or /ins";
        public List<string> Aliases => new List<string>() { "ins" };
        public List<string> Permissions => new List<string>() { "rocket.invsee", "rocket.invsee.edit" };
        public static CommandGetInventory Instance { get; private set; }

        public CommandGetInventory()
        {
            Instance = this;
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer lastCaller = (UnturnedPlayer)caller;
            EffectManager.sendUIEffect(8100, 22, lastCaller.CSteamID, true);
            for (byte i = 0; i < Provider.clients.Count; i++)
                EffectManager.sendUIEffectText(22, lastCaller.CSteamID, true, $"text{i}", $"{Provider.clients[i].playerID.characterName}");
            EffectManager.sendUIEffectText(22, lastCaller.CSteamID, true, $"page", "1");
            EffectManager.onEffectButtonClicked += new ManageUI((byte)Math.Ceiling(Provider.clients.Count / 24.0), lastCaller.Player, caller).OnEffectButtonClick;// feature
            EffectManager.sendUIEffectText(22, lastCaller.CSteamID, true, "pagemax", $"{ManageUI.PagesCountPl}");
            //ManageUI.UICallers.Add(lastCaller.Player);
            lastCaller.Player.serversideSetPluginModal(true);
            //Refresh.Callers.Add(lastCaller.CSteamID);
            //try
            //{


            //    
            //}
            //catch (System.Exception e)
            //{
            //    Rocket.Core.Logging.Logger.LogException(e, $"Exception in Invsee: caller: {caller.DisplayName}");
            //    for (byte i = 0; i < Refresh.Refreshes.Length; i++)
            //    {
            //        Refresh.Refreshes[i].TurnOff(i);
            //    }
            //}

            //System.Console.WriteLine($"/gi executed");
        }
    }
    //class Refresh
    //{
    //    internal static List<CSteamID> Callers { get; set; }
    //    CSteamID _steamID;

    //    static Refresh()
    //    {
    //        Callers = new List<CSteamID>();
    //    }

    //    static internal void OnPlayersChange(UnturnedPlayer player)
    //    {
    //        foreach (CSteamID caller in Callers)
    //        {

    //        }
    //    }
    //}
}
//Effect ID is the id parameter, key is an optional instance identifier for modifying instances of an effect, 
//and child name is the unity name of a GameObject with a Text component.