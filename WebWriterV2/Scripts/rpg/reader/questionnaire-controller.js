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
                var answers = getAnswers();

                if ($scope.questionnaire.Questions.some(a => a.withoutAnswer)) {
                    return;
                }

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
                var idx = question.Answers.indexOf(answerId);
                if (idx > -1) {
                    question.Answers.splice(idx, 1);
                } else {
                    question.Answers.push(answerId);
                }
            }

            $scope.requiredAnswersWasChecked = function (question) {
                var answers = getAnswers();
                return requiredAnswersWasChecked(question, answers);
            }

            function requiredAnswersWasChecked(question, answers) {
                var visibleIf = question.VisibleIf[0];
                return answers.some(a => a.Id == visibleIf);
            }

            function getAnswers() {
                var answers = [];
                $scope.questionnaire.Questions.forEach(function (question) {
                    // Check do we can see the question. If we can't, skip the question
                    if (question.VisibleIf && question.VisibleIf.length > 0 && !requiredAnswersWasChecked(question, answers)){
                        return;
                    }

                    var answersCount = answers.length;
                    if (question.AllowMultipleAnswers) {
                        question.Answers.forEach(function (a) { answers.push({ Id: a }) });
                    } else if (question.AnswerId) {
                        answers.push({ Id: question.AnswerId });
                    }

                    // if count of answer doesn't change, question doesn't have answer.
                    question.withoutAnswer = answersCount == answers.length;
                });
                return answers;
            }

            function loadQuestionnaire(questionnaireId) {
                questionnaireService.getQuestionnaire(questionnaireId).then(function (questionnaire) {
                    $scope.questionnaire = questionnaire;
                    $scope.questionnaire.Questions.forEach(function (q) {
                        q.Answers = [];
                    })
                });
            }

            function init() {
                var questionnaireId = $routeParams.questionnaireId;
                loadQuestionnaire(questionnaireId);

                $scope.userId = userService.getCurrentUserId();
            }
        }
    ]);