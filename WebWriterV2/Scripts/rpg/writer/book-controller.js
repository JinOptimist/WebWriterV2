angular.module('rpg')

    .controller('writerBookController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'chapterService',
        function ($scope, $routeParams, $location, $cookies, bookService, chapterService,
            eventService, CKEditorService, userService, genreService) {

            $scope.bookHasCycle = true;
            $scope.book = null;
            $scope.wait = true;
            init();

            $scope.addChapter = function () {
                var chapter = {
                    Name: 'Header',
                    Desc: 'Desc',
                    Level: 1,
                    BookId: $scope.book.Id,
                };
                chapterService.save(chapter);
            }

            function loadBook(bookId) {
                bookService.getWithChapters(bookId).then(function (book) {
                    $scope.book = book;
                    $scope.book.Levels.forEach(level => {
                        level.dynamicStyle = calcLevelWidthStyle;
                        level.Chapters.forEach(chapter => {
                            chapter.dynamicStyle = calcChapterStyle;
                            chapter.parent = level;
                        });
                    });
                });
            }

            function calcLevelWidthStyle() {
                var level = this;
                var style = {};
                var countChapter = level.Chapters.length;

                if (level.isWide) {
                    style.width = countChapter * (550 + 2) + 'px';
                } else {
                    style.width = 550 + (countChapter - 1) * 20 + 'px';
                }
                if (!level.isWide) {
                    style.height = 322 + (countChapter - 1) * (20 + 2) + 'px';
                }
                return style;
            }

            function calcChapterStyle() {
                var chapter = this;
                var level = chapter.parent;
                if (level.isWide) {
                    return {};
                }
                
                var index = level.Chapters.indexOf(chapter);
                return {
                    'position': 'relative',
                    'top': -1 * index * 300 + 'px',
                    'left': index * 20 + 'px'
                };
            }
            
            function init() {
                var bookId = $routeParams.bookId;
                loadBook(bookId);
            }
        }
    ]);