using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using System.Collections.Generic;
using System.Linq;

namespace SearchEpiFind.Business.Search.Util
   
{
    public static class CategoryPath
    {

#pragma warning disable 649
        private static Injected<CategoryRepository> _categoryRepository;
#pragma warning restore 649

        /// <summary>
        /// Recursively build category paths
        /// </summary>
        /// <param name="root"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static List<string> CreateCategoryPath(Category root, int categoryId)
        {
            var paths = new List<string>();

            var category = FindCategory(root, categoryId);

            if (category == null)
                return paths;

            paths.Add(category.Name);

            if (category.Parent != null)
            {
                var parentPaths = CreateCategoryPath(root, category.Parent.ID);

                if (parentPaths.Any())
                    paths.InsertRange(0, parentPaths);
            }

            return paths;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static List<string> CreateCategoryPath(Category root, string categoryId)
        {
            int id;

            int.TryParse(categoryId, out id);

            return CreateCategoryPath(root, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static string CreateCategoryPathStr(Category root, string categoryId)
        {
            int id;

            int.TryParse(categoryId, out id);

            return string.Join("~", CreateCategoryPath(root, id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Category GetCategoryRoot()
        {
            return _categoryRepository.Service.GetRoot();
        }


        /// <summary>
        /// Recursive method to find category in categorylist
        /// </summary>
        /// <param name="root"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static Category FindCategory(Category root, int categoryId)
        {
            var category = root.Categories.FirstOrDefault(c => c.ID == categoryId);

            if (category != null)
                return category;

            foreach (var subroot in root.Categories)
            {
                var c = FindCategory(subroot, categoryId);

                if (c != null)
                {
                    return c;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static string GetCategoryName(int categoryId)
        {
            var category = _categoryRepository.Service.Get(categoryId);

            return (category == null) ? "" : category.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryList"></param>
        /// <returns></returns>
        public static IList<string> GetCategoryNames(this CategoryList categoryList)
        {
            if (categoryList == null)
                return new List<string>();

            return categoryList.Select(x => _categoryRepository.Service.Get(x))
                .Where(x => x != null)
                .Select(x => x.Name)
                .ToList();
        }

        /// <summary>
        /// Generates a list of category paths
        /// </summary>
        /// <param name="categoryList"></param>
        /// <returns></returns>
        public static List<string> GetCategoryPaths(this CategoryList categoryList)
        {
            var categories = new List<string>();

            if (categoryList == null)
                return categories;
            
            var rootCategory = _categoryRepository.Service.GetRoot();

            if (categoryList.Any())
            {
                foreach (var categoryId in categoryList)
                {
                    var categoryPaths = CreateCategoryPath(rootCategory, categoryId);

                    var path = string.Empty;

                    foreach (var category in categoryPaths)
                    {
                        if (path == string.Empty)
                        {
                            path = category;
                        }
                        else
                        {
                            path = string.Format("{0}~{1}", path, category);
                        }

                        categories.Add(path);
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryList"></param>
        /// <returns></returns>
        public static IEnumerable<Category> GetCategoryListData(this CategoryList categoryList)
        {
            if (categoryList == null)
                return Enumerable.Empty<Category>();

            return categoryList
                .Select(x => _categoryRepository.Service.Get(x))
                .Where(x => x != null);
        }


        public static string GetTermFromPath(this string term)
        {
            return term.Contains('~') ? term.Split('~').Last().Trim() : term.Trim();
        }
    }
}