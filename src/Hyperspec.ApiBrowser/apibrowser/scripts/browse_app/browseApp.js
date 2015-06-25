
/// <reference path="_references.js"/>
var browseApp = angular.module('browseApp', ['messaging', 'ngRoute', 'hljs', 'ui.bootstrap']);

browseApp.config(['$routeProvider', function($routeProvider) {


}]);


// Make sure to instantiate any services that glue things together
browseApp.run(['LocationSvc', 'ContentSvc', function (LocationSvc, ContentSvc) {

}]);