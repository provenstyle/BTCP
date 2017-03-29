const paths = require("../paths");
const gulp = require("gulp");
const templateCache = require("gulp-angular-templatecache");

gulp.task('buildAngularTemplateCache', function () {
    return gulp.src('built/**/*.html')
      .pipe(templateCache({
          module: paths.angularModuleName,
          root: ''
      }))
      .pipe(gulp.dest('built/app'));
});
