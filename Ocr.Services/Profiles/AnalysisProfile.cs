using AutoMapper;
using Ocr.Data.Migrations;
using Ocr.Services.Dtos;

namespace Ocr.Services.Profiles;

public class AnalysisProfile : Profile
{
    public AnalysisProfile()
    {
        CreateMap<Analysis, AnalysisDto>();
        CreateMap<AnalysisDto, Analysis>();
    }
}