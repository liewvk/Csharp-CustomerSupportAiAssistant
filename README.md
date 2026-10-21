# C# Customer Support AI Assistant

A .NET console application that demonstrates a customer support assistant using a local retrieval-based knowledge base and Ollama-hosted LLM inference.

This project loads support policy documents, finds the most relevant articles for a customer question, and uses a local model to draft a professional support response. It also includes a ticket draft feature for capturing and saving issue details.

## Features

- Loads support articles from the `Documents` folder
- Searches the knowledge base using keyword matching
- Uses Ollama (`qwen3.5:9b`) to generate support responses
- Restricts responses to approved support content only
- Creates a customer support ticket draft with category and priority
- Saves ticket drafts to the local application data folder

## Project Structure

- `Program.cs` - interactive console menu and app flow
- `Knowledge/SupportKnowledgeBase.cs` - loads and searches support documents
- `Models/SupportArticle.cs` - support article model
- `Services/SupportService.cs` - communicates with Ollama and generates answers
- `Services/TicketDraftService.cs` - creates and saves ticket drafts
- `Documents/` - source policy documents used by the assistant

## Included Knowledge Documents

The app ships with these support documents:

- `printer-support.txt`
- `refund-policy.txt`
- `shipping-policy.txt`
- `warranty-policy.txt`

These files are copied to the output folder by the project configuration and used as the knowledge base for customer questions.

## Prerequisites

- .NET 10 SDK or Visual Studio 2026 with .NET 10 support
- Ollama installed and running locally
- The model `qwen3.5:9b` available in Ollama

## Setup

1. Install and start Ollama.
2. Pull the model:

```bash
ollama pull qwen3.5:9b
```

3. Restore and run the app:

```bash
dotnet restore
dotnet run
```

## Usage

When the application starts, it presents a menu:

1. Ask Support Question
2. Create Support Ticket Draft
3. List Support Documents
4. Reload Knowledge Base
0. Exit

### Example workflow

- Select `1` to ask a support question such as "Why is my printer not printing?"
- The app searches the support documents for the most relevant articles
- The LLM produces a support answer grounded in those documents
- Select `2` to generate a ticket draft with a category and priority
- Save the ticket draft to a local `.txt` file if needed

## Notes

- The assistant is designed to use approved support information only.
- It does not invent policies, guarantees, or refunds.
- If enough information is not available, it recommends human support.

## License

This project is provided as a sample application for educational use.
