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

            $scope.createNextChapter = function () {
                chapterService.createNextChapter($scope.chapter).then(function (newChapter) {
                    $location.path('/ar/writer/chapter/' + newChapter.Id);
                });
            }

            $scope.saveChapterLink = function () {
                saveChapterLinkAndResetForm($scope.newChapterLink, $scope.chapterLinkForm);
            }

            $scope.updateChpaterLink = function (chapterLink) {
                saveChapterLinkAndResetForm(chapterLink, $scope.chapterLinkBlockForm);
            }

            function saveChapterLinkAndResetForm(chapterLink, form) {
                chapterService.saveChapterLink(chapterLink).then(function (savedChapterLink) {
                    if (chapterLink.Id > 0) {
                        //ignore. Lazy revert statment for undefind
                    } else {
                        $scope.chapterLinks.push(savedChapterLink);
                    }
                    
                    if (form) {
                        form.$setPristine();
                        form.$setUntouched();
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