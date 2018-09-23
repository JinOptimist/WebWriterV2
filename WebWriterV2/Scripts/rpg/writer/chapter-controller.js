angular.module('rpg')

    .controller('chapterController', [
        '$scope', '$routeParams', '$location', '$cookies', '$q', 'chapterService', 'stateService',
        'userService', '$mdDialog', 'chapterId',
        function ($scope, $routeParams, $location, $cookies, $q, chapterService, stateService,
            userService, $mdDialog, chapterId) {

            $scope.chapterLinks = [];
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

                chapterLink.newRequirement.ChapterLinkId = chapterLink.Id;
            }

            $scope.saveNewRequirement = function (chapterLink) {
                stateService.addStateRequirement(chapterLink.newRequirement).then(function (requirement) {
                    chapterLink.Requirements.push(requirement);
                    chapterLink.newRequirement = {};
                });
            }

            $scope.removeRequirement = function (chapterLink, requirementId, index) {
                stateService.removeStateRequirement(requirementId).then(function () {
                    chapterLink.Requirements.splice(index, 1);
                });
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

                chapterLink.newChange.ChapterLinkId = chapterLink.Id;
            }

            $scope.saveNewChange = function (chapterLink) {
                if (!validateNewChange()) {
                    return false;
                }

                stateService.addStateChange(chapterLink.newChange).then(function (savedChange) {
                    chapterLink.Changes.push(savedChange);
                    chapterLink.newChange = {};
                });
            }

            $scope.removeChange = function (chapterLink, changeId, index) {
                stateService.removeStateChange(changeId).then(function () {
                    chapterLink.Changes.splice(index, 1);
                });
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
                
                $q.all([chapterPromise, linksPromise]).then(function (data) {
                    $scope.chapterLinks = data[1];
                    
                    var chapter = data[0];
                    loadChapter(chapter);
                });
            }
        }
    ]);