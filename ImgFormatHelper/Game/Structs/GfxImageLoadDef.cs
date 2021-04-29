using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWImgViewer
{
    struct GfxImageLoadDef
    {
        public byte levelCount;
        public byte flags;
        public short[] dimensions;
        public _D3DFORMAT format;
        public int resourceSize;
        public byte[] data;
    };
}
