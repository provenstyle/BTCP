const paths = require("../paths");
const gulp  = require("gulp");

var images = [
    "Content/improving.web.ui/images/logo.png"
];

gulp.task("buildImages", () => {
    gulp.src(images)
        .pipe(gulp.dest(paths.built + "images"))
        .pipe(gulp.dest(paths.dist + "images"));
});