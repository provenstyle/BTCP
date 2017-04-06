const paths = require("../paths");
const gulp  = require("gulp");

var images = [
    "src/img/**/*"
];

gulp.task("buildImages", () => {
    gulp.src(images)
        .pipe(gulp.dest(paths.built + "img"))
        .pipe(gulp.dest(paths.dist + "img"));
});