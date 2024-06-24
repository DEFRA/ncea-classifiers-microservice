using FluentAssertions;
using FluentValidation.TestHelper;
using Ncea.Classifier.Microservice.Models;
using Ncea.Classifier.Microservice.Validations;

namespace Ncea.Classifier.Microservice.Tests.Validations;

public class FilterCriteriaValidatorTests
{
    private readonly FilterCriteriaValidator _filterCriteriaValidator;

    public FilterCriteriaValidatorTests()
    {
        _filterCriteriaValidator = new FilterCriteriaValidator();
    }

    [Fact]
    public void GivenValidate_WhenLevelIdNotGiven_ThenValidationFails()
    {
        // Arrange
        var filterCriteria = new FilterCriteria();

        // Act
        var result = _filterCriteriaValidator.TestValidate(filterCriteria);

        //Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GivenValidate_WhenLevelIdNotGiven_ThenValidationSucceeds()
    {
        // Arrange
        var filterCriteria = new FilterCriteria()
        {
            Level = 1
        };

        // Act
        var result = _filterCriteriaValidator.TestValidate(filterCriteria);

        //Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void GivenValidate_WhenLevelIdNotInRange_ThenValidationFails()
    {
        // Arrange
        var filterCriteria = new FilterCriteria()
        {
            Level = 5
        };

        // Act
        var result = _filterCriteriaValidator.TestValidate(filterCriteria);

        //Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GivenValidate_WhenParentCodeNotWithValues_ThenValidationFails()
    {
        // Arrange
        var filterCriteria = new FilterCriteria()
        {
            Level = 3,
            Parents = string.Empty
        };

        // Act
        var result = _filterCriteriaValidator.TestValidate(filterCriteria);

        //Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GivenValidate_WhenParentCodeWithValues_ThenValidationFails()
    {
        // Arrange
        var filterCriteria = new FilterCriteria()
        {
            Level = 3,
            Parents = "test"
        };

        // Act
        var result = _filterCriteriaValidator.TestValidate(filterCriteria);

        //Assert
        result.IsValid.Should().BeTrue();
    }
}
