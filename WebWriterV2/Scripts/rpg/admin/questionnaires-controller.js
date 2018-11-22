angular.module('rpg')

    .controller('adminQuestionnairesController', [
        '$scope', '$routeParams', '$location', '$cookies', 'questionnaireService', 'userService',
        function ($scope, $routeParams, $location, $cookies, questionnaireService, userService) {

            $scope.questionnaires = null;
            $scope.user = null;
            $scope.resources = resources;

            init();

            $scope.addQuestionnaire = function () {
                if (!$scope.questionnaires) {
                    $scope.questionnaires = [];
                }

                $scope.questionnaires.push({
                    Name: resources.Admin_Questionnaire_DefaultName,
                    isEdit: true,
                    Questions: []
                });
            }
            $scope.saveQuestionnaire = function (questionnaire, index) {
                questionnaireService.saveQuestionnaire(questionnaire).then(function (savedQuestionnaire) {
                    $scope.questionnaires[index] = savedQuestionnaire;
                    savedQuestionnaire.isEdit = false;
                    fillAnswerFromPrevQuestion();
                });
            }
            $scope.removeQuestionnaire = function (questionnaire, index) {
                if (!confirm(resources.Admin_Questionnaire_ConfirmRemovingQuestionnaire.format(questionnaire.Name))) {
                    return false;
                }
                questionnaireService.removeQuestionnaire(questionnaire.Id).then(function () {
                    $scope.questionnaires.splice(index, 1);
                });
            }
            $scope.typedQuestionnaire = function ($event, questionnaire, index) {
                // 'enter'.keyEvent === 13
                if ($event.which !== 13) {
                    return false;
                }

                $scope.saveQuestionnaire(questionnaire, index);
            }

            $scope.addQuestion = function (questionnaire) {
                questionnaire.Questions.push({
                    Text: resources.Admin_Questionnaire_Question_DefaultName,
                    Order: questionnaire.Questions.length + 1,
                    QuestionnaireId: questionnaire.Id,
                    VisibleIf: [],
                    QuestionAnswers: [],
                    isEdit: true,
                });
            }
            $scope.saveQuestion = function (questionnaire, question, index) {
                questionnaireService.saveQuestion(question).then(function (savedQuestion) {
                    questionnaire.Questions[index] = savedQuestion;
                    savedQuestion.isEdit = false;
                    fillAnswerFromPrevQuestion();
                });
            }
            $scope.removeQuestion = function (questionnaire, question, index) {
                if (!confirm(resources.Admin_Questionnaire_ConfirmRemovingQuestion.format(question.Order))) {
                    return false;
                }
                questionnaireService.removeQuestion(question.Id).then(function () {
                    questionnaire.Questions.splice(index, 1);
                    fillAnswerFromPrevQuestion();
                });
            }
            $scope.typedQuestion = function ($event, questionnaire, question, index) {
                // 'enter'.keyEvent === 13
                if ($event.which !== 13) {
                    return false;
                }

                $scope.saveQuestion(questionnaire, question, index);
            }

            $scope.addQuestionAnswers = function (question) {
                question.QuestionAnswers.push({
                    Text: resources.Admin_Questionnaire_QuestionAnswer_DefaultName,
                    Order: question.QuestionAnswers.length + 1,
                    QuestionId: question.Id,
                    isEdit: true
                });
            }
            $scope.saveQuestionAnswer = function (questionAnswer, question, index) {
                questionnaireService.saveQuestionAnswer(questionAnswer).then(function (savedQuestionAnswer) {
                    question.QuestionAnswers[index] = savedQuestionAnswer;
                    savedQuestionAnswer.isEdit = false;
                    fillAnswerFromPrevQuestion();
                });
            }
            $scope.removeQuestionAnswer = function (question, questionAnswer, index) {
                if (!confirm(resources.Admin_Questionnaire_ConfirmRemovingQuestionAnswer.format(questionAnswer.Order, question.Order))) {
                    return false;
                }
                questionnaireService.removeQuestionAnswer(questionAnswer.Id).then(function () {
                    question.QuestionAnswers.splice(index, 1);
                    fillAnswerFromPrevQuestion();
                });
            }
            $scope.typedQuestionAnswer = function ($event, questionAnswer, question, index) {
                // 'enter'.keyEvent === 13
                if ($event.which !== 13) {
                    return false;
                }

                $scope.saveQuestionAnswer(questionAnswer, question, index);
            }

            function loadQuestionnaires() {
                questionnaireService.getAllQuestionnaire().then(function (questionnaires) {
                    $scope.questionnaires = questionnaires;
                    fillAnswerFromPrevQuestion();
                });
            }

            function fillAnswerFromPrevQuestion() {
                $scope.questionnaires.forEach(function (questionnaire) {
                    var answersFromPrevQuestion = [{
                        Id: -1,
                        QuestionId: -1,
                        Text: "---"
                    }];

                    questionnaire.Questions.forEach(function(question){
                        var cloneArray = answersFromPrevQuestion.slice(0);
                        question.answerFromPrevQuestion = cloneArray;
                        question.QuestionAnswers.forEach(function (qa) {
                            answersFromPrevQuestion.push(qa);
                        })
                    });
                });
            }

            function init() {
                loadQuestionnaires();
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