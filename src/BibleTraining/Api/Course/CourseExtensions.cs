namespace BibleTraining.Api.Course
{
    using System;
    using Entities;

    public static class CourseExtensions
    {
        public static Course Map(this Course course, CourseData data)
        {
            if (data.Name != null)
                course.Name = data.Name;

            if (data.Description != null)
                course.Description = data.Description;

            if (data.CreatedBy != null)
                course.CreatedBy = data.CreatedBy;

            if (data.ModifiedBy != null)
                course.ModifiedBy = data.ModifiedBy;

            course.Modified = DateTime.Now;

            return course;
        }
    }
}