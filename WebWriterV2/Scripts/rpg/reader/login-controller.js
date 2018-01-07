angular.module('rpg')

    .controller('loginController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService',
        'eventService', 'CKEditorService', 'userService', 'genreService', 'ConstCookies',
        function ($scope, $routeParams, $location, $cookies, bookService,
            eventService, CKEditorService, userService, genreService, ConstCookies) {

            $scope.user = {};
            $scope.wait = true;
            
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
                    } else {
                        $scope.error = 'Incorrect username or password';
                    }
                    $scope.waiting = false;
                    $scope.$emit('UpdateUserEvent');
                    $location.path('/');
                });
            }
        }
    ]);