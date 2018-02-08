using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontEnum
    {
        public FrontEnum()
        {
        }

        public FrontEnum(object enumValue)
        {
            if (enumValue == null)
                return;
            var type = enumValue.GetType();
            if (!type.IsEnum)
                throw new ArgumentException("FrontEnum only for a Enum");

            EnumType = type.FullName;
            Name = Enum.GetName(type, enumValue);
            Value = (int)Enum.ToObject(type, enumValue);
        }

        public string EnumType { get; set; }

        public string Name { get; set; }

        public long Value { get; set; }
    }

    public class FronEnumPlusValue : FrontEnum
    {
        public FronEnumPlusValue()
        {
        }

        public FronEnumPlusValue(FrontEnum fronEnum, long numer)
        {
            EnumType = fronEnum.EnumType;
            Name = fronEnum.Name;
            Value = fronEnum.Value;
            Number = numer;
        }

        public long Number { get; set; }
    }
}