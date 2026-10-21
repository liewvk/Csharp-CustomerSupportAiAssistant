using System.Text.RegularExpressions;
using CustomerSupportAiAssistant.Models;

namespace CustomerSupportAiAssistant.Knowledge;

public class SupportKnowledgeBase
{
    private readonly List<SupportArticle> _articles = new();

    private static readonly HashSet<string> StopWords = new()
    {
        "the", "is", "a", "an", "and", "or",
        "to", "of", "in", "for", "on", "with",
        "what", "how", "can", "i", "my", "me",
        "do", "does", "about", "are", "you",
        "your", "our", "it", "have", "has"
    };

    public int Count => _articles.Count;

    public void Load(string folderPath)
    {
        _articles.Clear();

        if (!Directory.Exists(folderPath))
            return;

        foreach (string file in Directory.GetFiles(
            folderPath, "*.txt"))
        {
            string content = File.ReadAllText(file);

            if (string.IsNullOrWhiteSpace(content))
                continue;

            _articles.Add(new SupportArticle
            {
                Title = Path.GetFileNameWithoutExtension(file),
                Source = Path.GetFileName(file),
                Content = content
            });
        }
    }

    private static HashSet<string> GetWords(string text)
    {
        return Regex.Matches(
                text.ToLowerInvariant(), @"[a-z0-9]+")
            .Select(m => m.Value)
            .Where(w => w.Length > 2 &&
                        !StopWords.Contains(w))
            .ToHashSet();
    }

    public List<SupportArticle> Search(
        string question,
        int maxResults = 2)
    {
        var keywords = GetWords(question);

        return _articles
            .Select(article => new
            {
                Article = article,
                Score = CalculateScore(article, keywords)
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(maxResults)
            .Select(x => x.Article)
            .ToList();
    }

    private static int CalculateScore(
        SupportArticle article,
        HashSet<string> keywords)
    {
        var titleWords = GetWords(article.Title);
        var contentWords = GetWords(article.Content);

        int score = 0;

        foreach (string word in keywords)
        {
            if (titleWords.Contains(word))
                score += 3;

            if (contentWords.Contains(word))
                score++;
        }

        return score;
    }
}

