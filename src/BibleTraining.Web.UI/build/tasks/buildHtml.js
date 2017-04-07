const paths   = require("../paths");
const gulp    = require("gulp");
const pug     = require('gulp-pug');
const plumber = require('gulp-plumber');

gulp.task("buildHtml", () => {
    return gulp.src(["src/**/*.pug"])
        .pipe(plumber())
    	.pipe(pug({
    		pretty: true
    	}))
        .pipe(gulp.dest(paths.built));
});
