using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IStudentLoginRepository
    {
        StudentLogin GetStudent(long id);

        List<StudentLogin> GetAllStudents(int maxResult = 10);

        bool SaveStudent(StudentLogin student);

        StudentLogin Login(string name, string pass);

        bool IsExist(string name);
    }
}