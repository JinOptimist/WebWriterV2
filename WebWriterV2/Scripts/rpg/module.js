
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('rpg', ['directives', 'services', 'ngRoute', 'underscore']) //, ['common', 'search', 'masha', 'ui.ace']
    .constant('_',
        window._
    )
    .config([
        '$locationProvider', '$routeProvider',
        function($locationProvider, $routeProvider) {
            // Routes configuration
            $routeProvider
                /* admin */
                .when('/AngularRoute/adminSkill', {
                    templateUrl: '/views/rpg/AdminSkill.html',
                    controller: 'adminSkillController'
                })
                .when('/AngularRoute/adminCharacteristic', {
                    templateUrl: '/views/rpg/AdminCharacteristic.html',
                    controller: 'adminCharacteristicController'
                })
                .when('/AngularRoute/adminQuest', {
                    templateUrl: '/views/rpg/AdminQuest.html',
                    controller: 'adminQuestController'
                })
                /* front */
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
                .when('/AngularRoute/battle', {
                    templateUrl: '/views/rpg/battle.html',
                    controller: 'battleController'
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
            $scope.quest = null;
            $scope.currentEvent = {};

            $scope.RaceList = [];
            $scope.SexList = [];
            $scope.SkillList = [];
            $scope.CharacteristicTypeList = [];

            var parentNeededToSave = false;

            init();

            $scope.currentEventWasChanged = function () {
                parentNeededToSave = false;
            }

            $scope.getParentEvent = function (currentEvent) {
                if (!currentEvent)
                    return [];
                var parentEvent = _.filter($scope.quest.AllEvents, function(filterEvent) {
                    return _.some(filterEvent.ChildrenEvents, function (someEvent) {
                        return someEvent.Id === currentEvent.Id;
                    });
                });

                return parentEvent;
            }

            $scope.removeConnectionParentEvent = function (currentEvent, parentEventId) {
                _.each($scope.quest.AllEvents, function (filterEvent) {
                    if (filterEvent.Id === parentEventId) {
                        var index = _.findLastIndex(filterEvent.ChildrenEvents,
                        {
                            Id: currentEvent.Id
                        });
                        filterEvent.ChildrenEvents.splice(index, 1);
                    }
                });
                parentNeededToSave = true;
                reloadGraph();
            }

            $scope.removeConnectionChildEvent = function (currentEvent, childEventId) {
                var index = _.findLastIndex(currentEvent.ChildrenEvents,
                    {
                        Id: childEventId
                    });
                currentEvent.ChildrenEvents.splice(index, 1);

                reloadGraph();
            }

            $scope.addConnection = function (parentEvent, childEvent) {
                parentEvent.ChildrenEvents.push(childEvent);
                parentNeededToSave = true;
                reloadGraph();
            }

            $scope.removeEvent = function (currentEvent) {
                eventService.remove(currentEvent.Id).then(function (response) {
                    //remove all link to event
                    _.each($scope.quest.AllEvents, function (filterEvent) {
                        var index = _.findLastIndex(filterEvent.ChildrenEvents,
                        {
                            Id: currentEvent.Id
                        });
                        if (index > -1)
                            filterEvent.ChildrenEvents.splice(index, 1);
                    });

                    //remove event
                    var index = _.findLastIndex($scope.quest.AllEvents,
                    {
                        Id: currentEvent.Id
                    });
                    $scope.quest.AllEvents.splice(index, 1);

                    reloadGraph();
                },
                function () {
                    alert("We can't remove event wich has child");
                });
            }

            $scope.addEvent = function () {
                var newEvent = {
                    Name: 'new',
                    ChildrenEvents: []
                };
                $scope.quest.AllEvents.push(newEvent);

                $scope.currentEvent = newEvent;

                reloadGraph();
            }

            $scope.saveEvent = function (event) {
                var questId = $scope.quest.Id;
                eventService.save(event, questId).then(function (response) {
                    updateEvent(event, response);
                    $scope.currentEvent = response;
                    alert('Event save +');
                    if (parentNeededToSave) {
                        var savedEvent = response;
                        event.Id = savedEvent.Id;
                        parentNeededToSave = false;
                        var parentEvents = $scope.getParentEvent(event);
                        var arrayPromise = parentEvents.map(function (eve) { return eventService.save(eve, questId); });
                        Promise.all(arrayPromise).then(function() {
                            reloadGraph();
                        });
                    } else {
                        reloadGraph();
                    }
                });
            }

            $scope.removeWileEvent = function(index) {
                $scope.quest.QuestEvents.splice(index, 1);
            }

            $scope.submitQuest = function() {
                var req = {
                    method: 'POST',
                    url: '/Rpg/SaveQuest',
                    data: { jsonQuest: angular.toJson($scope.quest) },
                };
                $http(req).then(
                    function(response) {
                        if (response.data)
                            alert('Save completed');
                        else {
                            alert('Some go wrong');
                        }
                    },
                    function() {
                        alert('We all gonna die');
                    }
                );
            }

            $scope.selectQuest = function (quest) {
                $scope.quest = quest;
                if ($scope.quest.AllEvents && $scope.quest.AllEvents.length > 0)
                    $scope.currentEvent = $scope.quest.AllEvents[0];
                reloadGraph();
            }

            $scope.clearActiveQuest = function () {
                $scope.quest = null;
            }

            $scope.removeQuest = function (quest, index) {
                if (confirm('Are sure? You try delte whole qeust: ' + quest.Name))
                    questService.removeQuest(quest.Id).then(function (result) {
                        $scope.quests.splice(index, 1);
                        reloadGraph();
                    });
            }

            $scope.addQuest = function (q) {
                var newQuest = {};
                $scope.quests.push(newQuest);
                $scope.selectQuest(newQuest);
            }

            function updateEvent(oldEvent, newEvent) {
                var index = $scope.quest.AllEvents.findIndex(function (e) { return e === oldEvent; })
                $scope.quest.AllEvents[index] = newEvent;
            }

            function reloadGraph() {
                EventGraph.drawGraph($scope.quest.AllEvents, 'eventsGraph', 900, 600);
            }

            function loadQuest(questId) {
                questService.getQuest(questId).then(function (result) {
                    $scope.quest = result;
                    reloadGraph();
                });
            }

            function loadQuests() {
                questService.getQuests().then(function (result) {
                    $scope.quests = result;
                });
            }

            function init() {
                $scope.SexList.push({ name: 'None', value: null });
                sexService.loadSexList().then(function (data) {
                    _.each(data, function (item) {
                        return $scope.SexList.push({ name: item.Name, value: item.Value });
                    });
                });

                $scope.RaceList.push({ name: 'None', value: null });
                raceService.loadRaceList().then(function (data) {
                    _.each(data, function (item) {
                        return $scope.RaceList.push({ name: item.Name, value: item.Value });
                    });
                });

                loadQuests();
            }
        }
    ])
    .controller('adminSkillController', [
                 '$scope', 'skillService', 'stateService',
        function ($scope, skillService, stateService) {
            $scope.skills = [];
            $scope.newSkill = {
                School: {},
                SelfChanging: [],
                TargetChanging: []
            };
            $scope.skillSchools = [];
            $scope.stateType = [];
            $scope.characteristicType = [];

            skillService.loadAll().then(function (skills) {
                $scope.skills = skills;
            });

            skillService.loadSkillsSchool().then(function (skillSchools) {
                var schoolsToSelect = _.map(skillSchools, function(school) {
                    return {
                        name: school.Name,
                        value: school
                    };
                });
                $scope.skillSchools = schoolsToSelect;
            });

            stateService.loadAllTypes().then(function (stateTypes) {
                $scope.stateType = _.map(stateTypes, function (stateType) {
                    return {
                        name: stateType.Name,
                        value: stateType
                    };
                });
            });

            $scope.addState = function (source, item) {
                var copied = {};
                angular.copy(item, copied);
                source.push(copied);
            }

            $scope.removeState = function (source, index) {
                source.splice(index, 1);
            }

            $scope.saveSkill = function() {
                skillService.saveSkill($scope.newSkill).then(function(skill) {
                    var copied = {};
                    angular.copy(skill, copied);
                    $scope.skills.push(copied);
                });
            }

            $scope.removeSkill = function (skill) {
                skillService.removeSkill(skill).then(function () {
                    var index = $scope.skills.indexOf(skill);
                    $scope.skills.splice(index, 1);
                });
            }
        }
    ])
    .controller('adminStateController', [
        '$scope', '$http', 'skillService', 'sexService', 'raceService',
        function ($scope, $http, skillService, sexService, raceService) {
            $scope.states = [];


        }
    ])
    .controller('adminCharacteristicController', [
        '$scope', 'characteristicService', 'stateService',
        function ($scope, characteristicService, stateService) {
            $scope.newCharacteristicType = {
                State: []
            };
            $scope.stateType = [];
            $scope.characteristicTypes = [];
            characteristicService.loadAllCharacteristic().then(function(characteristics) {
                $scope.characteristicTypes = characteristics;
            });

            stateService.loadAllTypes().then(function (stateTypes) {
                $scope.stateType = _.map(stateTypes, function (stateType) {
                    return {
                        name: stateType.Name,
                        value: stateType
                    };
                });
            });

            $scope.saveCharacteristicType = function () {
                characteristicService.save($scope.newCharacteristicType).then(function (savedCharacteristicType) {
                    var copied = {};
                    angular.copy(savedCharacteristicType, copied);
                    $scope.newCharacteristicType = {};
                    $scope.characteristicTypes.push(copied);
                });
            }

            $scope.removeCharacteristicType = function (characteristicType) {
                characteristicService.removeCharacteristicType(characteristicType).then(function () {
                    var index = $scope.characteristicTypes.indexOf(characteristicType);
                    $scope.characteristicTypes.splice(index, 1);
                });
            }

            $scope.addState = function (source, item) {
                var copied = {};
                angular.copy(item, copied);
                source.push(copied);
            }

            $scope.removeState = function (source, index) {
                source.splice(index, 1);
            }
        }
    ])
    .controller('createHeroController', [
        '$scope', '$http', '$location', 'heroService', 'raceService', 'sexService','guildService',
        function ($scope, $http, $location, heroService, raceService, sexService, guildService) {
            //$scope.hero = heroService.getDefaultHero();
            heroService.loadDefaultHero().then(function (defaultHero) {
                $scope.hero = defaultHero;
            });

            $scope.step = 1;
            $scope.freeStat = 10;
            $scope.RaceList = [];
            raceService.loadRaceList().then(function(raceList) {
                $scope.RaceList = raceService.addImageToList(raceList);
            });
            $scope.SexList = [];
            sexService.loadSexList().then(function(sexList) {
                $scope.SexList = sexService.addImageToList(sexList);
            });

            function updateState(stateChanges, positive) {
                var heroState = $scope.hero.State;

                var curStat = heroState.find(function (hState) {
                    return hState.StateType.Id == stateChanges.StateType.Id;
                });
                curStat.Number += stateChanges.Number * positive;
            }

            $scope.addStat = function(stat) {
                if ($scope.freeStat > 0) {
                    stat.Number++;
                    $scope.freeStat--;

                    _.each(stat.CharacteristicType.State, function(effState) {
                        updateState(effState, 1);
                    });
                }
            };
            $scope.minusStat = function(stat) {
                if (stat.Number > 1) {
                    stat.Number--;
                    $scope.freeStat++;

                    _.each(stat.CharacteristicType.State, function (effState) {
                        updateState(effState, -1);
                    });
                }
            };

            $scope.nextStep = function() {
                $scope.step++;
            };

            $scope.prevStep = function() {
                if ($scope.step > 1) {
                    $scope.step--;
                }
            };

            $scope.selectRace = function(race) {
                $scope.hero.Race = race;
            }

            $scope.selectSex = function (sex) {
                $scope.hero.Sex = sex;
            }

            $scope.saveHero = function() {
                heroService.saveHero($scope.hero).then(function (savedHero) {
                    var guild = guildService.getGuild();
                    guild.Heroes.push(angular.fromJson(savedHero));
                    guildService.setGuild(guild);

                    $location.path('/AngularRoute/guild');
                });
            }
        }
    ])
    .controller('listHeroesController', [
        '$scope', '$http', '$location', 'heroService', 'raceService', 'sexService',
        function($scope, $http, $location, heroService, raceService, sexService) {
            $scope.heroes = [];

            heroService.loadListHeroes().then(function(result) {
                $scope.heroes = result;
            });
        }
    ])
    .controller('guildController', [
        '$scope', '$location', 'guildService', 'questService', 'traningRoomService','heroService',
        function ($scope, $location, guildService, questService, traningRoomService, heroService) {
            $scope.guild = {};
            $scope.currentHero = {};
            $scope.quests = [];

            init();
            function init() {
                guildService.getGuildPromise.then(function (guild) {
                    $scope.guild = guild;
                });

                questService.getQuests().then(function (result) {
                    $scope.quests = result;
                });
            }
            
            $scope.selectHero = function(hero) {
                $scope.currentHero = hero;
            }

            $scope.deleteHero = function (hero, index) {
                heroService.removeHero(hero).then(function () {
                    $scope.guild.Heroes.splice(index, 1);
                });
            }

            $scope.createHero = function() {
                $location.path('/AngularRoute/createHero');
            }

            $scope.goToQuest = function(quest) {
                questService.setQuest(quest);
                questService.setExecutor($scope.currentHero);
                $location.path('/AngularRoute/travel');
            }

            $scope.goToTrain = function(room) {
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
        '$scope', '$http', '$location', 'questService', 'eventService', 'guildService', 'heroService',
        function($scope, $http, $location, questService, eventService, guildService, heroService) {
            //$scope.hero = heroService.loadListHeroes();
            $scope.quest = {};
            questService.getQuest().then(function(result) {
                $scope.quest = result;
                $scope.quest.Effective = 0;
                $scope.currentEvent = $scope.quest.RootEvent;
                $scope.chooseEvent($scope.quest.RootEvent.Id);
            });

            $scope.ways = {};

            $scope.chooseEvent = function(eventId) {
                eventService.getEventChildrenPromise(eventId).then(function(result) {
                    $scope.ways = result.ChildrenEvents;
                    $scope.quest.Effective += result.ProgressChanging;
                    $scope.currentEvent = result;
                });
            };

            $scope.endQuest = function() {
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
                    }, function() {
                        alert("Hero want relax. Wait and try again");
                        $scope.waiting = false;
                    });

            }

            $scope.batle = function() {
                heroService.selectHero($scope.quest.Executor);
                $location.path('/AngularRoute/battle');
            }
        }
    ])
    .controller('traningRoomController', [
        '$scope', '$http', 'traningRoomService',
        function($scope, $http, traningRoomService) {
            $scope.room = traningRoomService.getRoom();
            $scope.hero = traningRoomService.getHero();
            $scope.waiting = false;

            $scope.currentSkill = {};
            $scope.selectSkill = function(skill) {
                $scope.currentSkill = skill;
            }

            $scope.addSkill = function() {
                $scope.waiting = true;
                var url = '/Rpg/AddSkillToHero?heroId=' + $scope.hero.Id + '&skillId=' + $scope.currentSkill.Id;
                $http({
                        method: 'GET',
                        url: url,
                        headers: { 'Accept': 'application/json' }
                    })
                    .then(function(response) {
                        if (response == "+") {
                            $scope.hero.Skills.push($scope.currentSkill);
                        } else {
                            alert(response);
                        }
                        $scope.waiting = false;
                    },
                    function () {
                        alert("Training was failed. Try again");
                        $scope.waiting = false;
                    });
            }
        }
    ])
    .controller('questInfoController', [
        '$scope', '$http', '$location', 'questService', 'raceService', 'sexService',
        function($scope, $http, $location, questService, raceService, sexService) {
            $scope.quest = {};
            questService.getQuest().then(function(result) {
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
    ])
    .controller('battleController', [
        '$scope', '$http', '$location', 'heroService', 'raceService', 'sexService','skillService',
        function ($scope, $http, $location, heroService, raceService, sexService, skillService) {
            $scope.hero = heroService.getSelectedHero();
            $scope.enemy = {};
            heroService.loadEnemy().then(function(enemy) {
                $scope.enemy = enemy;
            });

            $scope.battleEnd = false;

            $scope.report = '';

            $scope.attack = function (skill) {
                $scope.report = '';
                $scope.useSkill($scope.hero, $scope.enemy, skill);
                $scope.useSkill($scope.enemy, $scope.hero, $scope.enemy.Skills[0]);
            }

            $scope.useSkill = function (self, target, skill) {
                skillService.loadSkillEffect(skill.Id).then(function (fullSkill) {

                    var isEnough = skillService.isEnough(self, fullSkill);
                    if (isEnough) {
                        alert(isEnough);
                        return;
                    }

                    $scope.report += ' ' + self.Name + ' use ' + fullSkill.Name;

                    var stat;
                    for (var i = 0; i < fullSkill.SelfChanging.length; i++) {
                        var selfChange = fullSkill.SelfChanging[i];
                        stat = getState(self, selfChange.StateType.Name);
                        if (stat) {
                            stat.Number += selfChange.Number;
                            $scope.report += ' his ' + stat.StateType.Name + ': ' + selfChange.Number + ' ';
                        }

                    }

                    for (var j = 0; j < fullSkill.TargetChanging.length; j++) {
                        var targetChange = fullSkill.TargetChanging[j];
                        stat = getState(target, targetChange.StateType.Name);// _.where(target.State, { Value: targetChange.Value });
                        if (stat) {
                            stat.Number += targetChange.Number;
                            $scope.report += ' enemys' + stat.StateType.Name + ': ' + targetChange.Number + ' ';
                        }
                    }

                    $scope.recalculateState(self);
                    $scope.recalculateState(target);
                });
            }

            $scope.recalculateState = function (hero) {
                if (!hero || !hero.State)
                    return;
                var currentHpName = "Hp";
                var maxHpName = "MaxHp";
                var currentMpName = "Mp";
                var maxMpName = "MaxMp";

                var maxHpNumber = getState(hero, maxHpName).Number;
                var currentHpNumber = getState(hero, currentHpName).Number;
                var hpPercent = Math.round(100 * currentHpNumber / maxHpNumber);

                var maxMpNumber = getState(hero, maxMpName).Number;
                var currentMpNumber = getState(hero, currentMpName).Number;
                var mpPercent = Math.round(100 * currentMpNumber / maxMpNumber);

                if (currentHpNumber < 1) {
                    alert(hero.Name + ' is dead');
                    $scope.battleEnd = true;
                }

                hero.healthWidth = { 'width': hpPercent + '%' };
                hero.manaWidth = { 'width': mpPercent + '%' };
            }

            function getState(hero,stateName) {
                return _.find(hero.State, function(state) { return state.StateType.Name == stateName; });
            }
        }
    ]);
