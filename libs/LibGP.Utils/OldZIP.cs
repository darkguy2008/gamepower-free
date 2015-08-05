using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Packaging;

namespace LibGP.Utils
{
    // TODO: Someone should redo this. Either make a new ZIP library or use another
    // compression method, 7-zip compatible. TAR is a good option, but I haven't found
    // any simple TAR classes out there. The format looks easy, but I ain't got time
    // to do it.
    // Taken from: http://www.csharpque.com/2013/05/zipfiles-dotnet35-CreateAssembly.html
    public class ZIP
    {
        private string strPath;
        private List<String> lError = new List<String>();
        public List<String> lFiles = new List<String>();
        public String[] ErrorList
        {
            get
            {
                return lError.ToArray();
            }
        }
        
        public ZIP(String sPath)
        {
            strPath = sPath;
        }
        public int Save()
        {
            foreach (String strFile in lFiles)
                AddFile(strFile);
            if (lError.Count > 0)
            {
                if (lError.Count < lFiles.Count)
                    return 0;
                else
                    return -1;
            }
            return 1;
        }

        private bool AddFile(String strFile)
        {
            PackagePart pkgPart = null;
            using (Package Zip = Package.Open(strPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
            {
                String strTemp = strFile.Replace(" ", "_");
                String zipURI = String.Concat("/", System.IO.Path.GetFileName(strTemp));
                Uri parturi = new Uri(zipURI, UriKind.Relative);
                try
                {
                    pkgPart = Zip.CreatePart(parturi, System.Net.Mime.MediaTypeNames.Application.Zip, CompressionOption.Normal);
                }
                catch (Exception ex)
                {
                    lError.Add(strFile + "; Error : " + ex.Message);
                    return false;
                }
                Byte[] bites = System.IO.File.ReadAllBytes(strFile);
                pkgPart.GetStream().Write(bites, 0, bites.Length);
                Zip.Close();
            }
            return true;
        }
    }
}