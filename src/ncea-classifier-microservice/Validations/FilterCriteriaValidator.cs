using FluentValidation;
using Ncea.Classifier.Microservice.Models;

namespace Ncea.Classifier.Microservice.Validations;

public class FilterCriteriaValidator : AbstractValidator<FilterCriteria>
{
    public FilterCriteriaValidator()
    {
        RuleFor(x => x.Level)
            .GreaterThan(0)
            .InclusiveBetween(1, 3);

        When(c => c.Parents != null, () => {
            RuleFor(vm => vm.Parents)
            .NotEmpty()
            .Must(u => !u!.Any(x => Char.IsWhiteSpace(x)));
        });
    }
}
