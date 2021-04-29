using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWImgViewer
{
    public enum GfxImageFileFormat : byte
    {
        IMG_FORMAT_INVALID = 0x0,
        IMG_FORMAT_BITMAP_RGBA = 0x1,
        IMG_FORMAT_BITMAP_RGB = 0x2,
        IMG_FORMAT_BITMAP_LUMINANCE_ALPHA = 0x3,
        IMG_FORMAT_BITMAP_LUMINANCE = 0x4,
        IMG_FORMAT_BITMAP_ALPHA = 0x5,
        IMG_FORMAT_WAVELET_RGBA = 0x6,
        IMG_FORMAT_WAVELET_RGB = 0x7,
        IMG_FORMAT_WAVELET_LUMINANCE_ALPHA = 0x8,
        IMG_FORMAT_WAVELET_LUMINANCE = 0x9,
        IMG_FORMAT_WAVELET_ALPHA = 0xA,
        IMG_FORMAT_DXT1 = 0xB,
        IMG_FORMAT_DXT3 = 0xC,
        IMG_FORMAT_DXT5 = 0xD,
        IMG_FORMAT_DXN = 0xE,
        IMG_FORMAT_DXT3A_AS_LUMINANCE = 0xF,
        IMG_FORMAT_DXT5A_AS_LUMINANCE = 0x10,
        IMG_FORMAT_DXT3A_AS_ALPHA = 0x11,
        IMG_FORMAT_DXT5A_AS_ALPHA = 0x12,
        IMG_FORMAT_DXT1_AS_LUMINANCE_ALPHA = 0x13,
        IMG_FORMAT_DXN_AS_LUMINANCE_ALPHA = 0x14,
        IMG_FORMAT_DXT1_AS_LUMINANCE = 0x15,
        IMG_FORMAT_DXT1_AS_ALPHA = 0x16,
        IMG_FORMAT_COUNT = 0x17,
    };
}
