angular.module('rpg')

    .controller('chapterController', [
        '$scope', '$routeParams', '$location', '$cookies', 'chapterService',
        function ($scope, $routeParams, $location, $cookies, chapterService,
            eventService, CKEditorService, userService, genreService) {

            $scope.chapter = null;
            $scope.wait = true;
            init();

            $scope.save = function () {
                $scope.wait = true;
                chapterService.save($scope.chapter).then(function (chapter) {
                    $scope.wait = false;

                    if ($scope.chapterForm) {
                        $scope.chapterForm.$setPristine();
                        $scope.chapterForm.$setUntouched();
                    }
                });
            }

            function loadChapter(chapterId) {
                chapterService.get(chapterId).then(function (chapter) {
                    $scope.chapter = chapter;
                    $scope.wait = false;
                });
            }

            function init() {
                var chapterId = $routeParams.chapterId;
                loadChapter(chapterId);
            }
        }
    ]);