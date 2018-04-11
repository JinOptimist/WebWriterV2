
angular.module('rpg')
.controller('writerBookController', [
    '$scope', '$routeParams', '$location', '$window', '$mdDialog', '$controller',
    'bookService', 'chapterService',
    function ($scope, $routeParams, $location, $window, $mdDialog, $controller,
        bookService, chapterService) {

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