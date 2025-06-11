using FluentValidation;
using learning_center_back.Tutorials.Application.CommandServices;
using learning_center_back.Tutorials.Domain.Models.Commands;

namespace learning_center_back.Tutorials.Domain.Models.Validadors;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(v => v.Description).NotEmpty().Length(10, 100).WithMessage("Description lenght bwtween 10 and 100 characters");
        RuleFor(v => v.PublishDate).LessThan(DateTime.Now).WithMessage("Publish Date must be greater than Publish Date");
    }

}