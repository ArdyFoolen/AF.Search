using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AF.Search
{
    public interface ISearchParameter
    {
        int Index { get; }
        int Expected { get; }
        void NextSearch(bool overlap);
        void Reset();
        bool IsSearchedAndFound(int increment);
    }

    internal class SearchParameter : ISearchParameter
    {
        private ISearchCriteria criteria;

        private int increment;
        private bool characterFound { get => increment == Expected; }

        private bool searchFound { get => Expected == criteria.Length; }

        public int Index { get; private set; }
        public int Expected { get; private set; }

        public SearchParameter(ISearchCriteria criteria)
        {
            this.criteria = criteria;
        }

        public void NextSearch(bool overlap)
        {
            if (overlap)
                Index += criteria.Length;
            else
                Index += criteria.Length + criteria.LastIndexToLength;
            Expected = 0;
        }

        public void Reset()
        {
            Index = criteria.LastIndexToLength;
            Expected = 0;
        }

        public bool IsSearchedAndFound(int increment)
        {
            setIncrement(increment);
            return Condition
                .If(characterFound)
                .Then(markCharacterFound)
                .Else(incrementIndex)
                .Return<bool>(isSearchCompleted);
        }

        private void setIncrement(int increment)
            => this.increment = increment;

        private void incrementIndex()
        {
            Index += increment;
            Expected = 0;
        }

        private void markCharacterFound()
        {
            Index -= 1;
            Expected += 1;
        }

        private bool isSearchCompleted()
        {
            if (searchFound)
                Index += 1;
            return searchFound;
        }
    }
}
