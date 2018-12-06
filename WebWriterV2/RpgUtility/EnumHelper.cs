using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Model;
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

        public static List<FrontEnum> GetFrontEnumList(Type type)
        {
            return GetFrontEnumList<FrontEnum>(type);
        }

        public static List<FrontEnum> GetFrontEnumList<T>(Type type) where T: FrontEnum
        {
            if (!type.IsEnum)
                throw new ArgumentException("For a Enum only");

            var list = new List<FrontEnum>();
            foreach (var requirementType in Enum.GetValues(type))
            {
                var frontEnum = (T)Activator.CreateInstance(typeof(T), requirementType);
                list.Add(frontEnum);
            }

            return list;
        }

        public static List<ChangeType> FilterChangeTypeByBasicType(StateBasicType basicType)
        {
            var result = new List<ChangeType>();
            var types = Enum.GetValues(typeof(ChangeType));

            foreach (ChangeType changeType in types) {
                switch (changeType) {
                    case ChangeType.Add:
                    case ChangeType.Reduce: {
                            if (basicType == StateBasicType.Number) {
                                result.Add(changeType);
                            }
                            break;
                        }
                    case ChangeType.Remove:
                    case ChangeType.Set: {
                            result.Add(changeType);
                            break;
                        }
                }
            }

            return result;
        }

        public static List<RequirementType> FilterRequirementTypeByBasicType(StateBasicType basicType)
        {
            var result = new List<RequirementType>();
            var types = Enum.GetValues(typeof(RequirementType));

            foreach (RequirementType changeType in types) {
                switch (changeType) {
                    case RequirementType.More:
                    case RequirementType.MoreOrEquals:
                    case RequirementType.Less:
                    case RequirementType.LessOrEquals: {
                            if (basicType == StateBasicType.Number) {
                                result.Add(changeType);
                            }
                            break;
                        }
                    case RequirementType.Exist:
                    case RequirementType.NotExist:
                    case RequirementType.Equals:
                    case RequirementType.NotEquals: {
                            result.Add(changeType);
                            break;
                        }
                }
            }

            return result;
        }
    }
}