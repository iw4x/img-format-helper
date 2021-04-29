using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWImgViewer
{
    public enum MapType : byte
    {
        MAPTYPE_NONE = 0x0,
        MAPTYPE_INVALID1 = 0x1,
        MAPTYPE_INVALID2 = 0x2,
        MAPTYPE_2D = 0x3,
        MAPTYPE_3D = 0x4,
        MAPTYPE_CUBE = 0x5,
        MAPTYPE_COUNT = 0x6,
    };
}
