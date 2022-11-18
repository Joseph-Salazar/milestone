using Domain.MainModule.Entity;
using Domain.MainModule.IRepository;
using FluentValidation;

namespace Domain.MainModule.Validations.PostulantValidations;

public class AddChoreValidator : AbstractValidator<Chore>
{
    private readonly IChoreRepository _chore;

    public AddChoreValidator (IChoreRepository chore)
    {
        _chore = chore;
    }
}