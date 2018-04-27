angular.module('rpg')

    .controller('accessController', ['$rootScope', '$scope', '$cookies', '$location', 'ConstCookies', 'userService',
        function ($rootScope, $scope, $cookies, $location, ConstCookies, userService) {
            $scope.user = {};
            $scope.loginObj = {};
            $scope.waiting = false;
            $scope.popupOpen = { main: false };
            $scope.error = '';
            $scope.resources = resources;

            init();

            // this event can be calling from any controller
            $rootScope.$on('UpdateUserEvent', function (event, args) {
                init();
            });

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
                $scope.popupOpen.isLogin = true;
                $scope.popupOpen.isRegistration = false;
            }
            $scope.openRegistration = function () {
                $scope.popupOpen.main = true;
                $scope.popupOpen.isLogin = false;
                $scope.popupOpen.isRegistration = true;
            }

            $scope.close = function () {
                $scope.popupOpen.main = false;
                $scope.popupOpen.isLogin = false;
                $scope.popupOpen.isRegistration = false;
            }

            $scope.idle = function ($event) {
                $event.stopPropagation();
            }

            $scope.passwordKeyPress = function ($event) {
                if ($event.which === 13) {// 'Enter'.keyEvent === 13
                    if ($scope.popupOpen.isLogin) {
                        $scope.login();
                    } else if ($scope.popupOpen.isRegistration) {
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
                        $scope.error = resources.IncorrectUsernameOrPassword;
                    }
                    $scope.waiting = false;
                    init();
                    $location.path('/');
                });
            }

            $scope.register = function () {
                if (!$scope.isUserValid()) {
                    return;
                }

                $scope.waiting = true;
                userService.register($scope.loginObj)
                    .then(function (result) {
                        if (!result || result.Error) {
                            $scope.error = result.Error;
                        } else {
                            $cookies.put(ConstCookies.userId, result.Id);
                            $scope.close();
                            init();
                        }
                    })
                    .catch(function (e) {
                        $scope.error = resources.IncorrectUsernameOrPassword;
                    })
                    .finally(function () {
                        $scope.waiting = false;
                        init();
                    });
            }

            $scope.isUserValid = function () {
                return !!$scope.loginObj.Name && !!$scope.loginObj.Password && !!$scope.loginObj.Email;
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