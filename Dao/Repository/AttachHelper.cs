using Dao;
using Dao.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Dal.Repository
{
    static class AttachHelper
    {
        public static T AttachIfNot<T>(this T model, WriterContext db) where T : BaseModel
        {
            if (db.Entry(model).State == EntityState.Detached) {
                model = db.Set<T>().Find(model.Id);
            }

            return model;
            // db.Set<T>().Attach(model);
        }
    }
}
