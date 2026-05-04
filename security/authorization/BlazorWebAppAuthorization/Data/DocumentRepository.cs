using BlazorWebAppAuthorization.Models;

namespace BlazorWebAppAuthorization.Data;

public class DocumentRepository(ApplicationDbContext context) : IDocumentRepository
{
    public Document? Find(string? documentId)
    {
        if (string.IsNullOrEmpty(documentId))
        {
            return null;
        }

        return context.Documents.FirstOrDefault(d => d.ID.ToString() == documentId);
    }
}

public interface IDocumentRepository
{
    Document? Find(string? documentId);
}
