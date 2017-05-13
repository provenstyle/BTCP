namespace BibleTraining.Api.Course
{
    using Entities;

    public static class CourseExtensions
    {
        public static Course Map(this Course course, CourseData data)
        {
            EntityMapper.Map(course, data);

            if (data.Name != null)
                course.Name = data.Name;

            if (data.Description != null)
                course.Description = data.Description;

            return course;
        }
    }
}