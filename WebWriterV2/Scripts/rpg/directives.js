
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
                    var maxHpValue = 1;
                    var maxMpValue = 2;
                    var currentHpValue = 4;
                    var currentMpValue = 5;

                    $scope.GetTextSex = sexService.getSexWord;
                    $scope.GetTextRace = raceService.getRaceWord;

                    //TODO Find way to watch by hero
                    $scope.recalc = function () {
                        $scope.recalculateState($scope.hero);
                        return '';
                    };

                    $scope.Hp = function() {
                        return _.where($scope.hero.State, { Value: currentHpValue })[0].Number;
                    }
                    $scope.MaxHp = function () {
                        return _.where($scope.hero.State, { Value: maxHpValue })[0].Number;
                    }
                    $scope.Mp = function () {
                        return _.where($scope.hero.State, { Value: currentMpValue })[0].Number;
                    }
                    $scope.MaxMp = function () {
                        return _.where($scope.hero.State, { Value: maxMpValue })[0].Number;
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