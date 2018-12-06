namespace Dal.Model
{
    public interface IUpdatable<T> where T: BaseModel
    {
        void UpdateFrom(T model);
    }
}