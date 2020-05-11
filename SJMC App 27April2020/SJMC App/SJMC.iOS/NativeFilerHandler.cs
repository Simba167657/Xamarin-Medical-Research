using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SJMC.Interfaces;
using SJMC.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeFileHandler))]
namespace SJMC.iOS
{
    public class NativeFileHandler : INativeFileHandler
    {
        public string path { get; set; }

        public NativeFileHandler()
        {

            //  path = Xamarin.Essentials.FileSystem.AppDataDirectory;

           // path = Path.GetTempPath() + "/Documents";
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            CreateFolder(path);
        }
        public string GetText(string fileName)
        {
            return File.ReadAllText($"{path}/{fileName}");
        }

        public bool SaveFile(string filename, byte[] data)
        {
            try
            {
                //string directoryName = Path.GetDirectoryName(fileNameAndPath);
                //string filename = Path.GetFileName(fileNameAndPath);
                //var  directoryName = Path.Combine(_path, filename);

                Directory.CreateDirectory(path);
                var filePath = Path.Combine(path, filename);
                File.WriteAllBytes(filePath, data);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public byte[] GetFile(string filename)
        {
            try
            {
                var filePath = Path.Combine(path, filename);
                var bytes = File.ReadAllBytes(filePath);
                return bytes;
            }
            catch (Exception ex)
            {
                return new byte[1];
            }

        }

        public byte[] GetFileByFilePath(string filePath)
        {
            try
            {
                var bytes = File.ReadAllBytes(filePath);
                return bytes;
            }
            catch (Exception ex)
            {
                return new byte[1];
            }
        }

        public void CreateFolder(string filepath = "")
        {
            if (!String.IsNullOrEmpty(filepath))
            {
                Directory.CreateDirectory(filepath);

            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        public void DeleteFile(string filepath)
        {
            File.Delete(filepath);
        }

        public void DeleteAllFiles()
        {
            //File dir = new System.IO.File(path);
            //var files = dir.ListFiles();
            //foreach (var item in files)
            //{
            //    DeleteFile(item.AbsolutePath);
            //}
        }
    }

}