
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('services', ['ngRoute', 'ngCookies', 'underscore'])
    .constant('_',
        window._
    )
    .run(['$http', 'sexService', 'raceService', 'heroService', function ($http, sexService, raceService, heroService) {
        $http({
            method: 'POST',
            url: '/Rpg/GetDefaultHero',
            headers: { 'Accept': 'application/json' }
        }).success(function (rawHero) {
            var hero = angular.fromJson(rawHero);
            heroService.setDefaultHero(hero);
        });
    }])
    .service('questService', ['$http', '$q', function ($http, $q) {
        function saveQuest(quest) {
            var deferred = $q.defer();

            var request = {
                method: 'POST',
                url: '/Rpg/SaveQuest',
                data: { jsonQuest: angular.toJson(quest) }
            };

            $http(request)
                .success(function (response) {
                    deferred.resolve(angular.fromJson(response));
                });

            return deferred.promise;
        }

        function changeRootEvent(questId, eventId) {
            var deferred = $q.defer();

            var request = {
                method: 'POST',
                url: '/Rpg/ChangeRootEvent',
                data: {
                    questId: questId,
                    eventId: eventId
                }
            };

            $http(request)
                .success(function (response) {
                    deferred.resolve(angular.fromJson(response));
                });

            return deferred.promise;
        }

        function getQuest(questId) {
            var deferred = $q.defer();

            $http({
                method: 'POST',
                url: '/Rpg/GetQuest?id=' + questId,
                headers: { 'Accept': 'application/json' }
            })
                .success(function (response) {
                    var quest = angular.fromJson(response);
                    deferred.resolve(quest);
                });
            return deferred.promise;
        }

        function getQuests() {
            var deferred = $q.defer();

            $http({
                method: 'POST',
                url: '/Rpg/GetQuests',
                headers: { 'Accept': 'application/json' }
            })
                .success(function (response) {
                    var quests = angular.fromJson(response);
                    deferred.resolve(quests);
                });
            return deferred.promise;
        }

        function removeQuest(questId) {
            var deferred = $q.defer();

            $http({
                method: 'POST',
                url: '/Rpg/RemoveQuest?id=' + questId,
                headers: { 'Accept': 'application/json' }
            })
                .success(function (response) {
                    deferred.resolve(response);
                });
            return deferred.promise;
        }

        function importQuest(questJson) {
            var deferred = $q.defer();

            var request = {
                method: 'POST',
                url: '/Rpg/ImportQuest',
                data: { jsonQuest: questJson }
            };

            $http(request)
                .success(function (response) {
                    deferred.resolve(angular.fromJson(response));
                });

            return deferred.promise;
        }

        return {
            saveQuest: saveQuest,
            getQuests: getQuests,
            getQuest: getQuest,
            changeRootEvent: changeRootEvent,
            removeQuest: removeQuest,
            importQuest: importQuest
        };
    }])
    .service('eventService', ['$http', '$q', function ($http, $q) {
        function getEventForTravel(eventId, heroId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/GetEventForTravel',
                    data: {
                        eventId: eventId,
                        heroId: heroId
                    },
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

        function eventChangesApplyToHero(eventId, heroId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/EventChangesApplyToHero',
                    data: {
                        eventId: eventId,
                        heroId: heroId
                    },
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

        function getEvent(currentEventId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/GetEvent?id=' + currentEventId,
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

        function getEvents(questId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/GetEvents?questId=' + questId,
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function getEndingEvents(questId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/GetEndingEvents?questId=' + questId,
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function getNotAvailableEvents(questId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/GetNotAvailableEvents?questId=' + questId,
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function save(event, questId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/SaveEvent',
                    data: {
                        jsonEvent: angular.toJson(event),
                        questId: questId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function remove(eventId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveEvent',
                    data: {
                        eventId: eventId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    if (response) {
                        resolve(angular.fromJson(response));
                    } else {
                        reject(Error("We can't remove event wich has child"));
                    }
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function saveEventLink(eventLink) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/SaveEventLink',
                    data: {
                        jsonEventLink: angular.toJson(eventLink)
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function removeEventLink(eventLinkId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveEventLink',
                    data: {
                        eventLinkId: eventLinkId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function addSkill(eventId, skillId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/AddSkillToEvent',
                    data: {
                        eventId: eventId,
                        skillId: skillId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function removeSkill(eventId, skillId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveSkillToEvent',
                    data: {
                        eventId: eventId,
                        skillId: skillId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function addCharacteristic(eventId, characteristicTypeId, characteristicValue, requirementType) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/AddCharacteristicToEvent',
                    data: {
                        eventId: eventId,
                        characteristicTypeId: characteristicTypeId,
                        characteristicValue: characteristicValue,
                        requirementType: requirementType.Value.Value
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function removeCharacteristic(characteristicId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveCharacteristicFromEvent',
                    data: {
                        characteristicId: characteristicId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function addState(eventId, stateTypeId, stateValue) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/AddStateToEvent',
                    data: {
                        eventId: eventId,
                        stateTypeId: stateTypeId,
                        stateValue: stateValue
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function removeState(stateId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveStateFromEvent',
                    data: {
                        stateId: stateId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function addRequirementThing(eventId, thingSampleId, count) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/AddRequirementThingToEvent',
                    data: {
                        eventId: eventId,
                        thingSampleId: thingSampleId,
                        count: count
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function removeRequirementThing(eventId, thingId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveRequirementThingFromEvent',
                    data: {
                        eventId: eventId,
                        thingId: thingId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function addThingChanges(eventId, thingSampleId, count) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/AddThingChangesToEvent',
                    data: {
                        eventId: eventId,
                        thingSampleId: thingSampleId,
                        count: count
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function removeThingChanges(eventId, thingId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveThingChangesFromEvent',
                    data: {
                        eventId: eventId,
                        thingId: thingId
                    },
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

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
            getEndingEvents: getEndingEvents,
            getNotAvailableEvents: getNotAvailableEvents,
            addRequirementThing: addRequirementThing,
            removeRequirementThing: removeRequirementThing,
            addThingChanges: addThingChanges,
            removeThingChanges: removeThingChanges,
            getEventForTravel: getEventForTravel,
            eventChangesApplyToHero: eventChangesApplyToHero
        };
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
    .service('stateService', ['$http', '$q', '_', function ($http, $q, _) {
        var states = [];

        function changeState(stateId, delta) {
            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: '/Rpg/ChangeState',
                data: {
                    stateId: stateId,
                    delta: delta
                },
                headers: { 'Accept': 'application/json' }
            })
                .success(function (response) {
                    var state = angular.fromJson(response);
                    deferred.resolve(state);
                });
            return deferred.promise;
        }

        return {
            loadAllTypes: function () {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/GetStateTypes',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        states = angular.fromJson(response);
                        deferred.resolve(states);
                    });
                return deferred.promise;
            },
            save: function (state) {
                return '+';
            },
            changeState: changeState
        };
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
    .service('thingService', ['$http', '$q', '_', function ($http, $q, _) {
        return {
            loadAllSamples: function () {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/GetThingSamples',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        deferred.resolve(angular.fromJson(response));
                    });
                return deferred.promise;
            },
        };
    }])
    //CKEditor
    .service('CKEditorService', ['$http', '$q', '_', function ($http, $q, _) {
        function reloadEditor(editorName, newData) {
            var editor = CKEDITOR.instances[editorName];
            if (editor) {
                editor.destroy(true);
            }
            editor = CKEDITOR.replace(editorName, {width: 1200, height: 600});
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
        }]);