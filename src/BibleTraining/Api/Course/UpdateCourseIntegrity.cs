namespace BibleTraining.Api.Course
{
    using FluentValidation;

    public class UpdateCourseIntegrity : AbstractValidator<UpdateCourse>
    {
        public UpdateCourseIntegrity()
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