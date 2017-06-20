
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
        function($locationProvider, $routeProvider, $httpProvider) {
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
                .when('/AngularRoute/admin/quest/:questId/event/:eventId?', {
                    templateUrl: '/views/rpg/admin/Event.html',
                    controller: 'adminEventGeneralController'
                })
                .when('/AngularRoute/admin/quest/:questId?', {
                    templateUrl: '/views/rpg/admin/Quest.html',
                    controller: 'adminQuestGeneralController'
                })
                .when('/AngularRoute/admin/genres', {
                    templateUrl: '/views/rpg/admin/Genre.html',
                    controller: 'adminGenreController'
                })
                /* front */
                .when('/AngularRoute/register', {
                    templateUrl: '/views/rpg/Register.html',
                    controller: 'registerController'
                })
                .when('/AngularRoute/aboutUs', {
                    templateUrl: '/views/rpg/AboutUs.html',
                    controller: 'aboutUsController'
                })
                .when('/AngularRoute/generalDefinition', {
                    templateUrl: '/views/rpg/GeneralDefinition.html',
                    controller: 'aboutUsController'
                })
                .when('/AngularRoute/listQuest', {
                    templateUrl: '/views/rpg/ListQuest.html',
                    controller: 'listQuestController'
                })
                .when('/AngularRoute/travel/quest/:questId/hero/:heroId/:isBookmark', {
                    templateUrl: '/views/rpg/Travel.html',
                    controller: 'travelController'
                })
                .when('/AngularRoute/profile', {
                    templateUrl: '/views/rpg/profile.html',
                    controller: 'profileController'
                })
                .otherwise({
                    redirectTo: '/AngularRoute/listQuest'
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