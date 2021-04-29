using System.IO;
using System.Text;
using System.Linq;

namespace IWImgViewer
{
    public class FFImgHeader : IImageFileHeader
    {
        public MapType mapType;
        public byte semantic;
        public ImageCategory category;
        public byte gfxImageFlags;
        public int cardMemory;
        public int dataLen1;
        public int dataLen2;
        public int height;
        public int width;
        public int depth;
        public string cName;
        public byte mipLevels;
        public byte flags;
        public int wrongHeight;
        public int wrongWidth;
        public int wrongDepth;
        public _D3DFORMAT format;
        public int resourceSize;

        public System.Collections.Generic.IReadOnlyList<IImageFileHeader> CompatibleDestinations => new System.Collections.Generic.List<IImageFileHeader>()
        {
            new IW4XImageHeader(),
            new CoD4_GfxImageFileHeader(),
            new CoD6_GfxImageFileHeader()
        }.AsReadOnly();

        public string FormatDescription => $"Zonetool-compatible (29/04/2021) ({FormatExtension})";

        public string FormatExtension => "ffImg";

        public byte IwiVersion => 8;

        public int Flags => flags;

        public byte MipLevels => mipLevels;

        public byte Semantic => semantic;

        public int Width => width;

        public int Height => height;

        public int Depth => depth;

        public int DataLength => resourceSize;

        public string Name => cName;

        public MapType MapType => mapType;

        public _D3DFORMAT D3dFormat => format;

        public GfxImageFileFormat GfxFormat
        {
            get
            {
                switch (format)
                {
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

        public void Deserialize(BinaryReader br)
        {

            mapType = (MapType)br.ReadByte();
            semantic = br.ReadByte();
            category = (ImageCategory)br.ReadByte();

            flags = br.ReadByte(); // Flags (gfximage)
            cardMemory = br.ReadInt32(); // CardMemory
            dataLen1 = br.ReadInt32(); // dataLen1
            dataLen2 = br.ReadInt32(); // dataLen2
            height = br.ReadInt32(); // height
            width = br.ReadInt32(); // width 
            depth = br.ReadInt32(); // depth

            // Read string
            StringBuilder sb = new StringBuilder();
            byte b;
            while ((b = br.ReadByte()) != 0)
            {
                sb.Append((char)b);
                // Skip!
            }
            cName = sb.ToString();

            mipLevels = br.ReadByte();
            flags = br.ReadByte(); // Flags again (loaddef)

            // We read wrong dimensions here
            wrongWidth = br.ReadInt32();
            wrongHeight = br.ReadInt32();
            wrongDepth = br.ReadInt32();

            format = (_D3DFORMAT)br.ReadInt32();

            resourceSize = br.ReadInt32();
        }

        public void Serialize(BinaryWriter bw)
        {
            bw.Write((byte)mapType);
            bw.Write(semantic);
            bw.Write((byte)category);
            bw.Write(flags);

            bw.Write(cardMemory);
            bw.Write(dataLen1);
            bw.Write(dataLen2);
            bw.Write(height);
            bw.Write(width);
            bw.Write(depth);

            // Null-terminated string
            bw.Write(cName.ToCharArray().Append('\0').ToArray());

            bw.Write(mipLevels);
            bw.Write(flags);
            bw.Write(wrongWidth);
            bw.Write(wrongHeight);
            bw.Write(wrongDepth);
            bw.Write((int)format);
            bw.Write(resourceSize);
        }

        public void From(IImageFileHeader header)
        {
            mapType = header.MapType;
            semantic = header.Semantic;
            category = header.Category;
            gfxImageFlags = (byte)header.Flags;
            height = header.Height;
            width = header.Width;
            depth = header.Depth;
            cName = header.Name;
            mipLevels = header.MipLevels;
            flags = (byte)header.Flags;
            wrongDepth = 1;
            wrongHeight = 1;
            wrongWidth = 1;
            format = header.D3dFormat;
            resourceSize = header.DataLength;
        }
    }
}
