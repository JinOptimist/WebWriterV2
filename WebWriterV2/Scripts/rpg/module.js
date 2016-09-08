
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
                 .when('/AngularRoute/createQuest', {
                     templateUrl: '/views/rpg/CreateQuest.html',
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
                 .when('/AngularRoute/traningRoom', {
                     templateUrl: '/views/rpg/TraningRoom.html',
                     controller: 'traningRoomController'
                 })
                 .otherwise({
                     redirectTo: '/AngularRoute/guild'
                 });

             // Uses HTLM5 history API for navigation
             $locationProvider.html5Mode(true);
         }
    ])
    .controller('adminQuestController', [
        '$scope', '$http', 'questService', 'sexService', 'raceService', 'eventService',
        function ($scope, $http, questService, sexService, raceService, eventService) {
            $scope.quest = {};

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

                eventService.getAllEvents($scope.quest.Id).then(function (result) {
                    EventGraph.drawGraph(result, 'eventsGraph', 900, 600);
                });
            }

            questService.getQuest().then(function (result) {
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
            $scope.heroes = [];

            sharedHeroes.getListHeroes().then(function (result) {
                $scope.heroes = result;
            });
        }
    ])
    .controller('guildController', [
        '$scope', '$location', 'guildService', 'questService', 'traningRoomService',
        function ($scope, $location, guildService, questService, traningRoomService) {
            $scope.guild = {};
            guildService.getGuildPromise.then(function (guild) {
                $scope.guild = guild;
            });

            questService.getQuest().then(function (result) {
                $scope.quest = result;
            });

            $scope.currentHero = {};
            $scope.selectHero = function (hero) {
                $scope.currentHero = hero;
            }

            $scope.goToQuest = function (quest) {
                questService.setExecutor($scope.currentHero);
                $location.path('/AngularRoute/travel');
            }

            $scope.goToTrain = function (room) {
                if (!$scope.currentHero) {
                    alert("Hero doesn't chosen");
                    return;
                }

                traningRoomService.chooseRoom(room, $scope.currentHero);
                $location.path('/AngularRoute/traningRoom');
            }
        }
    ])
    .controller('travelController', [
        '$scope', '$http', '$location', 'questService', 'eventService', 'guildService',
        function ($scope, $http, $location, questService, eventService, guildService) {
            //$scope.hero = sharedHeroes.getListHeroes();
            $scope.quest = {};
            questService.getQuest().then(function (result) {
                $scope.quest = result;
                $scope.quest.Effective = 0;
                $scope.currentEvent = $scope.quest.RootEvent;
                $scope.chooseEvent($scope.quest.RootEvent.Id);
            });

            $scope.ways = {};

            $scope.chooseEvent = function (eventId) {
                eventService.getEventChildrenPromise(eventId).then(function (result) {
                    $scope.ways = result.ChildrenEvents;
                    $scope.quest.Effective += result.ProgressChanging;
                    $scope.currentEvent = result;
                });
            };

            $scope.endQuest = function () {
                $scope.waiting = true;
                var guild = guildService.getGuild();
                var guildId = guild.Id;

                var url = '/Rpg/QuestCompleted?guildId=' + guildId + '&gold=' + $scope.quest.Effective;
                $http({
                        method: 'GET',
                        url: url,
                        headers: { 'Accept': 'application/json' }
                    })
                    .then(function(response) {
                        if (response.data == "+") {
                            guild.Gold += $scope.quest.Effective;
                            guildService.setGuild(guild);
                            $location.path('/AngularRoute/guild');
                        } else {
                            alert(response);
                        }
                        $scope.waiting = false;
                    },function() {
                        alert("Hero want relax. Wait and try again");
                        $scope.waiting = false;
                    });

            }
        }
    ])
    .controller('traningRoomController', [
        '$scope', '$http', 'traningRoomService',
        function ($scope, $http, traningRoomService) {
            $scope.room = traningRoomService.getRoom();
            $scope.hero = traningRoomService.getHero();
            $scope.waiting = false;

            $scope.currentSkill = {};
            $scope.selectSkill = function (skill) {
                $scope.currentSkill = skill;
            }

            $scope.addSkill = function () {
                $scope.waiting = true;
                var url = '/Rpg/AddSkillToHero?heroId=' + $scope.hero.Id + '&skillId=' + $scope.currentSkill.Id;
                $http({
                    method: 'GET',
                    url: url,
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        if (response == "+") {
                            $scope.hero.Skills.push($scope.currentSkill);
                        } else {
                            alert(response);
                        }
                        $scope.waiting = false;
                    }).fail(function () {
                        alert("Training was failed. Try again");
                        $scope.waiting = false;
                    });
            }
        }
    ])
    .controller('questInfoController', [
            '$scope', '$http', '$location', 'questService', 'raceService', 'sexService',
            function ($scope, $http, $location, questService, raceService, sexService) {
                $scope.quest = {};
                questService.getQuest().then(function (result) {
                    $scope.quest = result;
                });

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