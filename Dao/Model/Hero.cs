namespace Dao.Model
{
    public class Hero
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string FaceImgUrl { get; set; }

        public virtual string FullImgUrl { get; set; } 
    }
}