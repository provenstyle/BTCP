const gulp        = require("gulp");
const runSequence = require("run-sequence");
const size        = require("gulp-size");
const wait        = require("gulp-wait");

gulp.task("dist", callback => {
    runSequence("wait", ["buildAngularTemplateCache", "bundle", "webConfig", "kendoImages"], "distSize", callback);
});

gulp.task("buildanddist", ["build"], callback => {
    runSequence("dist", callback);
});

gulp.task("testanddist", callback => {
    runSequence("test", "dist", callback);
});

gulp.task("citestanddist", callback => {
    runSequence("ciTest", "dist", callback);
});

gulp.task("distSize", () => {
    return gulp.src("dist/**/*").pipe(size({ title: "built", gzip: true }));
});

gulp.task("webConfig", () => {
    return gulp.src([
      "built/Features/Web.config"
    ]).pipe(gulp.dest("dist/Features"));
});

gulp.task("kendoImages", () => {
    return gulp.src([
      "Content/kendo/2014.2.716/Default/**"
    ]).pipe(gulp.dest("dist/styles/Default"));
});

gulp.task("wait", () => {
    return gulp.src("")
        .pipe(wait(4000));
});
