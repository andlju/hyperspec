/// <reference path="../../_references.js"/>

(function() {
    browseApp.directive('resource', ['RecursionHelper', function(RecursionHelper) {

        return {
            scope: {
                model: '='
            },
            templateUrl: './scripts/browse_app/templates/resource.html',
            replace: true,
            restrict: 'E',
            compile: function (element) {
                return RecursionHelper.compile(element);
            }
        };
    }]);
})();