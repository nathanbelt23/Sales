namespace Sales.Domain.Models
{
    
    using System.Data.Entity;
        public class DataContext:DbContext
    {

        public DataContext() : base("DefaultConnection")//Se pasa  el parametro  segun el web config de backe, base se utiliza por
            // habia heredado  de dbcontext
        {

        }

        public System.Data.Entity.DbSet<Sales.Common.Models.Product> Products { get; set; }

    
    }
}
