angular.module('rpg')

    .controller('adminQuestionnaireResultsController', [
        '$scope', '$routeParams', '$location', '$cookies', 'questionnaireService',
        function ($scope, $routeParams, $location, $cookies, questionnaireService) {

            $scope.questionnaireResults = null;
            $scope.resources = resources;

            init();

            $scope.removeQuestionnaireResults = function (questionnaireResult, index) {
                if (!confirm("You try delete questionnaire result. Are you sure?")) {
                    return false;
                }

                questionnaireService.removeQuestionnaireResults(questionnaireResult.Id).then(function () {
                    $scope.questionnaireResults.splice(index, 1);
                });
            }

            $scope.sendResult = function (questionnaireResult, email) {
                questionnaireService.sendQuestionnaireResultsToEmail(questionnaireResult.Id, email).then(function () {
                    alert('jobs done');
                });
            }

            function loadQuestionnaireResult() {
                questionnaireService.getAllQuestionnaireResults().then(function (questionnaireResults) {
                    $scope.questionnaireResults = questionnaireResults;
                });
            }

            function init() {
                loadQuestionnaireResult();
            }
        }
    ]);