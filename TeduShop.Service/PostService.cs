using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;
namespace TeduShop.Service
{
    public interface IPostService
    {
        void Add(Post post);
        void Update(Post post);
        void Delete(int id);
        IEnumerable<Post> GetAll(); //Lay danh
        IEnumerable<Post> GetAllPaging(int page,int pageSize,out int totalRow);
        IEnumerable<Post> GetAllByCategoryPaging(int categoryId,int page, int pageSize, out int totalRow);
        Post GetById(int id);
        IEnumerable<Post> GetAllByTagPaging(string tag,int page, int pageSize, out int totalRow);
        void SaveChange();
    }

    public class PostService : IPostService
    {
        IPostRepository _posRepository;
        IUnitOfWork _unitOfWork;
        public PostService(IPostRepository postRepository,IUnitOfWork unitOfWork)
        {
            this._posRepository = postRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Add(Post post)
        {
            _posRepository.Add(post);
        }

        public void Delete(int id)
        {
            _posRepository.Delete(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _posRepository.GetAll(new string[] { "PostCategory"});
        }

        public IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _posRepository.GetMultiPaging(x => x.Status && x.CategoryID == categoryId, out totalRow, page, pageSize,new string[] {"PostCategory" });
        }

        public IEnumerable<Post> GetAllByTagPaging(string tag,int page, int pageSize, out int totalRow)
        {
            //TODO: sELECT all post by tag
            return _posRepository.GetAllByTag(tag,page,pageSize,out totalRow);
        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _posRepository.GetMultiPaging(x => x.Status, out totalRow, page, pageSize);
         }

        public IEnumerable<Post> GetAllPaging(string tag, int page, int pageSize, out int totalRow)
        {
            throw new NotImplementedException();
        }

        public Post GetById(int id)
        {
            return _posRepository.GetSingleById(id);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit(); //Commit 1 cai thi no se select vao trong db
        }

        public void Update(Post post)
        {
            _posRepository.Update(post);
        }
    }

}
