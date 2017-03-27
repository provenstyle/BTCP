namespace BibleTraining.Api.Course
{
    using Improving.MediatR;

    public class GetCourses : Request.WithResponse<CourseResult>
    {
        public GetCourses()
        {
            Ids = new int[0];
        }

        public GetCourses(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set;}

        public bool KeyProperties { get; set; }
    }
}