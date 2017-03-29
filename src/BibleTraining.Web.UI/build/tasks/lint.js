const gulp   = require("gulp");
const eslint = require("gulp-eslint");

gulp.task("lint", () => {
    return gulp.src(["src/app/**/*.js", "test/**/*.js"])
        .pipe(eslint())
        .pipe(eslint.format());
});
