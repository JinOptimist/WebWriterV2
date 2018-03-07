
angular.module('rpg')
.controller('graphController', [
    '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'chapterService',
    function ($scope, $routeParams, $location, $cookies, bookService, chapterService) {

        $scope.book = null;
        $scope.isRoadmap = false;
        
        init();

        function loadChapters(bookId) {
            chapterService.getAllChapters(bookId).then(function (chapters) {
                bookMap.start(chapters);
            });
        }

        function loadBookRoadmap(bookId) {
            bookService.getWithChaptersRoadmap(bookId).then(function (book) {
                $scope.book = book;
                bookMap.start(book.Levels);
            });
        }

        function init() {
            //var bookId = 30002;
            var bookId = 1;

            if ($scope.isRoadmap) {
                loadBookRoadmap(bookId);
            } else {
                loadChapters(bookId);
            }
        }
    }
]);