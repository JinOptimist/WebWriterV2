angular.module('services')
    .service('stateService', ['httpHelper', function (httpHelper) {
        return {
            addStateType: addStateType,
            removeStateType: removeStateType,
            addStateChange: addStateChange,
            removeStateChange: removeStateChange,
            addStateRequirement: addStateRequirement,
            removeStateRequirement: removeStateRequirement
        };

        function addStateType(stateType) {
            var url = '/api/book/AddState';
            var data = angular.toJson(stateType);
            return httpHelper.post(url, data);
        }

        function removeStateType(stateTypeId) {
            var url = '/api/book/removeState';
            var data = {
                stateTypeId: stateTypeId
            };
            return httpHelper.get(url, data);
        }

        function addStateChange(stateChange) {
            var url = '/api/chapterLink/AddStateChange';
            var data = angular.toJson(stateChange);
            return httpHelper.post(url, data);
        }

        function removeStateChange(stateChangeId) {
            var url = '/api/chapterLink/RemoveStateChange';
            var data = {
                stateChangeId: stateChangeId
            };
            return httpHelper.get(url, data);
        }

        function addStateRequirement(stateRequirement) {
            var url = '/api/chapterLink/AddStateRequirement';
            var data = angular.toJson(stateRequirement);
            return httpHelper.post(url, data);
        }

        function removeStateRequirement(stateRequirementId) {
            var url = '/api/chapterLink/RemoveStateRequirement';
            var data = {
                stateRequirementId: stateRequirementId
            };
            return httpHelper.get(url, data);
        }
    }]);