namespace BibleTraining.Api.Course
{
    using Entities;
    using Improving.Highway.Data.Scope.Concurrency;
    using Improving.MediatR;

    [RelativeOrder(5), StopOnFailure]
    public class CourseConcurency : CheckConcurrency<Course, CourseData>
    {
    }
}