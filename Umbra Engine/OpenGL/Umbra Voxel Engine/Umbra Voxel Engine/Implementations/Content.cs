using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbra.Implementations
{
    static public class Content
    {
        static public object Load(string assetName)
        {
            string extension = assetName.Substring(assetName.LastIndexOf('.'));

            switch(extension)
            {
                case ".png":
                    {
                        return (object)LoadBitmap(assetName);
                    }

                default:
                    {
                        throw new FileLoadException("The file extension \"" + extension + "\" is not supported!");
                    }
            }
        }

        static private Bitmap LoadBitmap(string assetName)
        {
            FileExists(assetName);

            return new Bitmap(assetName);
        }

        static private void FileExists(string assetName)
        {
            if (!File.Exists(assetName))
            {
                throw new FileNotFoundException("The file \"" + assetName + "\" could not be located!");
            }
        }
    }
}
