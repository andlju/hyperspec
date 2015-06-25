/// <reference path="../../_references.js"/>

(function () {
    
    browseApp.factory('LocationSvc', ['$rootScope', '$location', 'AppEvents', 'EVENTS', function ($rootScope, $location, AppEvents, EVENTS) {

        $rootScope.$on('$locationChangeSuccess', function (e, newUrl, oldUrl) {
            var href = $location.search().apiUrl || '/';

            AppEvents.dispatch({
                type: EVENTS.Navigation.Navigated,
                href: href
            });

        });
        
        var appEventListener = function (msg) {
            
            if (msg.type === EVENTS.Navigation.Navigate) {
                var href = msg.href;
                $location.search({ apiUrl: href });
            }
        };

        AppEvents.addListener(appEventListener);

        return {

        };
    }]);
})();