using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class TravelStep : BaseModel
    {
        public virtual Travel Travel { get; set; }

        public virtual DateTime DateTime { get; set; }

        public virtual ChapterLinkItem Сhoice { get; set; }
    }
}
