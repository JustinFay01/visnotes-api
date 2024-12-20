using AutoMapper;
using OCR.Data.Models;
using OCR.Services.DTOs;

namespace OCR.Services.Profiles;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteDto, Note>()
            .ReverseMap();
    }
}