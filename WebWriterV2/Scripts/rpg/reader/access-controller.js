angular.module('rpg')

    .controller('accessController', ['$rootScope', '$scope', '$route', '$cookies', '$location', 'ConstCookies', 'userService',
        function ($rootScope, $scope, $route, $cookies, $location, ConstCookies, userService) {
            const PopupState = {
                Login: 'Login',
                Registration: 'Registration',
                RecoverPassword: 'RecoverPassword',
                CheckEmail: 'CheckEmail'
            };

            $scope.PopupState = PopupState;
            $scope.user = {};
            $scope.loginObj = {};
            $scope.waiting = false;
            $scope.popupOpen = { main: false, popupState: null };
            $scope.error = '';
            $scope.resources = resources;
            $scope.route = {
                isTravel: $route.current.controller == 'travelController'
                    || $route.current.controller == 'travelGuestController'
            };

            init();

            // this event can be calling from any controller
            $rootScope.$on('UpdateUserEvent', function (event, args) {
                init();
            });

            $scope.$on('$routeChangeStart', function ($event, next, current) {
                $scope.route.isTravel = next.controller == 'travelController'
                    || next.controller == 'travelGuestController';
            });

            $scope.mousemove = function ($event) {
                $scope.route.isUp = $event.y < 20;
            }

            $scope.goToHomePage = function () {
                $location.path('/');
            }

            $scope.exit = function () {
                userService.logout();
                init();
                $scope.goToHomePage();
            }

            $scope.openLogin = function () {
                $scope.popupOpen.main = true;
                $scope.popupOpen.popupState = PopupState.Login;
            }
            $scope.openRegistration = function () {
                $scope.popupOpen.main = true;
                $scope.popupOpen.popupState = PopupState.Registration;
            }

            $scope.close = function () {
                $scope.popupOpen.main = false;
                $scope.popupOpen.popupState = null;
            }

            $scope.idle = function ($event) {
                $scope.error = '';
                //$event.stopPropagation();
            }

            $scope.passwordKeyPress = function ($event) {
                if ($event.which === 13) {// 'Enter'.keyEvent === 13
                    if ($scope.popupOpen.popupState == PopupState.Login) {
                        $scope.login();
                    } else if ($scope.popupOpen.popupState == PopupState.Registration) {
                        $scope.register();
                    }
                    
                }
            }

            $scope.login = function () {
                $scope.waiting = true;
                $scope.error = '';
                userService.login($scope.loginObj).then(function (result) {
                    if (result) {
                        $scope.user = result;
                        $cookies.put(ConstCookies.userId, $scope.user.Id);
                        $scope.close();
                    } else {
                        $scope.error = resources.Reader_IncorrectUsernameOrPassword;
                    }
                    $scope.waiting = false;
                    init();
                    $location.path('/');
                });
            }

            $scope.switchToRecoverPassword = function () {
                $scope.popupOpen.popupState = PopupState.RecoverPassword;
            }

            $scope.register = function () {
                $scope.waiting = true;
                userService.register($scope.loginObj)
                    .then(function (result) {
                        if (!result || result.Error) {
                            $scope.error = result.Error;
                        } else {
                            $cookies.put(ConstCookies.userId, result.Id);
                            $scope.close();
                            init();
                            $location.path('/');
                        }
                    })
                    .catch(function (e) {
                        $scope.error = resources.Reader_IncorrectUsernameOrPassword;
                    })
                    .finally(function () {
                        $scope.waiting = false;
                        init();
                    });
            }

            $scope.recoverPassword = function () {
                $scope.popupOpen.popupState = PopupState.CheckEmail;
            }

            $scope.back = function () {
                if ($scope.popupOpen.popupState == PopupState.RecoverPassword
                    || $scope.popupOpen.popupState == PopupState.CheckEmail) {
                    $scope.popupOpen.popupState = PopupState.Login;
                }
            }

            function init() {
                var userId = userService.getCurrentUserId();
                if (userId) {
                    userService.getById(userId).then(function (data) {
                        $scope.user = data;
                        if ($scope.user.AvatarUrl) {
                            $scope.user.AvatarUrl += "?d=" + new Date().getTime();
                        }
                    });
                } else {
                    $scope.user = null;
                }
            }
        }
    ]);