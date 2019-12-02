using System.Collections.Generic;

namespace ItemRestrictorAdvanced
{
    //[JsonObject(MemberSerialization.OptIn)]
    sealed class MyItem
    {
        //[JsonProperty]
        public ushort ID { get; set; }
        //[JsonProperty]
        public ushort Count { get; set; }
        //[JsonProperty]
        public byte x { get; set; }
        //[JsonProperty]
        public byte Quality { get; set; }
       // [JsonConverter(typeof(MyConverter)), JsonProperty]
        public byte[] State { get; set; }
        public byte Rot { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Size_x { get; set; }
        public byte Size_y { get; set; }
        //[JsonProperty]
        //public byte Page { get; set; }
        //[JsonProperty]
        //public byte Width { get; set; }
        //[JsonProperty]
        //public byte Height { get; set; }

        public MyItem()
        {
            ID = 0;
            Count = 0;
            x = 0;
            Quality = 0;
            State = new byte[0];
        }
        public MyItem(ushort id, byte amount, byte quality, byte[] state)/*, byte[] state), byte rot, byte x, byte y, byte index, byte width, byte height)*/
        {
            Count = 1;
            ID = id;
            this.x = amount;
            Quality = quality;
            //State = new byte[state.Length];
            //for (byte i = 0; i < state.Length; i++)
            //{
            //    State[i] = state[i];
            //}
            State = state ?? (new byte[0]);
            //Rot = rot;
            //X = x;
            //Y = y;
            //Page = page;
            //Width = width;
            //Height = height;
            //ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
            //Size_x = itemAsset.size_x;
            //Size_y = itemAsset.size_y;
        }
        public MyItem(ushort id, byte amount, byte quality, byte[] state, ushort count)/*, byte[] state), byte rot, byte x, byte y, byte index, byte width, byte height)*/
        {
            Count = count;
            ID = id;
            this.x = amount;
            Quality = quality;
            //State = new byte[state.Length];
            //for (byte i = 0; i < state.Length; i++)
            //{
            //    State[i] = state[i];
            //}
            State = state ?? (new byte[0]);
            //Rot = rot;
            //X = x;
            //Y = y;
            //Page = page;
            //Width = width;
            //Height = height;
            //ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
            //Size_x = itemAsset.size_x;
            //Size_y = itemAsset.size_y;
        }
        public MyItem(ushort id, byte amount, byte quality, byte rot, byte sizeX, byte sizeY, byte[] state)/*, byte[] state), byte rot, byte x, byte y, byte index, byte width, byte height)*/
        {
            Count = 1;
            ID = id;
            this.x = amount;
            Quality = quality;
            Rot = rot;
            Size_x = sizeX;
            Size_y = sizeY;
            //State = new byte[state.Length];
            //for (byte i = 0; i < state.Length; i++)
            //{
            //    State[i] = state[i];
            //}
            State = state;
        }
        public override bool Equals(object obj)
        {
            MyItem myItem = (MyItem)obj;

            if (this.State == null)
                this.State = new byte[0];
            if (myItem.State == null)
                myItem.State = new byte[0];

            //if (IsStateEqual(this.State, myItem.State) && (this.ID == myItem.ID) && (this.Quality == myItem.Quality) && (this.x == myItem.x))
            //if (this.ID == myItem.ID)
            //    return true;
            //else
            //    return false;
            return this.ID == myItem.ID;
        }
        private bool IsStateEqual(byte[] state1, byte[] state2)
        {
            if (state1.Length != state2.Length)
                return false;

            for (byte i = 0; i < state1.Length; i++)
            {
                if (state1[i] != state2[i])
                    return false;
            }

            return true;
        }
        //private bool HasIndex(ref byte[,] Pages, ushort index)
        //{
        //    for (byte i = 0; i < Pages.Length; i++)
        //    {
        //        if (Pages[i, 0] == index)
        //            return true;
        //    }

        //    return false;
        //}
    }
    class MyItemComparer : IComparer<MyItem>
    {
        public int Compare(MyItem item1, MyItem item2)
        {
            if ((item1.Size_x * item1.Size_y) > (item2.Size_x * item2.Size_y))
                return -1;
            else if ((item1.Size_x * item1.Size_y) < (item2.Size_x * item2.Size_y))
                return 1;
            else
                return 0;
        }
    }
}
