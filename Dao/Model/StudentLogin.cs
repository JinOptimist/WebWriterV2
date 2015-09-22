namespace Dao.Model
{
    public class StudentLogin
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
        
        public virtual string Password { get; set; }

        public virtual StudentType Type { get; set; }
    }

    public enum StudentType
    {
        Student = 0,
        BigStudent = 1, // Стараста группы
        Boss = 99,
        
    }
}