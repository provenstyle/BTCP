namespace BibleTraining.Api.Course
{
    using Entities;

    public static class CourseDataExtensions
    {
        public static CourseData Map(this CourseData data, Course course)
        {
            data.Id          = course.Id;
            data.Name        = course.Name;
            data.Description = course.Description;
            data.RowVersion  = course.RowVersion;
            data.CreatedBy   = course.CreatedBy;
            data.Created     = course.Created;
            data.ModifiedBy  = course.ModifiedBy;
            data.Modified    = course.Modified;

            return data;
        }
    }
}