using AutoMapper;
using Ncea.Classifier.Microservice.Mappers;

namespace Ncea.Classifier.Microservice.Tests.Mappers;

public class MappingProfileTests
{
    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        config.AssertConfigurationIsValid();
    }
}
