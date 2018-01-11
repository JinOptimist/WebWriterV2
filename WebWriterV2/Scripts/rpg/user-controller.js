angular.module('rpg')
    /*Move to separate file*/
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
                $location.path('/');
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
                    $scope.user = null;
                }
            }
        }
    ])
    /*Move to separate file*/
    .controller('registerControllerOld', ['$rootScope', '$scope', '$cookies', '$location', '$window', 'ConstCookies', 'userService',
        function ($rootScope, $scope, $cookies, $location, $window, ConstCookies, userService) {
            $scope.user = {};
            $scope.waiting = false;
            $scope.error = '';

            init();

            $scope.vk = function () {
                //https://vk.com/dev/auth_sites
                $window.open('https://oauth.vk.com/authorize?'
                        + 'client_id=' + 4279045
                        + '&redirect_uri=' + 'http://localhost:52079/rpg/RegisterVkComplete'
                        + '&display=' + 'popup'
                        + '&scope=' + 4194304 // bit rules https://vk.com/dev/permissions
                        + '&state=' + 'smile'
                    , '_blank');
            }

            $scope.goToHomePage = function () {
                $location.path('/');
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
        '$scope', '$location', '$anchorScroll',
        function ($scope, $location, $anchorScroll) {
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

            $scope.goTo = function (anchor) {
                $location.hash(anchor);
                $anchorScroll();
            }

            init();

            function init() {
                $scope.male.negativeType = $scope.negativeTypes[0];
                $scope.female.complexType = $scope.complexTypes[0];
                $scope.story.storyType = $scope.storyTypes[0];
            }
        }
    ])
    .controller('travelControllerOld', [
        '$scope', '$http', '$location', '$routeParams', '$cookies', '$timeout', '$q', 'evaluationService',
        'bookService', 'eventService', 'heroService', 'userService',
        function ($scope, $http, $location, $routeParams, $cookies, $timeout, $q, evaluationService,
            bookService, eventService, heroService, userService) {
            $scope.book = {};
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

            $scope.endBook = function() {
                var bookId = $scope.book.Id;
                $scope.evaluation.BookId = bookId;
                evaluationService.save($scope.evaluation);

                bookService.bookCompleted(bookId);
                $location.path('/AngularRoute/listBook');
            }

            $scope.batle = function() {
                heroService.selectHero($scope.book.Executor);
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

                var heroStatesChanging = $scope.currentEvent.HeroStatesChanging === null ? [] : $scope.currentEvent.HeroStatesChanging;
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
                    $scope.book.Effective += result.ProgressChanging;
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
                    var event = result.frontChapter;
                    $scope.hero = result.frontHero;

                    $scope.ways = event.LinksFromThisEvent;
                    $scope.book.Effective += event.ProgressChanging;
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
                var bookId = $routeParams.bookId;
                var eventId = $routeParams.eventId;
                var heroId = $routeParams.heroId;
                var isBookmark = angular.fromJson($routeParams.isBookmark);

                //if (eventId != -1)

                var heroPromise = heroService.load(heroId);
                var bookPromise = bookService.get(bookId);
                var eventPromise = eventService.get(eventId);
                $q.all([heroPromise, bookPromise, eventPromise]).then(function (results) {
                    $scope.hero = results[0];
                    $scope.book = results[1];
                    $scope.currentEvent = results[2]
                        ? results[2]
                        : $scope.hero.CurrentEvent
                            ? $scope.hero.CurrentEvent
                            : $scope.book.RootEvent;;

                    $scope.chooseEvent($scope.currentEvent.Id, isBookmark);
                });
            }
        }
    ])
    .controller('listBookController', [
        '$scope', '$http', '$location', 'bookService',
        function($scope, $http, $location, bookService) {
            //$scope.books = [];

            init();

            $scope.goToBook = function (book) {
                $location.path('/AngularRoute/travel/book/' + book.Id + '/event/' + -1 + '/hero/' + -1 + '/false');
            }

            function init() {
                bookService.getAll().then(function (result) {
                    $scope.books = result;
                });
            }
        }
    ])
    .controller('profileController', ['$scope', '$cookies', '$location', '$uibModal', 'ConstCookies', 'bookService', 'heroService', 'userService',
        function ($scope, $cookies, $location, $uibModal, ConstCookies, bookService, heroService, userService) {
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
                                var url = '/AngularRoute/listBook';
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
                var bookId = bookmark.CurrentEvent.book.Id;
                var url = '/AngularRoute/travel/book/' + bookId + '/event/' + -1 + '/hero/' + bookmark.Id + '/true';
                $location.path(url);
            }

            $scope.becomeWriter = function () {
                userService.becomeWriter().then(function () {
                    init();
                    $scope.$emit('UpdateUserEvent');
                    var url = '/AngularRoute/admin/book/';
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

            $scope.uploadAvatar = function (event) {
                userService.uploadAvatar($scope.user.newAvatarData).then(function (response) {
                    $scope.user.AvatarUrl = response;
                });
            }

            function init() {
                var userId = $cookies.get(ConstCookies.userId);
                if (userId) {
                    userService.getById(userId).then(function (data) {
                        $scope.user = data;
                        $scope.user.Bookmarks.forEach(function(hero) {
                            bookService.get(hero.CurrentEvent.BookId)
                                .then(function(book) {
                                    hero.CurrentEvent.book = book;
                                });
                        });


                    });
                }
            }
        }
    ]);
