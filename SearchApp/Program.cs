using AF.Search;
using NUnit.Framework;

SearchFactory factory = new SearchFactory(SearchOptions.CaseSensitive);
ISearcher searcher = factory.Create(10);

string text = "My very long string\nTo search some text in";
string searchText = "My";
var result = searcher.Search(searchText, text.ToArray());
Assert.IsTrue(result == 0);

searchText = " in";
result = searcher.Search(searchText, text.ToArray());
Assert.IsTrue(result == (text.Length - 3));

searchText = "very long string\n";
result = searcher.Search(searchText, text.ToArray());
Assert.IsTrue(result == 3);

searchText = "To search";
result = searcher.Search(searchText, text.ToArray());
Assert.IsTrue(result == 20);

searchText = "to search";
result = searcher.Search(searchText, text.ToArray());
Assert.IsTrue(result == -1);

factory.SetSearchOptions(SearchOptions.CaseInsensitive);
searchText = "to search";
result = searcher.Search(searchText, text.ToArray());
Assert.IsTrue(result == 20);

searchText = "entries";
var iter = searcher.SearchAll(searchText, "FileToSearch04.txt");
foreach (var item in iter)
    Console.Write($"{item} ");
Console.WriteLine();

searchText = "le\r\nwi";
long lresult = searcher.Search(searchText, "FileToSearch01.txt");
Assert.IsTrue(lresult == 8);

searchText = "enters\r\n";
lresult = searcher.Search(searchText, "FileToSearch01.txt");
Assert.IsTrue(lresult == 17);

searchText = "xxx";
lresult = searcher.Search(searchText, "FileToSearch02.txt");
Assert.IsTrue(lresult == -1);

text = "blablablabla";
searchText = "blabla";
var array = searcher.SearchAll(searchText, text.ToArray()).ToArray();
Assert.IsTrue(array.Length == 2);
Assert.IsTrue(array[0] == 0);
Assert.IsTrue(array[1] == 6);

array = searcher.SearchAll(searchText, text.ToArray(), true).ToArray();
Assert.IsTrue(array.Length == 3);
Assert.IsTrue(array[0] == 0);
Assert.IsTrue(array[1] == 3);
Assert.IsTrue(array[2] == 6);

var larray = searcher.SearchAll(searchText, "FileToSearch03.txt").ToArray();
Assert.IsTrue(larray.Length == 2);
Assert.IsTrue(larray[0] == 4);
Assert.IsTrue(larray[1] == 14);

larray = searcher.SearchAll(searchText, "FileToSearch03.txt", true).ToArray();
Assert.IsTrue(larray.Length == 4);
Assert.IsTrue(larray[0] == 4);
Assert.IsTrue(larray[1] == 7);
Assert.IsTrue(larray[2] == 14);
Assert.IsTrue(larray[3] == 17);
