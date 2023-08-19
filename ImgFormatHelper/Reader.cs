using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWImgViewer {
    public class Reader {
        public string ImageName => imagePath;
        public string ImageFormat { get; private set; }

        public byte[] Data { get; set; }

        private bool useAlpha = false;

        public IImageFileHeader CurrentHeader { get; private set; }
        public bool UseAlpha {
            get => useAlpha; set {
                useAlpha = value;
                Read();
            }
        }

        private readonly string imagePath;

        public Reader(string imagePath)
        {
            if (!File.Exists(imagePath)) {
                throw new FileNotFoundException();
            }

            this.imagePath = imagePath;
            Read();
        }

        public delegate void OnBitmapReadDelegate(Bitmap map, string name);
        public static event OnBitmapReadDelegate OnBitmapRead;

        private void Read()
        {
            string ext = System.IO.Path.GetExtension(imagePath).Substring(1).ToUpper();

            switch (ext.ToUpper()) {
                case "FFIMG":
                    DecodeFFImage();
                    break;

                case "IW4XIMAGE":
                    DecodeIW4XImage();
                    break;

                case "IWI":
                    DecodeIWI4();
                    break;
            }
        }

        void DecodeIWI4()
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open)) {
                using (BinaryReader br = new BinaryReader(fs)) {
                    CoD6_GfxImageFileHeader header;

                    try {
                        header = new CoD6_GfxImageFileHeader(Path.GetFileNameWithoutExtension(imagePath));
                        header.Deserialize(br);
                        CurrentHeader = header;
                    }
                    catch (FormatException) {
                        br.Dispose();
                        fs.Dispose();

                        // Maybe cod4?
                        DecodeIWI3();
                        return;
                    }

                    bool hasMipMaps = (header.flags & (byte)Iwi8Flags.IMG_FLAG_NOMIPMAPS) == 0;
                    Data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));

                    CreateBitmap(Data, (short)header.Width, (short)header.Height, header.format, header.MapType, hasMipMaps);
                }
            }
        }

        void DecodeIWI3()
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open)) {
                using (BinaryReader br = new BinaryReader(fs)) {
                    CoD4_GfxImageFileHeader header = new CoD4_GfxImageFileHeader(Path.GetFileNameWithoutExtension(imagePath));
                    header.Deserialize(br);
                    CurrentHeader = header;
                    bool hasMipMaps = (header.flags & (byte)Iwi6Flags.IMG_FLAG_NOMIPMAPS) == 0;
                    Data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));

                    CreateBitmap(Data, header.dimensions[0], header.dimensions[1], header.format, header.MapType, hasMipMaps);
                }
            }
        }

        void DecodeFFImage()
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open)) {
                using (BinaryReader br = new BinaryReader(fs)) {
                    FFImgHeader header = new FFImgHeader();
                    header.Deserialize(br);
                    CurrentHeader = header;
                    bool hasMipMaps = (header.flags & (byte)Iwi8Flags.IMG_FLAG_NOMIPMAPS) == 0;
                    Data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));

                    if (header.format == _D3DFORMAT.D3DFMT_UNKNOWN) {
                        throw new FormatException(header.format.ToString());
                    }

                    CreateBitmap(Data, (short)header.Width, (short)header.Height, header.D3dFormat, header.MapType);
                }
            }
        }

        void DecodeIW4XImage()
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open)) {
                using (BinaryReader br = new BinaryReader(fs)) {
                    if (new string(br.ReadChars(7)) != "IW4xImg") {
                        throw new FormatException();
                    }

                    IW4XImageHeader header = new IW4XImageHeader();
                    header.Deserialize(br);
                    CurrentHeader = header;
                    bool hasMipMaps = (header.flags & (byte)Iwi8Flags.IMG_FLAG_NOMIPMAPS) == 0;
                    Data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));

                    if (header.format == _D3DFORMAT.D3DFMT_UNKNOWN) {
                        throw new FormatException(header.format.ToString());
                    }

                    CreateBitmap(Data, (short)header.Width, (short)header.Height, header.D3dFormat, header.MapType);
                }
            }
        }

        void CreateBitmap(byte[] data, short width, short height, GfxImageFileFormat format, MapType mapType = MapType.MAPTYPE_2D, bool hasMips = false)
        {
            if (hasMips) {
                CreateBitmapsWithMips(data, width, height, format, mapType);
                return;
            }

            Bitmap bitmap = null;

            ImageFormat = format.ToString();

            byte channels;
            int maxI;

            switch (format) {
                case GfxImageFileFormat.IMG_FORMAT_BITMAP_ALPHA:
                    bitmap = new Bitmap(width, height);
                    BitmapSetPixels(bitmap, width, height, data, 1);
                    break;

                case GfxImageFileFormat.IMG_FORMAT_DXT1:
                case GfxImageFileFormat.IMG_FORMAT_DXT3:
                case GfxImageFileFormat.IMG_FORMAT_DXT5:

                    byte[] pixels;
                    bitmap = new Bitmap(width, height);

                    pixels = ManagedSquish.Squish.DecompressImage(data, width, height, FormatHelper.GfxFormatToSquishFormat[format]);

                    channels = 4;
                    maxI = channels * width * height;

                    if (maxI > pixels.Length) {
                        if (pixels.Length > (channels - 1) * width * height) {
                            System.Windows.Forms.MessageBox.Show($"Too many channels! Expected {channels} channels but there is only data for {channels - 1} channels.");
                            channels--;
                        }
                        else {
                            System.Windows.Forms.MessageBox.Show($"Wrong number of channels! For format {format}, {channels} channels is too much.");
                        }
                    }

                    BitmapSetPixels(bitmap, width, height, SwapChannels(pixels, new Dictionary<byte, byte>()
                        {
                            {0, 2}, // R -> B
                            {1, 1}, // G -> G
                            {2, 0}, // B -> R
                            {3, 3}  // A -> A
                        }, channels), channels);

                    break;

                //case GfxImageFileFormat.IMG_FORMAT_WAVELET_RGB:
                //case GfxImageFileFormat.IMG_FORMAT_WAVELET_RGBA:
                //    using (OpenJpegDotNet.IO.Reader reader = new OpenJpegDotNet.IO.Reader(data))
                //    {
                //        reader.ReadHeader();
                //        bitmap = reader.ReadData();
                //        Console.WriteLine("Read data");
                //    }
                //    break;

                //case GfxImageFileFormat.IMG_FORMAT_WAVELET_RGB:
                //    var codecs = new Leadtools.Codecs.RasterCodecs();
                //    codecs.StartDecompress(new Leadtools.Codecs.CodecsStartDecompressOptions()
                //    {

                //    })
                //    new Leadtools.Codecs.JPEG200
                //    codecs.Load(new MemoryStream(data), width, height, 24, Leadtools.RasterSizeFlags.None, Leadtools.Codecs.CodecsLoadByteOrder.Rgb);

                //    channels = 3;
                //    bitmap = new Bitmap(width, height);
                //    BitmapSetPixels(bitmap, width, height, data, channels);
                //    break;

                case GfxImageFileFormat.IMG_FORMAT_BITMAP_RGB:
                    channels = 3;
                    bitmap = new Bitmap(width, height);
                    BitmapSetPixels(bitmap, width, height, data, channels);
                    break;

                case GfxImageFileFormat.IMG_FORMAT_BITMAP_RGBA:
                    channels = 4;
                    bitmap = new Bitmap(width, height);
                    BitmapSetPixels(bitmap, width, height, data, channels);
                    break;


                default:
                    throw new FormatException(format.ToString());

            }

            OnBitmapRead(bitmap, string.Empty);
        }

        void CreateBitmapsWithMips(byte[] data, short width, short height, GfxImageFileFormat format, MapType mapType = MapType.MAPTYPE_2D)
        {
            // Generate mips
            List<(int width, int height)> mips = new List<(int width, int height)>();

            int sides = mapType == MapType.MAPTYPE_CUBE ? 6 : 1;

            short maxDimension = Math.Max(height, width);
            int mipmapFactor = 1;
            int minBlockSize = FormatHelper.GetBlockSize(format);

            while (maxDimension != 0) {
                maxDimension >>= 1;
                mipmapFactor *= 2;
                mips.Add((Math.Max(width / mipmapFactor, minBlockSize), Math.Max(height / mipmapFactor, minBlockSize)));
            }

            mips.Reverse();


            int compressedLengthToSkip = 0;

            for (int i = 0; i <= mips.Count; i++) {
                for (int side = 0; side < sides; side++) {
                    short thisWidth, thisHeight;

                    int currentSize = 0;
                    if (i < mips.Count) {
                        var mip = mips[i];
                        thisWidth = (short)mip.width;
                        thisHeight = (short)mip.height;

                        if (thisWidth <= 16 && thisHeight <= 16) {
                            // We skip them because at this point the windows form is bigger than the image
                            continue;
                        }
                    }
                    else {
                        thisWidth = width;
                        thisHeight = height;
                    }

                    if (FormatHelper.GfxFormatToSquishFormat.ContainsKey(format)) {
                        currentSize = ManagedSquish.Squish.GetStorageRequirements(thisWidth, thisHeight, FormatHelper.GfxFormatToSquishFormat[format]);
                    }
                    else {
                        currentSize = thisWidth * thisHeight * FormatHelper.ChannelCount[format];
                    }

                    CreateBitmap(data.Skip(compressedLengthToSkip).Take(currentSize).ToArray(), thisWidth, thisHeight, format);
                    compressedLengthToSkip += currentSize;
                }
            }
        }

        void CreateBitmap(byte[] data, short width, short height, _D3DFORMAT format, MapType mapType = MapType.MAPTYPE_2D)
        {
            int sides = mapType == MapType.MAPTYPE_CUBE ? 6 : 1;
            ImageFormat = format.ToString();

            for (int side = 0; side < sides; side++) {
                Bitmap bitmap = new Bitmap(width, height);
                byte[] truncatedData;
                int sizeOfOneSide;

                switch (format) {
                    case _D3DFORMAT.D3DFMT_L8:
                        sizeOfOneSide = width * height * 1;
                        truncatedData = data.Skip(sizeOfOneSide * side).Take(sizeOfOneSide).ToArray();
                        BitmapSetPixels(bitmap, width, height, truncatedData, 1);
                        break;

                    case _D3DFORMAT.D3DFMT_A8R8G8B8:
                        // ARGB ON COD IS ACTUALLY RGBA !
                        // YES, I CHECKED!
                        // THIS IS INSANE >.<'
                        sizeOfOneSide = width * height * 4;
                        truncatedData = data.Skip(sizeOfOneSide * side).Take(sizeOfOneSide).ToArray();

                        BitmapSetPixels(bitmap, width, height, truncatedData, 4);
                        break;

                    case _D3DFORMAT.D3DFMT_DXT1:
                        CreateBitmap(data, width, height, GfxImageFileFormat.IMG_FORMAT_DXT1, MapType.MAPTYPE_2D, hasMips: false);
                        return;


                    case _D3DFORMAT.D3DFMT_G16R16: {
                            sizeOfOneSide = width * height * 4;
                            truncatedData = data.Skip(sizeOfOneSide * side).Take(sizeOfOneSide).ToArray();
                            byte[] pixels = new byte[truncatedData.Length / 2];

                            for (int pixelIndex = 0; pixelIndex < truncatedData.Length; pixelIndex += 2) {
                                ushort pixel = BitConverter.ToUInt16(truncatedData, pixelIndex);
                                byte charPixel = (byte)(pixel / (float)ushort.MaxValue * byte.MaxValue);
                                pixels[pixelIndex / 2] = charPixel;
                            }

                            BitmapSetPixels(bitmap, width, height, pixels, 2);
                            break;
                        }

                    case _D3DFORMAT.D3DFMT_R5G6B5: {
                            sizeOfOneSide = width * height * 2;
                            truncatedData = data.Skip(sizeOfOneSide * side).Take(sizeOfOneSide).ToArray();

                            byte[] pixels = new byte[truncatedData.Length / 2 * 3];
                            for (int pixelIndex = 0; pixelIndex < truncatedData.Length; pixelIndex += 2) {

                                int RGB565_to_RGB888(ushort rgb565)
                                {
                                    byte R5 = (byte)((rgb565 & 0xf800) >> 11);
                                    byte G6 = (byte)((rgb565 & 0x07e0) >> 5);
                                    byte B5 = (byte)(rgb565 & 0x001f);

                                    byte r = (byte)((R5 * 527 + 23) >> 6);
                                    byte g = (byte)((G6 * 259 + 33) >> 6);
                                    byte b = (byte)((B5 * 527 + 23) >> 6);

                                    return
                                        (r) |
                                        (g) << 8 |
                                        (b) << 16; // RGBA ?


                                }

                                ushort shortColor = BitConverter.ToUInt16(truncatedData, pixelIndex);
                                int longColor = RGB565_to_RGB888(shortColor);
                                byte[] colorData = BitConverter.GetBytes(longColor);
                                colorData.Take(3).ToArray().CopyTo(pixels, pixelIndex / 2 * 3);
                            }

                            BitmapSetPixels(bitmap, width, height, pixels, 3);
                            break;
                        }

                    default:
                        throw new FormatException(format.ToString());
                }

                OnBitmapRead(bitmap, string.Empty);
            }
        }

        byte[] SwapChannels(byte[] data, Dictionary<byte, byte> swapMap, byte channels)
        {
            byte[] newData = new byte[data.Length];
            for (int i = 0; i < data.Length; i += channels) {
                try {
                    for (byte channel = 0; channel < channels; channel++) {
                        newData[i + channel] = data[i + swapMap[channel]];
                    }
                }
                catch (IndexOutOfRangeException) {
                    System.Windows.Forms.MessageBox.Show($"Wrong number of channels! {channels} channels is not correct!");
                }
            }

            return newData;
        }

        void BitmapSetPixels(Bitmap bitmap, short width, short height, byte[] pixelsRGBA, byte channels)
        {
            int maxI = channels * width * height;
            bool hasWarned = false;
            int x, y;

            if (pixelsRGBA.Length != maxI) {
                System.Windows.Forms.MessageBox.Show($"Unexpected number of pixels: Expected {maxI} pixels, found {pixelsRGBA.Length} (difference of {pixelsRGBA.Length - maxI})");
            }

            for (int i = 0; i < pixelsRGBA.Length - channels; i++) {
                if (i >= maxI) {
                    if (!hasWarned) {
                        hasWarned = true;
                        System.Windows.Forms.MessageBox.Show($"Found {pixelsRGBA.Length - maxI} extra bytes of information at the end of data (?)");
                    }
                }
                else {
                    if (i % channels == 0) {
                        x = (i / channels) % width;
                        y = i / (width * channels);

                        bitmap.SetPixel(x, y, Color.FromArgb(
                            (channels > 3 && useAlpha) ? pixelsRGBA[i + 3] : 255,
                            channels > 2 ? pixelsRGBA[i + 2] : pixelsRGBA[i],
                            channels > 1 ? pixelsRGBA[i + 1] : pixelsRGBA[i],
                            pixelsRGBA[i])
                        );
                    }
                }
            }
        }
    }
}
