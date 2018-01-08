angular.module('rpg')

    .controller('chapterController', [
        '$scope', '$routeParams', '$location', '$cookies', 'chapterService',
        function ($scope, $routeParams, $location, $cookies, chapterService,
            eventService, CKEditorService, userService, genreService) {

            $scope.chaptersBottom = [];

            $scope.chapterLinks = [];

            $scope.chapter = null;
            $scope.newChapterLink = { FromId: -1};
            $scope.wait = true;
            init();

            $scope.save = function (saveAndClose) {
                $scope.wait = true;
                chapterService.save($scope.chapter).then(function (chapter) {
                    $scope.wait = false;

                    if ($scope.chapterForm) {
                        $scope.chapterForm.$setPristine();
                        $scope.chapterForm.$setUntouched();
                    }

                    if (saveAndClose) {
                        $location.path('/ar/writer/book/' + chapter.BookId);
                    }
                });
            }

            $scope.saveChapterLink = function () {
                chapterService.saveChapterLink($scope.newChapterLink).then(function (chapterLink) {
                    if ($scope.chapterForm) {
                        $scope.chapterLinkForm.$setPristine();
                        $scope.chapterLinkForm.$setUntouched();
                    }

                    //if (saveAndClose) {
                    //    $location.path('/ar/writer/book/' + chapter.BookId);
                    //}
                });
            }

            function loadChapter(chapterId) {
                chapterService.get(chapterId).then(function (chapter) {
                    $scope.chapter = chapter;
                    $scope.newChapterLink.FromId = chapter.Id;
                    $scope.wait = false;
                    loadBottomChapters(chapter);
                });
            }

            function loadBottomChapters(chapter) {
                chapterService.getBottomChapters(chapter).then(function (chaptersBottom) {
                    $scope.chaptersBottom = chaptersBottom;
                });
            }

            function loadChapterLinks(chapterId) {
                chapterService.getLinksFromChapter(chapterId).then(function (chapterLinks) {
                    $scope.chapterLinks = chapterLinks;
                });
            }

            function init() {
                var chapterId = $routeParams.chapterId;
                loadChapter(chapterId);
                loadChapterLinks(chapterId);
            }
        }
    ]);