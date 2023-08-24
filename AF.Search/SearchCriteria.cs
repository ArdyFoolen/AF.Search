namespace AF.Search
{
    public enum SearchOptions
    {
        CaseInsensitive,
        CaseSensitive
    }

    public interface ISearchCriteria
    {
        int Length { get; }
        int LastIndexToLength { get; }
        void Prepare(string searchText);
        int GetIncrement(char c, int expected);
    }

    internal class SearchCriteria : ISearchCriteria
    {
        private Dictionary<char, int> forward = new Dictionary<char, int>();
        private char[] original;
        private SearchOptions searchOptions;

        private bool CaseSensitive { get => searchOptions == SearchOptions.CaseSensitive; }
        private char ToCase(char c)
            => CaseSensitive ? c : char.ToLowerInvariant(c);

        public int LastIndexToLength { get => Length - 1; }

        public string OriginalValue { get; private set; }
        public int Length { get; private set; }

        public SearchCriteria() : this(SearchOptions.CaseInsensitive) { }

        public SearchCriteria(SearchOptions searchOptions)
        {
            this.searchOptions = searchOptions;
        }

        public void SetSearchOptions(SearchOptions searchOptions)
            => this.searchOptions = searchOptions;

        public void Prepare(string searchText)
        {
            setProperties(searchText);
            setForward();
        }

        public int GetIncrement(char c, int expected)
        {
            c = ToCase(c);
            if (forward.ContainsKey(c))
            {
                int value = forward[c];
                if (value < expected)
                    if (original[expected] == c)
                        return expected;
                    else
                        return expected + 1;
                return value;
            }
            else
                return Length;
        }

        private void setProperties(string searchText)
        {
            OriginalValue = searchText;
            original = OriginalValue.Reverse().ToArray();
            Length = searchText.Length;
        }

        private void setForward()
        {
            forward.Clear();
            OriginalValue.Reverse().ForEach((c, i) => setForward(c, i));
        }

        private void setForward(char c, int value)
        {
            c = ToCase(c);
            if (!forward.ContainsKey(c))
                forward.Add(c, value);
        }
    }
}
