#region License
// Copyright (c) Dmitry Antonenko (http://hystrix.com.ua)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://fluentfilters.codeplex.com/
#endregion

namespace FluentFilters
{
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class FilterRecord
    {
        #region Fields

        private readonly List<FilterCriteriaResult> _requireCriteria;
        private readonly List<FilterCriteriaResult> _excludeCriteria;

        #endregion

        #region ctor

        public FilterRecord(IEnumerable<FilterCriteriaResult> criteria)
        {
            _requireCriteria = new List<FilterCriteriaResult>(criteria.Where(c => c.Type == FilterCriteriaType.And));
            _excludeCriteria = new List<FilterCriteriaResult>(criteria.Where(c => c.Type == FilterCriteriaType.Not));
        }

        #endregion

        #region Public methods

        public bool Match(FilterProviderContext context)
        {
            int lastLevel = _requireCriteria.Aggregate(0, (prev, c) => c.Level > prev ? c.Level : prev);
            for (int level = 0; level <= lastLevel; level++)
            {
                // Gets criteria results by current level
                var criteria = _requireCriteria.Where(c => c.Level.Equals(level));
                if (!criteria.Count().Equals(0))
                {
                    bool match = criteria.Aggregate(true, (prev, f) => prev ? f.Criteria.Match(context) : prev);

                    // If match exit from loop
                    if (match) { break; }

                    // If not match and this last level, then return false
                    if (level.Equals(lastLevel))
                    {
                        return false;
                    }
                }
            }

            lastLevel = _excludeCriteria.Aggregate(0, (prev, c) => c.Level > prev ? c.Level : prev);
            for (int level = 0; level <= lastLevel; level++)
            {
                // Gets criteria results by current level
                var criteria = _excludeCriteria.Where(c => c.Level.Equals(level));
                if (!criteria.Count().Equals(0))
                {
                    bool exclude = criteria.Aggregate(true, (prev, f) => prev ? f.Criteria.Match(context) : prev);

                    // If match return false
                    if (exclude) { return false; }
                }
            }

            return true;
        }

        #endregion
    }
}