using BLL.Entity;
using BLL.Interfaces.Repository;
using BLL.Interfaces.Services;
using System.Linq;
using System.Xml.Linq;

namespace BLL.Services
{
    public class ProductServices : IProductServices
    {
        IProductRepository _productRepository;
        IKeyParamsRepository _keyParams;
        public ProductServices(IProductRepository userRepository, IKeyParamsRepository keyParams)
        {
            _productRepository = userRepository;
            _keyParams = keyParams;
        }
        public async Task<bool> CreateProduct(Product entity)
        {
            await _productRepository.Create(entity);
            return true;
        }

        public async Task<bool> DeleteProduct(Product entity)
        {
            if (await _productRepository.GetById(entity.Id) == null)
            {
                return false;
            }

            await _productRepository.Delete(entity);
            return true;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task<IEnumerable<Product>> AllProducts()
        {
            return await _productRepository.Select();
        }

        public async Task<Product> UpdateProduct(Product entity)
        {
            return await _productRepository.Update(entity);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPrice(string category, int max, int min)
        {
            var products = (await _productRepository.Select()).Where(p => p.Category.Name == category && (min <= p.Price && p.Price >= max));
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndKeyWordsWithPrice(string category, string[] keywords, int max, int min)
        {
            var products = (await _productRepository.Select()).Where(p => p.Category.Name == category);

            var filteredProducts = products.Where(p => keywords.Any(kw => p.KeyWords.Any(k => k.Select().Result.Any(w => w.KeyWord.Header == kw))));

            var result = filteredProducts.Where(p => p.Price >= min && p.Price <= max);

            return result;
        }


        public async Task<Product> GetProductByName(string name)
        {
            return (await _productRepository.Select()).First(p => p.Name == name);
        }

        public async Task<IEnumerable<Product>> ProductsByWord(string word)
        {
            var products = await _productRepository.Select();
            return products.Where(p => p.Name.Contains(word) || p.KeyWords.Any(k => k.Select().Result.Any(w => w.KeyWord.Header == word)));
        }


    }
}
