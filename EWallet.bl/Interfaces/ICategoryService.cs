using EWallet.data;
using EWallet.viewModels;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace EWallet.bl
{
    public interface ICategoryService
    {
        void CreateCategory(CategoryViewModel category, IPrincipal user);
        CategoryViewModel GetCategory(int Id, IPrincipal user);
        void EditCategory(CategoryViewModel category, IPrincipal user);
        IEnumerable<CategoryViewModel> GetAllCategories(IPrincipal user);
        IEnumerable<CategoryViewModel> GetCategories(OperationType categoryType, IPrincipal user);
        void DeleteCategory(int id, IPrincipal user);
    }
}
