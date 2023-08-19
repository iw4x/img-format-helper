using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWImgViewer
{
    enum Iwi6Flags : int
    {
        IMG_FLAG_NOPICMIP = 1 << 0,
        IMG_FLAG_NOMIPMAPS = 1 << 1,
        IMG_FLAG_CUBEMAP = 1 << 2,
        IMG_FLAG_VOLMAP = 1 << 3,
        IMG_FLAG_STREAMING = 1 << 4,
        IMG_FLAG_LEGACY_NORMALS = 1 << 5,
        IMG_FLAG_CLAMP_U = 1 << 6,
        IMG_FLAG_CLAMP_V = 1 << 7,
        IMG_FLAG_DYNAMIC = 1 << 16,
        IMG_FLAG_RENDER_TARGET = 1 << 17,
        IMG_FLAG_SYSTEMMEM = 1 << 18
    };
}
