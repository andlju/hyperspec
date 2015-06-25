
(function () {
    if (!UriTemplate)
        throw 'UriTemplate not defined. Make sure uritemplate.js is included';

    browseApp.factory('UriTemplate', function() {
        return UriTemplate;
    });

})();