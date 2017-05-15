namespace $ApplicationName$.Api.$Entity$
{
    using FluentValidation;

    public class Update$Entity$Integrity : AbstractValidator<Update$Entity$>
    {
        public Update$Entity$Integrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new $Entity$DataIntegrity());
        }

        private class $Entity$DataIntegrity : AbstractValidator<$Entity$Data>
        {
            public $Entity$DataIntegrity()
            {
                RuleFor(x => x.Id)
                    .NotNull();
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}
