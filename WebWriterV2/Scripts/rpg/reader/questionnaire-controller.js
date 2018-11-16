angular.module('rpg')

    .controller('questionnaireController', [
        '$scope', '$routeParams', '$location', '$cookies', 'questionnaireService', 'userService',
        function ($scope, $routeParams, $location, $cookies, questionnaireService, userService) {

            $scope.questionnaire = null;
            $scope.userId = null;
            $scope.resources = resources;
            $scope.answer = {};
            $scope.questionnaireAreDone = false;

            init();

            $scope.saveAnswers = function () {

                var answers = [];
                $scope.questionnaire.Questions.forEach(function (q) {
                    if (q.AllowMultipleAnswers) {
                        q.Answers.forEach(function (a) { answers.push({ Id: a }) });
                    } else {
                        answers.push({ Id: q.AnswerId });
                    }
                });

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
                    $scope.questionnaireAreDone = true;
                });
            }

            $scope.toggleSelection = function (question, answerId) {
                if (!question.Answers) {
                    question.Answers = [];
                }

                var idx = question.Answers.indexOf(answerId);
                if (idx > -1) {
                    question.Answers.splice(idx, 1);
                } else {
                    question.Answers.push(answerId);
                }
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