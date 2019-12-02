// Type: MyPlugin
// Assembly: MyPlugin, Version=0.0.0.0, Culture=solid, PublicKeyToken=A2E8F929TH47NBSD8
// MVID: A3A3201109-C0AEF-43226B-A05510-1DBD43310D99C16
// Obfuscated: True
// Assembly location: C:\Users\Roy\Documents\GitHub\MyPlugin.dll

//using Steamworks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace ItemRestrictorAdvanced
//{
//    public class Block
//    {
//        public static readonly int BUFFER_SIZE = (int)ushort.MaxValue;
//        public static byte[] buffer = new byte[Block.BUFFER_SIZE];
//        private static object[][] objects = new object[7][]
//        {
//      new object[1],
//      new object[2],
//      new object[3],
//      new object[4],
//      new object[5],
//      new object[6],
//      new object[7]
//        };
//        public bool useCompression;
//        public bool longBinaryData;
//        public int step;
//        public byte[] block;

//        public Block(int prefix, byte[] contents)
//        {
//            this.reset(prefix, contents);
//        }

//        public Block(byte[] contents)
//        {
//            this.reset(contents);
//        }

//        public Block(int prefix)
//        {
//            this.reset(prefix);
//        }

//        public Block()
//        {
//            this.reset();
//        }

//        //private static object[] getObjects(int index)
//        //{
//        //    object[] objArray = Block.objects[index];
//        //    for (int index1 = 0; index1 < objArray.Length; ++index1)
//        //        objArray[index1] = (object)null;
//        //    return objArray;
//        //}

//        //public string readString()
//        //{
//        //    if (this.block == null || this.step >= this.block.Length)
//        //        return string.Empty;
//        //    byte num = this.block[this.step];
//        //    string str = this.step + (int)num > this.block.Length ? string.Empty : Encoding.UTF8.GetString(this.block, this.step + 1, (int)num);
//        //    this.step = this.step + 1 + (int)num;
//        //    return str;
//        //}

//        //public string[] readStringArray()
//        //{
//        //    if (this.block == null || this.step >= this.block.Length)
//        //        return new string[0];
//        //    string[] strArray = new string[(int)this.readByte()];
//        //    for (byte index = 0; (int)index < strArray.Length; ++index)
//        //        strArray[(int)index] = this.readString();
//        //    return strArray;
//        //}

//        //public bool readBoolean()
//        //{
//        //    if (this.block == null || this.step > this.block.Length - 1)
//        //        return false;
//        //    bool boolean = BitConverter.ToBoolean(this.block, this.step);
//        //    ++this.step;
//        //    return boolean;
//        //}

//        //public bool[] readBooleanArray()
//        //{
//        //    if (this.block == null || this.step >= this.block.Length)
//        //        return new bool[0];
//        //    bool[] flagArray = new bool[(int)this.readUInt16()];
//        //    ushort num = (ushort)Mathf.CeilToInt((float)flagArray.Length / 8f);
//        //    for (ushort index1 = 0; (int)index1 < (int)num; ++index1)
//        //    {
//        //        for (byte index2 = 0; index2 < (byte)8 && (int)index1 * 8 + (int)index2 < flagArray.Length; ++index2)
//        //            flagArray[(int)index1 * 8 + (int)index2] = ((int)this.block[this.step + (int)index1] & (int)Types.SHIFTS[(int)index2]) == (int)Types.SHIFTS[(int)index2];
//        //    }
//        //    this.step += (int)num;
//        //    return flagArray;
//        //}

//        public byte readByte()
//        {
//            if (this.block == null || this.step > this.block.Length - 1)
//                return 0;
//            byte num = this.block[this.step];
//            ++this.step;
//            return num;
//        }

//        public byte checkByte()
//        {
//            if (this.block == null || this.step > this.block.Length - 1)
//                return 0;
//            byte num = this.block[this.step];
//            return num;
//        }

//        public byte[] readByteArray()
//        {
//            if (this.block == null || this.step >= this.block.Length)
//                return new byte[0];
//            byte[] numArray;
//            if (this.longBinaryData)
//            {
//                int length = this.readInt32();
//                if (length >= 30000)
//                    return new byte[0];
//                numArray = new byte[length];
//            }
//            else
//            {
//                numArray = new byte[(int)this.block[this.step]];
//                ++this.step;
//            }
//            if (this.step + numArray.Length <= this.block.Length)
//            {
//                try
//                {
//                    Buffer.BlockCopy((Array)this.block, this.step, (Array)numArray, 0, numArray.Length);
//                }
//                catch
//                {
//                }
//            }
//            this.step += numArray.Length;
//            return numArray;
//        }

//        public short readInt16()
//        {
//            if (this.block == null || this.step > this.block.Length - 2)
//                return 0;
//            short int16 = BitConverter.ToInt16(this.block, this.step);
//            this.step += 2;
//            return int16;
//        }

//        public ushort readUInt16()
//        {
//            if (this.block == null || this.step > this.block.Length - 2)
//                return 0;
//            ushort uint16 = BitConverter.ToUInt16(this.block, this.step);
//            this.step += 2;
//            return uint16;
//        }

//        public int readInt32()
//        {
//            if (this.block == null || this.step > this.block.Length - 4)
//                return 0;
//            int int32 = BitConverter.ToInt32(this.block, this.step);
//            this.step += 4;
//            return int32;
//        }

//        //public int[] readInt32Array()
//        //{
//        //    ushort num1 = this.readUInt16();
//        //    int[] numArray = new int[(int)num1];
//        //    for (ushort index = 0; (int)index < (int)num1; ++index)
//        //    {
//        //        int num2 = this.readInt32();
//        //        numArray[(int)index] = num2;
//        //    }
//        //    return numArray;
//        //}

//        public uint readUInt32()
//        {
//            if (this.block == null || this.step > this.block.Length - 4)
//                return 0;
//            uint uint32 = BitConverter.ToUInt32(this.block, this.step);
//            this.step += 4;
//            return uint32;
//        }

//        //public float readSingle()
//        //{
//        //    if (this.block == null || this.step > this.block.Length - 4)
//        //        return 0.0f;
//        //    float single = BitConverter.ToSingle(this.block, this.step);
//        //    this.step += 4;
//        //    return single;
//        //}

//        //public long readInt64()
//        //{
//        //    if (this.block == null || this.step > this.block.Length - 8)
//        //        return 0;
//        //    long int64 = BitConverter.ToInt64(this.block, this.step);
//        //    this.step += 8;
//        //    return int64;
//        //}

//        public ulong readUInt64()
//        {
//            if (this.block == null || this.step > this.block.Length - 8)
//                return 0;
//            ulong uint64 = BitConverter.ToUInt64(this.block, this.step);
//            this.step += 8;
//            return uint64;
//        }

//        public ulong[] readUInt64Array()
//        {
//            ushort num1 = this.readUInt16();
//            ulong[] numArray = new ulong[(int)num1];
//            for (ushort index = 0; (int)index < (int)num1; ++index)
//            {
//                ulong num2 = this.readUInt64();
//                numArray[(int)index] = num2;
//            }
//            return numArray;
//        }

//        public CSteamID readSteamID()
//        {
//            return new CSteamID(this.readUInt64());
//        }

//        //public Guid readGUID()
//        //{
//        //    GuidBuffer guidBuffer = new GuidBuffer();
//        //    guidBuffer.Read(this.readByteArray(), 0);
//        //    return guidBuffer.GUID;
//        //}

//        //public Vector3 readUInt16RVector3()
//        //{
//        //    byte num1 = this.readByte();
//        //    double num2 = (double)this.readUInt16() / (double)ushort.MaxValue;
//        //    double num3 = (double)this.readUInt16() / (double)ushort.MaxValue;
//        //    byte num4 = this.readByte();
//        //    double num5 = (double)this.readUInt16() / (double)ushort.MaxValue;
//        //    return new Vector3((float)((double)((int)num1 * (int)Regions.REGION_SIZE) + num2 * (double)Regions.REGION_SIZE - 4096.0), (float)(num3 * 2048.0 - 1024.0), (float)((double)((int)num4 * (int)Regions.REGION_SIZE) + num5 * (double)Regions.REGION_SIZE - 4096.0));
//        //}

//        //public Vector3 readSingleVector3()
//        //{
//        //    return new Vector3(this.readSingle(), this.readSingle(), this.readSingle());
//        //}

//        //public Quaternion readSingleQuaternion()
//        //{
//        //    return Quaternion.Euler(this.readSingle(), this.readSingle(), this.readSingle());
//        //}

//        //public Color readColor()
//        //{
//        //    return new Color((float)this.readByte() / (float)byte.MaxValue, (float)this.readByte() / (float)byte.MaxValue, (float)this.readByte() / (float)byte.MaxValue);
//        //}

//        //public object read(Type type)
//        //{
//        //    if (type == Types.STRING_TYPE)
//        //        return (object)this.readString();
//        //    if (type == Types.STRING_ARRAY_TYPE)
//        //        return (object)this.readStringArray();
//        //    if (type == Types.BOOLEAN_TYPE)
//        //        return (object)this.readBoolean();
//        //    if (type == Types.BOOLEAN_ARRAY_TYPE)
//        //        return (object)this.readBooleanArray();
//        //    if (type == Types.BYTE_TYPE)
//        //        return (object)this.readByte();
//        //    if (type == Types.BYTE_ARRAY_TYPE)
//        //        return (object)this.readByteArray();
//        //    if (type == Types.INT16_TYPE)
//        //        return (object)this.readInt16();
//        //    if (type == Types.UINT16_TYPE)
//        //        return (object)this.readUInt16();
//        //    if (type == Types.INT32_TYPE)
//        //        return (object)this.readInt32();
//        //    if (type == Types.INT32_ARRAY_TYPE)
//        //        return (object)this.readInt32Array();
//        //    if (type == Types.UINT32_TYPE)
//        //        return (object)this.readUInt32();
//        //    if (type == Types.SINGLE_TYPE)
//        //        return (object)this.readSingle();
//        //    if (type == Types.INT64_TYPE)
//        //        return (object)this.readInt64();
//        //    if (type == Types.UINT64_TYPE)
//        //        return (object)this.readUInt64();
//        //    if (type == Types.UINT64_ARRAY_TYPE)
//        //        return (object)this.readUInt64Array();
//        //    if (type == Types.STEAM_ID_TYPE)
//        //        return (object)this.readSteamID();
//        //    if (type == Types.GUID_TYPE)
//        //        return (object)this.readGUID();
//        //    if (type == Types.VECTOR3_TYPE)
//        //    {
//        //        if (this.useCompression)
//        //            return (object)this.readUInt16RVector3();
//        //        return (object)this.readSingleVector3();
//        //    }
//        //    if (type == Types.COLOR_TYPE)
//        //        return (object)this.readColor();
//        //    Debug.LogError((object)("Failed to read type: " + (object)type));
//        //    return (object)null;
//        //}

//        //public object[] read(int offset, Type type_0)
//        //{
//        //    object[] objects = Block.getObjects(0);
//        //    if (offset < 1)
//        //        objects[0] = this.read(type_0);
//        //    return objects;
//        //}

//        //public object[] read(int offset, Type type_0, Type type_1)
//        //{
//        //    object[] objects = Block.getObjects(1);
//        //    if (offset < 1)
//        //        objects[0] = this.read(type_0);
//        //    if (offset < 2)
//        //        objects[1] = this.read(type_1);
//        //    return objects;
//        //}

//        //public object[] read(Type type_0, Type type_1)
//        //{
//        //    return this.read(0, type_0, type_1);
//        //}

//        //public object[] read(int offset, Type type_0, Type type_1, Type type_2)
//        //{
//        //    object[] objects = Block.getObjects(2);
//        //    if (offset < 1)
//        //        objects[0] = this.read(type_0);
//        //    if (offset < 2)
//        //        objects[1] = this.read(type_1);
//        //    if (offset < 3)
//        //        objects[2] = this.read(type_2);
//        //    return objects;
//        //}

//        //public object[] read(Type type_0, Type type_1, Type type_2)
//        //{
//        //    return this.read(0, type_0, type_1, type_2);
//        //}

//        //public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3)
//        //{
//        //    object[] objects = Block.getObjects(3);
//        //    if (offset < 1)
//        //        objects[0] = this.read(type_0);
//        //    if (offset < 2)
//        //        objects[1] = this.read(type_1);
//        //    if (offset < 3)
//        //        objects[2] = this.read(type_2);
//        //    if (offset < 4)
//        //        objects[3] = this.read(type_3);
//        //    return objects;
//        //}

//        //public object[] read(Type type_0, Type type_1, Type type_2, Type type_3)
//        //{
//        //    return this.read(0, type_0, type_1, type_2, type_3);
//        //}

//        //public object[] read(
//        //  int offset,
//        //  Type type_0,
//        //  Type type_1,
//        //  Type type_2,
//        //  Type type_3,
//        //  Type type_4)
//        //{
//        //    object[] objects = Block.getObjects(4);
//        //    if (offset < 1)
//        //        objects[0] = this.read(type_0);
//        //    if (offset < 2)
//        //        objects[1] = this.read(type_1);
//        //    if (offset < 3)
//        //        objects[2] = this.read(type_2);
//        //    if (offset < 4)
//        //        objects[3] = this.read(type_3);
//        //    if (offset < 5)
//        //        objects[4] = this.read(type_4);
//        //    return objects;
//        //}

//        //public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
//        //{
//        //    return this.read(0, type_0, type_1, type_2, type_3, type_4);
//        //}

//        //public object[] read(
//        //  int offset,
//        //  Type type_0,
//        //  Type type_1,
//        //  Type type_2,
//        //  Type type_3,
//        //  Type type_4,
//        //  Type type_5)
//        //{
//        //    object[] objects = Block.getObjects(5);
//        //    if (offset < 1)
//        //        objects[0] = this.read(type_0);
//        //    if (offset < 2)
//        //        objects[1] = this.read(type_1);
//        //    if (offset < 3)
//        //        objects[2] = this.read(type_2);
//        //    if (offset < 4)
//        //        objects[3] = this.read(type_3);
//        //    if (offset < 5)
//        //        objects[4] = this.read(type_4);
//        //    if (offset < 6)
//        //        objects[5] = this.read(type_5);
//        //    return objects;
//        //}

//        //public object[] read(
//        //  Type type_0,
//        //  Type type_1,
//        //  Type type_2,
//        //  Type type_3,
//        //  Type type_4,
//        //  Type type_5)
//        //{
//        //    return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5);
//        //}

//        //public object[] read(
//        //  int offset,
//        //  Type type_0,
//        //  Type type_1,
//        //  Type type_2,
//        //  Type type_3,
//        //  Type type_4,
//        //  Type type_5,
//        //  Type type_6)
//        //{
//        //    object[] objects = Block.getObjects(6);
//        //    if (offset < 1)
//        //        objects[0] = this.read(type_0);
//        //    if (offset < 2)
//        //        objects[1] = this.read(type_1);
//        //    if (offset < 3)
//        //        objects[2] = this.read(type_2);
//        //    if (offset < 4)
//        //        objects[3] = this.read(type_3);
//        //    if (offset < 5)
//        //        objects[4] = this.read(type_4);
//        //    if (offset < 6)
//        //        objects[5] = this.read(type_5);
//        //    if (offset < 7)
//        //        objects[6] = this.read(type_6);
//        //    return objects;
//        //}

//        //public object[] read(
//        //  Type type_0,
//        //  Type type_1,
//        //  Type type_2,
//        //  Type type_3,
//        //  Type type_4,
//        //  Type type_5,
//        //  Type type_6)
//        //{
//        //    return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5, type_6);
//        //}

//        //public object[] read(int offset, params Type[] types)
//        //{
//        //    object[] objArray = new object[types.Length];
//        //    for (int index = offset; index < types.Length; ++index)
//        //        objArray[index] = this.read(types[index]);
//        //    return objArray;
//        //}

//        //public object[] read(params Type[] types)
//        //{
//        //    return this.read(0, types);
//        //}

//        //public void writeString(string value)
//        //{
//        //    byte[] bytes = Encoding.UTF8.GetBytes(value);
//        //    byte length = (byte)bytes.Length;
//        //    Block.buffer[this.step] = length;
//        //    ++this.step;
//        //    Buffer.BlockCopy((Array)bytes, 0, (Array)Block.buffer, this.step, (int)length);
//        //    this.step += (int)length;
//        //}

//        //public void writeStringArray(string[] values)
//        //{
//        //    byte length = (byte)values.Length;
//        //    this.writeByte(length);
//        //    for (byte index = 0; (int)index < (int)length; ++index)
//        //        this.writeString(values[(int)index]);
//        //}

//        //public void writeBoolean(bool value)
//        //{
//        //    byte[] bytes = BitConverter.GetBytes(value);
//        //    Block.buffer[this.step] = bytes[0];
//        //    ++this.step;
//        //}

//        //public void writeBooleanArray(bool[] values)
//        //{
//        //    this.writeUInt16((ushort)values.Length);
//        //    ushort num = (ushort)Mathf.CeilToInt((float)values.Length / 8f);
//        //    for (ushort index1 = 0; (int)index1 < (int)num; ++index1)
//        //    {
//        //        Block.buffer[this.step + (int)index1] = (byte)0;
//        //        for (byte index2 = 0; index2 < (byte)8 && (int)index1 * 8 + (int)index2 < values.Length; ++index2)
//        //        {
//        //            if (values[(int)index1 * 8 + (int)index2])
//        //                Block.buffer[this.step + (int)index1] |= Types.SHIFTS[(int)index2];
//        //        }
//        //    }
//        //    this.step += (int)num;
//        //}

//        public void writeByte(byte value)
//        {
//            buffer[this.step] = value;
//            ++this.step;
//        }

//        public void writeByteArray(byte[] values)
//        {
//            if (values.Length >= 30000)
//                return;
//            if (this.longBinaryData)
//            {
//                this.writeInt32(values.Length);
//                Buffer.BlockCopy((Array)values, 0, (Array)Block.buffer, this.step, values.Length);
//                this.step += values.Length;
//            }
//            else
//            {
//                byte length = (byte)values.Length;
//                Block.buffer[this.step] = length;
//                ++this.step;
//                Buffer.BlockCopy((Array)values, 0, (Array)Block.buffer, this.step, (int)length);
//                this.step += (int)length;
//            }
//        }

//        //public void writeInt16(short value)
//        //{
//        //    byte[] bytes = BitConverter.GetBytes(value);
//        //    Buffer.BlockCopy((Array)bytes, 0, (Array)Block.buffer, this.step, bytes.Length);
//        //    this.step += 2;
//        //}

//        public void writeUInt16(ushort value)
//        {
//            byte[] bytes = BitConverter.GetBytes(value);
//            Buffer.BlockCopy((Array)bytes, 0, (Array)Block.buffer, this.step, bytes.Length);
//            this.step += 2;
//        }

//        public void writeInt32(int value)
//        {
//            byte[] bytes = BitConverter.GetBytes(value);
//            Buffer.BlockCopy((Array)bytes, 0, (Array)Block.buffer, this.step, bytes.Length);
//            this.step += 4;
//        }

//        //public void writeInt32Array(int[] values)
//        //{
//        //    this.writeUInt16((ushort)values.Length);
//        //    for (ushort index = 0; (int)index < values.Length; ++index)
//        //        this.writeInt32(values[(int)index]);
//        //}

//        public void writeUInt32(uint value)
//        {
//            byte[] bytes = BitConverter.GetBytes(value);
//            Buffer.BlockCopy((Array)bytes, 0, (Array)Block.buffer, this.step, bytes.Length);
//            this.step += 4;
//        }

//        //public void writeSingle(float value)
//        //{
//        //    byte[] bytes = BitConverter.GetBytes(value);
//        //    Buffer.BlockCopy((Array)bytes, 0, (Array)Block.buffer, this.step, bytes.Length);
//        //    this.step += 4;
//        //}

//        //public void writeInt64(long value)
//        //{
//        //    byte[] bytes = BitConverter.GetBytes(value);
//        //    Buffer.BlockCopy((Array)bytes, 0, (Array)Block.buffer, this.step, bytes.Length);
//        //    this.step += 8;
//        //}

//        public void writeUInt64(ulong value)
//        {
//            byte[] bytes = BitConverter.GetBytes(value);
//            Buffer.BlockCopy((Array)bytes, 0, (Array)Block.buffer, this.step, bytes.Length);
//            this.step += 8;
//        }

//        public void writeUInt64Array(ulong[] values)
//        {
//            this.writeUInt16((ushort)values.Length);
//            for (ushort index = 0; (int)index < values.Length; ++index)
//                this.writeUInt64(values[(int)index]);
//        }

//        public void writeSteamID(CSteamID steamID)
//        {
//            this.writeUInt64(steamID.m_SteamID);
//        }

//        //public void writeGUID(Guid GUID)
//        //{
//        //    new GuidBuffer(GUID).Write(GuidBuffer.GUID_BUFFER, 0);
//        //    this.writeByteArray(GuidBuffer.GUID_BUFFER);
//        //}

//        //public void writeUInt16RVector3(Vector3 value)
//        //{
//        //    double num1 = (double)value.x + 4096.0;
//        //    double num2 = (double)value.y + 1024.0;
//        //    double num3 = (double)value.z + 4096.0;
//        //    byte num4 = (byte)(num1 / (double)Regions.REGION_SIZE);
//        //    byte num5 = (byte)(num3 / (double)Regions.REGION_SIZE);
//        //    double num6 = num1 % (double)Regions.REGION_SIZE;
//        //    double num7 = num2 % 2048.0;
//        //    double num8 = num3 % (double)Regions.REGION_SIZE;
//        //    double num9 = num6 / (double)Regions.REGION_SIZE;
//        //    double num10 = num7 / 2048.0;
//        //    double num11 = num8 / (double)Regions.REGION_SIZE;
//        //    this.writeByte(num4);
//        //    this.writeUInt16((ushort)(num9 * (double)ushort.MaxValue));
//        //    this.writeUInt16((ushort)(num10 * (double)ushort.MaxValue));
//        //    this.writeByte(num5);
//        //    this.writeUInt16((ushort)(num11 * (double)ushort.MaxValue));
//        //}

//        //public void writeSingleVector3(Vector3 value)
//        //{
//        //    this.writeSingle(value.x);
//        //    this.writeSingle(value.y);
//        //    this.writeSingle(value.z);
//        //}

//        //public void writeSingleQuaternion(Quaternion value)
//        //{
//        //    Vector3 eulerAngles = value.eulerAngles;
//        //    this.writeSingle(eulerAngles.x);
//        //    this.writeSingle(eulerAngles.y);
//        //    this.writeSingle(eulerAngles.z);
//        //}

//        //public void writeColor(Color value)
//        //{
//        //    this.writeByte((byte)((double)value.r * (double)byte.MaxValue));
//        //    this.writeByte((byte)((double)value.g * (double)byte.MaxValue));
//        //    this.writeByte((byte)((double)value.b * (double)byte.MaxValue));
//        //}

//        //public void write(object objects)
//        //{
//        //    Type type = objects.GetType();
//        //    if (type == Types.STRING_TYPE)
//        //        this.writeString((string)objects);
//        //    else if (type == Types.STRING_ARRAY_TYPE)
//        //        this.writeStringArray((string[])objects);
//        //    else if (type == Types.BOOLEAN_TYPE)
//        //        this.writeBoolean((bool)objects);
//        //    else if (type == Types.BOOLEAN_ARRAY_TYPE)
//        //        this.writeBooleanArray((bool[])objects);
//        //    else if (type == Types.BYTE_TYPE)
//        //        this.writeByte((byte)objects);
//        //    else if (type == Types.BYTE_ARRAY_TYPE)
//        //        this.writeByteArray((byte[])objects);
//        //    else if (type == Types.INT16_TYPE)
//        //        this.writeInt16((short)objects);
//        //    else if (type == Types.UINT16_TYPE)
//        //        this.writeUInt16((ushort)objects);
//        //    else if (type == Types.INT32_TYPE)
//        //        this.writeInt32((int)objects);
//        //    else if (type == Types.INT32_ARRAY_TYPE)
//        //        this.writeInt32Array((int[])objects);
//        //    else if (type == Types.UINT32_TYPE)
//        //        this.writeUInt32((uint)objects);
//        //    else if (type == Types.SINGLE_TYPE)
//        //        this.writeSingle((float)objects);
//        //    else if (type == Types.INT64_TYPE)
//        //        this.writeInt64((long)objects);
//        //    else if (type == Types.UINT64_TYPE)
//        //        this.writeUInt64((ulong)objects);
//        //    else if (type == Types.UINT64_ARRAY_TYPE)
//        //        this.writeUInt64Array((ulong[])objects);
//        //    else if (type == Types.STEAM_ID_TYPE)
//        //        this.writeSteamID((CSteamID)objects);
//        //    else if (type == Types.GUID_TYPE)
//        //        this.writeGUID((Guid)objects);
//        //    else if (type == Types.VECTOR3_TYPE)
//        //    {
//        //        if (this.useCompression)
//        //            this.writeUInt16RVector3((Vector3)objects);
//        //        else
//        //            this.writeSingleVector3((Vector3)objects);
//        //    }
//        //    else if (type == Types.COLOR_TYPE)
//        //        this.writeColor((Color)objects);
//        //    else
//        //        Debug.LogError((object)("Failed to write type: " + (object)type));
//        //}

//        //public void write(object object_0, object object_1)
//        //{
//        //    this.write(object_0);
//        //    this.write(object_1);
//        //}

//        //public void write(object object_0, object object_1, object object_2)
//        //{
//        //    this.write(object_0, object_1);
//        //    this.write(object_2);
//        //}

//        //public void write(object object_0, object object_1, object object_2, object object_3)
//        //{
//        //    this.write(object_0, object_1, object_2);
//        //    this.write(object_3);
//        //}

//        //public void write(
//        //  object object_0,
//        //  object object_1,
//        //  object object_2,
//        //  object object_3,
//        //  object object_4)
//        //{
//        //    this.write(object_0, object_1, object_2, object_3);
//        //    this.write(object_4);
//        //}

//        //public void write(
//        //  object object_0,
//        //  object object_1,
//        //  object object_2,
//        //  object object_3,
//        //  object object_4,
//        //  object object_5)
//        //{
//        //    this.write(object_0, object_1, object_2, object_3, object_4);
//        //    this.write(object_5);
//        //}

//        //public void write(
//        //  object object_0,
//        //  object object_1,
//        //  object object_2,
//        //  object object_3,
//        //  object object_4,
//        //  object object_5,
//        //  object object_6)
//        //{
//        //    this.write(object_0, object_1, object_2, object_3, object_4, object_5);
//        //    this.write(object_6);
//        //}

//        //public void write(params object[] objects)
//        //{
//        //    for (int index = 0; index < objects.Length; ++index)
//        //        this.write(objects[index]);
//        //}

//        public byte[] getBytes(out int size)
//        {
//            if (this.block == null)
//            {
//                size = this.step;
//                return Block.buffer;
//            }
//            size = this.block.Length;
//            return this.block;
//        }

//        //public byte[] getHash()
//        //{
//        //    if (this.block == null)
//        //        return Hash.SHA1(Block.buffer);
//        //    return Hash.SHA1(this.block);
//        //}

//        public void reset(int prefix, byte[] contents)
//        {
//            this.step = prefix;
//            this.block = contents;
//        }

//        public void reset(byte[] contents)
//        {
//            this.step = 0;
//            this.block = contents;
//        }

//        public void reset(int prefix)
//        {
//            this.step = prefix;
//            this.block = (byte[])null;
//        }

//        public void reset()
//        {
//            this.step = 0;
//            this.block = (byte[])null;
//        }
//    }
//}
