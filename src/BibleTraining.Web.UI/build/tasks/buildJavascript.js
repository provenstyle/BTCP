const paths        = require("../paths");
const gulp         = require("gulp");
const babel        = require("gulp-babel");
const babelOptions = require("../babel-options");
const concat       = require("gulp-concat");
const plumber      = require("gulp-plumber");

gulp.task("buildJavascript", function () {
    return gulp.src(paths.files)
        .pipe(plumber())
        //vs.pipe(babel(babelOptions))
        .pipe(concat("app.js"))
        .pipe(gulp.dest("built/scripts"));
});
