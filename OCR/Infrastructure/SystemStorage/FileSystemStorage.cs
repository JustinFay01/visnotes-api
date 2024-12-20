using OCR.Infrastructure.SystemStorage.Exceptions;
using FileNotFoundException = OCR.Infrastructure.SystemStorage.Exceptions.FileNotFoundException;

namespace OCR.Infrastructure.SystemStorage;

public interface IFileSystemStorage
{
    /// <summary>
    /// Save file to file system amd returns the file path
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public Task<string> SaveFileAsync(IFormFile file);
    
    /// <summary>
    /// Delete file from file system
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public Task DeleteFileAsync(string fileName);
    
 
    /// <summary>
    ///  Get file from file system using the local path
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>IFormFile</returns>
    public Task<IFormFile> GetFileAsync(string filePath);
}

public class FileSystemStorage : IFileSystemStorage
{
    private readonly string _storagePath;

    private const double MaxFileSize = 4 * 1024 * 1024; // 4MB

    public FileSystemStorage(string storagePath)
    {
        _storagePath = storagePath;
    }
    
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file.Length <= 0 || file.Length > MaxFileSize)
        {
            throw new FileLengthInvalidException(file.Length);
        }
        try
        {
            // Check if the directory exists
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            
            var filePath = Path.Combine(_storagePath, file.FileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return filePath;
        } 
        catch (Exception e)
        {
            throw new FileSaveException(file.FileName, e);
        }
    }

    public Task DeleteFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public async Task<IFormFile> GetFileAsync(string filePath)
    {
        if(string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentNullException(nameof(filePath));
        }
        
        var fullPath = Path.Combine(_storagePath, filePath);
        
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException(fullPath);
        }
        
        var fileStream = new FileStream(fullPath, FileMode.Open);
        
        var file = await Task.Run(() =>  new FormFile(fileStream, 0, fileStream.Length, Path.GetFileName(fullPath), fullPath));
        
        return file;
    }
}