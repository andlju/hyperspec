/// <reference path="../../_references.js"/>

(function () {

    browseApp.directive('links', [
        function () {
            return {
                scope: {
                    model: '=',
                    linkType: '@',
                    title: '@'
                },
                controller: ['$scope', 'UriTemplate', '$modal', 'AppEvents', 'EVENTS', function ($scope, UriTemplate, $modal, AppEvents, EVENTS) {
                    var tmpLinks = $scope.model._links[$scope.linkType];
                    if (Array.isArray(tmpLinks)) {
                        $scope.links = tmpLinks;
                        $scope.link = tmpLinks[0]; // First link
                    } else {
                        $scope.link = tmpLinks;
                    }

                    var navigate = function(href) {
                        AppEvents.dispatch({
                            type: EVENTS.Navigation.Navigate,
                            method: 'GET',
                            href: href
                        });
                    };

                    $scope.followLink = function (link) {
                        if (!link.templated) {
                            navigate(link.href);
                            return;
                        }
                        var uriTemplate = UriTemplate.parse(link.href);

                        $modal.open({
                            templateUrl: './scripts/browse_app/templates/templatedLink.html',
                            controller: 'LinkModalCtrl',
                            resolve: {
                                uriTemplate: function () { return uriTemplate; }
                            }
                        }).result.then(function(result) {
                            navigate(result);
                        });
                    };
                }],
                templateUrl: './scripts/browse_app/templates/links.html',
                replace: true,
                restrict: 'E'
            };
        }
    ]);

    browseApp.controller('LinkModalCtrl', ['$scope', '$modalInstance', 'uriTemplate', function ($scope, $modalInstance, uriTemplate) {

        $scope.linkTemplate = uriTemplate;

        $scope.template = {};

        var specs = angular.copy(uriTemplate.expressions.filter(function (e) { return e.varspecs; }));
        for (var i = 0; i < specs.length; i++) {
            for (var j = 0; j < specs[i].varspecs.length; j++) {
                var spec = specs[i].varspecs[j];
                $scope.template[spec.varname] = { value: '' };
            }
        }

        $scope.ok = function () {

            var params = {};
            for (var paramName in $scope.template) {
                var param = $scope.template[paramName];
                params[paramName] = param.value;
            }
            var resolved = $scope.linkTemplate.expand(params);
            $modalInstance.close(resolved);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);
})();