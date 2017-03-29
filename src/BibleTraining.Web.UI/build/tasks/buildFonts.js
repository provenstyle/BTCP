const paths = require("../paths");
const gulp  = require("gulp");

const fonts = [
    "Content/improving.web.ui/fonts/**/*"
];

gulp.task("buildFonts", () => {
    return gulp.src(fonts)
        .pipe(gulp.dest(`${paths.built}fonts`))
        .pipe(gulp.dest(`${paths.dist}fonts`));
});
