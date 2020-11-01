using FluentValidation;
using FluentValidation.Results;

namespace CursoMongo.Api.Domain.ValueObjects
{
    public class Avaliacao : AbstractValidator<Avaliacao>
    {
        public Avaliacao(int estrelas, string comentario)
        {
            Estrelas = estrelas;
            Comentario = comentario;
        }

        public int Estrelas { get; private set; }

        public string Comentario { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public virtual bool Validar()
        {
            ValidarEstrelas();
            ValidarComentario();

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        private void ValidarEstrelas()
        {
            RuleFor(c => c.Estrelas)
                .GreaterThan(0).WithMessage("Número de estrelas deve ser maior que zero.")
                .LessThanOrEqualTo(5).WithMessage("Número de estrelas deve ser menor ou igual a cinco.");
        }

        private void ValidarComentario()
        {
            RuleFor(c => c.Comentario)
                .NotEmpty().WithMessage("Comentario não pode ser vazio.")
                .MaximumLength(100).WithMessage("Comentario pode ter no maximo 100 caracteres.");
        }
    }
}