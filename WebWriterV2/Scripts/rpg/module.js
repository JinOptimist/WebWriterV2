
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('rpg', ['directives', 'services', 'AppConst', 'underscore', 'ui.bootstrap', 'ngRoute', 'ngSanitize', 'ngCookies', 'ngAnimate', 'ngMaterial'])
    .constant('_',
        window._
    )
    .config([
        '$locationProvider', '$routeProvider', '$httpProvider',
        function ($locationProvider, $routeProvider, $httpProvider) {
            // Check login
            $httpProvider.interceptors.push('authInterceptorService');
            // Routes configuration
            $routeProvider
                /* writer */
                //resolve: resolveController('/app/controllers/customersController.js')
                .when('/ar/writer/book/:bookId', {
                    templateUrl: '/views/writer/Book.html',
                    controller: 'writerBookController',
                })
                .when('/ar/writer/books', {
                    templateUrl: '/views/writer/Books.html',
                    controller: 'writerBooksController'
                })
                .when('/ar/writer/chapter/:chapterId', {
                    templateUrl: '/views/writer/Chapter.html',
                    controller: 'chapterController'
                })
                .when('/ar/writer/generalDefinition', {
                    templateUrl: '/views/rpg/GeneralDefinition.html',
                    controller: 'aboutUsController'
                })
                /* reader */
                .when('/ar/reader/travel/:travelId/:chapterId?', { // ! Most important page !
                    templateUrl: '/views/reader/Travel.html',
                    controller: 'travelController'
                })
                .when('/ar/reader/books', {
                    templateUrl: '/views/reader/Books.html',
                    controller: 'readerBooksController'
                })
                .when('/ar/reader/profile/:userId', {
                    templateUrl: '/views/reader/Profile.html',
                    controller: 'profileController'
                })
                
                .otherwise({
                    redirectTo: '/ar/reader/books'
                });

            // Uses HTLM5 history API for navigation
            $locationProvider.html5Mode(true);
        }
    ])
    //Hack to publish raw HTML code with style. Use like that {{Desc | trusted}}
    .filter('trusted', function($sce) {
        return function(html) {
            return $sce.trustAsHtml(html);
        }
    });