using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWImgViewer
{
    public interface IImageFileHeader
    {
        string FormatDescription { get; }
        string FormatExtension { get; }

        IReadOnlyList<IImageFileHeader> CompatibleDestinations { get; }

        byte IwiVersion { get; }
        int Flags { get; }
        byte MipLevels { get; }
        byte Semantic { get; }

        int Width { get; }
        int Height { get; }
        int Depth { get; }

        int DataLength { get; }

        string Name { get; }

        MapType MapType { get; }
        _D3DFORMAT D3dFormat { get; }
        GfxImageFileFormat GfxFormat { get; }

        ImageCategory Category { get; }

        void Deserialize(BinaryReader reader);

        void Serialize(BinaryWriter writer);

        void From(IImageFileHeader header);
    }
}
