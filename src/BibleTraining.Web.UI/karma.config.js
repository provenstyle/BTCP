const paths        = require("./build/paths.js");
const babelOptions = require("./build/babel-options.js");

var files = [
    "bower_components/moment/moment.js",
    "bower_components/angular/angular.js",
    "bower_components/miruken-es5-angular/miruken-ng-bundle.js",
    "test/app/setup.js",
    "node_modules/babel-polyfill/dist/polyfill.min.js"]
    .concat(paths.files)
    .concat(["test/**/*.js"]);

module.exports = function (config) {
    config.set({
        frameworks: ["mocha", "chai", "sinon-chai"],

        files: files,

        preprocessors: {
            "src/**/*.js" : ["babel"],
            "test/**/*.js": ["babel"]
        },

        babelPreprocessor: {
            options: babelOptions
        },

        reporters: ["mocha"],

        port: 9876,

        colors: true,

        browsers: ["Chrome"]
    });
};
