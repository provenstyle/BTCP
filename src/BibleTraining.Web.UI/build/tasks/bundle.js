const paths      = require("../paths.js");
const gulp       = require("gulp");
const gulpIf     = require("gulp-if");
const useref     = require("gulp-useref");
const uglify     = require("gulp-uglify");
const cleanCss   = require("gulp-clean-css");
const rev        = require("gulp-rev");
const revReplace = require("gulp-rev-replace");
const concat     = require("gulp-concat");

gulp.task("bundle", ["buildStyles", "buildAngularTemplateCache", "bundleVendorJavascript"], function () {
    const assets = useref.assets({ searchPath: ["built", "."] });

    return gulp.src(["built/**/*.cshtml"])
      .pipe(assets)
      //.pipe(gulpIf("*.js",  uglify()))
      //.pipe(gulpIf("*.css", cleanCss()))
      .pipe(rev())
      .pipe(assets.restore())
      .pipe(useref())
      .pipe(revReplace({
          replaceInExtensions: [".cshtml"]
        }))
      .pipe(gulp.dest("dist"));
});

gulp.task("bundleVendorJavascript", () => {
    return gulp.src(paths.vendor)
        .pipe(concat("vendor.js"))
        .pipe(gulp.dest("built/scripts"));
});