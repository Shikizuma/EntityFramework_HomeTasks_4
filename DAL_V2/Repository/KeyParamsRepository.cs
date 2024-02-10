using DAL_V2.Entity;
using DAL_V2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL_V2.Repository
{
    public class KeyParamsRepository : IKeyParamsRepository
    {
        private readonly EntityDatabase _context;
        public KeyParamsRepository(EntityDatabase context)
        {
            _context = context;
        }

        public async Task<bool> Create(KeyParams entity)
        {
            try
            {
                _context.KeyLink.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(KeyParams entity)
        {
            try
            {
                _context.KeyLink.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<KeyParams> GetById(Guid id)
        {
            return await _context.KeyLink.FindAsync(id);
        }

        public async Task<IEnumerable<KeyParams>> Select()
        {
            return await _context.KeyLink.ToListAsync();
        }

        public async Task<IEnumerable<KeyParams>> SelectIncludeWords()
        {
            return await _context.KeyLink.Include(kp => kp.KeyWord).ToListAsync();
        }

        public async Task<KeyParams> Update(KeyParams entity)
        {
            var existingKeyParam = await _context.KeyLink.FindAsync(entity.Id);
            var existingWord = await _context.User.FindAsync(entity.WordId);
            var existingProduct = await _context.Product.FindAsync(entity.ProductId);

            if (existingKeyParam == null || existingWord == null || existingProduct == null)
            {
                return null;
            }

            existingKeyParam.ProductId = entity.ProductId;
            existingKeyParam.WordId = entity.WordId;
            existingKeyParam.Product = entity.Product;
            existingKeyParam.KeyWord = entity.KeyWord;

            await _context.SaveChangesAsync();

            return existingKeyParam;
        }
    }
}
