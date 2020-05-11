using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.Interfaces
{
    public interface INativeFileHandler
    {
        string path { get; set; }
        byte[] GetFile(string filename);
        byte[] GetFileByFilePath(string filePath);
        bool SaveFile(string filename, byte[] data);
        void CreateFolder(string filepath = "");
        void DeleteFile(string filepath);

        void DeleteAllFiles();
        string GetText(string fileName);
    }
}
