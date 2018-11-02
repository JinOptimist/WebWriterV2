angular.module('rpg')

    .controller('articlesController', [
        '$scope', '$routeParams', '$location', '$cookies', 'articleService', 'userService',
        function ($scope, $routeParams, $location, $cookies, articleService, userService) {

            $scope.articles = null;
            $scope.user = null;
            $scope.resources = resources;

            init();

            $scope.remove = function (article, index) {
                if (!confirm(resources.Reader_Articles_ConfirmRemovingArticle.format(article.Name))) {
                    return false;
                }
                articleService.remove(article.Id)
                    .then(function () {
                        $scope.articles.splice(index, 1);
                    });
            }

            function loadArticles() {
                articleService.getAll().then(function (articles) {
                    $scope.articles = articles;
                });
            }

            $scope.toggleEdit = function (article, show) {
                article.isEdit = show;
                if (!article.isExpand)
                    article.isExpand = true;
            }

            $scope.add = function () {
                if (!$scope.articles) {
                    $scope.articles = [];
                }

                $scope.articles.push({
                    Name: resources.Reader_Article_DefaultName,
                    ShortDesc: resources.Reader_Article_DefaultShortDesc,
                    Desc: resources.Reader_Article_DefaultDesc,
                    isEdit: true,
                });
            }

            $scope.togglePublish = function (article) {
                article.IsPublished = !article.IsPublished;
                articleService.save(article).then(function (saveArticle) {
                    
                });
            }

            $scope.save = function (article, index) {
                articleService.save(article).then(function (savedArticle) {
                    $scope.articles[index] = savedArticle;
                });
            }

            function init() {
                loadArticles();

                var userId = userService.getCurrentUserId();
                if (userId) {
                    userService.getById(userId).then(function (data) {
                        $scope.user = data;
                    });
                } else {
                    $scope.user = null;
                }
            }
        }
    ]);