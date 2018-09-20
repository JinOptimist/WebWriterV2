
angular.module('rpg')
.controller('writerBookController', [
    '$scope', '$routeParams', '$location', '$window', '$cookies', '$mdDialog', 'ConstCookies',
        'bookService', 'chapterService', 'stateTypeService',
    function ($scope, $routeParams, $location, $window, $cookies, $mdDialog, ConstCookies,
        bookService, chapterService, stateTypeService) {

        var step = 0.1;
        var scale = 1.0;
        var minScale = 0.1;
        $scope.book = null;
        $scope.resources = resources;
        $scope.newStateType = {};

        var width = $window.innerWidth;
        var height = $window.innerHeight - 70;
        $scope.canvas = {
            width: width,
            height: height
        };

        init();

        $scope.rightClick = function (e) {
            bookMap.rightClick();
        }

        $scope.onResize = function (direction) {
            scale += step * direction;
            if (scale < minScale)
                scale = minScale;
            bookMap.resize(scale);
        }

        $scope.createNewStateType = function () {
            $scope.newStateType.BookId = $scope.book.Id;
            $scope.newStateType.OwnerId = $cookies.get(ConstCookies.userId);

            stateTypeService.add($scope.newStateType, $scope.book.Id).then(function (newState) {
                if (!$scope.book.States)
                    $scope.book.States = [];
                $scope.book.States.push(newState);
                $scope.newStateType = {};
            });
        }

        $scope.removeStateType = function (stateTypeId, index) {
            stateTypeService.remove(stateTypeId).then(function () {
                $scope.book.States.splice(index, 1);
            });
        }

        function moveToEditChapter(chapterId, updateDrawnChapter) {
            var dialogModel = {
                controller: 'chapterController',
                templateUrl: '/views/writer/Chapter.html',
                parent: angular.element(document.body),
                //targetEvent: ev,
                clickOutsideToClose: false,
                fullscreen: $scope.customFullscreen, // Only for -xs, -sm breakpoints.
                locals: { chapterId: chapterId },
            };

            $mdDialog.show(dialogModel)
                .then(function (chapter) {
                    if (chapter) {
                        updateDrawnChapter(chapter);
                    }
                }, function (problem) {
                    $scope.status = 'You cancelled the dialog.';
                });
        };

        function addChapter(parentId, updateDrawnChapter) {
            chapterService.createChild(parentId).then(function (savedChapter) {
                moveToEditChapter(savedChapter.Id, updateDrawnChapter);
            });
        }

        function remove(chapter) {
            if (confirm(resources.RemoveChapterConfirmation.format(chapter.Name))) {
                chapterService.remove(chapter.Id).then(function (result) {
                    if (result) {
                        //init();
                    } else {
                        alert(resources.RemoveChapterImpossibleAlert.format(chapter.Name));
                        init();
                    }
                });
                return true;
            }
            return false;
        }

        function createLink(fromId, toId, onSuccessedLinkCreate) {
            chapterService.createLink(fromId, toId).then(function (link) {
                onSuccessedLinkCreate(link);
            });
        }

        function removeLink(linkId) {
            chapterService.removeLink(linkId).then(function (result) {
                if (result) {
                    //init();
                } else {
                    alert(resources.RemoveLinkImpossibleAlert);
                    init();
                }
            });
        }

        function validate(chapters) {
            $scope.valid = true;

            chapters.forEach(x => $scope.valid = $scope.valid && chapterProcessing.validateChapter(x));

            setTimeout(function () { $scope.$apply(); }, 1);
        }

        function loadChaptersV2(bookId) {
            var actions = {
                moveToEditChapter: moveToEditChapter,
                addChapter: addChapter,
                remove: remove,
                createLink: createLink,
                removeLink: removeLink,
                validate: validate
            };

            bookService.getWithChaptersV2(bookId).then(function (book) {
                $scope.book = book;
                bookMap.start(book, actions, $scope.canvas)
            });
        }

        function documentKeyPressed(e) {
            // 'esc'.which == 27
            if (e.which === 27) {
                bookMap.rightClick();
            }
        }

        function init() {
            var bookId = $routeParams.bookId;

            loadChaptersV2(bookId);

            document.onkeydown = function (e) {
                $scope.$apply(documentKeyPressed(e));
            };
        }
    }
]);