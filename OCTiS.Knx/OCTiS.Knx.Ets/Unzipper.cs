using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace OCTiS.Knx.Ets
{
    public class UnzippedDescriptor
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
    }

    public class Unzipper
    {
        public static void Unzip(string zipFile, Action<Stream, UnzippedDescriptor> perFile)
        {
            using (var s = File.OpenRead(zipFile))
            {
                Unzip(s, perFile);
            }
        }

        public static void Unzip(Stream stream, Action<Stream, UnzippedDescriptor> perFile)
        {
            using (ZipInputStream s = new ZipInputStream(stream))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.IsFile)
                    {
                        perFile(s, new UnzippedDescriptor()
                            {
                                FileName = theEntry.Name,
                                Extension = Path.GetExtension(theEntry.Name)
                            });
                    }
                }
            }
        }
    }
}
