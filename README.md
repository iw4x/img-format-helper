# ![img format](ImgFormatHelper/Resources/cardicon_juggernaut_1.png) IWI Image formats helper

This small tool is made to visualize, export, and eventually convert the most popular image formats among IW dumps and tools.

## Formats
It currently supports the following formats:
- IWI v6 (Call of Duty 4: Modern Warfare)
- IWI v8 (Call of Duty 6: Modern Warfare 2)
- FFIMG (as of 04/29/2021) ([Zonetool](https://github.com/ZoneTool/zonetool))
- IW4XImage v0 ([Zonebuilder](https://github.com/XLabsProject/iw4x-client))

The underlying supported formats are:
- IMG_FORMAT_BITMAP_ALPHA
- IMG_FORMAT_DXT1
- IMG_FORMAT_DXT3
- IMG_FORMAT_DXT5
- IMG_FORMAT_BITMAP_RGB
- IMG_FORMAT_BITMAP_RGBA
- D3DFMT_L8
- D3DFMT_A8R8G8B8

## Mipmaps
Mipmaps are supported and correctly separated for each format. Only 2D and Cube textures are supported, other map types will be treated as 2D.

## How to use
Drag & drop any supported file on the main window to start. 
This should open as many viewports as necessary to visualize the image (one viewport per mip, and one viewport per face if it's a cubemap)

### Export image
To export an image to a common format (PNG, JPEG...), select any viewport and press **X**.

### Convert image
To convert an image to another format, select the **main** window and press **C**.

*Note: IWI can be safely converted between different versions, and FFIMG can be converted to IW4XImage and back. However, converting IWI to FFImg/IW4XImg or the other way around might yield unexpected results.*
