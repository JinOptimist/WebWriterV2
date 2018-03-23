
angular.module('rpg')
.controller('writerBookController', [
    '$scope', '$routeParams', '$location', '$window', 'bookService', 'chapterService',
    function ($scope, $routeParams, $location, $window, bookService, chapterService) {

        //var offsetWidth = 20;
        //var offsetHeight = 25;
        //var addButtonWidth = 70;
        //var addButtonHeight = 30;
        //var chapterMargin = 10;
        //$scope.chapterWidth = 260;
        //$scope.chapterHeight = 288;

        $scope.scale = 1.0;
        $scope.bookHasCycle = true;
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

        function addChapter(parentId) {
            chapterService.createChild(parentId).then(function (savedChapter) {
                moveToEditChapter(savedChapter.Id);
            });
        }

        function remove(chapterId) {
            if (confirm('Are you sure?')) {
                chapterService.remove(chapterId).then(function () {
                    init();
                });
            }
        }

        function moveToEditChapter(chapterId) {
            $location.path('/ar/writer/chapter/' + chapterId);
            $scope.$apply()
        }

        function loadChaptersV2(bookId) {
            $scope.wait = true;

            var actions = {
                moveToEditChapter: moveToEditChapter,
                addChapter: addChapter,
                remove: remove
            };

            bookService.getWithChaptersV2(bookId).then(function (book) {
                $scope.book = book;
                $scope.canvas.height = book.Chapters.length * 100
                setTimeout(function () { bookMap.start(book.Chapters, 1, actions, $scope.canvas) }, 0);
                $scope.wait = false;
            });
        }

        //$scope.onChapterHover = function (chapter) {
        //    if (chapter) {
        //        var highlightToChapterIds = [];
        //        chapter.LinksFromThisEvent.forEach(function (link) {
        //            highlightToChapterIds.push(link.ToId);
        //        });
        //        var highlightFromChapterIds = [];
        //        chapter.LinksToThisEvent.forEach(function (link) {
        //            highlightFromChapterIds.push(link.FromId);
        //        });
        //    }
        //    highlightChapter(highlightToChapterIds, highlightFromChapterIds);
        //}

        //function highlightChapter(nextChapterIds, prevChapterIds) {
        //    $scope.book.Levels.forEach(function (level) {
        //        level.Chapters.forEach(function (chapter) {
        //            chapter.isNext = nextChapterIds && nextChapterIds.indexOf(chapter.Id) > -1;
        //            chapter.isPrev = prevChapterIds && prevChapterIds.indexOf(chapter.Id) > -1;
        //        });
        //    });
        //}

        function init() {
            var bookId = $routeParams.bookId;

            loadChaptersV2(bookId);
        }
    }
]);