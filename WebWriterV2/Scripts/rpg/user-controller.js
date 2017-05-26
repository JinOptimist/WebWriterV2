﻿angular.module('rpg')
    .controller('accessController', ['$rootScope', '$scope', '$cookies', '$location', 'ConstCookies', 'userService',
        function ($rootScope, $scope, $cookies, $location, ConstCookies, userService) {
            $scope.user = {};
            $scope.waiting = false;
            $scope.activeLogin = false;
            $scope.error = '';

            init();

            // this event can be calling from any controller
            $rootScope.$on('UpdateUserEvent', function (event, args) {
                init();
            });

            $scope.goToHomePage = function() {
                $location.path('/AngularRoute/listQuest');
            }

            $scope.passwordKeyPress = function ($event) {
                if ($event.which === 13) {// 'Enter'.keyEvent === 13
                    $scope.login();
                }
            }

            $scope.login = function () {
                $scope.waiting = true;
                $scope.error = '';
                userService.login($scope.user).then(function (result) {
                    if (result) {
                        $scope.user = result;
                        $cookies.put(ConstCookies.userId, $scope.user.Id);
                        $scope.activeLogin = false;
                    } else {
                        $scope.error = 'Incorrect username or password';
                    }
                    $scope.waiting = false;
                    init();
                });
            }

            $scope.openLogin = function() {
                $scope.activeLogin = true;
            }

            $scope.exit = function () {
                userService.logout();
                init();
                $scope.goToHomePage();
            }

            $scope.close = function () {
                $scope.activeLogin = false;
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
    .controller('registerController', ['$rootScope', '$scope', '$cookies', '$location', 'ConstCookies', 'userService',
        function ($rootScope, $scope, $cookies, $location, ConstCookies, userService) {
            $scope.user = {};
            $scope.waiting = false;
            $scope.error = '';

            init();

            $scope.goToHomePage = function () {
                $location.path('/AngularRoute/listQuest');
            }

            $scope.passwordKeyPress = function ($event) {
                if ($event.which === 13) {// 'Enter'.keyEvent === 13
                    $scope.register();
                }
            }

            $scope.register = function () {
                $scope.waiting = true;
                userService.register($scope.user)
                    .then(function (result) {
                        if (result) {
                            $cookies.put(ConstCookies.userId, result.Id);
                            $scope.$emit('UpdateUserEvent');
                            $scope.goToHomePage();
                        } else {
                            $scope.error = 'No!';
                        }
                    })
                    .catch(function (e) {
                        $scope.error = 'Nope';
                    })
                    .finally(function () {
                        $scope.waiting = false;
                        init();
                    });
            }

            $scope.isUserValid = function () {
                return !!$scope.user.Name && !!$scope.user.Password && !!$scope.user.Email;
            }

            function init() {
                $scope.user = {};
            }
        }
    ])
    .controller('aboutUsController', [
        '$scope',
        function ($scope) {
            $scope.negativeTypes = [
                {
                    type: 1,
                    name: "эгоист",
                    text: "эгоистом",
                    solution: "Однажды на утренней пробежке мальчик увидел как девочка заболиво осматривала лапу своего пса, и его сердце познало смысл слов «забота о ближнем»."
                },
                {
                    type: 2,
                    name: "трус",
                    text: "трусом",
                    solution: "Однажды на утренней пробежке мальчик увидел как девочка, стала на пути огромного пса исходившегося лаяем на её собачку. В тот момент он почувствовал что не имеет права оставаться в стороне и помог девочке став рядом с ней несмотря на трясущиеся коленки. Только на следующий день мальчик понял что стал лучше благодаря этому случаю."
                }
            ];
            $scope.storyTypes = [
                {
                    type: 1,
                    name: "сказка",
                    start: "Жили были мальчик и девочка.",
                    end: "И жили они долго и счастливо."
                },
                {
                    type: 2,
                    name: "повседневность",
                    start: "Мальчик встретил девочку.",
                    end: "Через 5 лет они поженились."
                },
                {
                    type: 3,
                    name: "фатализм",
                    start: "В вечности небытия словно искрыли вспыхнули две жизни.",
                    end: "И были они счасливы, но миру было наплевать ведь их жизнь ничего не изменили, а следовательно были совершенно бесмысленны."
                }
            ];
            $scope.complexTypes = [
                {
                    type: 1,
                    name: "недоверяет мальчик",
                    text: "Потому что её маму бросил папа и она недоверяла мальчик",
                    solution: "Как-то раз девочка увидела мальчика, заботливо помогающего своей маме нести сумки и её сердце оттаяло, она поняла, что не все мальчики плохие."
                },
                {
                    type: 2,
                    name: "некрасивая",
                    text: "Потому что её сестра была первой красавицей в школе и девочка считала себя некрасивой.",
                    solution: "Однако мальчик продолжал ухаживать за ней, делать ей комплименты и подарки подчёркивающие её положительные стороны и её сердце оттаяло."
                }
            ];
            $scope.male = {
                negativeType: {},
            };
            $scope.female = {
                complexType: {},
            };
            $scope.story = {
                storyType: {},
            };

            init();

            function init() {
                $scope.male.negativeType = $scope.negativeTypes[0];
                $scope.female.complexType = $scope.complexTypes[0];
                $scope.story.storyType = $scope.storyTypes[0];
            }

        }
    ])
    .controller('travelController', [
        '$scope', '$http', '$location', '$routeParams', '$cookies', '$timeout', 'evaluationService',
        'questService', 'eventService', 'heroService', 'userService',
        function ($scope, $http, $location, $routeParams, $cookies, $timeout, evaluationService,
            questService, eventService, heroService, userService) {
            $scope.quest = {};
            $scope.hero = {};
            $scope.currentEvent = {};
            $scope.ways = [];
            $scope.wait = true;
            /* animation */
            $scope.myCssVar = '';

            $scope.changes = [];
            $scope.markRange = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
            $scope.evaluation = {};

            init();

            $scope.chooseEvent = function (eventId, isBookmark) {
                if ($scope.hero.Id > 0 && !isBookmark) {
                    chooseEventWithHero(eventId);
                } else {
                    chooseEventOnClientSide(eventId, isBookmark);
                }
            };

            $scope.endQuest = function() {
                var questId = $scope.quest.Id;
                $scope.evaluation.QuestId = questId;
                evaluationService.save($scope.evaluation);

                questService.questCompleted(questId);
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

                var heroStatesChanging = $scope.currentEvent.HeroStatesChanging == null ? [] : $scope.currentEvent.HeroStatesChanging;
                var filterHeroStatesChangingFilter = heroStatesChanging.filter(function (state) {
                    return !state.StateType.HideFromReader;
                });
                if (filterHeroStatesChangingFilter.length > 0) {
                    filterHeroStatesChangingFilter.forEach(function (stateChanges) {
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
    .controller('listQuestController', [
        '$scope', '$http', '$location', 'questService',
        function($scope, $http, $location, questService) {
            //$scope.quests = [];

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
    .controller('profileController', ['$scope', '$cookies', '$location', '$uibModal', 'ConstCookies', 'questService', 'heroService', 'userService',
        function ($scope, $cookies, $location, $uibModal, ConstCookies, questService, heroService, userService) {
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

            $scope.becomeWriter = function () {
                userService.becomeWriter().then(function () {
                    init();
                    $scope.$emit('UpdateUserEvent');
                    var url = '/AngularRoute/admin/quest/';
                    $location.path(url);
                });
            }

            $scope.openStatePopup = function () {
                var model = {
                    templateUrl: 'views/rpg/admin/state.html',
                    controller: 'adminStateController',
                    windowClass: 'statesModal',
                    resolve: {
                        text: function () {
                            return 'Test';
                        }
                    }
                };
                $uibModal.open(model);
            }

            $scope.openThingPopup = function () {
                var model = {
                    templateUrl: 'views/rpg/admin/Thing.html',
                    controller: 'adminThingController',
                    windowClass: 'thingModal',
                    resolve: {
                        text: function () {
                            return 'Test';
                        }
                    }
                };
                $uibModal.open(model);
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
