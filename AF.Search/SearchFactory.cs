using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Search
{
    public interface ISearchFactory
    {
        ISearchCriteria Criteria { get; }
        ISearchParameter Parameter { get; }
        ISearchPosition FilePosition { get; }

        ISearcher Create(int bufferLength = 1024);
        void SetSearchOptions(SearchOptions searchOptions);
    }
    public class SearchFactory : ISearchFactory
    {
        private SearchOptions searchOptions;

        private SearchCriteria? criteria = null;
        public ISearchCriteria Criteria
        {
            get
            {
                if (criteria == null)
                    criteria = new SearchCriteria(searchOptions);
                return criteria;
            }
        }

        private SearchParameter? parameter = null;
        public ISearchParameter Parameter
        {
            get
            {
                if (parameter == null)
                    parameter = new SearchParameter(Criteria);
                return parameter;
            }
        }

        private SearchPosition? filePosition = null;
        public ISearchPosition FilePosition
        {
            get
            {
                if (filePosition == null)
                    filePosition = new SearchPosition(Criteria, Parameter);
                return filePosition;
            }
        }

        public SearchFactory() : this(SearchOptions.CaseInsensitive) { }

        public SearchFactory(SearchOptions searchOptions)
        {
            this.searchOptions = searchOptions;
        }

        public ISearcher Create(int bufferLength = 1024)
            => new Searcher(this, bufferLength);

        public void SetSearchOptions(SearchOptions searchOptions)
        {
            this.searchOptions = searchOptions;
            if (criteria != null)
                criteria.SetSearchOptions(searchOptions);
        }
    }
}
