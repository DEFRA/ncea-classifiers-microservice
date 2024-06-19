using AutoMapper;
using Ncea.Classifier.Microservice.Domain.Models;
using Ncea.Classifier.Microservice.Models.Response;

namespace Ncea.Classifier.Microservice;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Models.ClassifierInfo, Models.Response.ClassifierInfo>()
            ;
        CreateMap<GuidedSearchClassifierInfo, GuidedSearchClassifier>()
            ;
        CreateMap<Domain.Models.GuidedSearchClassifiersWithPageContent, Models.Response.GuidedSearchClassifiersWithPageContent>()
            ;
    }
}
