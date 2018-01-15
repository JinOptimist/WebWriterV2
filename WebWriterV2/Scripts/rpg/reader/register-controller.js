angular.module('rpg')

    .controller('registerController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService',
        'eventService', 'CKEditorService', 'userService', 'genreService', 'ConstCookies',
        function ($scope, $routeParams, $location, $cookies, bookService,
            eventService, CKEditorService, userService, genreService, ConstCookies) {

            $scope.user = {};
            $scope.waiting = false;
            $scope.error = '';

            init();

            //$scope.vk = function () {
            //    //https://vk.com/dev/auth_sites
            //    $window.open('https://oauth.vk.com/authorize?'
            //            + 'client_id=' + 4279045
            //            + '&redirect_uri=' + 'http://localhost:52079/rpg/RegisterVkComplete'
            //            + '&display=' + 'popup'
            //            + '&scope=' + 4194304 // bit rules https://vk.com/dev/permissions
            //            + '&state=' + 'smile'
            //        , '_blank');
            //}

            $scope.checkName = function () {
                userService.nameIsAvailable($scope.user.Name).then(function (isAvailable) {
                    $scope.error = isAvailable ? '' : 'already exist';
                });
            }

            $scope.passwordKeyPress = function ($event) {
                if ($event.which === 13) {// 'Enter'.keyEvent === 13
                    $scope.register();
                }
            }

            $scope.nameKeyPress = function () {
                var now = new Date();
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
                return !$scope.error && !!$scope.user.Name && !!$scope.user.Password && !!$scope.user.Email;
            }

            function init() {
                $scope.user = {};
            }
        }
    ]);