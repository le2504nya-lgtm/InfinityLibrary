namespace InfinityLibrary;

internal class Program
{
    private static char[] AllSymbols = 
        "1234567890-=!@#$%^&*()_+qwertyuiop[]asdfghjkl;'zxcvbnm,./йцукенгшщзхъфывапролджэячсмитьбю".ToCharArray();
    
    private static int PageSize = 128;
    
    private static Random Random = new Random();

    private static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("Enter word: ");
            string word = Console.ReadLine()?.Trim() ?? "";
            
            if (string.IsNullOrWhiteSpace(word)) continue;
            
            Search(word);
        }
    }

    private static void Search(string word)
    {
        var startTime = DateTime.Now;
        long pageCount = 0;
        
        foreach (var page in GeneratePagesStream())
        {
            pageCount++;
            if (ContainsWord(page.Content, word))
            {
                var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                
                PrintPage(page.Content, word);
                Console.WriteLine($"\nFound in {elapsed:F0}ms, pages generated: {pageCount}");
                break;
            }
        }
    }

    private static bool ContainsWord(string content, string word)
    {
        return content.AsSpan().IndexOf(word.AsSpan(), StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static IEnumerable<Page> GeneratePagesStream()
    {
        while (true)
        {
            char[] chars = new char[PageSize];
            for (int i = 0; i < PageSize; i++)
                chars[i] = AllSymbols[Random.Next(AllSymbols.Length)];
            yield return new Page(new string(chars));
        }
    }
    
    private static void PrintPage(string content, string word)
    {
        var parts = content.Split(new[] { word }, StringSplitOptions.None);
        for (int i = 0; i < parts.Length; i++)
        {
            Console.Write(parts[i]);
            if (i < parts.Length - 1) 
                Console.Write($"\x1b[33m{word}\x1b[0m");
        }
        Console.WriteLine("\n");
    }
}

public class Page
{
    public string Content { get; }

    public Page(string symbols)
    {
        Content = symbols;
    }
}