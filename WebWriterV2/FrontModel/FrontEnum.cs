using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Model;
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

    public class FrontEnumChangeType : FrontEnum
    {
        public FrontEnumChangeType() { }

        public FrontEnumChangeType(object enumValue) : base(enumValue)
        {
            if (EnumType != typeof(ChangeType).FullName)
                throw new Exception("FrontEnumChangeType only for ChangeType enum");
            Name = ApplyLocalizationForName();
        }

        public string ApplyLocalizationForName()
        {
            ChangeType changeType = (ChangeType)Value;
            switch (changeType)
            {
                case ChangeType.Add:
                    {
                        return Localization.MainRu.Writer_ChangeType_Add;
                    }
                case ChangeType.Reduce:
                    {
                        return Localization.MainRu.Writer_ChangeType_Reduce;
                    }
                case ChangeType.Remove:
                    {
                        return Localization.MainRu.Writer_ChangeType_Remove;
                    }
                case ChangeType.Set:
                    {
                        return Localization.MainRu.Writer_ChangeType_Set;
                    }
            }

            throw new Exception($"Unkow changeType {changeType}");
        }
    }

    public class FrontEnumRequirementType : FrontEnum
    {
        public FrontEnumRequirementType() { }

        public FrontEnumRequirementType(object enumValue) : base(enumValue)
        {
            if (EnumType != typeof(RequirementType).FullName)
                throw new Exception("FrontEnumRequirementType only for RequirementType enum");
            Name = ApplyLocalizationForName();
        }

        public string ApplyLocalizationForName()
        {
            RequirementType changeType = (RequirementType)Value;
            switch (changeType)
            {
                case RequirementType.More:
                    {
                        return Localization.MainRu.Writer_RequirementType_More;
                    }
                case RequirementType.MoreOrEquals:
                    {
                        return Localization.MainRu.Writer_RequirementType_MoreOrEquals;
                    }
                case RequirementType.Less:
                    {
                        return Localization.MainRu.Writer_RequirementType_Less;
                    }
                case RequirementType.LessOrEquals:
                    {
                        return Localization.MainRu.Writer_RequirementType_LessOrEquals;
                    }
                case RequirementType.Exist:
                    {
                        return Localization.MainRu.Writer_RequirementType_Exist;
                    }
                case RequirementType.NotExist:
                    {
                        return Localization.MainRu.Writer_RequirementType_NotExist;
                    }
                case RequirementType.Equals:
                    {
                        return Localization.MainRu.Writer_RequirementType_Equals;
                    }
                case RequirementType.NotEquals:
                    {
                        return Localization.MainRu.Writer_RequirementType_NotEquals;
                    }

            }

            throw new Exception($"Unkow changeType {changeType}");
        }
    }

    public class FrontEnumStateBasicType : FrontEnum
    {
        public FrontEnumStateBasicType() { }

        public FrontEnumStateBasicType(object enumValue) : base(enumValue)
        {
            if (EnumType != typeof(StateBasicType).FullName)
                throw new Exception("FrontEnumStateBasicType only for StateBasicType enum");
            Name = ApplyLocalizationForName();
        }

        public string ApplyLocalizationForName()
        {
            StateBasicType basicType = (StateBasicType)Value;
            switch (basicType)
            {
                case StateBasicType.Number:
                    {
                        return Localization.MainRu.Writer_BasicType_Number;
                    }
                case StateBasicType.Text:
                    {
                        return Localization.MainRu.Writer_BasicType_Text;
                    }
                case StateBasicType.Boolean:
                    {
                        return Localization.MainRu.Writer_BasicType_Boolean;
                    }
            }

            throw new Exception($"Unkow changeType {basicType}");
        }
    }
}