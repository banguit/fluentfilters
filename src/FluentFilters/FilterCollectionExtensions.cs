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
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Filters;

    public static class FilterCollectionExtensions
    {
        internal static Dictionary<IFilterMetadata, FilterRecord> Records = new Dictionary<IFilterMetadata, FilterRecord>();

        #region Public extension methods

        public static void Add(this FilterCollection collection, IFilterMetadata filter, Action<IFilterCriteriaBuilder> criteria)
        {
            collection.Add(filter);

            var builder = new FilterCriteriaBuilder();
            criteria(builder);
            Records.Add(filter, new FilterRecord(builder.GetResults()));
        }

        public static IFilterMetadata Add(this FilterCollection collection, Type filterType, Action<IFilterCriteriaBuilder> criteria)
        {
            return collection.Add(filterType, 0, criteria);
        }

        public static IFilterMetadata Add(this FilterCollection collection, Type filterType, int order, Action<IFilterCriteriaBuilder> criteria)
        {
            IFilterMetadata filter = collection.Add(filterType, order);

            var builder = new FilterCriteriaBuilder();
            criteria(builder);
            Records.Add(filter, new FilterRecord(builder.GetResults()));

            return filter;
        }

        public static IFilterMetadata AddService(this FilterCollection collection, Type filterType, Action<IFilterCriteriaBuilder> criteria)
        {
            return collection.AddService(filterType, 0, criteria); ;
        }

        public static IFilterMetadata AddService(this FilterCollection collection, Type filterType, int order, Action<IFilterCriteriaBuilder> criteria)
        {
            IFilterMetadata filter = collection.AddService(filterType, order);

            var builder = new FilterCriteriaBuilder();
            criteria(builder);
            Records.Add(filter, new FilterRecord(builder.GetResults()));

            return filter;
        }

        #endregion
    }
}