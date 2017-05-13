namespace BibleTraining.Api.Course
{
    using Entities;

    public static class CourseDataExtensions
    {
        public static CourseData Map(this CourseData data, Course course)
        {
            if (course == null) return null;

            ResourceMapper.Map(data, course);

            data.Name        = course.Name;
            data.Description = course.Description;

            return data;
        }
    }
}