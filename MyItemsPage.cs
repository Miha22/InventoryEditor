using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemRestrictorAdvanced
{
    class MyItemsPage
    {
        public byte Width { get; private set; }
        public byte Height { get; private set; }
        public bool[,] Slots { get; private set; }
        public byte Page { get; private set; }

        public List<ItemJar> items { get; set; }

        private void LoadSize(byte newWidth, byte newHeight)
        {
            Width = newWidth;
            Height = newHeight;
            Slots = new bool[(int)Width, (int)Height];
            for (byte index1 = 0; (int)index1 < (int)Width; ++index1)
            {
                for (byte index2 = 0; (int)index2 < (int)Height; ++index2)
                    Slots[(int)index1, (int)index2] = false;
            }
            List<ItemJar> itemJarList = new List<ItemJar>();
            if (items != null)
            {
                for (byte index = 0; (int)index < this.items.Count; ++index)
                {
                    ItemJar jar = this.items[(int)index];
                    byte num1 = jar.size_x;
                    byte num2 = jar.size_y;
                    if ((int)jar.rot % 2 == 1)
                    {
                        num1 = jar.size_y;
                        num2 = jar.size_x;
                    }
                    if (Width == (byte)0 || this.Height == (byte)0 || (int)Page >= (int)PlayerInventory.SLOTS && ((int)jar.x + (int)num1 > (int)Width || (int)jar.y + (int)num2 > (int)Height))
                    {

                    }
                    else
                    {
                        this.fillSlot(jar, true);
                        itemJarList.Add(jar);
                    }
                }
            }
            this.items = itemJarList;
        }
        private void fillSlot(ItemJar jar, bool isOccupied)
        {
            byte num1 = jar.size_x;
            byte num2 = jar.size_y;
            if ((int)jar.rot % 2 == 1)
            {
                num1 = jar.size_y;
                num2 = jar.size_x;
            }
            for (byte index1 = 0; (int)index1 < (int)num1; ++index1)
            {
                for (byte index2 = 0; (int)index2 < (int)num2; ++index2)
                {
                    if ((int)jar.x + (int)index1 < (int)Width && (int)jar.y + (int)index2 < (int)Height)
                        Slots[(int)jar.x + (int)index1, (int)jar.y + (int)index2] = isOccupied;
                }
            }
        }
    }
}
