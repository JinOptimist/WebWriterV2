﻿angular.module('services').service('userService', ['$cookies', '$q', 'httpHelper',
    function ($cookies, $q, httpHelper) {

        var currentUser = null;

        return {
            login: login,
            logout: logout,
            register: register,
            uploadAvatar: uploadAvatar,
            getCurrentUserId: getCurrentUserId,
            getById: getById,
            updateShowExtendedFunctionality: updateShowExtendedFunctionality,
            getAll: getAll,
            remove: remove,
            recoverPassword: recoverPassword,

            /* may be old */
            addBookmark: addBookmark,
            becomeWriter: becomeWriter,
            getCurrentUser: getCurrentUser
        };

        function getCurrentUserId() {
            return $cookies.get(ConstCookies.userId);
        }

        function login(user) {
            var url = '/api/user/LoginPost';
            return httpHelper.post(url, user);
        }

        function logout() {
            $cookies.remove(ConstCookies.userId);
            $cookies.remove(ConstCookies.isAdmin);
            $cookies.remove(ConstCookies.isWriter);
            currentUser = null;
        }

        function register(user) {
            var url = '/api/user/Register';
            var data = angular.toJson(user);
            return httpHelper.post(url, data);
        }

        function getCurrentUser() {
            var userId = $cookies.get(ConstCookies.userId);
            return getById(userId);
        }

        function getById(userId) {
            var url = '/Rpg/GetUserById';
            var data = {
                userId: userId
            };
            return httpHelper.post(url, data);
        }

        function getAll() {
            var url = '/Rpg/GetAllUsers';
            return httpHelper.get(url);
        }

        function updateShowExtendedFunctionality(userId, showExtendedFunctionality) {
            var url = '/api/user/UpdateShowExtendedFunctionality';
            var data = {
                userId: userId,
                showExtendedFunctionality: showExtendedFunctionality
            };
            return httpHelper.get(url, data);
        }

        function addBookmark(eventId, heroJson) {
            var url = '/Rpg/AddBookmark';
            var data = {
                eventId: eventId,
                heroJson: heroJson
            };
            return httpHelper.post(url, data);
        }

        function remove(userId) {
            var url = '/Rpg/RemoveUser';
            var data = {
                userId: userId
            };
            return httpHelper.post(url, data);
        }

        function becomeWriter() {
            var url = '/Rpg/BecomeWriter';
            return httpHelper.post(url);
        }

        function uploadAvatar(imageData) {
            var url = '/api/user/UploadAvatar';
            var data = {
                Data: imageData
            };
            return httpHelper.post(url, data);
        }

        function recoverPassword(email) {
            var url = '/api/user/RecoverPassword';
            var data = {
                email: email
            };
            return httpHelper.get(url, data);
        }
    }]);