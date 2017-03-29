const gulp        = require("gulp");
const runSequence = require("run-sequence");
const mkdirp      = require('mkdirp');
const fs          = require('fs');

gulp.task("build", callback => {
    runSequence("clean", ["lint", "buildStyles", "buildHtml", "buildImages", "buildFonts", "buildRazor", "buildJavascript", "bundleVendorJavascript", "clearTemplate", "inject:karma"], callback);
});

//Kind of a hack. In debug we don't want bundled templates for angular
//but do want it bundled in the distribution
gulp.task("clearTemplate", cb => {
    mkdirp("built/app", () => {
        fs.writeFile("built/app/templates.js",
            "//gulp will build the templates for distribution",
            cb);
    });
});