namespace Sales.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using interfaces;
    using SQLite;
    using Xamarin.Forms;

    public class DataService
    {
        private SQLiteAsyncConnection connection;

        public DataService()
        {
            this.OpenOrCreateDB();
        }

        private async Task OpenOrCreateDB()
        {

            //  esta linea  dice que depende  del servicio  yo  traere  la conexion   es decir  si es  android  o ios
            var databasePath = DependencyService.Get<IPathService>().GetDatabasePath();
            this.connection = new SQLiteAsyncConnection(databasePath);
             //  a partir del modelo  crea la tabla en base  de datos
            await connection.CreateTableAsync<Product>().ConfigureAwait(false);
        }

        public async Task Insert<T>(T model)
        {
            await this.connection.InsertAsync(model);
        }

        public async Task Insert<T>(List<T> models)
        {
            await this.connection.InsertAllAsync(models);
        }

        public async Task Update<T>(T model)
        {
            await this.connection.UpdateAsync(model);
        }

        public async Task Update<T>(List<T> models)
        {
            await this.connection.UpdateAllAsync(models);
        }

        public async Task Delete<T>(T model)
        {
            await this.connection.DeleteAsync(model);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var query = await this.connection.QueryAsync<Product>("select * from [Product]");
            var array = query.ToArray();
            var list = array.Select(p => new Product
            {
                Description = p.Description,
                ImagePath = p.ImagePath,
                IsVariable = p.IsVariable,
                Price = p.Price,
                ProductId = p.ProductId,
                PublishOn = p.PublishOn,
                Remarks = p.Remarks,
            }).ToList();
            return list;
        }

        public async Task DeleteAllProducts()
        {
            var query = await this.connection.QueryAsync<Product>("delete from [Product]");
        }
    }
}
