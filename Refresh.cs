//using Rocket.Unturned;
//using Rocket.Unturned.Player;
//using SDG.Unturned;
//using Steamworks;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ItemRestrictorAdvanced
//{
//    //public class RefreshInv
//    //{
//    //    public CSteamID CallerSteamID { get; private set; }
//    //    public byte CurrentPage { get; set; }



//    //    public void TurnOff(Player player)
//    //    {
//    //        player.inventory.onInventoryAdded -= this.OnInventoryChange;
//    //        player.inventory.onInventoryRemoved -= this.OnInventoryChange;
//    //    }

//    //    private void Do(byte pagemax)
//    //    {
//    //        EffectManager.askEffectClearByID(8100, CallerSteamID);
//    //        EffectManager.sendUIEffect(8100, 22, CallerSteamID, false);
//    //        byte multiplier = (byte)((CurrentPage - 1) * 24);
//    //        for (byte i = multiplier; (i < (24 + multiplier)) && (i < (byte)Provider.clients.Count); i++)
//    //            EffectManager.sendUIEffectText(22, CallerSteamID, false, $"text{i}", $"{Provider.clients[i].playerID.characterName}");
//    //        EffectManager.sendUIEffectText(22, CallerSteamID, false, "pagemax", $"{pagemax}");
//    //    }
//    //}
//    class Refresh
//    {
//        internal static Refresh[] Refreshes = new Refresh[1];
//        internal CSteamID CallerSteamID { get; private set; }
//        internal byte CurrentPage { get; set; }
//        //private static CancellationTokenSource cts;
//        //private static CancellationToken token;

//        static Refresh()
//        {
//            //cts = new CancellationTokenSource();
//            //token = cts.Token;
//        }

//        public Refresh(CSteamID steamID)
//        {
//            CallerSteamID = steamID;
//            CurrentPage = 1;
//            Refreshes[Refreshes.Length - 1] = this;
//            ReSizeUp();
//        }

//        public void OnPlayersChange(UnturnedPlayer connectedPlayer)
//        {
//            //if (token.IsCancellationRequested)
//                //return;
//            ManageUI.PagesCountPl = (byte)System.Math.Ceiling((double)Provider.clients.Count / 24.0);
//            Do(ManageUI.PagesCountPl);
//        }

//        public void TurnOff(byte index)
//        {
//            //cts.Cancel();
//            U.Events.OnPlayerConnected -= this.OnPlayersChange;
//            U.Events.OnPlayerDisconnected -= this.OnPlayersChange;
//            ReSizeDown(index);
//        }

//        private void Do(byte pagemax)
//        {
//            //if (ctoken.IsCancellationRequested)
//                //return;
//            try
//            {
//                EffectManager.askEffectClearByID(8100, CallerSteamID);
//                EffectManager.sendUIEffect(8100, 22, CallerSteamID, false);
//                byte multiplier = (byte)((CurrentPage - 1) * 24);
//                for (byte i = multiplier; (i < (24 + multiplier)) && (i < (byte)Provider.clients.Count); i++)
//                    EffectManager.sendUIEffectText(22, CallerSteamID, false, $"text{i}", $"{Provider.clients[i].playerID.characterName}");
//                EffectManager.sendUIEffectText(22, CallerSteamID, false, "pagemax", $"{pagemax}");
//                EffectManager.sendUIEffectText(22, CallerSteamID, false, "page", $"{CurrentPage}");
//            }
//            catch (System.Exception)
//            {
//                return;
//            }
//        }

//        private void ReSizeUp()
//        {
//            Refresh[] newRefreshes = new Refresh[Refresh.Refreshes.Length + 1];
//            for (byte i = 0; i < Refreshes.Length; i++)
//                newRefreshes[i] = Refreshes[i];
//            Refreshes = newRefreshes;
//        }

//        private void ReSizeDown(byte index)
//        {
//            Refresh[] newRefreshes = new Refresh[Refresh.Refreshes.Length - 1];
//            for (byte i = 0, j = 0; i < Refreshes.Length; i++, j++)
//            {
//                if (i == index)
//                {
//                    j--;
//                    continue;
//                }
//                newRefreshes[j] = Refreshes[j];
//            }
//            Refreshes = newRefreshes;
//        }
//    }
//    //public class RefreshOnD
//    //{
//    //    private CSteamID _steamID;
//    //    public RefreshOnD(CSteamID steamID)
//    //    {
//    //        this._steamID = steamID;
//    //    }
//    //    public void Execute(UnturnedPlayer connectedPlayer)
//    //    {
//    //        EffectManager.askEffectClearByID(8100, _steamID);
//    //        EffectManager.sendUIEffect(8100, 22, _steamID, false);
//    //        for (byte i = 0; i < Provider.clients.Count; i++)
//    //            EffectManager.sendUIEffectText(22, _steamID, false, $"text{i}", $"{Provider.clients[i].playerID.characterName}");

//    //        U.Events.OnPlayerDisconnected -= Execute;
//    //    }
//    //}
//}
////Effect ID is the id parameter, key is an optional instance identifier for modifying instances of an effect, 
////and child name is the unity name of a GameObject with a Text component.