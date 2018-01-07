angular.module('rpg')

    .controller('travelController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'chapterService',
        function ($scope, $routeParams, $location, $cookies, bookService, chapterService,
            eventService, CKEditorService, userService, genreService) {

            $scope.chapter = null;
            $scope.wait = true;
            init();

            function loadChapter(chapterId) {
                chapterService.get(chapterId).then(function (chapter) {
                    $scope.chapter = chapter;
                });
            }

            function loadBook(bookId) {
                //bookService.getRoot(bookId).then(function (books) {
                //    $scope.books = books;
                //});
            }

            function init() {
                var chapterId = $routeParams.chapterId;
                if (chapterId) {
                    loadChapter(chapterId);
                } else {
                    var bookId = $routeParams.bookId;
                    loadBook(bookId);
                }
            }
        }
    ]);