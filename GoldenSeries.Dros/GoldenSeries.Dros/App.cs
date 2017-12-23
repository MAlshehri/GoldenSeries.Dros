using System;
using System.IO;
using GoldenSeries.Dros.Helpers;
using GoldenSeries.Dros.Models;
using GoldenSeries.Dros.Services;

namespace GoldenSeries.Dros
{
    public class App
    {
        public static string DatabaseFilePath
        {
            get
            {
                //#if __ANDROID__
                //string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
                //#else

                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine(documentsPath, "..", "Library");
                //#endif

                var path = Path.Combine(libraryPath, "dros.db");
                return path;
            }
        }

        public static void Initialize()
        {
            using (var source = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GoldenSeries.Dros.dros.db"))
            {
                if (!File.Exists(DatabaseFilePath))
                {
                    using (var destination = File.Create(DatabaseFilePath))
                    {
                        source.CopyTo(destination);
                    }
                }
            }

            SQLitePCL.Batteries_V2.Init();
        }
    }
}
