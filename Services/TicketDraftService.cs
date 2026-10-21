namespace CustomerSupportAiAssistant.Services;

public class TicketDraftService
{
    private readonly string _folder;

    public TicketDraftService()
    {
        _folder = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "CustomerSupportAiAssistant",
            "TicketDrafts");

        Directory.CreateDirectory(_folder);
    }

    public string CreateDraft(
        string issue,
        string category,
        string priority)
    {
        if (string.IsNullOrWhiteSpace(issue))
            throw new ArgumentException(
                "Issue description is required.");

        string draft = $"""
            CUSTOMER SUPPORT TICKET DRAFT

            Created: {DateTime.Now:yyyy-MM-dd HH:mm}
            Category: {category}
            Priority: {priority}

            Issue Description:
            {issue}

            Status: Draft - Not Submitted

            Next Action:
            Review the issue and contact support staff
            if escalation is required.
            """;

        return draft;
    }

    public string SaveDraft(string draft)
    {
        if (string.IsNullOrWhiteSpace(draft))
            throw new ArgumentException("Draft is empty.");

        string fileName =
            $"Ticket_{DateTime.UtcNow:yyyyMMdd_HHmmss}_" +
            $"{Guid.NewGuid():N}.txt";

        string path = Path.Combine(_folder, fileName);

        using var stream = new FileStream(
            path, FileMode.CreateNew);

        using var writer = new StreamWriter(stream);

        writer.Write(draft);

        return path;
    }
}

