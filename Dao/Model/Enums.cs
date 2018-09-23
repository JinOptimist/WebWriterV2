namespace Dao.Model
{
    public enum UserType
    {
        Reader = 1,
        Writer = 2,
        Moderator = 3,

        Admin = 99
    }

    public enum RequirementType
    {
        More = 1,
        MoreOrEquals = 2,
        Less = 3,
        LessOrEquals = 4,
        Exist = 5,
        NotExist = 6,
        Equals = 7,
        NotEquals = 8
    }

    public enum ChangeType
    {
        Add = 1,
        Reduce = 2,
        //Create = 3,
        Remove = 4,
        Set = 5,
    }

    public enum StateBasicType
    {
        Number = 1,
        Text = 2,
        Boolean = 3,
    }
}