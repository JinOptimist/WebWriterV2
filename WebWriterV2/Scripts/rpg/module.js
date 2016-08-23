
angular.module('rpg', ['ngRoute']) //, ['common', 'search', 'masha', 'ui.ace']
    .directive('ngHeroInfo', [
        function() {
            return {
                restrict: 'E',
                replace: true,
                templateUrl: '/views/rpg/ngHeroInfo.html'
            }
        }
    ])
    .directive('ngQuestInfo', [
        function() {
            return {
                restrict: 'E',
                replace: true,
                templateUrl: '/views/rpg/ngQuestInfo.html',
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
                templateUrl: '/views/rpg/ngHeroCard.html',
                scope: { hero: '=' },
                link: function (scope, element, attrs) {
                    scope.$watch(attrs.hero, function (v) {
                        console.log('Hero value changed, new value is: ' + v);
                    });
                }
            }
        }
    ])
    .config(['$locationProvider', '$routeProvider',
         function ($locationProvider, $routeProvider) {
             // Routes configuration
             $routeProvider
                 .when('/AngularRoute/getQuest', {
                     templateUrl: '/views/rpg/GetQuest.html',
                     controller: 'getQuestController'
                 })
                 .when('/AngularRoute/addQuest', {
                     templateUrl: '/views/rpg/AddQuest.html',
                     controller: 'adminQuestController'
                 })
                 .when('/AngularRoute/createHero', {
                     templateUrl: '/views/rpg/CreateHero.html',
                     controller: 'createHeroController'
                 });
                 //.otherwise({
                 //    redirectTo: '/'
                 //});

             // Uses HTLM5 history API for navigation
             $locationProvider.html5Mode(true);
         }
    ])
    .service('sharedQuest', function () {
        var curentQuest = {};

        return {
            getQuest: function () {
                return curentQuest;
            },
            setQuest: function(value) {
                curentQuest = value;
            }
        };
    })
    .controller('getQuestController', [
        '$scope', "$http", "sharedQuest", //'$modal', '$route', '_', '$log',
        function($scope, $http, sharedQuest) { //, $modal, $route, _, $log
            $scope.quest = undefined; //GeneralInfo, AdditionalInfo
            $scope.heroes = undefined;
            $scope.currentHero = undefined;

            $scope.quest = sharedQuest.getQuest();

            $scope.GetQuest = function() {
                $http({
                    method: 'GET',
                    url: '/Rpg/GetRandomQuest',
                    headers: { 'Accept': 'application/json' }
                }).success(function(response) {
                    $scope.quest = angular.fromJson(response);
                    sharedQuest.setQuest($scope.quest);
                });
            }

            $scope.GetHeroes = function() {
                $http({
                    method: 'GET',
                    url: '/Rpg/GetHeroes',
                    headers: { 'Accept': 'application/json' }
                }).success(function(response) {
                    $scope.heroes = angular.fromJson(response);
                });
            }

            $scope.SelectHero = function(hero) {
                $scope.currentHero = hero;

                $scope.quest.Progress = 0;
                $scope.quest.EventsHistory = [];
                angular.forEach($scope.quest.Wiles, function(wile) {

                    angular.forEach(wile.Events, function(event) {
                        if ($scope.CheckRequrment(event)) {
                            $scope.quest.EventsHistory.push(event);
                            $scope.quest.Progress += event.ProgressChanging;
                        }
                    });
                });
            }

            $scope.CheckRequrment = function(event) {
                if (!$scope.currentHero)
                    return false;
                var valid = $scope.currentHero.Sex === event.RequrmentSex || event.RequrmentSex === 0;
                valid = valid && ($scope.currentHero.Race === event.RequrmentRace || event.RequrmentRace === 0);
                return valid;
            }

            $scope.GetTextSex = function(sexId) {
                switch (sexId) {
                case 1:
                    return String.fromCharCode(9794);
                case 2:
                    return String.fromCharCode(9794);
                case 3:
                    return "?";
                }

                return "?";
            }

            $scope.GetTextRace = function(raceId) {
                switch (raceId) {
                case 1:
                    return "Human";
                case 2:
                    return "Elf";
                case 3:
                    return "Orc";
                }

                return "?";
            }

            //$scope.GoTo = function() {
            //    $location.path('createHero');
            //}
        }
    ])
    .controller('adminQuestController', [
        '$scope', "$http", "sharedQuest",
        function ($scope, $http, sharedQuest) {
            $scope.quest = sharedQuest.getQuest();
            //$scope.quest.Wiles = [];
            $scope.SexList = [
                { name: "None", value: 0 },
                { name: "Male", value: 1 },
                { name: "Female", value: 2 },
                { name: "Unknown", value: 3 }
            ];
            $scope.RaceList = [
                { name: "None", value: 0 },
                { name: "Human", value: 1 },
                { name: "Elf", value: 2 },
                { name: "Orc", value: 3 }
            ];

            $scope.addWile = function() {
                $scope.quest.Wiles.push({
                    Desc: "",
                    Events: []
                });
            }

            $scope.removeWile = function (index) {
                $scope.quest.Wiles.splice(index, 1);
            }

            $scope.addWileEvent = function (wile) {
                wile.Events.push({
                    Desc: ""
                });
            }

            $scope.removeWileEvent = function (wile, index) {
                wile.Events.splice(index, 1);
            }

            $scope.submitQuest = function () {
                var req = {
                    method: 'POST',
                    url: '/Rpg/SaveQuest',
                    data: { jsonQuest: angular.toJson($scope.quest) },
                };
                $http(req).then(
                    function (response) {
                        if (response.data)
                            alert("Save completed");
                        else {
                            alert("Some go wrong");
                        }
                    },
                    function() {
                        alert("We all gonna die");
                    }
                );
            }

            //$scope.GoTo = function() {
            //    $location.path('createHero');
            //}
        }
    ])
    .controller('createHeroController', [
        '$scope', "$http",
        function ($scope, $http) {
            $scope.hero = {};
            $scope.step = 1;
            $scope.RaceList = [
                            { name: "Human", value: 1, src: "https://image.freepik.com/free-icon/_318-29133.jpg" },
                            { name: "Elf", value: 2, src: "http://1.bp.blogspot.com/-53rftQUcr-g/UQfij4gQS7I/AAAAAAAABrE/nObJ1OMCy50/s1600/AnimeArcherSilhouette.jpg" },
                            { name: "Orc", value: 3, src: "https://www.3dbuzz.com/forum/attachment.php?attachmentid=73845&d=1377379847" }
            ];

            $scope.SexList = [
                            { name: "Man", value: 1, src: "https://image.freepik.com/free-photo/_21005898.jpg" },
                            { name: "Woman", value: 2, src: "http://innastudio.ru/userfiles/images/siluet.jpg" },
                            { name: "#$*/%", value: 3 }
            ];

            $scope.nextStep = function() {
                if ($scope.step < 10) {
                    $scope.step++;
                }
            };

            $scope.prevStep = function () {
                if ($scope.step > 1) {
                    $scope.step--;
                }
            };

        }
    ]);