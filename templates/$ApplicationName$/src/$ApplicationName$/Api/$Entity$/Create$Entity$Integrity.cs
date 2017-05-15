namespace $ApplicationName$.Api.$Entity$
{
    using FluentValidation;
    using Entity;
    
    public class Create$Entity$Integrity : AbstractValidator<Create$Entity$>
    {
        public Create$Entity$Integrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new $Entity$DataIntegrity());
        }

        private class $Entity$DataIntegrity : AbstractValidator<$Entity$Data>
        {
            public $Entity$DataIntegrity()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}
