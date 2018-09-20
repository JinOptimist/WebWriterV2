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

            $scope.chapterTitleKeyPressed = function (e) {
                // 'enter'.keyEvent === 13
                if (e.which === 13) {
                    $scope.save(true);
                }
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

            $scope.updateChangeType = function (chapterLink) {
                //number = 1
                if (chapterLink.newChange.StateType.BasicType == 1) {
                    //copy array
                    chapterLink.ChangeTypes = $scope.chapter.ChangeTypes.slice();
                }
                //text = 2, boolean = 3,
                if (chapterLink.newChange.StateType.BasicType == 2
                    || chapterLink.newChange.StateType.BasicType == 3) {
                    chapterLink.ChangeTypes = $scope.chapter.ChangeTypes.filter(function (changeTypeEnum) {
                        //Remove = 4, Set = 5,
                        return changeTypeEnum.Value == 4 || changeTypeEnum.Value == 5;
                    });
                }
            }

            $scope.updateRequirementType = function (chapterLink) {
                //number = 1
                if (chapterLink.newRequirement.StateType.BasicType == 1) {
                    //copy array
                    chapterLink.RequirementTypes = $scope.chapter.RequirementTypes.slice();
                }
                //text = 2, boolean = 3,
                if (chapterLink.newRequirement.StateType.BasicType == 2
                    || chapterLink.newRequirement.StateType.BasicType == 3) {
                    chapterLink.RequirementTypes = $scope.chapter.RequirementTypes.filter(function (requirementTypeEnum) {
                        //Exist = 5, NotExist = 6, Equals = 7, NotEquals = 8
                        return requirementTypeEnum.Value == 5
                            || requirementTypeEnum.Value == 6
                            || requirementTypeEnum.Value == 7
                            || requirementTypeEnum.Value == 8;
                    });
                }
            }

            $scope.saveNewRequirement = function (chapterLink) {
                chapterLink.newRequirement;
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

                $scope.chapter.RequirementTypes;
                $scope.chapter.ChangeTypes;
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