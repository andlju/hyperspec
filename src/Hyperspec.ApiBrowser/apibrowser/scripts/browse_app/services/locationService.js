/// <reference path="../../_references.js"/>

(function () {
    
    browseApp.factory('LocationSvc', ['$rootScope', '$location', 'AppEvents', 'EVENTS', function ($rootScope, $location, AppEvents, EVENTS) {

        var triggerNavigation = true;

        $rootScope.$on('$locationChangeSuccess', function (e, newUrl, oldUrl) {
            var href = $location.search().apiUrl || '/';

            if (triggerNavigation) {
                console.log("Triggering navigation from locationChange");
                AppEvents.dispatch({
                    type: EVENTS.Navigation.Navigate,
                    href: href
                });
            } else {
                console.log("Ignoring this locationChange");
                triggerNavigation = true; // Reset so that we trigger the navigation again next time
            }

        });
        
        var appEventListener = function (msg) {
            
            if (msg.type === EVENTS.Navigation.Navigated) {
                var href = msg.href;
                if ($location.search().apiUrl !== href) {
                    console.log("Automatically setting the search property. Ignore next navigation change");
                    triggerNavigation = false; // Set this so that we don't send yet another navigation event
                    $location.search({ apiUrl: href });
                } else {
                    console.log("ApiUrl not changed. Don't bother updating it");
                }
            }
        };

        AppEvents.addListener(appEventListener);

        return {

        };
    }]);
})();