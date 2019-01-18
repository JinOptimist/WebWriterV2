angular.module('rpg')

    .controller('landingController', ['$scope', '$cookies', '$location', '$uibModal',
        'ConstCookies', 'bookService', 'userService', 'travelService',
        function ($scope, $cookies, $location, $uibModal,
            ConstCookies, bookService, userService, travelService) {

            $scope.user = null;
            $scope.loginObj = {};
            $scope.visibleBlock = {
                initButton: false
            };
            $scope.waiting = false;

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
                            $location.path('/');
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

            init();

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

                var canvasSize = {
                    width: 400,
                    height: 100
                };

                newOneBookMap().oneChapterLayer('add-chapter', canvasSize, fakeChapter('base state', drawShapes.chapterStateType.Initial));
                newOneBookMap().oneChapterLayer('available-chapter', canvasSize, fakeChapter('available to link', drawShapes.chapterStateType.AvailableToLink));
                newOneBookMap().oneChapterLayer('parent-chapter', canvasSize, fakeChapter('remove link', drawShapes.chapterStateType.Parent));
                newOneBookMap().oneChapterLayer('remove-chapter', canvasSize, fakeChapter('remove chapter', drawShapes.chapterStateType.Selected));
                newOneBookMap().oneChapterLayer('fake-chapter', canvasSize, fakeChapter('fake chapter', drawShapes.chapterStateType.FakeNew));
            }

            function fakeChapter(name, state) {
                return {
                    Name: name,
                    VisualParentIds: [],
                    LinksFromThisChapter: [{}],
                    overrideState: state,
                    Id: state == drawShapes.chapterStateType.FakeNew ? -1 : 0
                }
            }
        }
    ]);

