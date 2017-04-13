const paths        = require("../paths");
const gulp         = require("gulp");
const plumber      = require("gulp-plumber");
const sass         = require("gulp-sass");
const autoprefixer = require("gulp-autoprefixer");

gulp.task("buildStyles", ["buildApplicationStyles", "buildCssFiles", "buildCssDependencies", "buildFonts", "buildBootstrapFonts"]);

gulp.task("buildApplicationStyles", function () {
    return gulp.src(["src/styles/style.scss"])
     // .pipe(plumber())
      .pipe(sass.sync({
          outputStyle: "expanded",
          precision: 10,
          includePaths: ["."]
      }).on("error", sass.logError))
      .pipe(autoprefixer({ browsers: ["> 1%", "last 2 versions", "Firefox ESR"] }))
      .pipe(gulp.dest("built/styles"));
});

gulp.task("buildCssDependencies", () => {
  return gulp.src("bower_components/bootstrap-chosen/*.png")
    .pipe(gulp.dest(paths.dist + "styles"));
});

const fonts = [
    "src/styles/vendor/font-awesome/fonts/*",
    "src/styles/vendor/footable/fonts/*",
    "bower_components/bootstrap-sass/assets/fonts/bootstrap/*"
];

gulp.task("buildFonts", () => {
    return gulp.src(fonts)
        .pipe(gulp.dest(`${paths.built}fonts`))
        .pipe(gulp.dest(`${paths.dist}fonts`));
});

const bootstrapFonts = [
    "bower_components/bootstrap-sass/assets/fonts/bootstrap/*"
];

gulp.task("buildBootstrapFonts", () => {
    return gulp.src(bootstrapFonts)
        .pipe(gulp.dest(`${paths.built}fonts/bootstrap`))
        .pipe(gulp.dest(`${paths.dist}fonts/bootstrap`));
});

const css = [
    "src/styles/vendor/animate.css",
    "src/styles/vendor/datatables/datatables.min.css"
];

gulp.task("buildCssFiles", () => {
    return gulp.src(css)
        .pipe(gulp.dest(`${paths.built}styles`))
        .pipe(gulp.dest(`${paths.dist}styles`));
});
