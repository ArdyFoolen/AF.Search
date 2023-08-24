namespace AF.Search
{
    public interface ISearcher
    {
        int Search(string searchText, char[] text);
        long Search(string searchText, string filePath);
        IEnumerable<int> SearchAll(string searchText, char[] text, bool overlap = false);
        IEnumerable<long> SearchAll(string searchText, string filePath, bool overlap = false);
    }
    public class Searcher : ISearcher
    {
        private ISearchFactory factory;
        private int bufferLength;

        public Searcher(ISearchFactory factory, int bufferLength = 1024)
        {
            this.factory = factory;
            this.bufferLength = bufferLength;
        }

        public int Search(string searchText, char[] text)
        {
            prepare(searchText);
            return search(text);
        }

        public long Search(string searchText, string filePath)
        {
            byte[] buffer = new byte[bufferLength];
            prepareWithFilePosition(searchText);

            int result = 0;

            using (FileToCharStream fs = new FileToCharStream(filePath, FileMode.Open, FileAccess.Read))
            {
                do
                {
                    fs.Seek(factory.FilePosition.Position, SeekOrigin.Begin);
                    result = search(fs.ReadToCharArray(buffer, 0, buffer.Length));

                    if (!fs.Eof && result == -1)
                        factory.FilePosition.NextPosition(fs.BytesRead);
                } while (!fs.Eof && result == -1);
            }

            return result == -1 ? result : factory.FilePosition.Position + result;
        }

        public IEnumerable<int> SearchAll(string searchText, char[] text, bool overlap = false)
        {
            prepare(searchText);
            foreach (var search in searchAll(text, overlap))
                yield return search;
        }

        public IEnumerable<long> SearchAll(string searchText, string filePath, bool overlap = false)
        {
            byte[] buffer = new byte[bufferLength];
            prepareWithFilePosition(searchText);

            using (FileToCharStream fs = new FileToCharStream(filePath, FileMode.Open, FileAccess.Read))
            {
                do
                {
                    fs.Seek(factory.FilePosition.Position, SeekOrigin.Begin);
                    foreach (var search in searchAll(fs.ReadToCharArray(buffer, 0, buffer.Length), overlap))
                        yield return factory.FilePosition.Position + search;

                    if (!fs.Eof)
                        factory.FilePosition.NextPosition(fs.BytesRead);
                } while (!fs.Eof);
            }
        }

        private int search(char[] text)
        {
            while (factory.Parameter.Index < text.Length)
            {
                var increment = factory.Criteria.GetIncrement(text[factory.Parameter.Index], factory.Parameter.Expected);
                if (IsSearchCompletelyFound(increment))
                    return factory.Parameter.Index;
            }
            return -1;
        }

        private IEnumerable<int> searchAll(char[] text, bool overlap = false)
        {
            int result;
            do
            {
                result = search(text);
                if (result != -1)
                {
                    yield return result;
                    factory.Parameter.NextSearch(overlap);
                }
            }
            while (result != -1);
        }

        private bool IsSearchCompletelyFound(int increment)
            => factory.Parameter.IsSearchedAndFound(increment);

        private void prepare(string searchText)
        {
            factory.Criteria.Prepare(searchText);
            factory.Parameter.Reset();
        }

        private void prepareWithFilePosition(string searchText)
        {
            prepare(searchText);
            factory.FilePosition.Reset();
        }
    }
}
