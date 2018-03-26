
angular.module('rpg')
.controller('writerBookController', [
    '$scope', '$routeParams', '$location', '$window', 'bookService', 'chapterService',
    function ($scope, $routeParams, $location, $window, bookService, chapterService) {

        $scope.scale = 1.0;
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

        function createLink(fromId, toId) {
            chapterService.createLink(fromId, toId).then(function () {
                init();
            });
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
                remove: remove,
                createLink: createLink
            };

            bookService.getWithChaptersV2(bookId).then(function (book) {
                $scope.book = book;
                var maxDepth = Math.max.apply(Math, book.Chapters.map(x => x.Level));
                $scope.canvas.height = maxDepth * 70 + 100
                setTimeout(function () { bookMap.start(book.Chapters, 1, actions, $scope.canvas) }, 0);
                $scope.wait = false;
            });
        }

        function init() {
            var bookId = $routeParams.bookId;

            loadChaptersV2(bookId);
        }
    }
]);