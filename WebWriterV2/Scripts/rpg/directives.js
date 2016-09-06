
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('directives', ['services', 'underscore']) //, ['common', 'search', 'masha', 'ui.ace']
    .directive('ngQuestInfo', [
        function () {
            return {
                restrict: 'E',
                replace: true,
                templateUrl: '/views/rpg/directives/ngQuestInfo.html',
                scope: { quest: '=' },
                link: function (scope, element, attrs) {
                    scope.$watch(attrs.quest, function (v) {
                        console.log('Quest value changed, new value is: ' + v);
                    });
                }
            }
        }
    ])
    .directive('ngHeroCard', [
        function () {
            return {
                restrict: 'E',
                replace: true,
                templateUrl: '/views/rpg/directives/ngHeroCard.html',
                scope: { hero: '=' },
                controller: ['$scope', 'raceService', 'sexService', function ($scope, raceService, sexService) {
                    $scope.GetTextSex = sexService.getSexWord;
                    $scope.GetTextRace = raceService.getRaceWord;
                }],
                link: function (scope, element, attrs) {
                    scope.$watch(attrs.hero, function (v) {
                        console.log('Hero value changed, new value is: ' + v);
                    });
                }
            }
        }
    ])
    .directive('ngGuild', [
        function () {
            return {
                restrict: 'E',
                replace: true,
                templateUrl: '/views/rpg/directives/ngGuild.html',
                scope: { guild: '=' },
                controller: ['$scope', function ($scope) {
                    //$scope.GetTextSex = sexService.getSexWord;
                    //$scope.GetTextRace = raceService.getRaceWord;
                }]
            }
        }
    ])
    .constant('_',
        window._
    );