
angular.module('rpg', [])//, ['ngRoute', 'common', 'search', 'masha', 'ui.ace']
    .directive('ngHeroInfo', [function () {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'views/rpg/ngHeroInfo.html'
        }
    }])
    //.run(['$http', '$timeout', '_', function ($http, $timeout, _) {
    //    // Loading User Picture Url
    //    $http({
    //        method: 'GET',
    //        url: '/_api/SP.UserProfiles.PeopleManager/GetMyProperties/PictureUrl',
    //        headers: { 'Accept': 'application/json;odata=verbose' }
    //    }).success(function (response) {
    //        $timeout(function () {
    //            $rootScope.UserPictureUrl = response.d.PictureUrl;
    //        }, 0);
    //    });
    //}])
    .controller('heroCreateController', [
        '$scope', "$http", //'$modal', '$route', '_', '$log',
        function($scope, $http) { //, $modal, $route, _, $log
            $scope.quest = undefined; //GeneralInfo, AdditionalInfo
            $scope.heroes = undefined;
            $scope.currentHero = undefined;

            $scope.GetQuest = function() {
                $http({
                    method: 'GET',
                    url: '/WebWriterV2/Rpg/GetRandomQuest',
                    headers: { 'Accept': 'application/json' }
                }).success(function(response) {
                    $scope.quest = angular.fromJson(response);
                });
            }

            $scope.GetHeroes = function() {
                $http({
                    method: 'GET',
                    url: '/WebWriterV2/Rpg/GetHeroes',
                    headers: { 'Accept': 'application/json' }
                }).success(function(response) {
                    $scope.heroes = angular.fromJson(response);
                });
            }

            $scope.SelectHero = function(hero) {
                $scope.currentHero = hero;

                $scope.quest.Progress = 0;
                angular.forEach($scope.quest.PartsOfQuest, function (partOfQuest) {

                    angular.forEach(partOfQuest.Events, function (event) {
                        if ($scope.CheckRequrment(event)) {
                            $scope.quest.Progress += event.ProgressPlus;
                        }
                    });
                });
            }

            $scope.CheckRequrment = function (event) {
                if (!$scope.currentHero)
                    return false;
                var valid = $scope.currentHero.Sex === event.RequrmentSex || event.RequrmentSex === 0;
                valid = valid && ($scope.currentHero.Race === event.RequrmentRace || event.RequrmentRace === 0);
                return valid;
            }


            //$scope.GoTo = function() {
            //    $location.path('createHero');
            //}
        }
    ]);