angular.module('rpg')
    .controller('accessController', ['$rootScope', '$scope', '$cookies', '$location', 'ConstCookies', 'userService',
        function ($rootScope, $scope, $cookies, $location, ConstCookies, userService) {
            $scope.user = {};
            $scope.waiting = false;
            $scope.activeLogin = false;
            $scope.activeRegister = false;
            $scope.error = '';

            init();

            // this event can be calling from any controller
            $rootScope.$on('UpdateUserEvent', function (event, args) {
                init();
            });

            $scope.login = function () {
                $scope.waiting = true;
                $scope.error = '';
                userService.login($scope.user).then(function (result) {
                    if (result) {
                        $scope.user = result;
                        $cookies.put(ConstCookies.userId, $scope.user.Id);
                        $scope.activeRegister = false;
                        $scope.activeLogin = false;
                    } else {
                        $scope.error = 'Incorrect username or password';
                    }
                    $scope.waiting = false;
                    init();
                });
            }

            $scope.register = function () {
                $scope.waiting = true;
                userService.register($scope.user)
                    .then(function(result) {
                        if (result) {
                            $scope.user = result;
                            $cookies.put(ConstCookies.userId, result.Id);
                            $scope.activeRegister = false;
                            $scope.activeLogin = false;
                        } else {
                            $scope.error = 'No!';
                        }
                    })
                    .catch(function (e) {
                        $scope.error = 'Nope';
                    })
                    .finally(function() {
                        $scope.waiting = false;
                        init();
                    });
            }

            $scope.openLogin = function() {
                $scope.activeLogin = true;
            }

            $scope.openRegister = function() {
                $scope.activeRegister = true;
            }

            $scope.exit = function () {
                $cookies.remove(ConstCookies.userId);
                $cookies.remove(ConstCookies.isAdmin);
                $cookies.remove(ConstCookies.isWriter);
                init();
                $location.path('/AngularRoute/listQuest');
            }

            function init() {
                var userId = $cookies.get(ConstCookies.userId);
                if (userId) {
                    userService.getById(userId).then(function (data) {
                        $scope.user = data;
                    });
                } else {
                    $scope.user = {};
                }
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
        '$scope', '$location', '$cookies', 'guildService', 'questService', 'traningRoomService','heroService',
        function ($scope, $location, $cookies, guildService, questService, traningRoomService, heroService) {
            $scope.guild = {};
            $scope.currentHero = {};
            $scope.quests = [];

            init();

            $scope.restoreHero = function (hero) {
                heroService.restoreHero(hero.Id).then(function (data) {
                    heroService.updateHeroState(hero, data);
                    $scope.guild.Gold -= 5;
                });
            }

            $scope.hitHeroDebug = function (hero) {
                var st = hero.State.find(function (state) {
                    return state.StateType.Name == "Hp";
                });

                st.Number -= 5;
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
                if (!$scope.currentHero
                    || $scope.currentHero.Id < 1) {
                    alert('You forgot to select hero');
                    return;
                }
                if (heroService.getHp($scope.currentHero) < 1) {
                    alert('You hero is dead. How do you think he can go to quest?');
                    return;
                }
                $location.path('/AngularRoute/travel/quest/' + quest.Id + '/hero/' + $scope.currentHero.Id);
            }

            $scope.goToTrain = function(room) {
                if (!$scope.currentHero || $scope.currentHero.Id < 1) {
                    alert("Hero doesn't chosen");
                    return;
                }

                //traningRoomService.chooseRoom(room, $scope.currentHero);
                var url = '/AngularRoute/traningRoom/' + room.Id + '/hero/' + $scope.currentHero.Id;
                $location.path(url);
            }

            function init() {
                var guildId = $cookies.get('guildId');

                guildService.loadGuild(guildId).then(function (guild) {
                    $scope.guild = guild;
                });

                questService.getQuests().then(function (result) {
                    $scope.quests = result;
                });
            }
        }
    ])
    .controller('travelController', [
        '$scope', '$http', '$location', '$routeParams', '$cookies', '$timeout',
        'questService', 'eventService', 'guildService', 'heroService', 'userService',
        function ($scope, $http, $location, $routeParams, $cookies, $timeout,
            questService, eventService, guildService, heroService, userService) {
            $scope.quest = {};
            $scope.hero = {};
            $scope.currentEvent = {};
            $scope.ways = [];
            $scope.wait = true;
            /* animation */
            $scope.myCssVar = '';

            $scope.changes = [];

            init();

            $scope.chooseEvent = function (eventId, isBookmark) {
                if ($scope.hero.Id > 0 && !isBookmark) {
                    chooseEventWithHero(eventId);
                } else {
                    chooseEventOnClientSide(eventId, isBookmark);
                }
            };

            $scope.endQuest = function() {
                //$scope.waiting = true;
                //var guildId = $cookies.get('guildId');
                //var url = '/Rpg/QuestCompleted?guildId=' + guildId + '&gold=' + $scope.quest.Effective;
                //$http({
                //        method: 'POST',
                //        url: url,
                //        headers: { 'Accept': 'application/json' }
                //    })
                //    .then(function(response) {
                //        if (response.data == "+") {
                //            $location.path('/AngularRoute/listQuest');
                //        } else {
                //            alert(response);
                //        }
                //        $scope.waiting = false;
                //    }, function() {
                //        alert("Hero want relax. Wait and try again");
                //        $scope.waiting = false;
                //    });

                $location.path('/AngularRoute/listQuest');
            }

            $scope.batle = function() {
                heroService.selectHero($scope.quest.Executor);
                $location.path('/AngularRoute/battle');
            }

            $scope.removeChange = function (changesId) {
                $scope.changes.forEach(function (el) {
                    if (el.id === changesId) {
                        el.hideClassName = 'change-item';
                        return;
                    }
                });


                (function(id) {
                    $timeout(function() {
                        $scope.changes = $scope.changes.filter(function(change) {
                            return change.id !== id;
                        });
                    }, 2000);
                })(changesId);
            }

            $scope.createBookmark = function () {
                var eventId = $scope.currentEvent.Id;
                var heroJson = angular.toJson($scope.hero);
                userService.addBookmark(eventId, heroJson);
            }

            function alertChanges() {
                if ($scope.currentEvent.ThingsChanges
                    && $scope.currentEvent.ThingsChanges.length > 0) {
                    $scope.currentEvent.ThingsChanges.forEach(function (thingChanges) {
                        var symbol = thingChanges.Count > 0 ? '+' : '';
                        var message = thingChanges.ThingSample.Name + ':' + symbol + thingChanges.Count;
                        var changeId = 't' + thingChanges.Id;
                        $scope.changes.push({ id: changeId, text: message });
                        callRemoveChange(changeId);
                    });
                }

                if ($scope.currentEvent.HeroStatesChanging
                    && $scope.currentEvent.HeroStatesChanging.length > 0) {
                    $scope.currentEvent.HeroStatesChanging.forEach(function (stateChanges) {
                        var symbol = stateChanges.Number > 0 ? '+' : '';
                        var message = stateChanges.StateType.Name + ':' + symbol + stateChanges.Number;
                        var changeId = 's' + stateChanges.Id;
                        $scope.changes.push({ id: changeId, text: message });
                        callRemoveChange(changeId);
                    });
                }
            }

            function callRemoveChange(changeId) {
                $timeout(function () {
                    $scope.removeChange(changeId);
                }, 5000);
            }

            function chooseEventWithHero(eventId) {
                $scope.wait = true;
                eventService.getEventForTravel(eventId, $scope.hero.Id).then(function(result) {
                    $scope.ways = result.LinksFromThisEvent;
                    $scope.quest.Effective += result.ProgressChanging;
                    $scope.currentEvent = result;
                    $scope.wait = false;
                    alertChanges();
                });

                eventService.eventChangesApplyToHero(eventId, $scope.hero.Id).then(function (heroUpdated) {
                    var hero = $scope.hero;
                    heroUpdated.State.forEach(function (state) {
                        var stateTypeId = state.StateType.Id;
                        setState(hero, stateTypeId, state.Number);
                        //if (heroService.getHp(hero) < 1) {
                        //    alert('Your hero is Dead. Noob!');
                        //    $location.path('/AngularRoute/guild');
                        //    return;
                        //}
                    });

                    hero.Inventory = [];

                    heroUpdated.Inventory.forEach(function (thing) {
                        setThing(hero, thing.ThingSample, thing.Count);
                    });
                });
            }

            function chooseEventOnClientSide(eventId, isBookmark) {
                $scope.wait = true;
                eventService.getEventForTravelWithHero(eventId, $scope.hero, !isBookmark).then(function (result) {
                    var event = result.frontEvent;
                    $scope.hero = result.frontHero;

                    $scope.ways = event.LinksFromThisEvent;
                    $scope.quest.Effective += event.ProgressChanging;
                    $scope.currentEvent = event;
                    $scope.wait = false;
                    if (!isBookmark) {
                        alertChanges();
                    }
                });
            }

            function getState(hero, stateTypeId) {
                return _.find(hero.State, function (state) { return state.StateType.Id === stateTypeId; });
            }

            function setState(hero, stateTypeId, value) {
                return _.find(hero.State, function(state) {
                     if (state.StateType.Id === stateTypeId) {
                         state.Number = value;
                         return;
                     }
                });
            }

            function setThing(hero, thingSample, value) {
                var updateThing = _.find(hero.Inventory, function (thing) {
                    if (thing.ThingSample.Id === thingSample.Id) {
                        thing.Number = value;
                        return true;
                    }
                });

                if (!updateThing) {
                    hero.Inventory.push({
                        ItemInUse: false,
                        Count: value,
                        ThingSample: thingSample
                    });
                }
            }

            function init() {
                var questId = $routeParams.questId;
                var heroId = $routeParams.heroId;
                var isBookmark = angular.fromJson($routeParams.isBookmark);
                heroService.load(heroId).then(function(data) {
                    $scope.hero = data;

                    questService.get(questId).then(function (result) {
                        $scope.quest = result;
                        $scope.quest.Effective = 0;
                        if ($scope.hero.CurrentEvent) {
                            $scope.currentEvent = $scope.hero.CurrentEvent;
                        } else {
                            $scope.currentEvent = $scope.quest.RootEvent;
                        }

                        $scope.chooseEvent($scope.currentEvent.Id, isBookmark);
                    });
                });
            }
        }
    ])
    .controller('traningRoomController', [
        '$scope', '$http', '$routeParams', 'traningRoomService', 'heroService',
        function($scope, $http, $routeParams, traningRoomService, heroService) {
            $scope.room = {};//traningRoomService.getRoom();
            $scope.hero = {}; //traningRoomService.getHero();
            $scope.currentSkill = null;
            $scope.waiting = false;

            init();

            $scope.selectSkill = function(skill) {
                $scope.currentSkill = skill;
            }

            $scope.availableSkills = function () {
                if (!$scope.room
                    || !$scope.hero
                    || !$scope.room.School
                    || !$scope.room.School.Skills
                    || !$scope.hero.Skills) {
                    return [];
                }

                var roomSkills = $scope.room.School.Skills;
                var heroSkills = $scope.hero.Skills;

                return roomSkills.filter(function (roomSkill) {
                    return heroSkills.every(function (heroSkill) {
                        return roomSkill.Id != heroSkill.Id;
                    });
                });
            }

            $scope.addSkill = function () {
                $scope.waiting = true;
                heroService.addSkill($scope.hero.Id, $scope.currentSkill.Id).then(
                    function (response) {
                        $scope.hero = response;
                        $scope.currentSkill = null;
                        $scope.waiting = false;
                    },
                    function () {
                        alert("Training was failed. Try again");
                        $scope.waiting = false;
                    });
            }

            function init() {
                traningRoomService.load($routeParams.roomId).then(function (data) {
                    $scope.room = data;
                });

                heroService.load($routeParams.heroId).then(function (data) {
                    $scope.hero = data;
                });
            }
        }
    ])
    .controller('listQuestController', [
        '$scope', '$http', '$location', 'questService', 'raceService', 'sexService',
        function($scope, $http, $location, questService, raceService, sexService) {
            $scope.quests = [];

            init();

            $scope.goToQuest = function (quest) {
                $location.path('/AngularRoute/travel/quest/' + quest.Id + '/hero/' + -1 + '/false');
            }

            function init() {
                questService.getQuests().then(function (result) {
                    $scope.quests = result;
                });
            }
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
    ])
    .controller('profileController', ['$scope', '$cookies', '$location', 'ConstCookies', 'questService', 'heroService', 'userService',
        function ($scope, $cookies, $location, ConstCookies, questService, heroService, userService) {
            $scope.user = {};
            $scope.waiting = false;

            init();

            $scope.removeAccount = function () {
                if (confirm('Are you sure that you whant remove your account?')) {
                    var userId = $cookies.get(ConstCookies.userId);
                    userService.removeAccount(userId)
                        .then(function (data) {
                            if (data) {
                                $cookies.remove(ConstCookies.userId);
                                $cookies.remove(ConstCookies.isAdmin);
                                $cookies.remove(ConstCookies.isWriter);
                                $scope.$emit('UpdateUserEvent');
                                var url = '/AngularRoute/listQuest';
                                $location.path(url);
                            }
                        });
                }
            }

            $scope.removeBookmark = function (bookmark, index) {
                heroService.removeHero(bookmark)
                    .then(function() {
                        $scope.user.Bookmarks.splice(index, 1);
                    });
            }

            $scope.goToBookmark = function (bookmark) {
                var questId = bookmark.CurrentEvent.quest.Id;
                var url = '/AngularRoute/travel/quest/' + questId + '/hero/' + bookmark.Id + '/true';
                $location.path(url);
            }

            function init() {
                var userId = $cookies.get(ConstCookies.userId);
                if (userId) {
                    userService.getById(userId).then(function (data) {
                        $scope.user = data;
                        $scope.user.Bookmarks.forEach(function(hero) {
                            questService.get(hero.CurrentEvent.QuestId)
                                .then(function(quest) {
                                    hero.CurrentEvent.quest = quest;
                                });
                        });


                    });
                }
            }
        }
    ]);
