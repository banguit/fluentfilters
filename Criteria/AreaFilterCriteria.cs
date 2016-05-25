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

namespace FluentFilters.Criteria
{
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class AreaFilterCriteria : IFilterCriteria
    {
        #region Fields

        /// <summary>
        /// Name of the area
        /// </summary>
        private readonly string _areaName;

        #endregion

        #region Constructor

        /// <summary>
        /// Filter by root area
        /// </summary>
        public AreaFilterCriteria()
        {
            _areaName = string.Empty;
        }

        /// <summary>
        /// Filter for specified area
        /// </summary>
        /// <param name="areaName">Name of the area</param>
        public AreaFilterCriteria(string areaName)
        {
            _areaName = areaName;
        }

        #endregion

        #region Implementation of IActionFilterCriteria

        public bool Match(FilterProviderContext context)
        {
            var currentArea = context.ActionContext.RouteData.DataTokens["area"];

            // Apply if root area
            if (currentArea == null && string.IsNullOrEmpty(_areaName))
            {
                return true;
            }

            // Apply for specified area
            return currentArea != null && string.Equals(currentArea.ToString(), _areaName, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
