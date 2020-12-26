

[assembly:Xamarin.Forms.Dependency (typeof(Sales.iOS.Implementations.PathService))]
namespace Sales.iOS.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Foundation;
    using Sales.interfaces;
    using UIKit;

    public class PathService : IPathService
    {
        public string GetDatabasePath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, "Sales.db3");
        }
    }
}