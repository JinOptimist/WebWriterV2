
angular.module('rpg')
.controller('writerBookController', [
    '$scope', '$routeParams', '$location', '$window', '$cookies', '$mdDialog', '$q', 'ConstCookies',
        'bookService', 'chapterService', 'stateService', 'userService',
    function ($scope, $routeParams, $location, $window, $cookies, $mdDialog, $q, ConstCookies,
        bookService, chapterService, stateService, userService) {

        var step = 0.1;
        var scale = 1.0;
        var minScale = 0.1;
        var bookMapObj = newOneBookMap();
        $scope.book = null;
        $scope.resources = resources;
        $scope.newStateType = {};
        $scope.user = {};
        $scope.newCoAuthor = {};

        $scope.canvas = {
            width: $window.innerWidth,
            height: $window.innerHeight
        };

        init();

        $scope.rightClick = function (e) {
            bookMapObj.rightClick();
        }

        $scope.onResize = function (direction) {
            scale += step * direction;
            if (scale < minScale)
                scale = minScale;
            bookMapObj.resize(scale);
        }

        $scope.createNewStateType = function () {
            $scope.newStateType.BookId = $scope.book.Id;
            $scope.newStateType.OwnerId = $cookies.get(ConstCookies.userId);

            stateService.addStateType($scope.newStateType).then(function (newState) {
                if (!$scope.book.States)
                    $scope.book.States = [];
                $scope.book.States.push(newState);
                $scope.newStateType = {};
            });
        }

        $scope.removeStateType = function (stateType, index) {
            if (!confirm(resources.Writer_StateTypeRemoveConfirm.format(stateType.Name))) {
                return false;
            }
            stateService.removeStateType(stateType.Id).then(function () {
                $scope.book.States.splice(index, 1);
            });
        }

        $scope.addCoAuthor = function () {
            bookService.addCoAuthor($scope.newCoAuthor.email, $scope.book.Id).then(function (isSaved) {
                if (isSaved) {
                    $scope.book.CoAuthors.push($scope.newCoAuthor.email);
                    $scope.newCoAuthor.email = '';
                } else {
                    $scope.newCoAuthor.notFound = true;
                }
            });
        }

        $scope.removeCoAuthor = function (email, index) {
            if (!confirm(resources.Writer_CoAuthorRemoveConfirm.format(email))) {
                return false;
            }

            bookService.removeCoAuthor(email, $scope.book.Id).then(function (isSaved) {
                $scope.book.CoAuthors.splice(index, 1);
            });
        }

        function moveToEditChapter(chapterId, updateDrawnChapter) {
            var dialogModel = {
                controller: 'chapterController',
                templateUrl: '/views/writer/Chapter.html',
                parent: angular.element(document.body),
                //targetEvent: ev,
                clickOutsideToClose: false,
                fullscreen: $scope.customFullscreen, // Only for -xs, -sm breakpoints.
                locals: { chapterId: chapterId },
            };

            $mdDialog.show(dialogModel)
                .then(function (chapter) {
                    if (chapter) {
                        updateDrawnChapter(chapter);
                    }
                }, function (problem) {
                    $scope.status = 'You cancelled the dialog.';
                });
        };

        function addChapter(parentId, updateDrawnChapter) {
            chapterService.createChild(parentId).then(function (savedChapter) {
                moveToEditChapter(savedChapter.Id, updateDrawnChapter);
            });
        }

        function remove(chapter) {
            if (confirm(resources.RemoveChapterConfirmation.format(chapter.Name))) {
                chapterService.remove(chapter.Id).then(function (result) {
                    if (result) {
                        //init();
                    } else {
                        alert(resources.RemoveChapterImpossibleAlert.format(chapter.Name));
                        init();
                    }
                });
                return true;
            }
            return false;
        }

        function createLink(fromId, toId, onSuccessedLinkCreate) {
            chapterService.createLink(fromId, toId).then(function (link) {
                onSuccessedLinkCreate(link);
            });
        }

        function removeLink(linkId) {
            chapterService.removeLink(linkId).then(function (result) {
                if (result) {
                    //init();
                } else {
                    alert(resources.RemoveLinkImpossibleAlert);
                    init();
                }
            });
        }

        function validate(chapters) {
            $scope.valid = true;

            chapters.forEach(x => $scope.valid = $scope.valid && chapterProcessing.validateChapter(x));

            setTimeout(function () { $scope.$apply(); }, 1);
        }

        function getActions() {
            return {
                moveToEditChapter: moveToEditChapter,
                addChapter: addChapter,
                remove: remove,
                createLink: createLink,
                removeLink: removeLink,
                validate: validate
            };
        }

        function documentKeyPressed(e) {
            // 'esc'.which == 27
            if (e.which === 27) {
                bookMapObj.rightClick();
            }
        }

        function init() {
            
            var userId = $cookies.get(ConstCookies.userId);
            var userPromise = userService.getById(userId);

            var bookId = $routeParams.bookId;
            var actions = getActions();
            var bookPromise = bookService.getWithChaptersV2(bookId);

            $q.all([bookPromise, userPromise]).then(function (data) {
                $scope.user = data[1];

                $scope.canvas.height -= ($scope.user.ShowExtendedFunctionality ? 210 : 120);

                $scope.book = data[0];
                bookMapObj.start(data[0], actions, $scope.canvas, $scope.user);
            });

            document.onkeydown = function (e) {
                $scope.$apply(documentKeyPressed(e));
            };
        }
    }
]);