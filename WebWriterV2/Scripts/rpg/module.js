
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('rpg', ['directives', 'services', 'ngRoute', 'underscore']) //, ['common', 'search', 'masha', 'ui.ace']
    .constant('_',
        window._
    )
    .config(['$locationProvider', '$routeProvider',
         function ($locationProvider, $routeProvider) {
             // Routes configuration
             $routeProvider
                 .when('/AngularRoute/guild', {
                     templateUrl: '/views/rpg/Guild.html',
                     controller: 'guildController'
                 })
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
                 })
                 .when('/AngularRoute/travel', {
                     templateUrl: '/views/rpg/Travel.html',
                     controller: 'travelController'
                 })
                 .otherwise({
                     redirectTo: '/AngularRoute/guild'
                 });

             // Uses HTLM5 history API for navigation
             $locationProvider.html5Mode(true);
         }
    ])
    .controller('getQuestController', [
        '$scope', '$http', '$location', 'questService', 'sharedHeroes', 'sexService', 'raceService',
        function ($scope, $http, $location, questService, sharedHeroes, sexService, raceService) { //, $modal, $route, _, $log
            $scope.quests = []; //GeneralInfo, AdditionalInfo

            questService.getQuestPromise.then(function (result) {
                $scope.quests.push(result);
            });

            $scope.heroes = [];
            $scope.selectHero = undefined;

            $scope.GetHeroes = function () {
                $scope.heroes = sharedHeroes.getListHeroes();
            }

            $scope.GetHeroes();

            $scope.SelectHero = function (hero) {
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
                quest.Executor = $scope.selectHero;
                quest.EventsHistory = [];
                angular.forEach(quest.Wiles, function (wile) {
                    angular.forEach(wile.Events, function (event) {
                        if (checkRequrment(event)) {
                            quest.EventsHistory.push(event);
                            quest.Progress += event.ProgressChanging;
                        }
                    });
                });
                questService.setQuest(quest);
                $location.path('/AngularRoute/travel');
            }

            $scope.GetTextSex = sexService.getSexWord;

            $scope.GetTextRace = raceService.getRaceWord;
        }
    ])
    .controller('adminQuestController', [
        '$scope', '$http', 'questService', 'sexService', 'raceService', 'eventService',
        function ($scope, $http, questService, sexService, raceService, eventService) {
            $scope.quest = {}//questService.getQuest();}

            $scope.RaceList = [];
            $scope.SexList = [];
            $scope.levels = [];

            function init() {
                var baseSexList = sexService.getSexList();
                baseSexList.splice(0, 0, { name: 'None', value: 0 });
                $scope.SexList = baseSexList;

                var baseRaceList = raceService.getRaceList();
                baseRaceList.splice(0, 0, { name: 'None', value: 0 });
                $scope.RaceList = baseRaceList;

                eventService.getAllEvents($scope.quest.Id).then(function(result) {
                    EventGraph.drawGraph(result, 'eventsGraph', 900, 600);
                });
            }

            questService.getQuestPromise.then(function (result) {
                $scope.quest = result;
                init();
            });

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
                            alert('Save completed');
                        else {
                            alert('Some go wrong');
                        }
                    },
                    function () {
                        alert('We all gonna die');
                    }
                );
            }
        }
    ])
    .controller('createHeroController', [
        '$scope', '$http', '$location', 'sharedHeroes', 'raceService', 'sexService',
        function ($scope, $http, $location, sharedHeroes, raceService, sexService) {
            $scope.hero = {
                Characteristics: [
                    { Name: 'Strength', Value: 1 },
                    { Name: 'Agility', Value: 1 },
                    { Name: 'Charism', Value: 1 }
                ]
            };
            $scope.step = 1;
            $scope.freeStat = 10;
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

            $scope.nextStep = function () {
                if ($scope.step < 10) {
                    $scope.step++;
                }
            };

            $scope.prevStep = function () {
                if ($scope.step > 1) {
                    $scope.step--;
                }
            };

            $scope.saveHero = function () {
                sharedHeroes.addHero($scope.hero);
                $location.path('/AngularRoute/getQuest');
            }
        }
    ])
    .controller('listHeroesController', [
        '$scope', '$http', '$location', 'sharedHeroes', 'raceService', 'sexService',
        function ($scope, $http, $location, sharedHeroes, raceService, sexService) {
            $scope.heroes = sharedHeroes.getListHeroes();
        }
    ])
    .controller('guildController', [
        '$scope', 'guildService',
        function ($scope, guildService) {
            $scope.guild = {};
            guildService.getGuildPromise.then(function(guild) {
                $scope.guild = guild;
            });
        }
    ])
    .controller('travelController', [
        '$scope', 'questService', 'eventService',
        function ($scope, questService, eventService) {
            //$scope.hero = sharedHeroes.getListHeroes();
            $scope.quest = questService.getQuest();
            $scope.currentEvent = $scope.quest.RootEvent;
            $scope.ways = {};
            eventService.getEventChildrenPromise($scope.quest.RootEvent.Id).then(function (result) {
                $scope.ways = result.ChildrenEvents;
                $scope.quest.Effective = $scope.quest.RootEvent.ProgressChanging;
            });

            $scope.chooseEvent = function(event) {
                eventService.getEventChildrenPromise(event.Id).then(function (result) {
                    $scope.currentEvent = result;
                    $scope.ways = result.ChildrenEvents;
                    $scope.quest.Effective += event.ProgressChanging;
                });
            }
        }
    ])
    .controller('questInfoController', [
            '$scope', '$http', '$location', 'questService', 'raceService', 'sexService',
            function ($scope, $http, $location, questService, raceService, sexService) {
                $scope.quest = questService.getQuest();

                processQuest();
                function processQuest() {
                    var quest = $scope.quest;
                    quest.Effective = 0;
                    var hero = $scope.quest.Hero;
                    var events = quest.QuestEvents;

                    var parentEvent = null;
                    var possibleEvent;
                    do {
                        possibleEvent = events.filter(function (event) {
                            if (event.ParentEvents == null && parentEvent == null)
                                return true;
                            if (event.ParentEvents == null || parentEvent == null)
                                return false;
                            var thisEventHasCorretParrent = event.ParentEvents.some(function (x) {
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