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

using System.Linq.Expressions;

namespace FluentFilters.Criteria
{
    using System;
    using FluentFilters;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ActionFilterCriteria : IFilterCriteria
    {
        #region Fields

        private readonly string _actionName;

        #endregion

        #region Properties

        public string ControllerName
        {
            get
            {
                
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Filter by specified action
        /// </summary>
        /// <param name="actionName">Name of the action</param>
        public ActionFilterCriteria(string actionName)
        {
            _actionName = actionName;
        }

        #endregion

        #region Implementation of IActionFilterCriteria

        public bool Match(FilterProviderContext context)
        {
            return string.Equals(_actionName, context.ActionContext.RouteData.GetRequiredString("action"), StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
