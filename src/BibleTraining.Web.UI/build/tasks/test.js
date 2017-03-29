const gulp        = require("gulp");
const KarmaServer = require("karma").Server;
const path        = require('path');

const karmaConfig = path.join(__dirname, "../../karma.config.js");

gulp.task("test", ["build"], callback => {
    new KarmaServer({
        configFile: karmaConfig,
        autoWatch: false,
        singleRun: true
    }, callback).start();
});

gulp.task("watchTest", callback => {
    new KarmaServer({
        configFile: karmaConfig,
        autoWatch: true,
        singleRun: false,
        autoWatchBatchDelay: delay,
        usePolling: true,  // important! this fixes an issue with the browser not downloading the latest files in chrome
        browsers: ["Chrome"]
    }).start();
});

gulp.task("ciTest", ["build"], callback => {
    new KarmaServer({
        configFile:karmaConfig,
        autoWatch: false,
        singleRun: true,
        browsers: ["Chrome"],
        reporters: ["teamcity"]
    }, karmaCompleted).start();

    function karmaCompleted(karmaResult) {
        console.log("Karma completed");
        console.log("KarmaResult: " + karmaResult);
        if (karmaResult === 1) {
            callback("karma: tests failed with code " + karmaResult);
        } else {
            callback();
        }
    }
});
