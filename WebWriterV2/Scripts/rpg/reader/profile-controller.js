angular.module('rpg')

    .controller('profileController', ['$scope', '$cookies', '$location', '$uibModal',
        'ConstCookies', 'bookService', 'userService', 'travelService',
        function ($scope, $cookies, $location, $uibModal,
            ConstCookies, bookService, userService, travelService) {

            $scope.user = {};
            $scope.travels = [];
            $scope.waiting = false;

            init();

            //$scope.becomeWriter = function () {
            //    userService.becomeWriter().then(function () {
            //        init();
            //        $scope.$emit('UpdateUserEvent');
            //        var url = '/AngularRoute/admin/book/';
            //        $location.path(url);
            //    });
            //}

            $scope.uploadAvatar = function (event) {
                userService.uploadAvatar($scope.user.newAvatarData).then(function (response) {
                    $scope.user.AvatarUrl = response.AvatarUrl + "?d=" + new Date().getTime();
                    $scope.$emit('UpdateUserEvent');
                });
            }
            
            $scope.removeTravel = function (travelId, index) {
                travelService.remove(travelId).then(function () {
                    $scope.travels.splice(index, 1);
                });
            }

            $scope.updateShowExtendedFunctionality = function () {
                userService.updateShowExtendedFunctionality($scope.user.Id, $scope.user.ShowExtendedFunctionality)
                    .then(function (showExtendedFunctionality) {
                        $scope.user.ShowExtendedFunctionality = showExtendedFunctionality;
                    });
            }

            function init() {
                var userId = $cookies.get(ConstCookies.userId);
                if (userId) {
                    userService.getById(userId).then(function (data) {
                        $scope.user = data;
                    });
                }

                travelService.getByUserId(userId).then(function (travels){
                    $scope.travels = travels;
                });
            }
        }
    ]);

