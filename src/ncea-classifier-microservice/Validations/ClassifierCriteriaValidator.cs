using FluentValidation;
using Ncea.Classifier.Microservice.Models;

namespace Ncea.Classifier.Microservice.Validations;

public class ClassifierCriteriaValidator : AbstractValidator<ClassifierCriteria>
{
    public ClassifierCriteriaValidator()
    {
        RuleFor(c => c.Step).GreaterThan(0);
    }
}
