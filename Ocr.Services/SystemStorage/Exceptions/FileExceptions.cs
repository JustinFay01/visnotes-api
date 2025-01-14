namespace Ocr.Services.SystemStorage.Exceptions;

public class FileLengthInvalidException(double length) : Exception("File length is invalid: " + length);

public class FileNotFoundException(string fileName) : Exception("File not found: " + fileName);

public class FileSaveException(string fileName, Exception innerException)
    : Exception("File save failed: " + fileName, innerException);
    
    
public class FileExistsException(string fileName) : Exception("File already exists: " + fileName);