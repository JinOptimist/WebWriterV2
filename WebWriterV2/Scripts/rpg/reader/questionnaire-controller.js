angular.module('rpg')

    .controller('questionnaireController', [
        '$scope', '$routeParams', '$location', '$cookies', 'questionnaireService', 'userService',
        function ($scope, $routeParams, $location, $cookies, questionnaireService, userService) {

            $scope.questionnaire = null;
            $scope.userId = null;
            $scope.resources = resources;
            $scope.answer = {};

            init();

            $scope.saveAnswers = function () {
                var answers = $scope.questionnaire.Questions.map(function (q) { return { Id: q.AnswerId }; });

                var questionnaireResult = {
                    QuestionnaireId: $scope.questionnaire.Id,
                    UserId: $scope.userId,
                    QuestionAnswers: answers
                };

                questionnaireService.saveQuestionnaireResult(questionnaireResult).then(function () {
                    $scope.answer = {
                        answers: answers,
                        result: 'good'
                    };
                });
            }

            function loadQuestionnaire(questionnaireId) {
                questionnaireService.getQuestionnaire(questionnaireId).then(function (questionnaire) {
                    $scope.questionnaire = questionnaire;
                });
            }

            function init() {
                var questionnaireId = $routeParams.questionnaireId;
                loadQuestionnaire(questionnaireId);

                $scope.userId = userService.getCurrentUserId();
            }
        }
    ]);