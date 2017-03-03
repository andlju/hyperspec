/// <reference path="../../_references.js"/>

(function () {
    browseApp.directive('forms', [
    function () {
        return {
            scope: {
                model: '=',
                formType: '@',
                title: '@'
            },
            controller: ['$scope', function ($scope) {
                var tmpForms = $scope.model._forms[$scope.formType];
                if (Array.isArray(tmpForms)) {
                    $scope.forms = tmpForms;
                    $scope.form = tmpForms[0]; // First form
                } else {
                    $scope.forms = [tmpForms];
                    $scope.form = tmpForms; // First form
                }
            }],
            templateUrl: './scripts/browse_app/templates/forms.html',
            replace: true,
            restrict: 'E'
        };
    }
    ]);


})();