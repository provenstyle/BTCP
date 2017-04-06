const fs = require("fs");

const angularModuleName = "bt";
const appRoot           = "src/";
const pkg               = JSON.parse(fs.readFileSync("./package.json", "utf-8"));

const paths   = {
  root  : appRoot,
  source: [
    appRoot + "app/setup.js",
    appRoot + "**/*.js"
  ],
  html       : appRoot + "**/*.html",
  style      : appRoot + "styles/style.scss",
  index      : appRoot + "index.pug",
  built      : "built/",
  dist       : "dist/",
  packageName: pkg.name,
  ignore     : [],
  localUrl   : "http://localhost/BibleTraining",
  angularModuleName: angularModuleName
};

paths.vendor = [
    "src/scripts/polyfill.min.js",

    //jquery and boostrap
    "bower_components/jquery/dist/jquery.min.js",
    "bower_components/jquery-ui/jquery-ui.min.js",
    "bower_components/chosen/chosen.jquery.js",
    "bower_components/bootstrap/dist/js/bootstrap.min.js",
    "bower_components/metismenu/dist/metisMenu.min.js",
    "bower_components/slimScroll/jquery.slimscroll.min.js",
    "bower_components/pace/pace.min.js",
    "src/scripts/inspinia/inspinia.js",

    //Angular
    "bower_components/angular/angular.js",
    "bower_components/angular-ui-router/release/angular-ui-router.js",
    "bower_components/angular-messages/angular-messages.min.js",
    "bower_components/angular-sanitize/angular-sanitize.min.js",
    "bower_components/angular-idle/angular-idle.min.js",
    "bower_components/angular-translate/angular-translate.min.js",
    "bower_components/angular-chosen-localytics/dist/angular-chosen.min.js",
    "bower_components/angular-localization/dist/angular-localization.min.js",
    "bower_components/miruken-es5-angular/miruken-ng-bundle.js",
    "bower_components/moment/min/moment.min.js",
    "src/scripts/bootstrap/ui-bootstrap-tpls-1.1.2.min.js",
    "src/scripts/inspinia/app.js",
    "src/scripts/inspinia/directives.js"


    //"bower_components/angular-cookies/angular-cookies.min.js",
    //"bower_components/angular-sanitize/angular-sanitize.min.js",
];

paths.files = [
    "src/app/bt.js",
    "src/app/infrastructure/**/*.js",
    "src/app/setup.js",
    "src/app/domain/**/*.js",
    "src/app/**/*Feature.js",
    "src/app/**/*.js"
];

module.exports = paths;