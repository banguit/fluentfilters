#region License
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
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class FilterCriteriaBuilder : IFilterCriteriaBuilder
    {
        #region Fileds

        private readonly List<FilterCriteriaResult> _results;

        #endregion

        #region Constructor

        public FilterCriteriaBuilder()
        {
            _results = new List<FilterCriteriaResult>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add required criteria
        /// </summary>
        /// <param name="criteria">The criteria</param>
        public IRequireResult Require(IFilterCriteria criteria)
        {
            if (_results.Any(c => c.Type.Equals(FilterCriteriaType.And)))
            {
                throw new InvalidOperationException(
                    "Required criteria were already registered. Use methods And(...) or Or(..) at required criteria chain for register new one.");
            }

            return new RequireResult(this, criteria);
        }

        /// <summary>
        /// Add excluded criteria
        /// </summary>
        /// <param name="criteria">The criteria</param>
        public IExcludeResult Exclude(IFilterCriteria criteria)
        {
            if (_results.Any(c => c.Type.Equals(FilterCriteriaType.Not)))
            {
                throw new InvalidOperationException(
                    "Excluded criteria were already registered. Use method And(...) at excluded criteria chain for register new one.");
            }

            return new ExcludeResult(this, criteria);
        }

        /// <summary>
        /// Gets the results
        /// </summary>
        public List<FilterCriteriaResult> GetResults()
        {
            return _results;
        }

        #endregion

        #region Nested classes

        public class RequireResult : IRequireResult
        {
            #region Fileds

            private readonly FilterCriteriaBuilder _builder;

            #endregion

            #region Constructor

            public RequireResult(FilterCriteriaBuilder builder, IFilterCriteria criteria)
            {
                _builder = builder;
                And(criteria);
            }

            #endregion

            #region Implementation of IRequireResult

            /// <summary>
            /// Add required criteria
            /// </summary>
            /// <param name="criteria">The criteria</param>
            public IRequireResult And(IFilterCriteria criteria)
            {
                _builder._results.Add(new FilterCriteriaResult(criteria, FilterCriteriaType.And, GetLevel()));
                return this;
            }

            /// <summary>
            /// Add required criteria that will be used if previous required criteria are false
            /// </summary>
            /// <param name="criteria">The criteria</param>
            public IRequireResult Or(IFilterCriteria criteria)
            {
                _builder._results.Add(new FilterCriteriaResult(criteria, FilterCriteriaType.And, GetLevel() + 1));
                return this;
            }

            #endregion

            #region Private methods

            /// <summary>
            /// Gets level for current required criteria chain
            /// </summary>
            private int GetLevel()
            {
                int lastItemIndex = _builder._results.FindLastIndex(p => p.Level >= 0);
                if (lastItemIndex == -1)
                {
                    return 0;
                }

                FilterCriteriaResult lastCriteria = _builder._results[lastItemIndex];
                return lastCriteria.Level;
            }

            #endregion
        }

        public class ExcludeResult : IExcludeResult
        {
            #region Fileds

            private readonly FilterCriteriaBuilder _builder;

            #endregion

            #region Constructor

            public ExcludeResult(FilterCriteriaBuilder builder, IFilterCriteria criteria)
            {
                _builder = builder;
                And(criteria);
            }

            #endregion

            #region Methods

            /// <summary>
            /// Add excluded criteria
            /// </summary>
            /// <param name="criteria">The criteria</param>
            public IExcludeResult And(IFilterCriteria criteria)
            {
                _builder._results.Add(new FilterCriteriaResult(criteria, FilterCriteriaType.Not, GetLevel()));
                return this;
            }

            /// <summary>
            /// Add criteria for excluding that will be used if previous chain of criteria which should be  excluded are false
            /// </summary>
            /// <param name="criteria">The criteria</param>
            /// <returns></returns>
            public IExcludeResult Or(IFilterCriteria criteria)
            {
                _builder._results.Add(new FilterCriteriaResult(criteria, FilterCriteriaType.Not, GetLevel() + 1));
                return this;
            }

            #endregion

            #region Private methods

            /// <summary>
            /// Gets level for current required criteria chain
            /// </summary>
            private int GetLevel()
            {
                int lastItemIndex = _builder._results.FindLastIndex(p => p.Level >= 0);
                if (lastItemIndex == -1)
                {
                    return 0;
                }

                FilterCriteriaResult lastCriteria = _builder._results[lastItemIndex];
                return lastCriteria.Level;
            }

            #endregion
        }

        #endregion
    }
}