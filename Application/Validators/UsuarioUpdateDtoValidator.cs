using Application.DTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Validators;

public class UsuarioUpdateDtoValidator : AbstractValidator<UsuarioUpdateDto>
{
    public UsuarioUpdateDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email deve ter um formato válido")
            .Must(email => !string.IsNullOrWhiteSpace(email)).WithMessage("Email não pode ser vazio");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória")
            .Must(data => CalcularIdade(data) >= 18)
            .WithMessage("Usuário deve ter pelo menos 18 anos");

        RuleFor(x => x.Telefone)
            .Must(telefone => string.IsNullOrEmpty(telefone) || ValidarTelefone(telefone))
            .WithMessage("Telefone deve estar no formato (XX) XXXXX-XXXX")
            .When(x => !string.IsNullOrEmpty(x.Telefone));
    }

    private static int CalcularIdade(DateTime dataNascimento)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - dataNascimento.Year;
        if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }

    private static bool ValidarTelefone(string telefone)
    {
        var regex = new Regex(@"^\(\d{2}\)\s\d{5}-\d{4}$");
        return regex.IsMatch(telefone);
    }
}



