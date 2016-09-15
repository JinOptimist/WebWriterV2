
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
                    var maxHpStateName = "MaxHp";
                    var maxMpStateName = "MaxMp";
                    var hpStateName = "Hp";
                    var mpStateName = "Mp";

                    $scope.GetTextSex = sexService.getSexWord;
                    $scope.GetTextRace = raceService.getRaceWord;

                    //TODO Find way to watch by hero
                    $scope.recalc = function () {
                        $scope.recalculateState($scope.hero);
                        return '';
                    };

                    function getState(stateName) {
                        var currentState = _.find($scope.hero.State, function(state) {
                            return state.StateType.Name == stateName;
                        });
                        return currentState.Number;
                    }

                    $scope.Hp = function () {
                        return getState(hpStateName);
                    }
                    $scope.MaxHp = function () {
                        return getState(maxHpStateName);
                    }
                    $scope.Mp = function () {
                        return getState(mpStateName);
                    }
                    $scope.MaxMp = function () {
                        return getState(maxMpStateName);
                    }

                    $scope.recalculateState = function () {
                        if (!$scope.hero || !$scope.hero.State)
                            return;

                        var hpPercent = Math.round(100 * $scope.Hp() / $scope.MaxHp());
                        var mpPercent = Math.round(100 * $scope.Mp() / $scope.MaxMp());

                        if ($scope.Hp() < 1) {
                            alert($scope.hero.Name + ' is dead');
                        }

                        $scope.hero.healthWidth = { 'width': hpPercent + '%' };
                        $scope.hero.manaWidth = { 'width': mpPercent + '%' };
                    }

                    $scope.recalculateState($scope.hero);

                    $scope.$watch(function () { return $scope.hero; }, function (v) {
                        $scope.recalculateState($scope.hero);
                    });

                }],
                link: function (scope, element, attrs) {

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