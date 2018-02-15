angular.module('rpg')

    .controller('accessController', ['$rootScope', '$scope', '$cookies', '$location', 'ConstCookies', 'userService',
        function ($rootScope, $scope, $cookies, $location, ConstCookies, userService) {
            $scope.user = {};
            $scope.waiting = false;
            $scope.isLoginPopupOpen = { val: false };
            $scope.error = '';

            init();

            // this event can be calling from any controller
            $rootScope.$on('UpdateUserEvent', function (event, args) {
                init();
            });

            $scope.openLogin = function () {
                $scope.isLoginPopupOpen.val = true;
            }

            $scope.close = function () {
                $scope.isLoginPopupOpen.val = false;
            }


            $scope.exit = function () {
                userService.logout();
                init();
                $scope.goToHomePage();
            }

            $scope.goToHomePage = function () {
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
                        $scope.isLoginPopupOpen.val = false;
                    } else {
                        $scope.error = 'Incorrect username or password';
                    }
                    $scope.waiting = false;
                    init();
                });
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
    ]);