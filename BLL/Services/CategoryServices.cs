using BLL.Entity;
using BLL.Interfaces.Repository;
using BLL.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BLL.Services
{
    public class CategoryServices : ICategoryServices
    {
        ICategoryRepository _categoryRepository;
        IKeyParamsRepository _keyParamsRepository;
        public CategoryServices(ICategoryRepository userRepository, IKeyParamsRepository productRepository)
        {
            _categoryRepository = userRepository;
            _keyParamsRepository = productRepository;
        }
        public async Task<bool> CreateCategory(Category entity)
        {
            await _categoryRepository.Create(entity);
            return true;
        }

        public async Task<bool> DeleteCategory(Category entity)
        {
            if (await _categoryRepository.GetById(entity.Id) == null)
            {
                return false;
            }

            await _categoryRepository.Delete(entity);
            return true;
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            var existingCategory = await _categoryRepository.GetById(id);
            if (existingCategory == null)
            {
                return null;
            }

            return existingCategory;
        }
        public async Task<Category> GetCategoryByName(string category)
        {
            var existingCategory = (await _categoryRepository.Select()).FirstOrDefault(eC => eC.Name == category);
            if (existingCategory == default)
            {
                return null;
            }

            return existingCategory;
        }

        public async Task<IEnumerable<Category>> AllCategories()
        {
            return await _categoryRepository.Select();
        }

        public async Task<Category> UpdateCategory(Category entity)
        {
            var existingCategory = await _categoryRepository.GetById(entity.Id);
            if (existingCategory == null)
            {
                return null;
            }

            await _categoryRepository.Update(entity);
            return entity;
        }

        public async Task<CategoryInfo> GetCategoryInfoByName(string category)
        {
   
            var categoryData = _categoryRepository.Select().Result.FirstOrDefault(c => c.Name == category);
            if (categoryData == null)
            {
                throw new Exception("Category not found");
            }

            var products = categoryData.Products;

            double maxPrice = products.Max(p => p.Price);
            double minPrice = products.Min(p => p.Price);

            var selections = new Dictionary<string, List<string>>();
            var keyParams = await _keyParamsRepository.SelectIncludeWords();
            foreach (var product in products)
            {
                var productKeyParams = keyParams.Where(kp => kp.Product.Id == product.Id);
                foreach (var keyParam in productKeyParams)
                {
                    if (!selections.ContainsKey(keyParam.KeyWords.Header))
                    {
                        selections[keyParam.KeyWords.Header] = new List<string>();
                    }
                    selections[keyParam.KeyWords.Header].Add(keyParam.KeyWords.KeyWord);
                }
            }

            var categoryInfo = new CategoryInfo
            {
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                CategoryName = categoryData.Name,
                Selections = selections.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray())
            };

            return categoryInfo;
        }


    }
}
