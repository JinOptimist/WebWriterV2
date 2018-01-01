angular.module('rpg')

    .controller('bookController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService',
        function ($scope, $routeParams, $location, $cookies, bookService,
            eventService, CKEditorService, userService, genreService) {

            $scope.bookHasCycle = true;
            $scope.book = null;
            $scope.wait = true;
            init();

            function loadBook(bookId) {
                var defaultBook = {
                    name: 'hit and slash',
                    levels: [],
                };

                var level = fakeGenerateLevel(1);
                level.chapters.push(fakeGenerateChapter('1 1', level));
                level.chapters.push(fakeGenerateChapter('1 2', level));
                defaultBook.levels.push(level);
                level = fakeGenerateLevel(2);
                level.chapters.push(fakeGenerateChapter('2 1', level));
                defaultBook.levels.push(level);
                level = fakeGenerateLevel(3);
                level.chapters.push(fakeGenerateChapter('3 1', level));
                level.chapters.push(fakeGenerateChapter('3 2', level));
                level.chapters.push(fakeGenerateChapter('3 3', level));
                defaultBook.levels.push(level);

                return defaultBook;
            }

            function fakeGenerateChapter(desc, level) {
                return {
                    name: 'ch Name',
                    desc: desc,
                    parent: level,
                    dynamicStyle: calcChapterStyle,
                };
            }

            function fakeGenerateLevel(levelNumber) {
                return {
                    chapters: [],
                    dynamicStyle: calcLevelWidthStyle,
                };
            }

            function calcLevelWidthStyle() {
                var level = this;
                var style = {};
                var countChapter = level.chapters.length;

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
                
                var index = level.chapters.indexOf(chapter);
                return {
                    'position': 'relative',
                    'top': -1 * index * 300 + 'px',
                    'left': index * 20 + 'px'
                };
            }
            
            function init() {
                var bookId = $routeParams.bookId;
                $scope.book = loadBook(bookId);
            }
        }
    ]);