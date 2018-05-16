angular.module('rpg')

    .controller('chapterController', [
        '$scope', '$routeParams', '$location', '$cookies', '$q', 'chapterService', 'userService', '$mdDialog', 'chapterId',
        function ($scope, $routeParams, $location, $cookies, $q, chapterService, userService, $mdDialog, chapterId) {

            $scope.chapterLinks = [];
            $scope.availableDecision = [];
            $scope.chapter = null;
            $scope.wait = true;
            $scope.resources = resources;
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
                $mdDialog.hide($scope.chapter);
            }

            $scope.createNextChapter = function () {
                chapterService.createNextChapter($scope.chapter).then(function (newChapter) {
                    $location.path('/ar/writer/chapter/' + newChapter.Id);
                });
            }

            $scope.updateChpaterLink = function (chapterLink) {
                saveChapterLinkAndResetForm(chapterLink, $scope.chapterLinkBlockForm);
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
                $scope.wait = false;
            }

            function init() {
                var chapterPromise = chapterService.get(chapterId);
                var linksPromise = chapterService.getLinksFromChapter(chapterId);
                var availableDecisionPromise = chapterService.getAvailableDecision(chapterId);
                
                $q.all([chapterPromise, linksPromise, availableDecisionPromise]).then(function (data) {
                    $scope.chapterLinks = data[1];
                    $scope.availableDecision = data[2];

                    var chapter = data[0];
                    loadChapter(chapter);
                });
            }
        }
    ]);