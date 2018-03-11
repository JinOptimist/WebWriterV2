
angular.module('rpg')
.controller('graphController', [
    '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'chapterService',
    function ($scope, $routeParams, $location, $cookies, bookService, chapterService) {

        $scope.book = null;
        $scope.isRoadmap = false;
        $scope.canvas = {
            width: 800,
            height: 500
        };
        
        init();

        $scope.update = function () {
            bookMap.redraw($scope.book.Chapters, $scope.scale);
        }

        function loadChaptersV2(bookId) {
            bookService.getWithChaptersV2(bookId).then(function (book) {
                $scope.book = book;
                bookMap.start(book.Chapters);
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
            var bookId = 10002;
            //var bookId = 1;

            if ($scope.isRoadmap) {
                loadBookRoadmap(bookId);
            } else {
                loadChaptersV2(bookId);
            }
        }
    }
]);