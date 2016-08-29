
angular.module('rpg', ['ngRoute']) //, ['common', 'search', 'masha', 'ui.ace']
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
    .service('sharedQuest', ['$http', function ($http) {
        var curentQuest = {};

        $http({
            method: 'GET',
            url: '/Rpg/GetRandomQuest',
            headers: { 'Accept': 'application/json' }
        }).success(function (response) {
            curentQuest = angular.fromJson(response);
        });

        return {
            getQuest: function () {
                return curentQuest;
            },
            setQuest: function (value) {
                curentQuest = value;
            }
        };
    }])
    .service('sharedHeroes', function () {
        var listHeroes = [
            {
                TempId: 0,
                Name: "Mage",
                Race: "1",
                Sex: "1",
                Characteristics: [
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
                Characteristics: [
                    { Name: "Strength", Value: 13 },
                    { Name: "Agility", Value: 3 },
                    { Name: "Charism", Value: 7 }
                ]
            },
            {
                TempId: 1,
                Name: "Thief",
                Race: "2",
                Sex: "2",
                Characteristics: [
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
                            { name: "Human", value: 1, src: "/Content/rpg/human.jpg" },
                            { name: "Elf", value: 2, src: "/Content/rpg/elf.jpg" },
                            { name: "Orc", value: 3, src: "/Content/rpg/orc.jpg" },
                            { name: "Dragon", value: 4, src: "/Content/rpg/dragon.jpg" },
                            { name: "Gnom", value: 5, src: "/Content/rpg/gnom.jpg" }
        ];

        return {
            getRaceList: function () {
                var copyRace = raceList.map(function(value) {
                    return angular.copy(value);
                });
                return copyRace;//clone array
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
                            { name: "Man", value: 1, src: "/Content/rpg/man.jpg" },
                            { name: "Woman", value: 2, src: "/Content/rpg/woman.jpg" },
                            { name: "Unknown", value: 3, src: "/Content/rpg/unknown.jpg" }
        ];

        return {
            getSexList: function () {
                var copySex = sexList.map(function (value) {
                    return angular.copy(value);
                });
                return copySex; // clone array
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
                $scope.quests.push(sharedQuest.getQuest());
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
        '$scope', "$http", "sharedQuest", "sexService", "raceService",
        function ($scope, $http, sharedQuest, sexService, raceService) {
            $scope.quest = sharedQuest.getQuest();
            $scope.RaceList = [];
            $scope.SexList = [];
            $scope.levels = [];

            function init() {
                var baseSexList = sexService.getSexList();
                baseSexList.splice(0, 0, { name: "None", value: 0 });
                $scope.SexList = baseSexList;

                var baseRaceList = raceService.getRaceList();
                baseRaceList.splice(0, 0, { name: "None", value: 0 });
                $scope.RaceList = baseRaceList;

                EventGraph.drawGraph($scope.quest.QuestEvents, 'eventsGraph', 900, 600);
            }

            init();

            $scope.addEvent = function (event) {
                $scope.quest.QuestEvents.push({
                    Desc: event.Desc
                });
            }

            $scope.removeWileEvent = function (index) {
                $scope.quest.QuestEvents.splice(index, 1);
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
        }
    ])
    .controller('createHeroController', [
        '$scope', "$http", "$location", "sharedHeroes", "raceService", "sexService",
        function ($scope, $http, $location, sharedHeroes, raceService, sexService) {
            $scope.hero = {
                Characteristics: [
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

                processQuest();
                function processQuest() {
                    var quest = $scope.quest;
                    quest.Effective = 0;
                    var hero = $scope.quest.Hero;
                    var events = quest.QuestEvents;

                    var parentEvent = null;
                    var possibleEvent;
                    do {
                        possibleEvent = events.filter(function(event) {
                            if (event.ParentEvents == null && parentEvent == null)
                                return true;
                            if (event.ParentEvents == null || parentEvent == null)
                                return false;
                            var thisEventHasCorretParrent = event.ParentEvents.some(function(x) {
                                return x.Id == parentEvent.Id;
                            });
                            if (!thisEventHasCorretParrent)
                                return false;
                            return (event.RequrmentSex == null || event.RequrmentSex == 0 || event.RequrmentSex == hero.Sex)
                                && (event.RequrmentRace == null || event.RequrmentRace == 0 || event.RequrmentRace == hero.Race);
                        });
                        if (possibleEvent.length > 0) {
                            var currentEvent = possibleEvent[0];
                            parentEvent = currentEvent;
                            quest.EventsHistory.push(currentEvent);
                            quest.Effective += currentEvent.ProgressChanging;
                        }
                    } while (possibleEvent.length > 0)
                }

                $scope.getRaceWord = raceService.getRaceWord;
                $scope.getSexWord = sexService.getSexWord;
            }
    ]);