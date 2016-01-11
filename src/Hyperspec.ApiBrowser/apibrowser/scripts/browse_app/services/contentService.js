/// <reference path="../../_references.js"/>

(function() {
    browseApp.factory('ContentSvc', ['$http', '$modal', 'AppEvents', 'ContentEvents', 'EVENTS', function($http, $modal, AppEvents, ContentEvents, EVENTS) {

        function buildUrl(url, params) {
            if (!params) return url;
            var parts = [];

            angular.forEach(params, function (value, key) {
                if (value === null || angular.isUndefined(value)) return;
                if (!angular.isArray(value)) value = [value];

                angular.forEach(value, function (v) {
                    if (angular.isObject(v)) {
                        v = toJson(v);
                    }
                    parts.push(encodeURIComponent(key) + '=' +
                               encodeURIComponent(v));
                });
            });
            return url + ((url.indexOf('?') === -1) ? '?' : '&') + parts.join('&');
        }

        AppEvents.addListener(function(msg) {
            var href = msg.href;
            var url = href;
            var method = msg.method;
            var data = msg.data;

            if (msg.type === EVENTS.Navigation.DispatchForm) {
                method = method || 'POST';
                $http({
                    method: method,
                    url: url,
                    headers: {
                        'Accept': 'application/hal+json'
                    },
                    data: data
                }).then(function(responseconfig) {

                        $modal.open({
                            templateUrl: './scripts/browse_app/templates/dispatchResult.html',
                            controller: 'ResourcePopupCtrl',
                            resolve: {
                                response: function () { return response; }
                            }
                        }).result.then(function(result) {

                            console.log(result);
                            AppEvents.dispatch({
                                type: EVENTS.Navigation.Navigate,
                                href: result
                            });

                        });

                    },
                    function(response) {
                        $modal.open({
                            templateUrl: './scripts/browse_app/templates/dispatchResult.html',
                            controller: 'ResourcePopupCtrl',
                            resolve: {
                                response: function () { return response; }
                            }
                        });
                    });
            }

            if (msg.type === EVENTS.Navigation.Navigate) {
                data = null;
                method = method || 'GET';
                if (method === 'GET') {
                    url = buildUrl(href, msg.data);
                } else {
                    data = msg.data;
                }
                console.log("Fetching ", href);

                $http({
                    method: method,
                    url: url,
                    headers: {
                        'Accept': 'application/hal+json'
                    },
                    data: data
                }).then(function(response) {
                        AppEvents.dispatch({
                            type: EVENTS.Navigation.Navigated,
                            href: url
                        });
                        ContentEvents.dispatch(response.data);
                    },
                    function(response) {
                        $modal.open({
                            templateUrl: './scripts/browse_app/templates/dispatchResult.html',
                            controller: 'ResourcePopupCtrl',
                            resolve: {
                                response: function() { return response; }
                            }
                        });
                    });

            }
        });

        return {};
    }]);

    browseApp.controller('ResourcePopupCtrl', ['$scope', '$modalInstance', 'response', function ($scope, $modalInstance, response) {

        var decorateModel = function(model) {
            model._src = getRawJson(model, false);
            for (var embeddedName in model._embedded) {
                for (var i = 0; i < model._embedded[embeddedName].length; i++) {
                    var childModel = model._embedded[embeddedName][i];
                    decorateModel(childModel);
                }
            }
        };

        var getRawJson = function(model, includeHalProperties) {
            var raw = {};
            for (var prop in model) {
                if (prop == '_src' || !includeHalProperties && prop.charAt(0) === '_')
                    continue;

                raw[prop] = model[prop];
            }
            return JSON.stringify(raw, null, 2);
        };
        if (!response.data) {
            response.data = {};
        }
        $scope._src = JSON.stringify(response.data, null, 2);
        decorateModel(response.data);
        $scope.content = response.data;
        $scope.status = response.status;
        if (response.headers) {
            $scope.location = response.headers('Location');
        }

        $scope.followLocation = function () {
            $modalInstance.close($scope.location);
        };

        $scope.close = function () {
            $modalInstance.dismiss('close');
        };

    }]);

})();