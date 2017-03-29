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
    "scripts/polyfill.min.js",
    "bower_components/jquery/dist/jquery.min.js",
    "bower_components/chosen/chosen.jquery.js",
    "bower_components/angular/angular.min.js",
    "bower_components/angular-ui-router/release/angular-ui-router.min.js",
    "bower_components/angular-messages/angular-messages.min.js",
    "bower_components/angular-chosen-localytics/dist/angular-chosen.min.js",
    "bower_components/miruken-es5-angular/miruken-ng-bundle.js",
    "bower_components/moment/min/moment.min.js",
    "bower_components/angular-cookies/angular-cookies.min.js",
    "bower_components/angular-sanitize/angular-sanitize.min.js",
    "bower_components/angular-localization/dist/angular-localization.min.js",
    "bower_components/bootstrap/dist/js/bootstrap.min.js"
];

paths.files = [
    //"Scripts/improving.web.ui/infrastructure/**/*.js",
    //"Scripts/improving.web.ui/serviceBus/*.js",
    "src/app/setup.js",
    "src/app/infrastructure/**/*.js",
    "src/app/api/**/*.js",
    "src/app/domain/**/*.js",
    "src/app/**/*Feature.js",
    "src/app/productGroup/updateProductGroupController.js",
    "src/app/productType/updateProductTypeController.js",
    "src/app/**/*.js"
];

module.exports = paths;