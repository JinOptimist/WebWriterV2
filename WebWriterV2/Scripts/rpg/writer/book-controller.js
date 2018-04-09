
angular.module('rpg')
.controller('writerBookController', [
    '$scope', '$routeParams', '$location', '$window', 'bookService', 'chapterService',
    function ($scope, $routeParams, $location, $window, bookService, chapterService) {

        var step = 0.1;
        var scale = 1.0;
        var minScale = 0.1;
        $scope.book = null;
        $scope.wait = false;

        //max 1560
        //min 1020
        var width = $window.innerWidth > 1560
            ? 1560
            : $window.innerWidth < 1020 
                ? 1020
                : $window.innerWidth
        $scope.canvas = {
            width: width,
            height: 500
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

        $scope.showAdvanced = function ($event) {
            $mdDialog.show({
                controller: chapterController,
                templateUrl: '/views/writer/Chapter.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                clickOutsideToClose: true,
                fullscreen: true // Only for -xs, -sm breakpoints.
            })
            .then(function (answer) {
                $scope.status = 'You said the information was "' + answer + '".';
            }, function () {
                $scope.status = 'You cancelled the dialog.';
            });
        };

        function addChapter(parentId) {
            chapterService.createChild(parentId).then(function (savedChapter) {
                moveToEditChapter(savedChapter.Id, true);
            });
        }

        function remove(chapter) {
            if (confirm(resources.RemoveChapterConfirmation.format(chapter.Name))) {
                chapterService.remove(chapter.Id).then(function (result) {
                    if (result) {
                        init();
                    } else {
                        alert(resources.RemoveChapterImpossibleAlert.format(chapter.Name));
                    }
                });
                return true;
            }
            return false;
        }

        function createLink(fromId, toId) {
            chapterService.createLink(fromId, toId).then(function () {
                init();
            });
        }

        function removeLink(linkId) {
            chapterService.removeLink(linkId).then(function (result) {
                if (result) {
                    init();
                } else {
                    alert(resources.RemoveLinkImpossibleAlert);
                }
            });
        }

        function moveToEditChapter(chapterId, avoidApply) {
            $location.path('/ar/writer/chapter/' + chapterId);
            if (!avoidApply) {
                $scope.$apply();
            }
        }

        function loadChaptersV2(bookId) {
            $scope.wait = true;

            var actions = {
                moveToEditChapter: moveToEditChapter,
                addChapter: addChapter,
                remove: remove,
                createLink: createLink,
                removeLink: removeLink
            };

            bookService.getWithChaptersV2(bookId).then(function (book) {
                $scope.book = book;
                var maxDepth = Math.max.apply(Math, book.Chapters.map(x => x.Level));
                $scope.canvas.height = maxDepth * (Const.ChapterSize.Height + Const.ChapterSize.Padding) + 100
                setTimeout(function () { bookMap.start(book.Chapters, actions, $scope.canvas) }, 0);
                $scope.wait = false;
            });
        }

        function init() {
            var bookId = $routeParams.bookId;

            loadChaptersV2(bookId);
        }
    }
]);