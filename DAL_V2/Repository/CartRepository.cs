using DAL_V2.Entity;
using DAL_V2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL_V2.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly EntityDatabase _context;
        public CartRepository(EntityDatabase context)
        {
            _context = context;
        }

        public async Task<bool> Create(Cart entity)
        {
            try
            {
                _context.Cart.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {           
                return false;
            }
        }

        public async Task<bool> Delete(Cart entity)
        {
            try
            {
                _context.Cart.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Cart> GetById(Guid id)
        {
            return await _context.Cart.FindAsync(id);
        }

        public async Task<IEnumerable<Cart>> Select()
        {
            return await _context.Cart.ToListAsync();
        }

        public async Task<Cart> Update(Cart entity)
        {
            var existingCart = await _context.Cart.FindAsync(entity.Id);
            var existingUser = await _context.User.FindAsync(entity.UserId);
            var existingProduct = await _context.Product.FindAsync(entity.ProductId);

            if (existingCart == null || existingUser == null || existingProduct == null)
            {
                return null;
            }

            existingCart.UserId = entity.UserId;
            existingCart.ProductId = entity.ProductId;
            existingCart.User = entity.User;
            existingCart.Product = entity.Product;

            await _context.SaveChangesAsync();

            return existingCart;
        }
    }
}
