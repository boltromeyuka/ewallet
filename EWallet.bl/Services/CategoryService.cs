using EWallet.data;
using EWallet.viewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System;

namespace EWallet.bl
{
    public class CategoryService : ICategoryService
    {
        IUnitOfWork Database { get; set; }

        public CategoryService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void CreateCategory(CategoryViewModel category, IPrincipal user)
        {
            try
            {
                var create = new Category
                {
                    Name = category.Name,
                    CategoryType = category.CategoryType,
                    UserId = user.Identity.GetUserId()
                };

                Database.Categories.Create(create);
                Database.Save();
            }
            catch(Exception e)
            {
                throw new Exception(String.Format("При добавлении категории возникла ошибка: {0}", e.InnerException != null ? e.Message + " " + e.InnerException : e.Message));
            }
        }

        public void EditCategory(CategoryViewModel category, IPrincipal user)
        {
            var edit = Database.Categories.Find(x=>x.UserId==user.Identity.GetUserId()).FirstOrDefault(x => x.Id == category.Id);

            try
            {
                edit.Name = category.Name;

                Database.Categories.Update(edit);
                Database.Save();
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("При изменении категории возникла ошибка: {0}", e.InnerException != null ? e.Message + " " + e.InnerException : e.Message));
            }           
        }

        public void DeleteCategory(int id, IPrincipal user)
        {
            var delete = Database.Categories.Find(x => x.UserId == user.Identity.GetUserId()).FirstOrDefault(x => x.Id == id);

            try
            {
                delete.IsArchive = true;

                Database.Categories.Update(delete);
                Database.Save();
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("При удалении категории возникла ошибка: {0}", e.InnerException != null ? e.Message + " " + e.InnerException : e.Message));
            }
        }


        /// <summary>
        /// Get all categories of user by type
        /// </summary>
        /// <param name="categoryType"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<CategoryViewModel> GetCategories(OperationType categoryType, IPrincipal user)
        {
            return Database.Categories
                                .Find(x =>!x.IsArchive && x.CategoryType == categoryType && x.UserId == user.Identity.GetUserId())
                                                                    .Select(x => new CategoryViewModel {
                                                                                                    Id = x.Id,
                                                                                                    CategoryType = x.CategoryType,
                                                                                                    Name = x.Name
                                                                                                });
        }



        public CategoryViewModel GetCategory(int Id, IPrincipal user)
        {
            var current = Database.Categories.Find(x=> !x.IsArchive && x.UserId==user.Identity.GetUserId()).FirstOrDefault(x=>x.Id==Id);

            if (current != null)
            {
                return new CategoryViewModel
                {
                    Id = current.Id,
                    Name = current.Name,
                    CategoryType = current.CategoryType
                };
            }

            throw new Exception("Категория не найдена");
        }

        public IEnumerable<CategoryViewModel> GetAllCategories(IPrincipal user)
        {
            return Database.Categories
                                .Find(x => !x.IsArchive && x.UserId == user.Identity.GetUserId())
                                                                    .Select(x => new CategoryViewModel
                                                                    {
                                                                        Id = x.Id,
                                                                        CategoryType = x.CategoryType,
                                                                        Name = x.Name
                                                                    });
        }
    }
}
