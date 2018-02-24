angular.module('rpg')

    .controller('writerBookController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'chapterService',
        function ($scope, $routeParams, $location, $cookies, bookService, chapterService,
            eventService, CKEditorService, userService, genreService) {

            var offsetWidth = 20;
            var offsetHeight = 25;

            var addButtonWidth = 70;
            var addButtonHeight = 30;

            var chapterMargin = 10;

            $scope.chapterWidth = 260;
            $scope.chapterHeight = 288;

            $scope.scale = 1.0;
            $scope.bookHasCycle = true;
            $scope.book = null;
            $scope.wait = true;
            $scope.isRoadmap = false;

            init();

            $scope.onChapterHover = function (chapter) {
                if (chapter) {
                    var highlightToChapterIds = [];
                    chapter.LinksFromThisEvent.forEach(function (link) {
                        highlightToChapterIds.push(link.ToId);
                    });

                    var highlightFromChapterIds = [];
                    chapter.LinksToThisEvent.forEach(function (link) {
                        highlightFromChapterIds.push(link.FromId);
                    });
                }

                highlightChapter(highlightToChapterIds, highlightFromChapterIds);
            }

            $scope.addChapter = function (level) {
                var chapter = {
                    Name: 'Chapter: ' + level,
                    Desc: 'Desc',
                    Level: level,
                    BookId: $scope.book.Id,
                    IsRootChapter: $scope.book.RootChapterId < 0
                };
                chapterService.save(chapter).then(function (savedChapter) {
                    $location.path('/ar/writer/chapter/' + savedChapter.Id);
                });
            }

            $scope.remove = function (chapter, index) {
                chapterService.remove(chapter.Id)
                    .then(function () {
                        chapter.parent.Chapters.splice(index, 1);
                        init();
                    });
            }

            $scope.toggleWide = function(level){
                level.isWide = !level.isWide;
                recalcLevelStyle(level);
                level.Chapters.forEach(function (chapter) {
                    recalcChapterStyle(chapter);
                });
                
            }

            $scope.updateStyleForAllLevel = function () {
                $scope.book.Levels.forEach(function (level) {
                    recalcLevelStyle(level);
                    level.Chapters.forEach(function (chapter) {
                        recalcChapterStyle(chapter);
                    });
                })
            }

            $scope.toggleRoadmap = function () {
                $scope.isRoadmap = !$scope.isRoadmap;
                init();
            }

            function loadBook(bookId) {
                bookService.getWithChapters(bookId).then(function (book) {
                    $scope.book = book;
                    $scope.book.Levels.forEach(level => {
                        recalcLevelStyle(level)
                        level.Chapters.forEach(chapter => {
                            chapter.parent = level;
                            recalcChapterStyle(chapter);
                        });
                    });
                });
            }

            function loadBookRoadmap(bookId) {
                bookService.getWithChaptersRoadmap(bookId).then(function (book) {
                    $scope.book = book;
                });
            }

            function recalcLevelStyle(level) {
                var style = {};
                var countChapter = level.Chapters.length;

                if (level.isWide) {
                    style.width = countChapter * ($scope.chapterWidth * $scope.scale + chapterMargin) + addButtonWidth + 'px';
                } else {
                    style.width = ($scope.chapterWidth * $scope.scale) + (countChapter * offsetWidth) + addButtonWidth + 'px';
                }
                if (!level.isWide) {
                    style.height = ($scope.chapterHeight * $scope.scale) + (countChapter * offsetHeight) + chapterMargin + 'px';
                }

                style.dynamicStyleForAddButton = {};
                style.dynamicStyleForAddButton.marginTop = Math.round($scope.chapterHeight * $scope.scale / 2) + 'px';

                level.dynamicStyle = style;
            }

            function recalcChapterStyle(chapter) {
                var level = chapter.parent;
                var style = {};
                style.width = $scope.chapterWidth * $scope.scale + 'px';
                style.height = $scope.chapterHeight * $scope.scale + 'px';
                if (level.isWide) {
                    chapter.dynamicStyle = style;
                    return false;
                }

                var index = level.Chapters.indexOf(chapter);
                style.position = 'relative';
                style.top = -1 * index * ($scope.chapterHeight * $scope.scale - offsetHeight) + 'px';
                style.left = index * offsetWidth * $scope.scale + 'px';
                
                chapter.dynamicStyle = style;
            }

            function highlightChapter(nextChapterIds, prevChapterIds) {
                $scope.book.Levels.forEach(function (level) {
                    level.Chapters.forEach(function (chapter) {
                        chapter.isNext = nextChapterIds && nextChapterIds.indexOf(chapter.Id) > -1;
                        chapter.isPrev = prevChapterIds && prevChapterIds.indexOf(chapter.Id) > -1;
                    });
                });
            }

            function init() {
                var bookId = $routeParams.bookId;

                if ($scope.isRoadmap) {
                    loadBookRoadmap(bookId);
                } else {
                    loadBook(bookId);
                }
            }
        }
    ]);