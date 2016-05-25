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

    public class ControllerFilterCriteria : IFilterCriteria
    {
        #region Fields

        private readonly string _controllerName;

        #endregion

        #region Constructor

        /// <summary>
        /// Filter by specified controller
        /// </summary>
        /// <param name="controllerName">Name of the controller</param>
        public ControllerFilterCriteria(Type controllerType)
        {
            var typeName = controllerType.Name;

            if (typeName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                _controllerName = typeName.Substring(0, typeName.Length - "Controller".Length);
            }

            _controllerName = typeName;
        }

        #endregion

        #region Implementation of IActionFilterCriteria

        public bool Match(FilterProviderContext context)
        {
            return string.Equals(_controllerName, context.ActionContext.RouteData.GetRequiredString("controller"), StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
