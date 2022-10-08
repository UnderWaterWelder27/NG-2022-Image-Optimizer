/// <binding Clean='clean' />
"use strict";
 
var gulp = require("gulp"),
    sass = require('gulp-sass')(require('sass'));
 
var paths = {
    webroot: "./wwwroot/"
};

gulp.task("sass", function () {
    return gulp.src('Sass/main.scss')
            .pipe(sass())
            .pipe(gulp.dest(paths.webroot + '/css'));
});