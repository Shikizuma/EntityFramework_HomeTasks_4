using DAL_V2.Entity;
using DAL_V2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL_V2.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EntityDatabase _context;
        public CategoryRepository(EntityDatabase context)
        {
            _context = context;
        }


        public async Task<bool> Create(Category entity)
        {
            try
            {
                _context.Category.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Category entity)
        {
            try
            {
                _context.Category.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Category> GetById(Guid id)
        {
            return await _context.Category.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> Select()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<IEnumerable<Category>> SelectIncludeProducts()
        {
            return await _context.Category.Include(p => p.Products).ToListAsync();
        }

        public async Task<Category> Update(Category entity)
        {
            var existingCategory = await _context.Category.FindAsync(entity.Id);

            if (existingCategory == null)
            {
                return null;
            }       

            existingCategory.Name = entity.Name;
            existingCategory.Icon = entity.Icon;
            existingCategory.Products = entity.Products;


            await _context.SaveChangesAsync();

            return existingCategory;
        }
    }
}
