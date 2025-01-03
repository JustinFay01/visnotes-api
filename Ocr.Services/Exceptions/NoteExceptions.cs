using Ocr.Services.Exceptions.Abstract;

namespace Ocr.Services.Exceptions;

public class NoteNotFoundException() : EntityNotFoundException("Note not found");

public class NoteAlreadyExistsException() : EntityAlreadyExistsException("Note already exists");