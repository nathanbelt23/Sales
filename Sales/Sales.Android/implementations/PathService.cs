[assembly:Xamarin.Forms.Dependency(typeof(Sales.Droid.implementations.PathService))] 

namespace Sales.Droid.implementations
{
    using Sales.interfaces;
    using System;
    using System.IO;

    public class PathService : IPathService
    {

        public string GetDatabasePath()
        {

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, "Sales.db3");

        }
    }
}

