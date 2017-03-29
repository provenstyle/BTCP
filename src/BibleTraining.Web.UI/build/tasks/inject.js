const paths   = require("../paths");
const gulp    = require("gulp");
const inject  = require("gulp-inject");
const wiredep = require('wiredep').stream;

const sources = () => gulp.src(paths.files, { read: false });

//inject bower components and application javascript into index.cshtml
gulp.task("buildRazor", function () {
    return gulp.src(["Features/**/*.cshtml", "Features/**/Web.config"])
        .pipe(gulp.dest("built/Features"));
});

gulp.task("inject:karma", function () {
    return gulp.src("karma.config.js")
        .pipe(wiredep({
            fileTypes: {
                js: {
                    block: /(([ \t]*)\/\*\s*bower:*(\S*)\s*\*\/)(\n|\r|.)*?(\/\*\s*endbower\s*\*\/)/gi,
                    detect: {
                        js: /['"]([^'"]+)/gi
                    },
                    replace: {
                        js: '"../{{filePath}}",'
                    }
                }
            }
        }))
        .pipe(inject(sources(), {
            starttag: "/* inject:js */",
            endtag: "/* endinject */",
            transform: (filepath, file, i, length) => {
                const path = filepath.replace("src", "built");
                return `"..${path}",`;
            }
        }))
        .pipe(gulp.dest("built"));
});
