using System.Collections.Generic;

namespace IWImgViewer
{
    public static class FormatHelper
    {
        public static Dictionary<GfxImageFileFormat, byte> ChannelCount = new Dictionary<GfxImageFileFormat, byte>()
        {
            {GfxImageFileFormat.IMG_FORMAT_BITMAP_ALPHA, 1 },
            {GfxImageFileFormat.IMG_FORMAT_BITMAP_LUMINANCE, 1 },
            {GfxImageFileFormat.IMG_FORMAT_BITMAP_RGB, 3 },
            {GfxImageFileFormat.IMG_FORMAT_BITMAP_RGBA, 4 },
            {GfxImageFileFormat.IMG_FORMAT_DXT1, 4 },
            {GfxImageFileFormat.IMG_FORMAT_DXT3, 4 },
            {GfxImageFileFormat.IMG_FORMAT_DXT5, 4 }
        };

        public static Dictionary<GfxImageFileFormat, ManagedSquish.SquishFlags> GfxFormatToSquishFormat = new Dictionary<GfxImageFileFormat, ManagedSquish.SquishFlags>()
            {
                { GfxImageFileFormat.IMG_FORMAT_DXT1, ManagedSquish.SquishFlags.Dxt1 },
                { GfxImageFileFormat.IMG_FORMAT_DXT3, ManagedSquish.SquishFlags.Dxt3 },
                { GfxImageFileFormat.IMG_FORMAT_DXT5, ManagedSquish.SquishFlags.Dxt5 },
            };

        public static byte GetBlockSize(GfxImageFileFormat format)
        {
            return (byte)(format.ToString().Contains("DXT") ? 4 : 1);
        }

    }
}
