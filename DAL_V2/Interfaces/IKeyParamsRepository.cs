using DAL_V2.Entity;

namespace DAL_V2.Interfaces
{
    public interface IKeyParamsRepository : IBaseRepository<KeyParams>
    {
        public Task<IEnumerable<KeyParams>> SelectIncludeWords();
    }
}
