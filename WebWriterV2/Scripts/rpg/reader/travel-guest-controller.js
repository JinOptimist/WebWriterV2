angular.module('rpg')

    .controller('travelGuestController', [
        '$scope', '$routeParams', '$location', '$cookies', '$window', 'bookService', 'chapterService',
        function ($scope, $routeParams, $location, $cookies, $window, bookService, chapterService) {

            $scope.resources = resources;
            $scope.chapter = null;
            init();

            $scope.nextChapter = function (chapterLink) {
                $window.scrollTo(0, document.getElementById('travel'));
                var url = '/ar/reader/travel-guest/' + chapterLink.ToId;
                $location.path(url);
            }

            function loadChapter(chapterId) {
                chapterService.getForTravel(chapterId).then(function (chapter) {
                    $scope.chapter = chapter;
                });
            }

            function init() {
                var chapterId = $routeParams.chapterId;
                loadChapter(chapterId);
            }
        }
    ]);