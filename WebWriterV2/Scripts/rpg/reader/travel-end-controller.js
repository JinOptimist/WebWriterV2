angular.module('rpg')

    .controller('travelEndController', [
        '$scope', '$routeParams', '$location', '$cookies', '$window', 'bookService', 'chapterService', 'travelService',
        function ($scope, $routeParams, $location, $cookies, $window, bookService, chapterService, travelService) {

            $scope.resources = resources;
            $scope.travelIsEnded = null;
            $scope.wait = true;
            init();

            function loadTravelEnd(travelId) {
                travelService.travelIsEnded(travelId).then(function (travel) {
                    $scope.travelIsEnded = travel;
                });
            }

            function init() {
                var travelId = $routeParams.travelId;
                loadTravelEnd(travelId);
            }
        }
    ]);