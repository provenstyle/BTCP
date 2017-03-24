namespace BibleTraining.Api.Course
{
    using FluentValidation;

    public class CreateCourseIntegrity : AbstractValidator<CreateCourse>
    {
        public CreateCourseIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new CourseDataIntegrity());
        }

        private class CourseDataIntegrity : AbstractValidator<CourseData>
        {
            public CourseDataIntegrity()
            {
                RuleFor(x => x.Description)
                    .NotEmpty();
            }
        }
    }
}