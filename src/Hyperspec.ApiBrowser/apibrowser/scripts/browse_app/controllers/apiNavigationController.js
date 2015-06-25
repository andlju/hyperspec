/// <reference path="../../_references.js"/>

(function () {

    browseApp.controller('ApiNavigationCtrl', ['$scope', 'AppEvents', 'EVENTS', function ($scope, AppEvents, EVENTS) {
        
        $scope.navigate = function () {
            AppEvents.dispatch({
                type: EVENTS.Navigation.Navigate,
                href: $scope.apiUrl
            });
        };

        var eventHandler = function (msg) {
            switch (msg.type) {
                case EVENTS.Navigation.Navigated:
                    $scope.apiUrl = msg.href;
                    break;
                default:
            }
        };

        AppEvents.addListener(eventHandler);

    }]);
})();