namespace Dao.Model
{
    public enum RequirementType
    {
        More = 1,
        MoreOrEquals = 2,
        Less = 3,
        LessOrEquals = 4,
        Exist = 5,
        NotExist = 6,
        Equals = 7
    }

    public enum Race
    {
        None = 0,
        Человек = 1,
        Эльф = 2,
        Орк = 3,
        Гном = 4,
        Дракон = 5,
    }

    public enum Sex
    {
        None = 0,
        Муж = 1,
        Жен = 2,
        Скрывает = 3
    }
}