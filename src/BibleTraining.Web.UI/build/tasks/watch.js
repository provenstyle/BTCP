const paths       = require("../paths");
const gulp        = require("gulp");
const runSequence = require("run-sequence");

//Watch the src files
gulp.task("watch", callback => {

    gulp.watch("src/styles/**/*.scss", ["buildStyles"]);

    gulp.watch([paths.html, "src/**/*.pug"], ["buildHtml"]);

    gulp.watch("Features/**/*.cshtml", ["buildRazor"]);

    gulp.watch([
        "src/**/*.js",
        "build/paths.js"
    ]).on("change", function () {
        runSequence(["lint", "buildJavascript"]);
    });

    gulp.watch("test/**/*.js").on("change", function() {
        runSequence(["lint"]);
    });
});

