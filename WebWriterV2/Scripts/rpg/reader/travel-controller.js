angular.module('rpg')

    .controller('travelController', [
        '$scope', '$routeParams', '$location', '$cookies', '$window', 'bookService', 'chapterService',
        'eventService', 'CKEditorService', 'userService', 'genreService', 'travelService',
        function ($scope, $routeParams, $location, $cookies, $window, bookService, chapterService,
            eventService, CKEditorService, userService, genreService, travelService) {

            $scope.resources = resources;
            $scope.travel = null;
            $scope.wait = true;
            init();

            $scope.choice = function (linkItemId) {
                travelService.choice($scope.travel.Id, linkItemId).then(function (chapter) {
                    $scope.travel.Chapter = chapter;
                    $window.scrollTo(0, document.getElementById('travel'));
                });
            }

            $scope.travelIsEnd = function () {
                if ($scope.travel.Id > 0) {
                    travelService.travelIsEnd($scope.travel.Id).then(function () {
                        // go to profile or MainPage or BookEndPage
                    });
                } else {
                    // go to MainPage or BookEndPage
                }
                
            }

            function loadTravel(travelId, chapterId) {
                travelService.get(travelId, chapterId).then(function (travel) {
                    $scope.travel = travel;
                });
            }

            function init() {
                var travelId = $routeParams.travelId;
                var chapterId = $routeParams.chapterId;
                loadTravel(travelId, chapterId);

                //var chapterId = $routeParams.chapterId;
                //if (chapterId) {
                //    loadChapter(chapterId);
                //} else {
                //    var bookId = $routeParams.bookId;
                //    loadBook(bookId);
                //}
            }
        }
    ]);