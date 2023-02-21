/// <binding BeforeBuild='clean, min' Clean='clean' />
var gulp = require("gulp"),
  rimraf = require("rimraf"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-cssmin"),
  uglify = require("gulp-uglify");

var paths = {
    webroot: "./wwwroot/"
};

paths.adminJs = paths.webroot + "app/**/*.js";
paths.DevextremeJs = paths.webroot + "js/devextreme/dx.all.js";

paths.minJs = paths.webroot + "**/*.min.js";

paths.adminCss = paths.webroot + "css/*.css";
paths.DevextremeCss = paths.webroot + "css/devextreme/dx.light.css";

paths.minCss = paths.webroot + "**/*.min.css";

paths.concatJsAdminDest = paths.webroot + "bundled/site-admin.min.js";
paths.concatAdminCssDest = paths.webroot + "bundled/site-admin.min.css";

paths.concatDevextremeCssDest = paths.webroot + "bundled/site-Devextreme.min.css";
paths.concatDevextremeJsDest = paths.webroot + "bundled/site-Devextreme.min.js";


gulp.task("clean:adminJs", function (cb) {
    rimraf(paths.concatJsAdminDest, cb);
});

gulp.task("clean:devextremeJS", function (cb) {
    rimraf(paths.concatDevextremeJsDest, cb);
});

gulp.task("clean:adminCss", function (cb) {
    rimraf(paths.concatAdminCssDest, cb);
});

gulp.task("clean:devextremeCss", function (cb) {
    rimraf(paths.concatDevextremeCssDest, cb);
});

gulp.task("clean", gulp.series("clean:adminJs", "clean:adminCss", "clean:devextremeJS", "clean:devextremeCss"));

gulp.task("min:adminJs", function () {
    return gulp.src([paths.adminJs, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsAdminDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:devextremeJS", function () {
    return gulp.src([paths.DevextremeJs, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatDevextremeJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:adminCss", function () {
    return gulp.src([paths.adminCss, "!" + paths.minCss])
        .pipe(concat(paths.concatAdminCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min:devextremeCss", function () {
    return gulp.src([paths.DevextremeCss, "!" + paths.minCss])
        .pipe(concat(paths.concatDevextremeCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", gulp.series("min:adminJs", "min:adminCss", "min:devextremeJS", "min:devextremeCss"));