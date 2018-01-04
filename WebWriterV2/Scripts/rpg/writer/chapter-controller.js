angular.module('rpg')

    .controller('chapterController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService',
        function ($scope, $routeParams, $location, $cookies, bookService,
            eventService, CKEditorService, userService, genreService) {

            $scope.chapter = null;
            $scope.wait = true;
            init();

            function loadChapter(chapterId) {
                bookService.get(chapterId).then(function (chapter) {
                    $scope.chapter = chapter;
                });
            }

            function init() {
                var chapterId = $routeParams.chapterId;
                loadChapter(chapterId);
            }
        }
    ]);