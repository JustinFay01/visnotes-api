using AutoMapper;
using Ocr.Data.Models;
using Ocr.Services.Dtos;

namespace Ocr.Services.Profiles;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteDto, Note>()
            .ReverseMap();
    }
}