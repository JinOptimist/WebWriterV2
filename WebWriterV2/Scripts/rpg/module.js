
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('rpg', ['directives', 'services', 'underscore', 'ui.bootstrap', 'ngRoute', 'ngSanitize', 'ngCookies', 'ngAnimate', 'AppConst'])
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
                /* admin */
                .when('/AngularRoute/admin/Thing', {
                    templateUrl: '/views/rpg/admin/thing.html',
                    controller: 'adminThingController'
                })
                .when('/AngularRoute/admin/State', {
                    templateUrl: '/views/rpg/admin/state.html',
                    controller: 'adminStateController'
                })
                .when('/AngularRoute/admin/book/:bookId/event/:eventId?', {
                    templateUrl: '/views/rpg/admin/Event.html',
                    controller: 'adminEventGeneralController'
                })
                .when('/AngularRoute/admin/book/:bookId?', {
                    templateUrl: '/views/rpg/admin/Book.html',
                    controller: 'adminBookGeneralController'
                })
                .when('/AngularRoute/admin/genres', {
                    templateUrl: '/views/rpg/admin/Genre.html',
                    controller: 'adminGenreController'
                })
                /* front */
                
                .when('/AngularRoute/aboutUs', {
                    templateUrl: '/views/rpg/AboutUs.html',
                    controller: 'aboutUsController'
                })
                .when('/AngularRoute/generalDefinition', {
                    templateUrl: '/views/rpg/GeneralDefinition.html',
                    controller: 'aboutUsController'
                })
                .when('/AngularRoute/listBook', {
                    templateUrl: '/views/rpg/ListBook.html',
                    controller: 'listBookController'
                })
                .when('/AngularRoute/travel/book/:bookId/event/:eventId/hero/:heroId/:isBookmark', {
                    templateUrl: '/views/rpg/Travel.html',
                    controller: 'travelController'
                })
                .when('/AngularRoute/profile', {
                    templateUrl: '/views/rpg/profile.html',
                    controller: 'profileControllerOld'
                })

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
                .when('/ar/reader/travel/:travelId', { // ! Most important page !
                    templateUrl: '/views/reader/Travel.html',
                    controller: 'travelController'
                })
                .when('/ar/reader/books', {
                    templateUrl: '/views/reader/Books.html',
                    controller: 'readerBooksController'
                })
                .when('/ar/reader/login', {
                    templateUrl: '/views/reader/Login.html',
                    controller: 'loginController'
                })
                .when('/ar/reader/register', {
                    templateUrl: '/views/reader/Register.html',
                    controller: 'registerController'
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