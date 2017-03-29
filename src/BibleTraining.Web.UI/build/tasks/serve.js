const paths       = require("../paths");
const gulp        = require("gulp");
const browserSync = require('browser-sync');
const reload      = browserSync.reload;

gulp.task('serve', ['build'], callback => {
    gulp.watch([
        'built/**/*',
        '!built/**/*.css',
        'src/**/*.html'
    ]).on('change', reload);

    gulp.watch([
        'built/**/*.css'
    ]).on('change', () => reload({stream: true}));

    browserSync({
        notify: false,
        proxy:  paths.localUrl,
        port:   '7200'
    });
});
