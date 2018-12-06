using Dal;
using Dal.IRepository;
using Dal.Model;
using Dal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebWriterV2.DI;
using WebWriterV2.FrontModels;
using WebWriterV2.VkUtility;

namespace WebWriterV2.Controllers
{
    public class ArticleController : BaseApiController
    {
        public ArticleController(IArticleRepository articleRepository)
        {
            ArticleRepository = articleRepository;
        }

        private IArticleRepository ArticleRepository { get; set; }

        [AcceptVerbs("POST")]
        public FrontArticle Save(FrontArticle frontArticle)
        {
            var article = frontArticle.ToDbModel();
            article = ArticleRepository.Save(article);
            return new FrontArticle(article);
        }

        [AcceptVerbs("GET")]
        public FrontArticle Get(long id)
        {
            var article = ArticleRepository.Get(id);
            var frontArticle = new FrontArticle(article);
            return frontArticle;
        }

        [AcceptVerbs("GET")]
        public List<FrontArticle> GetAll()
        {
            var articles = ArticleRepository.GetAll();
            var frontArticle = articles.Select(x => new FrontArticle(x)).ToList();
            return frontArticle;
        }

        [AcceptVerbs("GET")]
        public bool Remove(long id)
        {
            if (User.UserType != UserType.Admin) {
                throw new UnauthorizedAccessException($"userId-{User.Id} try to remove article");
            }
            ArticleRepository.Remove(id);

            return true;
        }
    }
}
