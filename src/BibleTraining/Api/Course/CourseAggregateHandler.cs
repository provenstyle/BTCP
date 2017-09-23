namespace BibleTraining.Api.Course
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken.Mediate;
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
                var course = new Course().Map(message.Resource);
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
                })).Select(x => new CourseData().Map(x)).ToArray();

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
                var resource = request.Resource;
                if (Course == null && resource != null)
                {
                    Course = await _repository.FetchByIdAsync<Course>(resource.Id);
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
            Course.Map(request.Resource);

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

    }
}