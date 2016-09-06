using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FronEnum
    {
        public string EnumType { get; set; }

        public string Name { get; set; }

        public long Value { get; set; }
    }

    public class FronEnumPlusValue : FronEnum
    {
        public FronEnumPlusValue()
        {
        }

        public FronEnumPlusValue(FronEnum fronEnum, long numer)
        {
            EnumType = fronEnum.EnumType;
            Name = fronEnum.Name;
            Value = fronEnum.Value;
            Number = numer;
        }

        public long Number { get; set; }
    }
}