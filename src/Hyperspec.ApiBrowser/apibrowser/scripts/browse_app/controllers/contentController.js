/// <reference path="../../_references.js"/>

(function () {

    browseApp.controller('ContentCtrl', ['$scope', 'ContentEvents', 'EVENTS', function($scope, ContentEvents, EVENTS) {

        var getRawJson = function(model, includeHalProperties) {
            var raw = {};
            for (var prop in model) {
                if (prop === '_src' || !includeHalProperties && prop.charAt(0) === '_')
                    continue;

                raw[prop] = model[prop];
            }
            return JSON.stringify(raw, null, 2);
        };

        var decorateModel = function (model) {
            model._src = getRawJson(model, false);
            for (var embeddedName in model._embedded) {

                if (!Array.isArray(model._embedded[embeddedName])) {
                    model._embedded[embeddedName] = [model._embedded[embeddedName]];
                }
                for (var i = 0; i < model._embedded[embeddedName].length; i++) {
                    var childModel = model._embedded[embeddedName][i];
                    decorateModel(childModel);
                }
            }
        };

        ContentEvents.addListener(function (msg) {
            $scope._src = JSON.stringify(msg, null, 2);
            decorateModel(msg);
            var links = {};
            for (var linkType in msg._links) {
                if (linkType !== 'profile') {
                    links[linkType] = msg._links[linkType];
                }
            }
            $scope.content = msg;
            $scope.content.profile = msg._links['profile'];
            $scope.content._links = links;
        });
    }]);
})();