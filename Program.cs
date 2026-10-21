using CustomerSupportAiAssistant.Knowledge;
using CustomerSupportAiAssistant.Services;

Console.Title = "C# Customer Support AI Assistant";

string folder = Path.Combine(
    AppContext.BaseDirectory, "Documents");

var knowledgeBase = new SupportKnowledgeBase();
knowledgeBase.Load(folder);

using var supportService = new SupportService();
var ticketService = new TicketDraftService();

Console.WriteLine("==================================");
Console.WriteLine("     CUSTOMER SUPPORT AI ASSISTANT");
Console.WriteLine("       Visual Studio 2026");
Console.WriteLine("       Ollama + Qwen 3.5");
Console.WriteLine("==================================");

Console.WriteLine(
    $"Loaded {knowledgeBase.Count} support documents.");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Ask Support Question");
    Console.WriteLine("2. Create Support Ticket Draft");
    Console.WriteLine("3. List Support Documents");
    Console.WriteLine("4. Reload Knowledge Base");
    Console.WriteLine("0. Exit");
    Console.WriteLine();

    Console.Write("Select option: ");
    string? choice = Console.ReadLine();

    if (choice == "0")
        break;

    try
    {
        switch (choice)
        {
            case "1":
                Console.Write("Enter your question: ");

                string question =
                    Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(question))
                    break;

                var articles =
                    knowledgeBase.Search(question);

                Console.WriteLine();
                Console.WriteLine("Retrieved Sources:");

                if (articles.Count == 0)
                {
                    Console.WriteLine(
                        "No matching support documents.");
                }
                else
                {
                    foreach (var article in articles)
                    {
                        Console.WriteLine(article.Source);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Preparing response...");

                string answer =
                    await supportService.AnswerAsync(
                        question, articles);

                Console.WriteLine();
                Console.WriteLine("Support Assistant:");
                Console.WriteLine(answer);

                break;

            case "2":
                Console.Write("Issue description: ");

                string issue = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(issue))
                {
                    Console.WriteLine(
                        "Issue description is required.");
                    break;
                }

                Console.Write(
                    "Category (Technical/Refund/Delivery/Other): ");

                string category =
                    (Console.ReadLine() ?? "").Trim();

                Console.Write(
                    "Priority (Low/Medium/High): ");

                string priority =
                    (Console.ReadLine() ?? "").Trim();

                if (!new[] { "Technical", "Refund",
                              "Delivery", "Other" }
                        .Contains(category,
                            StringComparer.OrdinalIgnoreCase)
                    ||
                    !new[] { "Low", "Medium", "High" }
                        .Contains(priority,
                            StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine(
                        "Invalid category or priority.");
                    break;
                }

                string draft =
                    ticketService.CreateDraft(
                        issue, category, priority);

                Console.WriteLine();
                Console.WriteLine(draft);

                Console.WriteLine();
                Console.Write(
                    "Save this ticket draft? (Y/N): ");

                if (Console.ReadLine()?.Trim().Equals(
                    "Y", StringComparison.OrdinalIgnoreCase)
                    == true)
                {
                    string path =
                        ticketService.SaveDraft(draft);

                    Console.WriteLine(
                        $"Draft saved to: {path}");
                }
                else
                {
                    Console.WriteLine(
                        "Draft not saved.");
                }

                break;

            case "3":
                Console.WriteLine(
                    "Available Support Documents:");

                if (Directory.Exists(folder))
                {
                    foreach (string file in Directory.GetFiles(
                        folder, "*.txt"))
                    {
                        Console.WriteLine(
                            Path.GetFileName(file));
                    }
                }

                break;

            case "4":
                knowledgeBase.Load(folder);

                Console.WriteLine(
                    $"Loaded {knowledgeBase.Count} documents.");

                break;

            default:
                Console.WriteLine("Invalid selection.");
                break;
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine(
            "Ollama API error: " + ex.Message);
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine(
            "The AI request timed out.");
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            "Error: " + ex.Message);
    }
}

