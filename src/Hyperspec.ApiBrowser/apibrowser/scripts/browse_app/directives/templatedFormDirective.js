/// <reference path="../../_references.js"/>

(function () {
    browseApp.directive('templatedForm', function() {
        return {
            scope: {
                form: '='
            },
            controller: ['$scope', 'AppEvents', 'EVENTS', function($scope, AppEvents, EVENTS) {
                $scope.submitForm = function(e) {
                    var values = {};
                    var template = $scope.form.template;

                    for (var item in template) {
                        values[item] = template[item].value || template[item].defaultValue;
                    }
                    
                    AppEvents.dispatch({
                        type: EVENTS.Navigation.DispatchForm,
                        href: $scope.form.href,
                        method: $scope.form.method,
                        data: values
                    });
                };
            }],
            restrict: 'E',
            templateUrl: './scripts/browse_app/templates/templatedForm.html'
        };
    })
})();