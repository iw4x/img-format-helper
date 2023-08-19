using System.Collections.Generic;
using System.IO;

namespace IWImgViewer
{
    public class IW4XImageHeader : IImageFileHeader
    {
        public MapType mapType;
        public byte semantic;
        public ImageCategory category;
        public int size;
        public byte levelCount;
        public byte flags;
        public short[] dimensions; // 3
        public _D3DFORMAT format;
        public int resourceSize;

        public IReadOnlyList<IImageFileHeader> CompatibleDestinations => new List<IImageFileHeader>()
        {
            new FFImgHeader(),
            new CoD4_GfxImageFileHeader(),
            new CoD6_GfxImageFileHeader()
        }.AsReadOnly();

        public string FormatDescription => $"Zonebuilder/IW4OF compatible ({FormatExtension} v1)";

        public string FormatExtension => "iw4xImage";

        public byte IwiVersion => 8;

        public int Flags => flags;

        public byte MipLevels => levelCount;

        public byte Semantic => semantic;

        public int Width => dimensions[0];

        public int Height => dimensions[1];

        public int Depth => dimensions[2];

        public int DataLength => resourceSize;

        public string Name => "iw4ximage";

        public MapType MapType => mapType;

        public _D3DFORMAT D3dFormat => format;

        public GfxImageFileFormat GfxFormat {
            get {
                switch (format) {
                    default:
                        return GfxImageFileFormat.IMG_FORMAT_BITMAP_RGBA;

                    case _D3DFORMAT.D3DFMT_A8R8G8B8:
                        return GfxImageFileFormat.IMG_FORMAT_BITMAP_RGBA; // Channels will be swapped 

                    case _D3DFORMAT.D3DFMT_L8:
                        return GfxImageFileFormat.IMG_FORMAT_BITMAP_ALPHA;
                }
            }
        }

        public ImageCategory Category => category;

        private byte version = 0;

        public void Deserialize(BinaryReader br)
        {
            version = br.ReadByte();

            mapType = (MapType)br.ReadByte();
            semantic = br.ReadByte();
            category = (ImageCategory)br.ReadByte();
            size = br.ReadInt32();

            if (version == '0') {
                levelCount = br.ReadByte();
                flags = br.ReadByte();
                dimensions = new short[]
                {
                    br.ReadInt16(),
                    br.ReadInt16(),
                    br.ReadInt16()
                };
                format = (_D3DFORMAT)br.ReadInt32();
                resourceSize = br.ReadInt32();

            }
            else  if (version == 1){
                levelCount = br.ReadByte();
                flags = (byte)br.ReadInt32();
                dimensions = new short[]
                {
                    br.ReadInt16(),
                    br.ReadInt16(),
                    br.ReadInt16()
                };

                resourceSize = size;
                format = (_D3DFORMAT)br.ReadInt32();
            }
            else {
                throw new System.Exception($"Unsupported IW4XImage version {version}");
            }
        }

        public void Serialize(BinaryWriter bw)
        {
            bw.Write((byte)mapType);
            bw.Write(semantic);
            bw.Write((byte)category);
            bw.Write(size);
            bw.Write(levelCount);
            bw.Write(flags);
            bw.Write(dimensions[0]);
            bw.Write(dimensions[1]);
            bw.Write(dimensions[2]);
            bw.Write((int)format);
            bw.Write(resourceSize);
        }

        public void From(IImageFileHeader header)
        {
            mapType = header.MapType;
            semantic = header.Semantic;
            category = header.Category;
            size = header.DataLength;
            levelCount = header.MipLevels;
            flags = (byte)header.Flags;
            dimensions = new short[]{
                (short)header.Width,
                (short)header.Height,
                (short)header.Depth
            };
            format = header.D3dFormat;
            resourceSize = header.DataLength; // Yes, we have the same info twice
        }
    }
}
