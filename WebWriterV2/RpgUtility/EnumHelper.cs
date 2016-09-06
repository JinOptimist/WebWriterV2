using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using WebWriterV2.FrontModels;

namespace WebWriterV2.RpgUtility
{
    public static class EnumHelper
    {
        //public static List<string> GetEnumNames<T>()
        //{
        //    var listRace = Enum.GetValues(typeof (T)).Cast<T>();
        //    var listNameValue = listRace.Select(race => Enum.GetName(typeof (T), race));
        //    return listNameValue.ToList();
        //}

        public static string GetEnumName(Type type, object value)
        {
            return Enum.GetName(type, value);
        }

        public static FronEnum GetFronEnum(Type type, long enumValue)
        {
            return new FronEnum
            {
                EnumType = type.FullName,
                Name = Enum.GetName(type, enumValue),
                Value = enumValue,
            };
        }
    }
}