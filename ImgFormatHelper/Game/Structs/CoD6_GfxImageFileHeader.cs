using System;
using System.IO;

namespace IWImgViewer
{
    public class CoD6_GfxImageFileHeader : IImageFileHeader
    {
        public char[] tag = new char[] { 'I', 'W', 'i' }; // 3 bytes
        public byte version = 8;
        public uint flags;
        public GfxImageFileFormat format = GfxImageFileFormat.IMG_FORMAT_BITMAP_RGBA;
        public byte unused;
        public short[] dimensions; // 3 shorts
        public int[] fileSizeForPicmip = new int[4]; // 4 ints

        [NonSerialized] string name = "iwi8";
        [NonSerialized] int dataLength = 0;

        public System.Collections.Generic.IReadOnlyList<IImageFileHeader> CompatibleDestinations => new System.Collections.Generic.List<IImageFileHeader>()
        {
            new CoD4_GfxImageFileHeader(),
            new CoD6_GfxImageFileHeader()
        }.AsReadOnly();

        public string FormatDescription => "Modern Warfare 2 IWI (version 8)";

        public string FormatExtension => "iwi";

        public byte IwiVersion => version;

        public int Flags => (int)flags;

        public byte MipLevels => (byte)((flags & (byte)IwiFlags.IMG_FLAG_NOMIPMAPS) == 0 ? 0 : 1 + Math.Floor(Math.Log(Math.Max(dimensions[0], Math.Max(dimensions[1], dimensions[2])), 2)));

        public byte Semantic => 0;

        public int Width => dimensions[0];

        public int Height => dimensions[1];

        public int Depth => dimensions[2];

        public int DataLength => dataLength; // :shrug:

        public string Name => name;

        public MapType MapType => (flags & (int)IwiFlags.IMG_FLAG_CUBEMAP) == 0 ? MapType.MAPTYPE_2D : MapType.MAPTYPE_CUBE;

        public _D3DFORMAT D3dFormat => _D3DFORMAT.D3DFMT_A8B8G8R8;

        public GfxImageFileFormat GfxFormat => format;

        public ImageCategory Category => ImageCategory.IMG_CATEGORY_LOAD_FROM_FILE;

        public void Deserialize(BinaryReader br)
        {
            tag = br.ReadChars(3);

            if (new string(tag) != "IWi")
            {
                throw new FormatException();
            }

            version = br.ReadByte();

            if (version < 8)
            {
                throw new FormatException();
            }
            else if (version != 8)
            {
                System.Windows.Forms.MessageBox.Show($"Invalid IWI version {version} (expected 8), I will try to deserialize regardless.");
            }

            flags = br.ReadUInt32();
            format = (GfxImageFileFormat)br.ReadByte();
            unused = br.ReadByte();
            dimensions = new short[]
            {
                br.ReadInt16(),
                br.ReadInt16(),
                br.ReadInt16()
            };

            fileSizeForPicmip = new int[]
            {
                br.ReadInt32(),
                br.ReadInt32(),
                br.ReadInt32(),
                br.ReadInt32()
            };

            dataLength = (int)br.BaseStream.Length;
        }

        public void Serialize(BinaryWriter bw)
        {
            bw.Write(tag[0]);
            bw.Write(tag[1]);
            bw.Write(tag[2]);
            bw.Write(version);
            bw.Write(flags);
            bw.Write((byte)format);
            bw.Write(unused);
            bw.Write(dimensions[0]);
            bw.Write(dimensions[1]);
            bw.Write(dimensions[2]);
            for (int i = 0; i < 4; i++)
            {
                bw.Write(fileSizeForPicmip[i]);
            }
        }

        public void From(CoD4_GfxImageFileHeader header)
        {
            flags = header.flags;
            format = header.format;
            dimensions = header.dimensions;
            fileSizeForPicmip = header.fileSizeForPicmip;
        }

        public void From(IImageFileHeader header)
        {
            if (header is CoD4_GfxImageFileHeader)
            {
                From(header as CoD4_GfxImageFileHeader);
                return;
            }

            format = header.GfxFormat;
            flags = (byte)header.Flags;
            dimensions = new short[]{
                (short)header.Width,
                (short)header.Height,
                (short)header.Depth
            };
            name = header.Name;

            // Compute mips filesizes
            var currentFileSize = 32; // IWI Header Size
            var textureMipCount = (flags & (uint)IwiFlags.IMG_FLAG_NOMIPMAPS) == 0 ? 1 : header.MipLevels;
            for (var currentMipLevel = textureMipCount - 1; currentMipLevel >= 0; currentMipLevel--)
            {
                var blockSize = FormatHelper.GetBlockSize(format);
                var mipLevelSize =
                    Math.Max(blockSize, Width / Math.Pow(2, currentMipLevel)) * Math.Max(blockSize, Height / Math.Pow(2, currentMipLevel))
                    * FormatHelper.ChannelCount[format]
                    * (header.MapType == MapType.MAPTYPE_CUBE ? 6 : 1);

                currentFileSize += (int)mipLevelSize;

                if (currentMipLevel < fileSizeForPicmip.Length)
                {
                    fileSizeForPicmip[currentMipLevel] = currentFileSize;
                }
            }
        }

        public CoD6_GfxImageFileHeader()
        {

        }

        public CoD6_GfxImageFileHeader(string name)
        {
            this.name = name;
        }
    }
}
