angular.module('rpg')

    .controller('profileController', ['$scope', '$cookies', '$location', '$uibModal', 'ConstCookies', 'bookService', 'heroService', 'userService',
        function ($scope, $cookies, $location, $uibModal, ConstCookies, bookService, heroService, userService) {
            $scope.user = {};
            $scope.waiting = false;

            init();

            $scope.becomeWriter = function () {
                userService.becomeWriter().then(function () {
                    init();
                    $scope.$emit('UpdateUserEvent');
                    var url = '/AngularRoute/admin/book/';
                    $location.path(url);
                });
            }

            $scope.uploadAvatar = function (event) {
                userService.uploadAvatar($scope.user.newAvatarData).then(function (response) {
                    $scope.user.AvatarUrl = response.AvatarUrl;
                });
            }

            function init() {
                var userId = $cookies.get(ConstCookies.userId);
                if (userId) {
                    userService.getById(userId).then(function (data) {
                        $scope.user = data;
                        //$scope.user.Bookmarks.forEach(function (hero) {
                        //    bookService.get(hero.CurrentEvent.BookId)
                        //        .then(function (book) {
                        //            hero.CurrentEvent.book = book;
                        //        });
                        //});


                    });
                }
            }
        }
    ]);


 //$scope.removeAccount = function () {
            //    if (confirm('Are you sure that you whant remove your account?')) {
            //        var userId = $cookies.get(ConstCookies.userId);
            //        userService.removeAccount(userId)
            //            .then(function (data) {
            //                if (data) {
            //                    $cookies.remove(ConstCookies.userId);
            //                    $cookies.remove(ConstCookies.isAdmin);
            //                    $cookies.remove(ConstCookies.isWriter);
            //                    $scope.$emit('UpdateUserEvent');
            //                    var url = '/AngularRoute/listBook';
            //                    $location.path(url);
            //                }
            //            });
            //    }
            //}

            //$scope.removeBookmark = function (bookmark, index) {
            //    heroService.removeHero(bookmark)
            //        .then(function () {
            //            $scope.user.Bookmarks.splice(index, 1);
            //        });
            //}

            //$scope.goToBookmark = function (bookmark) {
            //    var bookId = bookmark.CurrentEvent.book.Id;
            //    var url = '/AngularRoute/travel/book/' + bookId + '/event/' + -1 + '/hero/' + bookmark.Id + '/true';
            //    $location.path(url);
            //}

            //$scope.openStatePopup = function () {
            //    var model = {
            //        templateUrl: 'views/rpg/admin/state.html',
            //        controller: 'adminStateController',
            //        windowClass: 'statesModal',
            //        resolve: {
            //            text: function () {
            //                return 'Test';
            //            }
            //        }
            //    };
            //    $uibModal.open(model);
            //}

            //$scope.openThingPopup = function () {
            //    var model = {
            //        templateUrl: 'views/rpg/admin/Thing.html',
            //        controller: 'adminThingController',
            //        windowClass: 'thingModal',
            //        resolve: {
            //            text: function () {
            //                return 'Test';
            //            }
            //        }
            //    };
            //    $uibModal.open(model);
            //}