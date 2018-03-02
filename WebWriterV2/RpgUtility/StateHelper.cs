using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using Dao.DataGeneration;

namespace WebWriterV2.RpgUtility
{
    public static class StateHelper
    {
        public static List<ChapterLinkItem> FilterLink(this Travel travel, List<ChapterLinkItem> linkItems)
        {
            var result = new List<ChapterLinkItem>();

            foreach (var linkItem in linkItems) {
                if (!linkItem.StateRequirement.Any()) {
                    result.Add(linkItem);
                    continue;
                } else {

                    var condition = linkItem.StateRequirement.SingleOrDefault()?.Text;
                    if (travel.State?.Any(x => x.Text == condition) ?? false) {
                        result.Add(linkItem);
                    }
                }

                
            }

            return result;
        }
    }
}