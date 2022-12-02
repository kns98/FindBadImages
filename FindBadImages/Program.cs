using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Image = System.Drawing.Image;

namespace MovePics
{
    class Program
    {
        private static Regex r = new Regex(":");
        //retrieves the datetime WITHOUT loading the whole image
        public static DateTime? GetDateTakenFromImage(string path)
        {
            try
            {
                if ((path.ToLower().EndsWith(".jpg")) || (path.ToLower().EndsWith(".jpeg")))
                {
                    try
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        using (Image myImage = Image.FromStream(fs, false, false))
                        {
                            PropertyItem propItem;
                            if (myImage.PropertyIdList.Contains(36867))
                            {
                                propItem = myImage.GetPropertyItem(36867);
                                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                                return DateTime.Parse(dateTaken);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        File.Move(path, path + ".corrupt.jpg");
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }

        public static void DirSearch_ex3(string sDir, List<FileInfo> files)
        {
            //Console.WriteLine("DirSearch..(" + sDir + ")");
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {

                    files.Add(new FileInfo(f));

                }

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    DirSearch_ex3(d, files);
                }
            }
            catch (System.Exception excpt)
            {
                //Console.WriteLine(excpt.Message);
            }

        }


        static void Main(string[] args)

        {
            var folder = @"D:\OneDrive\Pictures";
            DirectoryInfo d = new DirectoryInfo(folder);//Assuming Test is your Folder

            List<FileInfo> files = new List<FileInfo>();

            DirSearch_ex3(folder, files);

            foreach (FileInfo file in files.ToArray())
            {
                var fd = GetDateTakenFromImage(file.FullName);
            }

        }
    }
}

