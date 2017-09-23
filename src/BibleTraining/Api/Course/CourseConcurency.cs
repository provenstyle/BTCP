namespace BibleTraining.Api.Course
{
    using Entities;
    using Improving.Highway.Data.Scope.Concurrency;

    [RelativeOrder(5), StopOnFailure]
    public class CourseConcurency : CheckConcurrency<Course, CourseData>
    {
    }
}