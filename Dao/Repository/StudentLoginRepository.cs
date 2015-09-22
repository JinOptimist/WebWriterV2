using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac.Features.ResolveAnything;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class StudentLoginRepository : IStudentLoginRepository
    {
        private readonly WriterContext _db = new WriterContext();

        public StudentLogin GetStudent(long id)
        {
            return _db.StudentLogin.SingleOrDefault(x => x.Id == id);
        }

        public List<StudentLogin> GetAllStudents(int maxResult = 10)
        {
            return _db.StudentLogin.Take(maxResult).ToList();
        }

        public bool SaveStudent(StudentLogin student)
        {
            lock (_db)
            {
                if (student.Id > 0)
                {
                    _db.StudentLogin.Attach(student);
                    _db.Entry(student).State = EntityState.Modified;
                    _db.SaveChanges();
                    return true;
                }

                if (IsExist(student.Name))
                {
                    return false;
                }

                _db.StudentLogin.Add(student);
                _db.SaveChanges();
                return true;
            }
        }

        public StudentLogin Login(string name, string pass)
        {
            return _db.StudentLogin.SingleOrDefault(x => name.Equals(x.Name) && pass.Equals(x.Password));
        }

        public bool IsExist(string name)
        {
            return _db.StudentLogin.Any(x => name.Equals(x.Name));
        }
    }
}