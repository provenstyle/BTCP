require("require-dir")("build/tasks");

const gulp = require('gulp');

gulp.task('default', ['buildanddist']);

