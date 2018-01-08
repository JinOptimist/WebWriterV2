angular.module('rpg')

    .controller('travelController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'chapterService',
        'eventService', 'CKEditorService', 'userService', 'genreService',
        function ($scope, $routeParams, $location, $cookies, bookService, chapterService,
            eventService, CKEditorService, userService, genreService) {

            $scope.chapter = null;
            $scope.chapterLinks = [];
            $scope.wait = true;
            init();

            function loadChapter(chapterId) {
                chapterService.get(chapterId).then(function (chapter) {
                    $scope.chapter = chapter;
                });

                loadChapterLinks(chapterId);
            }

            function loadBook(bookId) {
                //bookService.getRoot(bookId).then(function (books) {
                //    $scope.books = books;
                //});
            }

            function loadChapterLinks(chapterId) {
                chapterService.getLinksFromChapter(chapterId).then(function (chapterLinks) {
                    $scope.chapterLinks = chapterLinks;
                });
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