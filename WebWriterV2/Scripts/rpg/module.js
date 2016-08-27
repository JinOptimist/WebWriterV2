
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
                controller: ['$scope', 'raceService', 'sexService', function ($scope, raceService, sexService) {
                    $scope.GetTextSex = sexService.getSexWord;
                    $scope.GetTextRace = raceService.getRaceWord;
                }]
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
                })
                .when('/AngularRoute/listHeroes', {
                    templateUrl: '/views/rpg/ListHeroes.html',
                    controller: 'listHeroesController'
                })
                 .when('/AngularRoute/questInfo', {
                     templateUrl: '/views/rpg/QuestInfo.html',
                     controller: 'questInfoController'
                 });

                //questInfoController
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
    .service('sharedHeroes', function () {
        var listHeroes = [
            {
                TempId: 0,
                Name: "Mage",
                Race: "1",
                Sex: "1",
                Stats: [
                    { Name: "Strength", Value: 3 },
                    { Name: "Agility", Value: 3 },
                    { Name: "Charism", Value: 7 }
                ]
            },
            {
                TempId: 1,
                Name: "Warrior",
                Race: "3",
                Sex: "2",
                Stats: [
                    { Name: "Strength", Value: 13 },
                    { Name: "Agility", Value: 3 },
                    { Name: "Charism", Value: 7 }
                ]
            },
            {
                TempId: 1,
                Name: "Elf",
                Race: "2",
                Sex: "2",
                Stats: [
                    { Name: "Strength", Value: 2 },
                    { Name: "Agility", Value: 8 },
                    { Name: "Charism", Value: 3 }
                ]
            }
        ];
        var lastId = 3;

        return {
            getListHeroes: function () {
                return listHeroes;
            },
            addHero: function (newHero) {
                if (newHero.TempId) {
                    listHeroes = listHeroes.filter(function (obj) {
                        return obj.TempId !== newHero.TempId;
                    });
                } else {
                    newHero.TempId = ++lastId;
                }

                newHero.Sex = newHero.Sex - 0;
                newHero.Race = newHero.Race - 0;

                listHeroes.push(newHero);
            }
        };
    })
    .service('raceService', function () {
        var raceList = [
                            { name: "Human/Dragon", value: 1, src: "/Content/HeroImg/dragon.jpg" },
                            { name: "Elf", value: 2, src: "/Content/HeroImg/elf.jpg" },
                            { name: "Orc/Gnom", value: 3, src: "/Content/HeroImg/gnom.jpg" }
        ];

        return {
            getRaceList: function () {
                return raceList;
            },
            getRaceWord: function (raceValue) {
                var filtered = raceList.filter(function (race) { return race.value == raceValue });
                if (filtered.length === 1)
                    return filtered[0].name;
                return "?";
            }
        };
    })
    .service('sexService', function () {
        var sexList = [
                            { name: "Man", value: 1, src: "https://image.freepik.com/free-photo/_21005898.jpg" },
                            { name: "Woman", value: 2, src: "http://innastudio.ru/userfiles/images/siluet.jpg" },
                            { name: "#$*/%", value: 3 }
        ];

        return {
            getSexList: function () {
                return sexList;
            },
            getSexWord: function (sexValue) {
                var filtered = sexList.filter(function(sex) { return sex.value == sexValue });
                if (filtered.length === 1)
                    return filtered[0].name;
                return "?";
            }
        };
    })
    .controller('getQuestController', [
        '$scope', "$http", "$location", "sharedQuest", "sharedHeroes", "sexService", "raceService",
        function ($scope, $http, $location, sharedQuest, sharedHeroes, sexService, raceService) { //, $modal, $route, _, $log
            $scope.quests = []; //GeneralInfo, AdditionalInfo
            $scope.heroes = [];
            $scope.selectHero = undefined;

            $scope.GetQuest = function() {
                $http({
                    method: 'GET',
                    url: '/Rpg/GetRandomQuest',
                    headers: { 'Accept': 'application/json' }
                }).success(function(response) {
                    var quest = angular.fromJson(response);
                    $scope.quests.push(quest);
                    sharedQuest.setQuest(quest);
                });
            }

            $scope.GetHeroes = function() {
                //$http({
                //    method: 'GET',
                //    url: '/Rpg/GetHeroes',
                //    headers: { 'Accept': 'application/json' }
                //}).success(function(response) {
                //    $scope.heroes = angular.fromJson(response);
                //});
                $scope.heroes = sharedHeroes.getListHeroes();
            }

            $scope.GetQuest();
            $scope.GetHeroes();

            $scope.SelectHero = function(hero) {
                $scope.selectHero = hero;
            }

            var checkRequrment = function (event) {
                if (!$scope.selectHero)
                    return false;
                var valid = $scope.selectHero.Sex == event.RequrmentSex || event.RequrmentSex == 0;
                valid = valid && ($scope.selectHero.Race == event.RequrmentRace || event.RequrmentRace == 0);
                return valid;
            }

            $scope.goToTheQuest = function (quest) {
                quest.Progress = 0;
                quest.Hero = $scope.selectHero;
                quest.EventsHistory = [];
                angular.forEach(quest.Wiles, function (wile) {
                    angular.forEach(wile.Events, function (event) {
                        if (checkRequrment(event)) {
                            quest.EventsHistory.push(event);
                            quest.Progress += event.ProgressChanging;
                        }
                    });
                });
                sharedQuest.setQuest(quest);
                $location.path("/AngularRoute/questInfo");
            }


            $scope.GetTextSex = sexService.getSexWord;

            $scope.GetTextRace = raceService.getRaceWord;
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
        '$scope', "$http", "$location", "sharedHeroes", "raceService", "sexService",
        function ($scope, $http, $location, sharedHeroes, raceService, sexService) {
            $scope.hero = {
                Stats: [
                    { Name: "Strength", Value: 1 },
                    { Name: "Agility", Value: 1 },
                    { Name: "Charism", Value: 1 }
                ]
            };
            $scope.step = 1;
            $scope.freeStat= 10;
            $scope.RaceList = raceService.getRaceList();
            $scope.SexList = sexService.getSexList();

            $scope.addStat = function (stat) {
                if ($scope.freeStat > 0) {
                    stat.Value++;
                    $scope.freeStat--;
                }
            };
            $scope.minusStat = function (stat) {
                if (stat.Value > 1) {
                    stat.Value--;
                    $scope.freeStat++;
                }
            };

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

            $scope.saveHero = function() {
                sharedHeroes.addHero($scope.hero);
                $location.path('/AngularRoute/getQuest');
            }
        }
    ])
    .controller('listHeroesController', [
        '$scope', "$http", "$location", "sharedHeroes", "raceService", "sexService",
        function ($scope, $http, $location, sharedHeroes, raceService, sexService) {
            $scope.heroes = sharedHeroes.getListHeroes();
        }
    ])
    .controller('questInfoController', [
            '$scope', "$http", "$location", "sharedQuest", "raceService", "sexService",
            function ($scope, $http, $location, sharedQuest, raceService, sexService) {
                $scope.quest = sharedQuest.getQuest();

                $scope.getRaceWord = raceService.getRaceWord;
                $scope.getSexWord = sexService.getSexWord;
            }
    ]);