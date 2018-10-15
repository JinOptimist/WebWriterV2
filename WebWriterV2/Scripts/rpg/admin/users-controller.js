angular.module('rpg')

    .controller('adminUsersController', [
        '$scope', '$routeParams', '$location', '$cookies', 'userService', 'tagService',
        function ($scope, $routeParams, $location, $cookies, userService, tagService) {

            $scope.users = null;
            $scope.resources = resources;

            init();

            $scope.remove = function (user, index) {
                if (!confirm(resources.Writer_ConfirmRemovingUser.format(user.Name))) {
                    return false;
                }
                userService.remove(user.Id)
                    .then(function () {
                        $scope.users.splice(index, 1);
                    });
            }

            function loadUsers() {
                userService.getAll().then(function (users) {
                    $scope.users = users;
                });
            }

            function init() {
                loadUsers();
            }
        }
    ]);