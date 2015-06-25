/// <reference path="../../_references.js"/>

(function() {

    browseApp
        .factory('ContentEvents', ['EventDispatcher', function(EventDispatcher) {
            return EventDispatcher.getInstance("ContentEvents");
        }])
        .factory('AppEvents', ['EventDispatcher', function (EventDispatcher) {
            return EventDispatcher.getInstance("AppEvents");
        }]);

})();
