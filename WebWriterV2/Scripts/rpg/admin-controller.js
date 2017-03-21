angular.module('rpg')

    .controller('adminQuestGeneralController', [
        '$scope', '$http', '$routeParams', '$location', '$cookies', 'questService',
            'eventService', 'CKEditorService', 'userService',
        function ($scope, $http, $routeParams, $location, $cookies, questService,
            eventService, CKEditorService, userService) {

            $scope.quest = null;
            $scope.quests = [];
            $scope.wait = true;
            $scope.endingEvents = [];
            $scope.notAvailableEvents = null;
            init();

            $scope.addQuest = function() {
                $scope.quest = { Name: 'New quest Title' };
                $scope.endingEvents = [];
                $scope.notAvailableEvents = [];

                CKEditorService.reloadEditor('desc', 'New quest Desc');
            }

            $scope.selectQuest = function (quest) {
                var url = '/AngularRoute/admin/quest/' + quest.Id;
                $location.path(url);
            }

            $scope.saveQuest = function () {
                var isNew = !($scope.quest.Id > 0);
                var text = CKEditorService.getData('desc');
                $scope.quest.Desc = text;
                $scope.quest.OwnerId = $scope.user.Id;
                questService.saveQuest($scope.quest).then(
                    function (response) {
                        if (response) {
                            if (isNew) {
                                $scope.selectQuest(response);
                            } else {
                                alert('Save completed');
                            }
                        } else {
                            alert('Some go wrong');
                        }
                    },
                    function () {
                        alert('We all gonna die');
                    }
                );
            }

            $scope.exportQuest = function () {
                questService.get($scope.quest.Id).then(function (result) {
                    var text = angular.toJson(result);
                    var blob = new Blob([text]);
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = $scope.quest.Name + '.json';
                    link.click();
                });
            }

            $scope.importQuest = function () {
                questService.importQuest($scope.importJson).then(function (data) {
                    //alert('We did it!');
                    var url = '/AngularRoute/admin/quest/' + data;
                    $location.path(url);
                });
            }

            $scope.removeQuest = function (quest, index) {
                if (confirm('Are you sure? You try delete whole event: ' + quest.Name))
                    questService.removeQuest(quest.Id).then(function (result) {
                        $scope.quests.splice(index, 1);
                    });
            }

            $scope.goToEvent = function (quest) {
                var url = '/AngularRoute/admin/quest/' + quest.Id + '/event/';
                $location.path(url);
            }

            $scope.selectEvent = function (eventId) {
                $scope.wait = true;
                var url = '/AngularRoute/admin/quest/' + $scope.quest.Id + '/event/' + eventId;
                $location.path(url);
            }

            $scope.changeRootEvent = function() {
                questService.changeRootEvent($scope.quest.Id, $scope.newRootEvent.Id).then(function (data) {
                    $scope.quest.RootEvent = data;
                });
            }

            $scope.toggleShowEditPanel = function () {
                $scope.showEditPanel = !$scope.showEditPanel;
            }

            function loadQuest(questId) {
                questService.get(questId).then(function (result) {
                    $scope.quest = result;
                    $scope.wait = false;

                    CKEditorService.reloadEditor('desc', $scope.quest.Desc);

                    loadEndingEvents(questId);
                    reloadGraph();
                });
            }

            function loadQuests() {
                var userId = $scope.user.Id;
                if ($scope.user.IsAdmin) {
                    // if userId == null getQuests return all quests
                    userId = null;
                }
                questService.getQuests(userId).then(function (result) {
                    $scope.quests = result;
                });
            }

            function loadEndingEvents(questId) {
                eventService.getEndingEvents(questId).then(function (result) {
                    $scope.endingEvents = result;
                });
            }

            function loadNotAvailableEvents(questId) {
                eventService.getNotAvailableEvents(questId).then(function (result) {
                    $scope.notAvailableEvents = result;
                });
            }

            function reloadGraph() {
                var count = $scope.quest.AllEvents.length;
                EventGraph.drawGraph($scope.quest.AllEvents, 'eventsGraph', 900, 200 * count / 3);
            }

            function init() {
                var questId = $routeParams.questId;
                if (questId) {
                    loadQuest(questId);
                    loadEndingEvents(questId);
                    loadNotAvailableEvents(questId);
                }

                userService.getCurrentUser().then(function (data) {
                    $scope.user = data;
                    loadQuests();
                });
            }
        }
    ])
    .controller('adminEventGeneralController', [
        '$scope', '$http', '$routeParams', '$location', '$uibModal', 'eventService', 'questService',
        'requirementTypeService', 'stateService', 'thingService', 'CKEditorService',
        function ($scope, $http, $routeParams, $location, $uibModal, eventService, questService,
            requirementTypeService, stateService, thingService, CKEditorService) {

            $scope.event = null;
            $scope.quest = null;
            $scope.selectedEvent = null;
            $scope.selectedSkill = null;
            $scope.events = [];
            $scope.wait = true;

            $scope.RaceList = [];
            $scope.SexList = [];
            $scope.Skills = [];
            $scope.CharacteristicTypes = [];
            $scope.StateTypes = [];
            $scope.ThingSamples = [];
            $scope.RequirementTypes = [];

            $scope.parentExpand = true;
            $scope.reqExpand = false;
            $scope.stateExpand = false;
            $scope.eventEdit = true;
            $scope.childExpand = true;

            var questId = $routeParams.questId;
            var raceNoneObject = { name: 'None', value: 0 };
            var sexNoneObject = { name: 'None', value: 0 };

            init();

            /* Thing */
            $scope.addThingsChanges = function () {
                var thingChangesSampleId = $scope.selectedThingChangesSample.Id;
                var value = $scope.newThingChangesSampleValue;

                eventService.addThingChanges($scope.event.Id, thingChangesSampleId, value).then(function (data) {
                    if (!$scope.event.ThingsChanges) {
                        $scope.event.ThingsChanges = [];
                    }

                    $scope.event.ThingsChanges.push(data);
                    $scope.newThingChangesSampleValue = 0;
                });
            }

            $scope.removeThing = function (thingId, index) {
                eventService.removeThingChanges($scope.event.Id, thingId).then(function () {
                    $scope.event.ThingsChanges.splice(index, 1);
                });
            };

            $scope.availableThingSamples = function () {
                if (!$scope.event) {
                    return [];
                }
                if (!$scope.event.ThingsChanges) {
                    $scope.event.ThingsChanges = [];
                }
                return $scope.ThingSamples.filter(function (thingSample) {
                    return !$scope.event.ThingsChanges.some(function (thing) {
                        return thingSample.Id === thing.ThingSample.Id;
                    });
                });
            }

            $scope.addRequirementThing = function () {
                var requirementThingSampleId = $scope.selectedRequirementThingSample.Id;
                var value = $scope.newRequirementThingSampleValue;

                eventService.addRequirementThing($scope.event.Id, requirementThingSampleId, value).then(function (data) {
                    if (!$scope.event.RequirementThings) {
                        $scope.event.RequirementThings = [];
                    }

                    $scope.event.RequirementThings.push(data);
                    $scope.newRequirementThingSampleValue = 0;
                });
            }

            $scope.removeRequirementThing = function (requirementThingId, index) {
                eventService.removeRequirementThing($scope.event.Id, requirementThingId).then(function () {
                    $scope.event.RequirementThings.splice(index, 1);
                });
            };

            $scope.availableRequirementThingSamples = function () {
                if (!$scope.event) {
                    return [];
                }
                if (!$scope.event.RequirementThings) {
                    $scope.event.RequirementThings = [];
                }
                return $scope.ThingSamples.filter(function (thingSample) {
                    return !$scope.event.RequirementThings.some(function (requirementThing) {
                        return thingSample.Id === requirementThing.ThingSample.Id;
                    });
                });
            }

            /* State */
            $scope.addState = function() {
                var typeId = $scope.selectedState.Id;
                var value = $scope.newStateValue;

                eventService.addState($scope.event.Id, typeId, value).then(function (data) {
                    if (!$scope.event.HeroStatesChanging) {
                        $scope.event.HeroStatesChanging = [];
                    }

                    $scope.event.HeroStatesChanging.push(data);
                    $scope.newStateValue = 0;
                });
            }

            $scope.removeState = function (stateId, index) {
                eventService.removeState(stateId).then(function () {
                    $scope.event.HeroStatesChanging.splice(index, 1);
                });
            };

            $scope.availableStateTypes = function () {
                if (!$scope.event) {
                    return [];
                }
                if (!$scope.event.HeroStatesChanging) {
                    $scope.event.HeroStatesChanging = [];
                }
                return $scope.StateTypes.filter(function (stateType) {
                    return !$scope.event.HeroStatesChanging.some(function (state) {
                        return stateType.Id === state.StateType.Id;
                    });
                });
            }

            $scope.addReqState = function () {
                var typeId = $scope.newRequirementStatesType.Id;
                var value = $scope.newReqStatesValue;
                var reqType = $scope.newStateReqType.Value;

                eventService.addReqState($scope.event.Id, typeId, reqType, value).then(function (data) {
                    if (!$scope.event.RequirementStates) {
                        $scope.event.RequirementStates = [];
                    }

                    $scope.event.RequirementStates.push(data);
                    $scope.newReqStatesValue = 0;
                });
            }

            $scope.removeReqState = function (stateId, index) {
                eventService.removeReqState(stateId).then(function () {
                    $scope.event.RequirementStates.splice(index, 1);
                });
            };

            $scope.availableReqStateTypes = function () {
                if (!$scope.event) {
                    return [];
                }
                if (!$scope.event.RequirementStates) {
                    $scope.event.RequirementStates = [];
                }
                return $scope.StateTypes.filter(function (stateType) {
                    return !$scope.event.RequirementStates.some(function (state) {
                        return stateType.Id === state.StateType.Id;
                    });
                });
            }

            $scope.openStatePopup = function () {
                var model = {
                    templateUrl: 'views/rpg/admin/state.html',
                    controller: 'adminStateController',
                    windowClass: 'statesModal',
                    resolve: {
                        text: function () {
                            return 'Test';
                        }
                    }
                };
                $uibModal.open(model);
            }

            $scope.openThingPopup = function () {
                var model = {
                    templateUrl: 'views/rpg/admin/thing.html',
                    controller: 'adminThingController',
                    windowClass: 'thingModal',
                    resolve: {
                        text: function () {
                            return 'Test';
                        }
                    }
                };
                $uibModal.open(model);
            }

            /* Characteristic */
            $scope.addCharacteristic = function () {
                var typeId = $scope.newRequirementCharacteristicsType.Id;
                var value = $scope.newRequirementCharacteristicsValue;
                var requirementType = $scope.newRequirementType.Value;

                eventService.addCharacteristic($scope.event.Id, typeId, value, requirementType).then(function (data) {
                    if (!$scope.event.RequirementCharacteristics) {
                        $scope.event.RequirementCharacteristics = [];
                    }

                    $scope.event.RequirementCharacteristics.push(data);
                    $scope.newRequirementCharacteristicsValue = 0;
                });
            }

            $scope.removeCharacteristic = function (characteristicId, index) {
                eventService.removeCharacteristic(characteristicId).then(function() {
                    $scope.event.RequirementCharacteristics.splice(index, 1);
                });
            };

            $scope.availableCharacteristicTypes = function () {
                if (!$scope.event) {
                    return [];
                }
                if (!$scope.event.RequirementCharacteristics) {
                    $scope.event.RequirementCharacteristics = [];
                }
                return $scope.CharacteristicTypes.filter(function (charaType) {
                    return !$scope.event.RequirementCharacteristics.some(function (chara) {
                        return charaType.Id === chara.CharacteristicType.Id;
                    });
                });
            }

            /* Skill */
            $scope.addSkill = function() {
                eventService.addSkill($scope.event.Id, $scope.selectedSkill.Id).then(function () {
                    if (!$scope.event.RequirementSkill) {
                        $scope.event.RequirementSkill = [];
                    }

                    $scope.event.RequirementSkill.push($scope.selectedSkill);
                });
            }

            $scope.removeSkill = function (skill, index) {
                eventService.removeSkill($scope.event.Id, skill.Id).then(function () {
                    $scope.event.RequirementSkill.splice(index, 1);
                });
            }

            $scope.availableSkill = function () {
                if (!$scope.event) {
                    return [];
                }
                if (!$scope.event.RequirementSkill) {
                    $scope.event.RequirementSkill = [];
                }
                return $scope.Skills.filter(function(skillType) {
                    return  !$scope.event.RequirementSkill.some(function(skill) {
                        return skillType.Id === skill.Id;
                    });
                });
            }

            /* Event */
            $scope.addEvent = function () {
                $scope.event = {
                    Name: 'new',
                    ProgressChanging: 0,
                    Desc: '',
                    ChildrenEvents: []
                };

                CKEditorService.reloadEditor('desc');
                CKEditorService.setData('desc', '');
            }

            $scope.selectEvent = function (eventId) {
                $scope.wait = true;
                var url = '/AngularRoute/admin/quest/' + questId + '/event/' + eventId;
                $location.path(url);
            }

            $scope.saveEvent = function () {
                var isNew = !($scope.event.Id && $scope.event.Id > 0);
                var text = CKEditorService.getData('desc');
                $scope.event.Desc = text;

                eventService.save($scope.event, questId).then(
                    function (response) {
                        if (response) {
                            //$scope.eventForm.$setPristine();
                            //$scope.eventForm.eventName.$setPristine();
                            //$scope.eventForm.eventProgressChanging.$setPristine();
                            if (isNew) {
                                $scope.selectEvent(response.Id);
                            }
                        }
                        else {
                            alert('Some go wrong');
                        }
                    },
                    function () {
                        alert('We all gonna die');
                    }
                );
            }

            $scope.removeEvent = function (event, index) {
                if (confirm('Are you sure? You try delete whole event: ' + event.Name))
                    eventService.remove(event.Id).then(function (result) {
                        $scope.events.splice(index, 1);
                    });
            }

            /* Event Link */
            $scope.saveEventLink = function (eventLink) {
                eventLink.disable = true;
                eventService.saveEventLink(eventLink, questId).then(
                    function (response) {
                        if (response) {
                            eventLink.disable = false;
                        }
                        else {
                            alert('Some go wrong');
                        }
                    },
                    function () {
                        alert('We all gonna die');
                    }
                );
            }

            $scope.addEventLink = function() {
                var newEventLink = {
                    Id: 0,
                    Text: $scope.selectedEvent.Name,
                    FromId: $scope.event.Id,
                    ToId: $scope.selectedEvent.Id
                };

                $scope.event.LinksFromThisEvent.push(newEventLink);
            }

            $scope.removeEventLink = function (event, eventLink, index) {
                if (confirm('Are you sure? You try delete event link: ' + eventLink.Text))
                    eventService.removeEventLink(eventLink.Id).then(function (result) {
                        event.LinksFromThisEvent.splice(index, 1);
                    });
            }

            $scope.removeEventLinkParent = function (event, eventLink, index) {
                if (confirm('Are you sure? You try delete event link: ' + eventLink.Text))
                    eventService.removeEventLink(eventLink.Id).then(function (result) {
                        event.LinksToThisEvent.splice(index, 1);
                    });
            }

            $scope.addParentEventLink = function () {
                var newEventLink = {
                    Id: 0,
                    Text: $scope.selectedEvent.Name,
                    FromId: $scope.selectedEvent.Id,
                    ToId: $scope.event.Id
                };

                $scope.event.LinksToThisEvent.push(newEventLink);
            }

            $scope.goToQuest = function () {
                var url = '/AngularRoute/admin/quest/' + $scope.quest.Id;
                $location.path(url);
            }

            function loadEvent(eventId) {
                eventService.getEvent(eventId).then(function (result) {
                    $scope.event = result;
                    $scope.wait = false;

                    CKEditorService.reloadEditor('desc', $scope.event.Desc);

                    if (!$scope.event.RequirementRace) {
                        $scope.event.RequirementRace = {};
                    }
                    if ($scope.event.RequirementRace.Value === 0) {
                        $scope.event.RequirementRace.Value = raceNoneObject.value;
                    }

                    if (!$scope.event.RequirementSex) {
                        $scope.event.RequirementSex = {};
                    }
                    if ($scope.event.RequirementSex.Value === 0) {
                        $scope.event.RequirementSex.Value = sexNoneObject.value;
                    }
                });
            }

            function loadQuest(questId) {
                questService.get(questId).then(function (result) {
                    $scope.quest = result;
                    $scope.wait = false;
                });
            }

            function loadEvents() {
                eventService.getEvents($routeParams.questId).then(function (result) {
                    $scope.events = result;
                });
            }

            function init() {
                var eventId = $routeParams.eventId;
                if (eventId) {
                    loadEvent(eventId);
                }

                loadEvents();

                loadQuest(questId);

                stateService.loadTypesAvailbleForUser().then(function(data) {
                    $scope.StateTypes = data;
                    $scope.StateTypes.forEach(function (stateType) {
                        stateType.group = !!stateType.OwnerId ? 'My' : 'Base';
                    });
                    $scope.StateTypes.sort(function (a, b) {
                        return b.OwnerId - a.OwnerId;
                    });
                });

                thingService.loadAllSamples().then(function(data) {
                    $scope.ThingSamples = data;

                    $scope.ThingSamples = data;
                    $scope.ThingSamples.forEach(function (thingSample) {
                        thingSample.group = !!thingSample.OwnerId ? 'My' : 'Base';
                    });
                    $scope.ThingSamples.sort(function (a, b) {
                        return b.OwnerId - a.OwnerId;
                    });
                });

                requirementTypeService.load().then(function (data) {
                    $scope.RequirementTypes = data;
                });
            }
        }
    ])
    .controller('adminStateController', ['$scope', '$uibModalInstance', 'stateService', function($scope, $uibModalInstance, stateService) {
        $scope.states = [];
        $scope.newStateName = '';
        $scope.newStateDesc = '';

        init();

        $scope.close = function () {
            $uibModalInstance.close();
        }

        $scope.edit = function(state) {
            state.isEditing = true;
        }

        $scope.confirm = function (state) {
            state.isEditing = false;
            state.isSaved = true;

            stateService.edit(state)
                .then(function (data) {
                    if (data) {
                        state.isEditing = false;
                    }
                }).finally(function() {
                    state.isSaved = false;
                });
        }

        $scope.add = function() {
            stateService.add($scope.newStateName, $scope.newStateDesc)
                .then(function(data) {
                    $scope.states.push(data);

                    $scope.newStateName = '';
                    $scope.newStateDesc = '';
                });
        }

        $scope.remove = function (state, index) {
            if (confirm('Are you sure that you want delete "' + state.Name+ '" status?'))
                stateService.remove(state.Id)
                    .then(function() {
                        $scope.states.splice(index, 1);
                    });
        }

        function init() {
            stateService.loadTypesAvailbleForEdit().then(function(states) {
                $scope.states = states;
            });
        }
    }])
    .controller('adminThingController', ['$scope', '$uibModalInstance', 'thingService', function ($scope, $uibModalInstance, thingService) {
        $scope.thingsSample = [];
        $scope.newThingName = '';
        $scope.newThingDesc = '';

        init();

        $scope.add = function () {
            thingService.add($scope.newThingName, $scope.newThingDesc)
                .then(function (data) {
                    $scope.thingsSample.push(data);

                    $scope.newThingName = '';
                    $scope.newThingDesc = '';
                });
        }

        $scope.remove = function (stateId, index) {
            thingService.remove(stateId)
                .then(function () {
                    $scope.thingsSample.splice(index, 1);
                });
        }

        $scope.close = function () {
            $uibModalInstance.close();
        }

        function init() {
            thingService.loadAllSamples().then(function (data) {
                $scope.thingsSample = data;
            });
        }
    }
    ]);