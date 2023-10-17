
public static class MyUtils
{
    static string GetOrCreateFilePath(string fileName, string contentRootPath,
         string filesDirectory = "uploadFiles")
    {
        var directoryPath = Path.Combine(contentRootPath, filesDirectory);
        Directory.CreateDirectory(directoryPath);
        return Path.Combine(directoryPath, fileName);
    }

     public static async Task SaveFileWithName(IFormFile file, string fileSaveName, string contentRootPath)
    {
        var filePath = GetOrCreateFilePath(fileSaveName, contentRootPath);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }

    public static string GenerateHtmlForm(string formFieldName, string requestToken)
    {
        return $"""
      <html>
        <body>
          <form action="/upload" method="POST" enctype="multipart/form-data">
            <input name="{formFieldName}" type="hidden" value="{requestToken}" required/>
            <br/>
            <input name="Name" type="text" placeholder="Name of file" pattern=".*\.(jpg|jpeg|png)$" title="Please enter a valid name ending with .jpg, .jpeg, or .png" required/>
            <br/>
            <input name="Description" type="text" placeholder="Description of file" required/>
            <br/>
            <input type="file" name="FileDocument" placeholder="Upload an image..." accept=".jpg, 
                                                                            .jpeg, .png" />
            <input type="submit" />
          </form> 
        </body>
      </html>
    """;
    }
}
