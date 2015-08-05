using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace LibGP.Utils
{
    public class ZIP
    {    
        public List<String> Files = new List<String>();
        private ZipStorer Zip = new ZipStorer();
        private String zipFile = String.Empty;

        public ZIP()
        {
        }

        public ZIP(String filename)
        {
            zipFile = filename;
        }

        public void Open()
        {
            Zip = ZipStorer.Open(zipFile, FileAccess.Read);
        }

        public void Save()
        {
            Zip = ZipStorer.Create(zipFile, "GamePower Package");
            Zip.EncodeUTF8 = false;
            foreach (String file in Files)
                Zip.AddFile(ZipStorer.Compression.Store, file, Path.GetFileName(file), String.Empty);
        }

        public void Close()
        {
            Zip.Close();
        }

        public List<ZipStorer.ZipFileEntry> ListFiles()
        {
            return Zip.ReadCentralDir();
        }

        public bool ExtractFile(ZipStorer.ZipFileEntry zipFileEntry, String filename)
        {
            return Zip.ExtractFile(zipFileEntry, filename);
        }

        public bool ExtractFile(String filenameInZip, String filename)
        {
            ZipStorer.ZipFileEntry? file = Zip.ReadCentralDir().Where(x => x.FilenameInZip == filenameInZip).SingleOrDefault();
            if (file != null)
                Zip.ExtractFile(file.Value, filename);
            else
                return false;
            return true;
        }

        public String ExtractFileToString(String filenameInZip)
        {
            ZipStorer.ZipFileEntry? file = Zip.ReadCentralDir().Where(x => x.FilenameInZip == filenameInZip).SingleOrDefault();
            if (file != null)
                return ExtractFileToString(file.Value);
            else
                return null;
        }
        public String ExtractFileToString(ZipStorer.ZipFileEntry zipFileEntry)
        {
            String rv = null;
            MemoryStream ms = new MemoryStream();
            if (Zip.ExtractFile(zipFileEntry, ms))
            {
                ms.Position = 0;
                rv = new StreamReader(ms).ReadToEnd().Trim();
            }
            ms.Close();
            ms.Dispose();
            return rv;
        }

        public bool ExtractFileToStream(String filenameInZip, ref MemoryStream s)
        {
            ZipStorer.ZipFileEntry? file = Zip.ReadCentralDir().Where(x => x.FilenameInZip == filenameInZip).SingleOrDefault();
            if (file != null)
                return ExtractFileToStream(file.Value, ref s);
            else
                return false;
        }
        public bool ExtractFileToStream(ZipStorer.ZipFileEntry zipFileEntry, ref MemoryStream s)
        {
            if (Zip.ExtractFile(zipFileEntry, s))
                return true;
            return false;
        }

    }
}
