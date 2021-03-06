angular.module('rpg')

    .controller('travelController', [
        '$scope', '$routeParams', '$location', '$window', 'travelService',
        function ($scope, $routeParams, $location, $window, travelService) {

            $scope.resources = resources;
            $scope.travel = null;
            $scope.stepInTheWay = false;
            init();

            $scope.choice = function (chapterLink) {
                if (($scope.travel.NextChapterId && $scope.travel.NextChapterId != chapterLink.ToId)
                    || $scope.stepInTheWay) {
                    return;
                }

                $scope.stepInTheWay = true;

                travelService.choice($scope.travel.Id, chapterLink.Id).then(function (travel) {
                    $window.scrollTo(0, document.getElementById('travel'));
                    var url = '/ar/reader/travel/' + travel.Id + '/' + travel.CurrentStepId;
                    $scope.stepInTheWay = false;
                    $location.path(url);
                });
            }

            function loadTravel(travelId, stepId) {
                travelService.get(travelId, stepId).then(function (travel) {
                    $scope.travel = travel;
                });
            }

            function init() {
                var travelId = $routeParams.travelId;
                var stepId = $routeParams.stepId;
                loadTravel(travelId, stepId);
            }
        }
    ]);