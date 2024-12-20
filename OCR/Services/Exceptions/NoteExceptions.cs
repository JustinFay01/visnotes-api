using OCR.Services.Exceptions.Abstract;

namespace OCR.Services.Exceptions;

public class NoteNotFoundException() : EntityNotFoundException("Note not found");

public class NoteAlreadyExistsException() : EntityAlreadyExistsException("Note already exists");