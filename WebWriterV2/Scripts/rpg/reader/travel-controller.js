angular.module('rpg')

    .controller('travelController', [
        '$scope', '$routeParams', '$location', '$cookies', '$window', 'bookService', 'chapterService', 'travelService',
        function ($scope, $routeParams, $location, $cookies, $window, bookService, chapterService, travelService) {

            $scope.resources = resources;
            $scope.travel = null;
            $scope.wait = true;
            init();

            $scope.choice = function (chapterLink) {
                if ($scope.travel.NextChapterId && $scope.travel.NextChapterId != chapterLink.ToId) {
                    return;
                }

                travelService.choice($scope.travel.Id, chapterLink.Id).then(function (travel) {
                    $window.scrollTo(0, document.getElementById('travel'));
                    var url = '/ar/reader/travel/' + travel.Id + '/' + travel.Chapter.Id;
                    $location.path(url);

                });
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
            }
        }
    ]);