using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class TravelStep : BaseModel
    {
        public virtual Travel Travel { get; set; }
        public virtual Travel TravelForCurrentStep { get; set; }

        /// <summary>
        /// Chpater where user be after choice
        /// </summary>
        public virtual Chapter CurrentChapter { get; set; }

        public virtual DateTime DateTime { get; set; }

        public virtual ChapterLinkItem Choice { get; set; }

        public virtual TravelStep NextStep { get;set; }
        public virtual TravelStep PrevStep { get; set; }
    }
}
