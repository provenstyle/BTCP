namespace BibleTraining.Api.Course
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Improving.MediatR;
    using Improving.MediatR.Pipeline;
    using MediatR;
    using Queries;

    [RelativeOrder(Stage.Validation - 1)]
    public class CourseAggregateHandler :
        IAsyncRequestHandler<CreateCourse, CourseData>,
        IAsyncRequestHandler<GetCourses, CourseResult>,
        IAsyncRequestHandler<UpdateCourse, CourseData>,
        IRequestMiddleware<UpdateCourse, CourseData>,
        IAsyncRequestHandler<RemoveCourse, CourseData>,
        IRequestMiddleware<RemoveCourse, CourseData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;
        private readonly DateTime _now;

        public Course Course { get; set; }

        public CourseAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
            _now        = DateTime.Now;
        }

        #region Create Course

        public async Task<CourseData> Handle(CreateCourse message)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var course = Map(new Course(), message.Resource);
                course.Created = _now;

                _repository.Context.Add(course);

                var data = new CourseData();

                await scope.SaveChangesAsync((dbScope, count) =>
                                                 {
                                                     data.Id = course.Id;
                                                     data.RowVersion = course.RowVersion;
                                                 });

                return data;
            }
        }

        #endregion

        #region Get Course

        public async Task<CourseResult> Handle(GetCourses message)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var courses = (await _repository.FindAsync(new GetCoursesById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => Map(new CourseData(), x)).ToArray();

                return new CourseResult
                {
                    Courses = courses
                };
            }
        }

        #endregion

        #region Update Course

        public async Task<CourseData> Apply(UpdateCourse request, Func<UpdateCourse, Task<CourseData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var data = request.Resource;
                if (Course == null && data != null)
                {
                    Course = await _repository.FetchByIdAsync<Course>(data.Id);
                    Env.Use(Course);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = Course?.RowVersion;
                return result;
            }
        }

        public Task<CourseData> Handle(UpdateCourse request)
        {
            Map(Course, request.Resource);

            return Task.FromResult(new CourseData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove Course

        public async Task<CourseData> Apply(
            RemoveCourse request, Func<RemoveCourse, Task<CourseData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (Course == null && resource != null)
                {
                    Course = await _repository.FetchByIdAsync<Course>(resource.Id);
                    Env.Use(Course);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();
                return result;
            }
        }

        public Task<CourseData> Handle(RemoveCourse request)
        {
            _repository.Context.Remove(Course);

            return Task.FromResult(new CourseData
            {
                Id         = Course.Id,
                RowVersion = Course.RowVersion
            });
        }

        #endregion


        #region Mapping

        public Course Map(Course course, CourseData data)
        {
            if (data.Name != null)
                course.Name = data.Name;

            if (data.Description != null)
                course.Description = data.Description;

            if (data.CreatedBy != null)
                course.CreatedBy = data.CreatedBy;

            if (data.ModifiedBy != null)
                course.ModifiedBy = data.ModifiedBy;

            course.Modified = _now;

            return course;
        }

        public CourseData Map(CourseData data, Course course)
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

        #endregion
    }
}