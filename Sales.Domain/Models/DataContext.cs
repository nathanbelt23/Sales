namespace Sales.Domain.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Sales.Common.Models;

    public class DataContext:DbContext
    {

        public DataContext() : base("DefaultConnection")//Se pasa  el parametro  segun el web config de backe, base se utiliza por
            // habia heredado  de dbcontext
        {

        }

        public System.Data.Entity.DbSet<Sales.Common.Models.Product> Products { get; set; }

        /*
        public IQueryable<Product> Products()
        {
            throw new NotImplementedException();
        }*/
    }
}
