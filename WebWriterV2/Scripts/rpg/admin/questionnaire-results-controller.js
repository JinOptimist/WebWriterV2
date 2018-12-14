angular.module('rpg')

    .controller('adminQuestionnaireResultsController', [
        '$scope', '$routeParams', '$location', '$cookies', 'questionnaireService',
        function ($scope, $routeParams, $location, $cookies, questionnaireService) {

            $scope.questionnaireResults = null;
            $scope.resources = resources;

            init();

            $scope.removeQuestionnaire = function (questionnaire, index) {
                if (!confirm(resources.Admin_Questionnaire_ConfirmRemovingQuestionnaire.format(questionnaire.Name))) {
                    return false;
                }
                questionnaireService.removeQuestionnaire(questionnaire.Id).then(function () {
                    $scope.questionnaireResults.splice(index, 1);
                });
            }

            function loadQuestionnaireResult() {
                questionnaireService.getAllQuestionnaireResults().then(function (questionnaireResults) {
                    $scope.questionnaireResults = questionnaireResults;
                    fillAnswerFromPrevQuestion();
                });
            }

            function init() {
                loadQuestionnaireResult();
            }
        }
    ]);