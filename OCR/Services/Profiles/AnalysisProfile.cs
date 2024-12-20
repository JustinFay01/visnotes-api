using AutoMapper;
using OCR.Data.Models;
using OCR.Services.DTOs;

namespace OCR.Services.Profiles;

public class AnalysisProfile : Profile
{
    public AnalysisProfile()
    {
        CreateMap<Analysis, AnalysisDto>();
        CreateMap<AnalysisDto, Analysis>();
    }
}