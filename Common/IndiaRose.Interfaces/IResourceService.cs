﻿using System.IO;

namespace IndiaRose.Interfaces
{
    public interface IResourceService
    {
        void ShowPdfFile(string pdfFileName);
        Stream OpenZip(string zipFileName);
        void Copy(string src,string dest);

    }
}
