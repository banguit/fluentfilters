﻿#region License
// Copyright (c) Dmytro Antonenko (http://hedgehog.com.ua)
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
// The latest version of this file can be found at https://github.com/banguit/fluentfilters/
#endregion

namespace FluentFilters
{
    public interface IRequireResult
    {
        /// <summary>
        /// Add required criteria
        /// </summary>
        /// <param name="criteria">The criteria</param>
        IRequireResult And(IFilterCriteria criteria);

        /// <summary>
        /// Add required criteria that will be used if previous required criteria chain are false
        /// </summary>
        /// <param name="criteria">The criteria</param>
        IRequireResult Or(IFilterCriteria criteria);
    }
}