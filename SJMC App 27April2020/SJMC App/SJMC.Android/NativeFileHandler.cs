using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SJMC.Droid;
using SJMC.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeFileHandler))]
namespace SJMC.Droid
{
   
    public class NativeFileHandler : INativeFileHandler
    {
        public string path { get; set; }
        public NativeFileHandler()
        {
            path = Xamarin.Essentials.FileSystem.AppDataDirectory;


            path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/SJMC";


            CreateFolder();

        }
        public bool SaveFile(string filename, byte[] fileData)
        {
            try
            {
                //string directoryName = Path.GetDirectoryName(fileNameAndPath);
                //string filename = Path.GetFileName(fileNameAndPath);
                //  var   directoryName = Path.Combine(_path, filename);

                CreateFolder(path);
                var dir = new Java.IO.File(path);
                string filePath = System.IO.Path.Combine(dir.Path, filename);
                File.WriteAllBytes(filePath, fileData);
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        public string GetText(string fileName)
        {
          return  File.ReadAllText($"{path}/{fileName}");
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
                // var filePath = Path.Combine(_path, filename);
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
            Java.IO.File dir;
            if (!String.IsNullOrEmpty(filepath))
            {
                dir = new Java.IO.File(filepath);
            }
            else
            {
                dir = new Java.IO.File(path);
            }

            if (!dir.Exists())
                dir.Mkdirs();
        }

        public void DeleteFile(string filepath)
        {
            Java.IO.File dir;
            dir = new Java.IO.File(filepath);

            if (dir.Exists())
                dir.Delete();
        }

        public void DeleteAllFiles()
        {
            Java.IO.File dir;
            dir = new Java.IO.File(path);
            var files = dir.ListFiles();
            foreach (var item in files)
            {
                DeleteFile(item.AbsolutePath);
            }


        }

    }

}