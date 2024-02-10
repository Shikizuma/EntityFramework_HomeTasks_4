using DAL_V2.Entity;

namespace DAL_V2.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<IEnumerable<Product>> SelectIncludeCategory();
        public Task<Product> GetByIdIncludWord(string name);
    }
}
