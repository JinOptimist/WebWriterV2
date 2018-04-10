angular.module('rpg')

    .controller('chapterController', [
        '$scope', '$routeParams', '$location', '$cookies', '$q', 'chapterService', 'userService', '$mdDialog', 'chapterId',
        function ($scope, $routeParams, $location, $cookies, $q, chapterService, userService, $mdDialog, chapterId) {

            $scope.chaptersBottom = [];
            $scope.chapterLinks = [];
            $scope.availableDecision = [];
            $scope.chapter = null;
            $scope.newChapterLink = { FromId: -1 };
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
                        //$location.path('/ar/writer/book/' + chapter.BookId);
                        $mdDialog.hide(chapter);
                    }
                });
            }

            $scope.close = function () {
                $mdDialog.hide();
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

            $scope.deleteLink = function (chapterLinkId, index) {
                chapterService.removeChapterLink(chapterLinkId).then(function () {
                    $scope.chapterLinks.splice(index, 1);
                });
            }

            $scope.saveDecision = function (chapterLink) {
                chapterService.linkDecisionToChapterLink(chapterLink.Id, chapterLink.Decision).then(function () {
                    init();
                });
                //TODO Direct update decision
            }

            $scope.saveCondition = function (chapterLink) {
                chapterService.linkConditionToChapterLink(chapterLink.Id, chapterLink.Condition).then(function () {
                    init();
                });
                //TODO Direct update decision
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
                });
            }

            function loadChapter(chapter) {
                $scope.chapter = chapter;
                $scope.newChapterLink.FromId = chapter.Id;
                $scope.wait = false;
            }

            function loadBottomChapters(chapter) {
                chapterService.getBottomChapters(chapter).then(function (chaptersBottom) {
                    $scope.chaptersBottom = chaptersBottom;
                });
            }

            function loadAllChapters(bookId) {
                chapterService.getAllChapters(bookId).then(function (chapters) {
                    $scope.chaptersBottom = chapters;
                });
            }

            function init() {
                // get chapter id from scope if we open popup
                //var chapterId = $scope.chapterId
                //    ? $scope.chapterId
                //    : $routeParams.chapterId;

                var userPromise = userService.getCurrentUser();
                var chapterPromise = chapterService.get(chapterId);
                var linksPromise = chapterService.getLinksFromChapter(chapterId);
                var availableDecisionPromise = chapterService.getAvailableDecision(chapterId);
                
                $q.all([userPromise, chapterPromise, linksPromise, availableDecisionPromise]).then(function (data) {
                    $scope.user = data[0];
                    $scope.chapterLinks = data[2];
                    $scope.availableDecision = data[3];

                    var chapter = data[1];
                    loadChapter(chapter);
                    if ($scope.user.IsAdmin) {
                        //use experemental functionality
                        loadAllChapters(chapter.BookId);
                    } else {
                        loadBottomChapters(chapter);
                    }
                });
            }
        }
    ]);