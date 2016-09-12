using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public abstract class BaseFront<T> where T : BaseModel
    {
        public long Id { get; set; }

        public abstract T ToDbModel();
    }
}