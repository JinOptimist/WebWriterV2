
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

        var width = $window.innerWidth - 20;
        var height = $window.innerHeight - 150;
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
            var actions = {
                moveToEditChapter: moveToEditChapter,
                addChapter: addChapter,
                remove: remove,
                createLink: createLink,
                removeLink: removeLink
            };

            bookService.getWithChaptersV2(bookId).then(function (book) {
                $scope.book = book;
                //var maxDepth = Math.max.apply(Math, book.Chapters.map(x => x.Level));
                //var height = maxDepth * (Const.ChapterSize.Height + Const.ChapterSize.Padding) + 100;
                //if (height < minHeight)
                //    height = minHeight;
                //$scope.canvas.height = height;
                
                setTimeout(function () { bookMap.start(book.Chapters, actions, $scope.canvas) }, 0);
            });
        }

        function init() {
            var bookId = $routeParams.bookId;

            loadChaptersV2(bookId);
        }
    }
]);