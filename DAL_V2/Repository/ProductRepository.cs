using DAL_V2.Entity;
using DAL_V2.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_V2.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly EntityDatabase _context;
        public ProductRepository(EntityDatabase context)
        {
            _context = context;
        }
        public async Task<bool> Create(Product entity)
        {
            try
            {
                _context.Product.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Product entity)
        {
            try
            {
                _context.Product.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _context.Product.FindAsync(id);
        }

        public async Task<Product> GetByIdIncludWord(string name)
        {
            return await _context.Product.Include(p => p.KeyWords).ThenInclude(k => k.KeyWord).FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Product>> Select()
        {
            return await _context.Product.ToListAsync();
        }
        public async Task<IEnumerable<Product>> SelectIncludeCategory()
        {
            return await _context.Product.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> Update(Product entity)
        {
            var existingProduct = await _context.Product.FindAsync(entity.Id);
            var existingCategory = await _context.Product.FindAsync(entity.CategoryId);

            if (existingProduct == null || existingCategory == null)
            {
                return null;
            }

            existingProduct.CategoryId = entity.CategoryId;
            existingProduct.Name = entity.Name;
            existingProduct.Description = entity.Description;
            existingProduct.Price = entity.Price;
            existingProduct.ActionPrice = entity.ActionPrice;
            existingProduct.ImageUrl = entity.ImageUrl;
            existingProduct.Category = entity.Category;
            existingProduct.KeyWords = entity.KeyWords;

            await _context.SaveChangesAsync();

            return existingProduct;
        }
    }
}
