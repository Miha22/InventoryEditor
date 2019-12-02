using SDG.Unturned;
using System.Collections.Generic;

namespace ItemRestrictorAdvanced
{
    public class SteamPlayerCompaper : IComparer<SteamPlayer>
    {
        public int Compare(SteamPlayer player1, SteamPlayer player2)
        {
            ECharName eCharname = NameComparer(player1.playerID.characterName.ToLower(), player2.playerID.characterName.ToLower());
            if (eCharname == ECharName.Before)
                return -1;
            else if (eCharname == ECharName.After)
                return 1;
            else
                return 0;
        }
        private ECharName NameComparer(string player1, string player2)
        {
            (player1, player2) = ShortNameLongName(player1, player2);
            for (byte i = 0; i < player1.Length; i++)
            {
                if ((byte)player1[i] < (byte)player2[i])
                    return ECharName.Before;
                else if((byte)player1[i] > (byte)player2[i])
                    return ECharName.After;
            }

            return ECharName.Same;
        }
        private (string, string) ShortNameLongName(string player1, string player2)
        {
            return player1.Length > player2.Length ? (player2, player1) : (player1, player2);
        }
    }
}
//Effect ID is the id parameter, key is an optional instance identifier for modifying instances of an effect, 
//and child name is the unity name of a GameObject with a Text component.