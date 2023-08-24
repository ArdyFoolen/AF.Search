using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Search
{
    public interface ISearchPosition
    {
        long Position { get; }
        void Reset();
        void NextPosition(int count);
    }
    internal class SearchPosition : ISearchPosition
    {
        private ISearchCriteria criteria;
        private ISearchParameter parameter;
        public long Position { get; private set; }
        public SearchPosition(ISearchCriteria criteria, ISearchParameter parameter)
        {
            this.criteria = criteria;
            this.parameter = parameter;
        }

        public void Reset()
            => Position = 0;

        public void NextPosition(int read)
        {
            Position += calculateToNextIndex(read);
            parameter.Reset();
        }

        private long calculateToNextIndex(int read)
            => read - lengthInPreviousBuffer(read);

        private int lengthInPreviousBuffer(int read)
            => criteria.LastIndexToLength - lengthInCurrentBuffer(read);

        private int lengthInCurrentBuffer(int read)
            => parameter.Index - read;
    }
}
