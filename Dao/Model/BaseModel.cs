namespace Dao.Model
{
    public abstract class BaseModel //: IUpdatable
    {
        public long Id { get; set; }

        //TODO  Do I need this method in general model?
        //public abstract void Update(BaseModel model);
    }
}