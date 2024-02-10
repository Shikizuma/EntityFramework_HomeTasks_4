using DAL_V2.Entity;

namespace DAL_V2.Interfaces
{
    public interface IWordRepository : IBaseRepository<Word>
    {
        public Task<IEnumerable<Word>> SelectIncludeKeyParamsProducts();
    }
}
