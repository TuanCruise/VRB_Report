using DevExpress.LookAndFeel;
using DevExpress.Skins;
using FIS.Common;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FIS.Utils
{
    public static class ThemeUtils
    {
        public static ImageList Image16;
        public static ImageList Image24;
        public static ImageList Image32;
        public static ImageList Image48;
        public static Color EditRowColor;
        public static Color BackTitleColor;
        public static Color TitleColor;

        internal static void Initialize()
        {
            Image16 = new ImageList();
            CreateImageCache(Image16, ColorDepth.Depth32Bit, new Size(16, 16), CONSTANTS.IMAGE16_FOLDER);
            Image24 = new ImageList();
            CreateImageCache(Image24, ColorDepth.Depth32Bit, new Size(24, 24), CONSTANTS.IMAGE24_FOLDER);
            Image32 = new ImageList();
            CreateImageCache(Image32, ColorDepth.Depth32Bit, new Size(32, 32), CONSTANTS.IMAGE32_FOLDER);
            Image48 = new ImageList();
            CreateImageCache(Image48, ColorDepth.Depth32Bit, new Size(48, 48), CONSTANTS.IMAGE48_FOLDER);
        }

        public static void ChangeSkin(string skinName)
        {
            UserLookAndFeel.Default.SkinName = skinName;
            var skin = GridSkins.GetSkin(UserLookAndFeel.Default);
            EditRowColor = skin[GridSkins.SkinGridGroupPanel].Color.BackColor;

            skin = BarSkins.GetSkin(UserLookAndFeel.Default);
            BackTitleColor = skin[BarSkins.SkinMainMenu].Color.BackColor;
            TitleColor = skin[BarSkins.SkinMainMenu].Color.ForeColor;
        }

        public static void CreateImageCache(ImageList cache, ColorDepth depth, Size size, string folder)
        {
            cache.ColorDepth = depth;
            cache.ImageSize = size;

            if (Directory.Exists(folder))
                foreach (var fileName in Directory.GetFiles(folder))
                {
                    if (fileName.EndsWith(".PNG", true, null) || fileName.EndsWith(".JPG", true, null) || fileName.EndsWith(".GIF", true, null))
                    {
                        var name = new FileInfo(fileName).Name;
                        var extlength = new FileInfo(fileName).Extension.Length;
                        cache.Images.Add(name.Remove(name.Length - extlength), Image.FromFile(fileName));
                    }
                }
        }

        public static int GetImage16x16Index(string key)
        {
            return Image16.Images.IndexOfKey(key);
        }

        public static int GetImage24x24Index(string key)
        {
            return Image24.Images.IndexOfKey(key);
        }

        public static int GetImage32x32Index(string key)
        {
            return Image32.Images.IndexOfKey(key);
        }
    }
}
