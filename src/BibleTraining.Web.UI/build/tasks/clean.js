const del  = require("del");
const gulp = require("gulp");

gulp.task("clean", cb => {
    del(["built", "dist"], cb);
});

