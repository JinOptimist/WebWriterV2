using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontArticle : BaseFront<Article>
    {
        public FrontArticle()
        {
        }

        public FrontArticle(Article article)
        {
            Id = article.Id;
            Name = article.Name;
            ShortDesc = article.ShortDesc;
            Desc = article.Desc;
            HtmlDesc = WordHelper.GenerateHtmlForDesc(article.Desc);
            IsPublished = article.IsPublished;
        }

        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string Desc { get; set; }
        public string HtmlDesc { get; set; }
        public bool IsPublished { get; set; }

        public override Article ToDbModel()
        {
            return new Article {
                Id = Id,
                Name = Name,
                ShortDesc = ShortDesc,
                Desc = Desc,
                IsPublished = IsPublished,
                DateCreate = DateTime.Now
            };
        }
    }
}
