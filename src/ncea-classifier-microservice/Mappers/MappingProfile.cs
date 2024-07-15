using AutoMapper;
using Ncea.Classifier.Microservice.Domain.Models;
using Ncea.Classifier.Microservice.Models.Response;

namespace Ncea.Classifier.Microservice.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;

        CreateMap<Domain.Models.ClassifierInfo, Models.Response.ClassifierInfo>()
            ;
        CreateMap<Domain.Models.ClassifierInfo, GuidedSearchClassifier>()
            ;
        CreateMap<GuidedSearchClassifierInfo, GuidedSearchClassifier>()
            ;
        CreateMap<Domain.Models.GuidedSearchClassifiersWithPageContent, Models.Response.GuidedSearchClassifiersWithPageContent>()
            ;
    }
}
