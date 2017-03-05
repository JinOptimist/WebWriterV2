
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('services', ['ngRoute', 'ngCookies', 'underscore', 'AppConst'])
    .constant('_',
        window._
    )
    .run(['$http', 'sexService', 'raceService', 'heroService', function ($http, sexService, raceService, heroService) {
        //$http({
        //    method: 'POST',
        //    url: '/Rpg/GetDefaultHero',
        //    headers: { 'Accept': 'application/json' }
        //}).success(function (rawHero) {
        //    var hero = angular.fromJson(rawHero);
        //    heroService.setDefaultHero(hero);
        //});
    }])
    .service('questService', ['httpHelper', function (httpHelper) {
        return {
            saveQuest: saveQuest,
            getQuests: getQuests,
            get: get,
            changeRootEvent: changeRootEvent,
            removeQuest: removeQuest,
            importQuest: importQuest,
            questCompleted: questCompleted
        };

        function questCompleted(questId) {
            var url = '/Rpg/QuestCompleted';
            var data = {
                questId: questId
            };
            return httpHelper.call(url, data);
        }

        function saveQuest(quest) {
            var url = '/Rpg/SaveQuest';
            var data = {
                jsonQuest: angular.toJson(quest)
            };
            return httpHelper.call(url, data);
        }

        function changeRootEvent(questId, eventId) {
            var url = '/Rpg/ChangeRootEvent';
            var data = {
                questId: questId,
                eventId: eventId
            };
            return httpHelper.call(url, data);
        }

        function get(questId) {
            var url = '/Rpg/GetQuest?id=' + questId;
            return httpHelper.call(url);
        }

        function getQuests(userId) {
            var url = '/Rpg/GetQuests';
            var data = {
                userId: userId
            };
            return httpHelper.call(url, data);
        }

        function removeQuest(questId) {
            var url = '/Rpg/RemoveQuest?id=' + questId;
            return httpHelper.call(url);
        }

        function importQuest(questJson) {
            var url = '/Rpg/ImportQuest';
            var data = {
                jsonQuest: questJson
            };
            return httpHelper.call(url, data);
        }
    }])
    .service('eventService', ['httpHelper', function (httpHelper) {
        return {
            getEvent: getEvent,
            getEvents: getEvents,
            save: save,
            remove: remove,
            saveEventLink: saveEventLink,
            removeEventLink: removeEventLink,
            addSkill: addSkill,
            removeSkill: removeSkill,
            addCharacteristic: addCharacteristic,
            removeCharacteristic: removeCharacteristic,
            addState: addState,
            removeState: removeState,
            addReqState: addReqState,
            removeReqState: removeReqState,
            getEndingEvents: getEndingEvents,
            getNotAvailableEvents: getNotAvailableEvents,
            addRequirementThing: addRequirementThing,
            removeRequirementThing: removeRequirementThing,
            addThingChanges: addThingChanges,
            removeThingChanges: removeThingChanges,
            getEventForTravel: getEventForTravel,
            eventChangesApplyToHero: eventChangesApplyToHero,
            getEventForTravelWithHero: getEventForTravelWithHero
        };

        function getEventForTravel(eventId, heroId) {
            var url = '/Rpg/GetEventForTravel';
            var data = {
                eventId: eventId,
                heroId: heroId
            };
            return httpHelper.call(url,data);
        }

        function getEventForTravelWithHero(eventId, hero, applyChanges) {
            var url = '/Rpg/GetEventForTravelWithHero';
            var data = {
                eventId: eventId,
                heroJson: angular.toJson(hero),
                applyChanges: applyChanges
            };
            return httpHelper.call(url,data);
        }

        function eventChangesApplyToHero(eventId, heroId) {
            var url = '/Rpg/EventChangesApplyToHero';
            var data = {
                eventId: eventId,
                heroId: heroId
            };
            return httpHelper.call(url, data);
        }

        function getEvent(currentEventId) {
            var url = '/Rpg/GetEvent?id=' + currentEventId;
            return httpHelper.call(url);
        }

        function getEvents(questId) {
            var url = '/Rpg/GetEvents?questId=' + questId;
            return httpHelper.call(url);
        }

        function getEndingEvents(questId) {
            var url = '/Rpg/GetEndingEvents?questId=' + questId;
            return httpHelper.call(url);
        }

        function getNotAvailableEvents(questId) {
            var url = '/Rpg/GetNotAvailableEvents?questId=' + questId;
            return httpHelper.call(url);
        }

        function save(event, questId) {
            var url = '/Rpg/SaveEvent';
            var data = {
                jsonEvent: angular.toJson(event),
                questId: questId
            };
            return httpHelper.call(url,data);
        }

        function remove(eventId) {
            var url = '/Rpg/RemoveEvent';
            var data = {
                eventId: eventId
            };
            return httpHelper.call(url, data);
        }

        function saveEventLink(eventLink) {
            var url = '/Rpg/SaveEventLink';
            var data = {
                jsonEventLink: angular.toJson(eventLink)
            };
            return httpHelper.call(url, data);
        }

        function removeEventLink(eventLinkId) {
            var url = '/Rpg/RemoveEventLink';
            var data = {
                eventLinkId: eventLinkId
            };
            return httpHelper.call(url, data);
        }

        function addSkill(eventId, skillId) {
            var url = '/Rpg/AddSkillToEvent';
            var data = {
                eventId: eventId,
                skillId: skillId
            };
            return httpHelper.call(url, data);
        }

        function removeSkill(eventId, skillId) {
            var url = '/Rpg/RemoveSkillToEvent';
            var data = {
                eventId: eventId,
                skillId: skillId
            };
            return httpHelper.call(url, data);
        }

        function addCharacteristic(eventId, characteristicTypeId, characteristicValue, requirementType) {
            var url = '/Rpg/AddCharacteristicToEvent';
            var data = {
                eventId: eventId,
                characteristicTypeId: characteristicTypeId,
                characteristicValue: characteristicValue,
                requirementType: requirementType
            };
            return httpHelper.call(url, data);
        }

        function removeCharacteristic(characteristicId) {
            var url = '/Rpg/RemoveCharacteristicFromEvent';
            var data = {
                characteristicId: characteristicId
            };
            return httpHelper.call(url, data);
        }

        function addState(eventId, stateTypeId, stateValue) {
            var url = '/Rpg/AddStateToEvent';
            var data = {
                eventId: eventId,
                stateTypeId: stateTypeId,
                stateValue: stateValue
            };
            return httpHelper.call(url, data);
        }

        function removeState(stateId) {
            var url = '/Rpg/RemoveStateFromEvent';
            var data = {
                stateId: stateId
            };
            return httpHelper.call(url, data);
        }

        function addReqState(eventId, stateTypeId, reqType, stateValue) {
            var url = '/Rpg/AddReqStateToEvent';
            var data = {
                eventId: eventId,
                stateTypeId: stateTypeId,
                reqType: reqType,
                stateValue: stateValue
            };
            return httpHelper.call(url, data);
        }

        function removeReqState(stateId) {
            var url = '/Rpg/RemoveReqStateFromEvent';
            var data = {
                stateId: stateId
            };
            return httpHelper.call(url, data);
        }

        function addRequirementThing(eventId, thingSampleId, count) {
            var url = '/Rpg/AddRequirementThingToEvent';
            var data = {
                eventId: eventId,
                thingSampleId: thingSampleId,
                count: count
            };
            return httpHelper.call(url, data);
        }

        function removeRequirementThing(eventId, thingId) {
            var url = '/Rpg/RemoveRequirementThingFromEvent';
            var data = {
                eventId: eventId,
                thingId: thingId
            };
            return httpHelper.call(url, data);
        }

        function addThingChanges(eventId, thingSampleId, count) {
            var url = '/Rpg/AddThingChangesToEvent';
            var data = {
                eventId: eventId,
                thingSampleId: thingSampleId,
                count: count
            };
            return httpHelper.call(url, data);
        }

        function removeThingChanges(eventId, thingId) {
            var url = '/Rpg/RemoveThingChangesFromEvent';
            var data = {
                eventId: eventId,
                thingId: thingId
            };
            return httpHelper.call(url, data);
        }
    }])
    .service('heroService', ['$http', '$q', function ($http, $q) {
        var maxHpStateName = "MaxHp";
        var maxMpStateName = "MaxMp";
        var hpStateName = "Hp";
        var mpStateName = "Mp";

        var listHeroes = null;
        var selectedHero = null;
        var defaultHero = {};

        function addSkill(heroId, skillId) {
            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: '/Rpg/AddSkillToHero',
                data: {
                    heroId: heroId,
                    skillId: skillId
                },
                headers: { 'Accept': 'application/json' }
            })
                .success(function (response) {
                    var hero = angular.fromJson(response);
                    deferred.resolve(hero);
                });
            return deferred.promise;
        }

        function restoreHero(heroId, skillId) {
            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: '/Rpg/RestoreHero',
                data: {
                    heroId: heroId
                },
                headers: { 'Accept': 'application/json' }
            })
                .success(function (response) {
                    var hero = angular.fromJson(response);
                    deferred.resolve(hero);
                });
            return deferred.promise;
        }

        function getState(hero, stateName) {
            if (!hero || !hero.State)
                return -1;
            var currentState = _.find(hero.State, function (state) {
                return state.StateType.Name == stateName;
            });
            return currentState.Number;
        }

        function getHp(hero) {
            return getState(hero, hpStateName);
        }

        function updateHeroState(heroOrigin, heroNew) {
            for (var i = 0; i < heroNew.State.length; i++) {
                var newStat = heroNew.State[i];
                var st = heroOrigin.State.find(function (stat) {
                    return stat.StateType.Id == newStat.StateType.Id;
                });
                st.Number = newStat.Number;
            }
        }

        return {
            load: function(heroId) {
                var deferred = $q.defer();
                $http({
                        method: 'POST',
                        url: '/Rpg/GetHero',
                        data: {
                            heroId: heroId
                        },
                        headers: { 'Accept': 'application/json' }
                    })
                    .success(function (response) {
                        var hero= angular.fromJson(response);
                        deferred.resolve(hero);
                    });
                return deferred.promise;
            },
            loadListHeroes: function () {
                var deferred = $q.defer();

                if (listHeroes) {
                    deferred.resolve(listHeroes);
                    return deferred.promise;
                }

                $http({
                    method: 'POST',
                    url: '/Rpg/GetHeroes',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        listHeroes = angular.fromJson(response);
                        deferred.resolve(listHeroes);
                    });
                return deferred.promise;
            },
            removeHero: function (hero) {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveHero?id=' + hero.Id,
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        deferred.resolve(response);
                    });
                return deferred.promise;
            },
            addHero: function (newHero) {
                newHero.Sex = newHero.Sex - 0;
                newHero.Race = newHero.Race - 0;

                listHeroes.push(newHero);
            },
            getAllHeroes: function () {
                return listHeroes;
            },
            saveHero: function (newHero) {
                var deferred = $q.defer();
                $http({
                    method: 'POST',
                    url: '/Rpg/SaveHero',
                    data: { jsonHero: angular.toJson(newHero) },
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        deferred.resolve(response);
                    });

                return deferred.promise;
            },
            selectHero: function (hero) {
                selectedHero = hero;
            },
            getSelectedHero: function () {
                return selectedHero;
            },
            loadEnemy: function () {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/GetEnemy',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var enemy = angular.fromJson(response);
                        deferred.resolve(enemy);
                    });
                return deferred.promise;
            },
            getDefaultHero: function () {
                return defaultHero;
            },
            setDefaultHero: function (hero) {
                defaultHero = hero;
            },
            loadDefaultHero: function () {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/GetDefaultHero',
                    headers: { 'Accept': 'application/json' }
                }).success(function (rawHero) {
                    var hero = angular.fromJson(rawHero);
                    defaultHero = hero;
                    deferred.resolve(defaultHero);
                });
                return deferred.promise;
            },
            addSkill: addSkill,
            restoreHero: restoreHero,
            getHp: getHp,
            updateHeroState: updateHeroState
        };
    }])
    .service('raceService', ['$http', '$q', '_', function ($http, $q, _) {
        var raceList;

        function updateRaceList(newList) {
            _.each(newList, function (item) {
                switch (item.Value) {
                    case 1:
                        item.src = "/Content/rpg/human.jpg";
                        break;
                    case 2:
                        item.src = "/Content/rpg/elf.jpg";
                        break;
                    case 3:
                        item.src = "/Content/rpg/orc.jpg";
                        break;
                    case 4:
                        item.src = "/Content/rpg/gnom.jpg";
                        break;
                    case 5:
                        item.src = "/Content/rpg/dragon.jpg";
                        break;
                }
            });
            return newList;
        }

        return {
            loadRaceList: function () {
                var deferred = $q.defer();
                if (raceList) {
                    deferred.resolve(raceList);
                    return deferred.promise;
                }

                $http({
                    method: 'POST',
                    url: '/Rpg/GetListRace',
                    headers: { 'Accept': 'application/json' }
                })
                .success(function (response) {
                    raceList = angular.fromJson(response);
                    deferred.resolve(raceList);
                });
                return deferred.promise;
            },
            getRaceList: function () {
                var copyRace = raceList.map(function (value) {
                    return angular.copy(value);
                });
                return copyRace;//clone array
            },
            getRaceWord: function (raceValue) {
                var filtered = raceList.filter(function (race) { return race.value == raceValue });
                if (filtered.length === 1)
                    return filtered[0].name;
                return "?";
            },
            setList: function (newRaceList) {
                raceList = updateRaceList(newRaceList);
            },
            addImageToList: updateRaceList
        };
    }])
    .service('requirementTypeService', ['$http', '$q', '_', function ($http, $q, _) {
        return {
            load: function () {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/GetListRequirementType',
                    headers: { 'Accept': 'application/json' }
                })
                .success(function (response) {
                    var requirementTypes = angular.fromJson(response);
                    deferred.resolve(requirementTypes);
                });
                return deferred.promise;
            },
        };
    }])
    .service('sexService', ['$http', '$q', '_', function ($http, $q, _) {
        var sexList;

        function updateSexList(newList) {
            _.each(newList, function (item) {
                switch (item.Value) {
                    case 1:
                        item.src = "/Content/rpg/man.jpg";
                        break;
                    case 2:
                        item.src = "/Content/rpg/woman.jpg";
                        break;
                    case 3:
                        item.src = "/Content/rpg/unknown.jpg";
                        break;
                }
            });
            return newList;
        }

        return {
            loadSexList: function () {
                var deferred = $q.defer();
                if (sexList) {
                    deferred.resolve(sexList);
                    return deferred.promise;
                }

                $http({
                    method: 'POST',
                    url: '/Rpg/GetListSex',
                    headers: { 'Accept': 'application/json' }
                })
                .success(function (response) {
                    sexList = angular.fromJson(response);
                    deferred.resolve(sexList);
                });
                return deferred.promise;
            },
            getSexList: function () {
                var copySex = sexList.map(function (value) {
                    return angular.copy(value);
                });
                return copySex; // clone array
            },
            getSexWord: function (sexValue) {
                var filtered = sexList.filter(function (sex) { return sex.value == sexValue });
                if (filtered.length === 1)
                    return filtered[0].name;
                return "?";
            },
            setList: function (newSexList) {
                sexList = updateSexList(newSexList);
            },
            addImageToList: updateSexList
        };
    }])
    .service('guildService', ['$http', '$q', '_', function ($http, $q, _) {
        var currentGuild;
        var guildPromise = function(guildId) {
            return $q(function(resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/GetGuild',
                    data: {
                        guildId: guildId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function(response) {
                        currentGuild = angular.fromJson(response);
                        resolve(currentGuild);
                    },
                    function() {
                        reject(Error("Sorry :( we have fail"));
                    });
            });
        };

        return {
            loadGuild: guildPromise
        };
    }])
    .service('traningRoomService', ['$http', '$q', '_', function ($http, $q, _) {
        function getTraningRoom(roomId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/GetTraningRoom?traningRoomId=' + roomId,
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    var event = angular.fromJson(response);
                    resolve(event);
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        return {
            load: getTraningRoom
        };
    }])
    .service('skillService', ['$http', '$q', '_', function ($http, $q, _) {
        var allSkills = [];

        function getState(hero, stateName) {
            return _.find(hero.State, function (state) { return state.StateType.Name == stateName; });
        }

        return {
            loadSkillEffect: function (skillId) {
                return $q(function (resolve, reject) {
                    $http({
                        method: 'POST',
                        url: '/Rpg/GetSkillEffect?skillId=' + skillId,
                        headers: { 'Accept': 'application/json' }
                    }).success(function (response) {
                        resolve(angular.fromJson(response));
                    },
                    function () {
                        reject(Error("Sorry :( we have fail"));
                    });
                });
            },
            isEnough: function (hero, skill) {
                // return null if ok, and stat name id you have lack
                for (var i = 0; i < skill.SelfChanging.length; i++) {
                    var selfChange = skill.SelfChanging[i];
                    var stat = getState(hero, selfChange.StateType.Name);//_.where(hero.State, { Value: selfChange.Value });
                    if (stat)
                        if (stat.Number + selfChange.Number < 1) {
                            return "Not enough " + stat.StateType.Name;
                        }
                }

                return null;
            },
            loadAll: function () {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/GetSkills',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        allSkills = angular.fromJson(response);
                        deferred.resolve(allSkills);
                    });
                return deferred.promise;
            },
            loadSkillsSchool: function () {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/GetSkillsSchool',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var skillSchools = angular.fromJson(response);
                        deferred.resolve(skillSchools);
                    });
                return deferred.promise;
            },
            saveSkill: function (skill) {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/SaveSkill',
                    data: { jsonSkill: angular.toJson(skill) },
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var skill = angular.fromJson(response);
                        deferred.resolve(skill);
                    });
                return deferred.promise;
            },
            removeSkill: function (skill) {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveSkill',
                    data: { skillId: skill.Id },
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var skill = angular.fromJson(response);
                        deferred.resolve(skill);
                    });
                return deferred.promise;
            }
        };
    }])
    .service('stateService', ['_', 'httpHelper', function (_, httpHelper) {
        return {
            loadTypesAvailbleForUser: loadTypesAvailbleForUser,
            loadTypesAvailbleForEdit: loadTypesAvailbleForEdit,
            add: add,
            edit: edit,
            remove: remove,
            changeState: changeState
        };

        function loadTypesAvailbleForUser() {
            var url = '/Rpg/GetStateTypesAvailbleForUser';
            return httpHelper.call(url);
        }

        function loadTypesAvailbleForEdit() {
            var url = '/Rpg/GetStateTypesAvailbleForEdit';
            return httpHelper.call(url);
        }

        function changeState(stateId, delta) {
            var url = '/Rpg/ChangeState';
            var data = {
                stateId: stateId,
                delta: delta
            };
            return httpHelper.call(url, data);
        }

        function add(name, desc) {
            var url = '/Rpg/AddState';
            var data = {
                name: name,
                desc: desc
            };
            return httpHelper.call(url, data);
        }

        function edit(state) {
            var url = '/Rpg/EditStateType';
            var data = {
                jsonStateType: angular.toJson(state)
            };
            return httpHelper.call(url, data);
        }

        function remove(stateId) {
            var url = '/Rpg/RemoveState';
            var data = {
               stateId: stateId,
            };
            return httpHelper.call(url, data);
        }
    }])
    .service('characteristicService', ['$http', '$q', '_', function ($http, $q, _) {
        var characteristicTypes = [];
        return {
            loadAllCharacteristicType: function () {
                var deferred = $q.defer();

                $http({
                        method: 'POST',
                        url: '/Rpg/GetCharacteristicTypes',
                        headers: { 'Accept': 'application/json' }
                    })
                    .success(function(response) {
                        characteristicTypes = angular.fromJson(response);
                        deferred.resolve(characteristicTypes);
                    });
                return deferred.promise;
            },
            removeCharacteristicType: function (characteristicType) {
                var deferred = $q.defer();

                $http({
                        method: 'POST',
                        url: '/Rpg/RemoveCharacteristicType',
                        data: { characteristicTypeId: characteristicType.Id },
                        headers: { 'Accept': 'application/json' }
                    })
                    .success(function(response) {
                        deferred.resolve(response);
                    });
                return deferred.promise;
            },
            save: function(characteristicType) {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/SaveCharacteristicType',
                    data: { jsonCharacteristicType: angular.toJson(characteristicType) },
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var savedCharacteristicType = angular.fromJson(response);
                        deferred.resolve(savedCharacteristicType);
                    });
                return deferred.promise;
            }
        };
    }])
    .service('thingService', ['httpHelper', '_', function (httpHelper, _) {
        return {
            loadAllSamples: loadAllSamples,
            add: add,
            remove: remove,
        };

        function loadAllSamples() {
            var url = '/Rpg/GetThingSamples';
            return httpHelper.call(url);
        }

        function add(name, desc) {
            var url = '/Rpg/AddThing';
            var data= {
                name: name,
                desc: desc
            };
            return httpHelper.call(url,data);
        }

        function remove(thingId) {
            var url = '/Rpg/RemoveThing';
            var data = {
                thingId: thingId
            };
            return httpHelper.call(url, data);
        }
    }])
    .service('userService', ['httpHelper', function (httpHelper) {
        return {
            login: login,
            register: register,
            getById: getById,
            addBookmark: addBookmark,
            removeAccount: removeAccount,
            becomeWriter: becomeWriter
        };

        function login(user) {
            var url = '/Rpg/Login';
            var data = {
                username: user.Name,
                password: user.Password
            };
            return httpHelper.call(url, data);
        }

        function register(user) {
            var url = '/Rpg/Register';
            var userJson = angular.toJson(user);
            var data = {
                userJson: userJson
            };
            return httpHelper.call(url, data);
        }

        function getById(userId) {
            var url = '/Rpg/GetUserById';
            var data = {
                userId: userId
            };
            return httpHelper.call(url, data);
        }

        function addBookmark(eventId, heroJson) {
            var url = '/Rpg/AddBookmark';
            var data = {
                eventId: eventId,
                heroJson: heroJson
            };
            return httpHelper.call(url, data);
        }

        function removeAccount(userId) {
            var url = '/Rpg/RemoveUser';
            var data = {
                userId: userId
            };
            return httpHelper.call(url, data);
        }

        function becomeWriter() {
            var url = '/Rpg/BecomeWriter';
            return httpHelper.call(url);
        }
    }])
    //CKEditor
    .service('CKEditorService', ['$http', '$q', '_', function ($http, $q, _) {
        function reloadEditor(editorName, newData) {
            var editor = CKEDITOR.instances[editorName];
            if (editor) {
                editor.destroy(true);
            }
            editor = CKEDITOR.replace(editorName, {width: 900, height: 600});
            editor.setData(newData);
        }

        function setData(editorName, data) {
            var editor = CKEDITOR.instances[editorName];
            editor.setData(data);
        }

        function getData(editorName) {
            var editor = CKEDITOR.instances[editorName];
            return editor.getData();
        }

        return {
            reloadEditor: reloadEditor,
            setData: setData,
            getData: getData
        };
    }])
    // Login
    .factory('authInterceptorService', ['$q', '$location', '$cookies', '_',
        function ($q, $location, $cookies, _) {
            function request(request) {
                // TODO
                //goToLogin();
                $cookies.put('guildId', 2);
                return request;
            }

            function requestError(rejection) {
                // do something on error
                return $q.reject(rejection);
            }

            function response(response) {
                // do something on success
                //var gamerId = $cookies.get('guildId');
                return response;
            }

            function responseError(rejection) {
                // do something on error
                return $q.reject(rejection);
            }

            return {
                request: request,
                requestError: requestError,
                response: response,
                responseError: responseError
            };
        }])
    .service('httpHelper', ['$q', '$http', function ($q, $http) {
        return { call: call };

        function call(url, data, success, error) {
            if (!success) {
                success = defaultSuccess;
            }
            if (!error) {
                error = defaultError;
            }

            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: url,
                data: data,
                headers: { 'Accept': 'application/json' }
            }).then(success, error);

            return deferred.promise;

            function defaultSuccess(response) {
                deferred.resolve(angular.fromJson(response.data));
            }

            function defaultError() {
                deferred.reject(Error("Sorry :( we have fail"));
            }
        }
    }]);