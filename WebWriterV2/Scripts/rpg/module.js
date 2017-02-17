
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('rpg', ['directives', 'services', 'underscore', 'ngRoute', 'ngSanitize', 'ngCookies', 'ngAnimate', 'AppConst'])
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
                .when('/AngularRoute/admin/Skill', {
                    templateUrl: '/views/rpg/admin/Skill.html',
                    controller: 'adminSkillController'
                })
                .when('/AngularRoute/admin/Thing', {
                    templateUrl: '/views/rpg/admin/thing.html',
                    controller: 'adminThingController'
                })
                .when('/AngularRoute/admin/State', {
                    templateUrl: '/views/rpg/admin/state.html',
                    controller: 'adminStateController'
                })
                .when('/AngularRoute/admin/Characteristic', {
                    templateUrl: '/views/rpg/admin/Characteristic.html',
                    controller: 'adminCharacteristicController'
                })
                .when('/AngularRoute/admin/QuestOld', {
                    templateUrl: '/views/rpg/admin/QuestOld.html',
                    controller: 'adminQuestController'
                })
                .when('/AngularRoute/admin/quest/:questId/event/:eventId?', {
                    templateUrl: '/views/rpg/admin/Event.html',
                    controller: 'adminEventGeneralController'
                })
                .when('/AngularRoute/admin/quest/:questId?', {
                    templateUrl: '/views/rpg/admin/Quest.html',
                    controller: 'adminQuestGeneralController'
                })
                /* front */
                .when('/AngularRoute/login', {
                    templateUrl: '/views/rpg/Login.html',
                    controller: 'loginController'
                })
                .when('/AngularRoute/guild', {
                    templateUrl: '/views/rpg/Guild.html',
                    controller: 'guildController'
                })
                .when('/AngularRoute/createHero', {
                    templateUrl: '/views/rpg/CreateHero.html',
                    controller: 'createHeroController'
                })
                .when('/AngularRoute/listHeroes', {
                    templateUrl: '/views/rpg/ListHeroes.html',
                    controller: 'listHeroesController'
                })
                .when('/AngularRoute/listQuest', {
                    templateUrl: '/views/rpg/ListQuest.html',
                    controller: 'listQuestController'
                })
                .when('/AngularRoute/travel/quest/:questId/hero/:heroId/:isBookmark', {
                    templateUrl: '/views/rpg/Travel.html',
                    controller: 'travelController'
                })
                .when('/AngularRoute/traningRoom/:roomId/hero/:heroId', {
                    templateUrl: '/views/rpg/TraningRoom.html',
                    controller: 'traningRoomController'
                })
                .when('/AngularRoute/battle', {
                    templateUrl: '/views/rpg/battle.html',
                    controller: 'battleController'
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