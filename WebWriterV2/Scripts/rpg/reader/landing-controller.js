angular.module('rpg')

    .controller('landingController', ['$scope', '$cookies', '$location', '$uibModal',
        'ConstCookies', 'bookService', 'userService', 'travelService',
        function ($scope, $cookies, $location, $uibModal,
            ConstCookies, bookService, userService, travelService) {

            $scope.user = {};
            $scope.visibleBlock = {
                initButton: false
            };
            $scope.waiting = false;

            init();

            function init() {
                var userId = $cookies.get(ConstCookies.userId);
                if (userId) {
                    userService.getById(userId).then(function (data) {
                        $scope.user = data;
                    });
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

