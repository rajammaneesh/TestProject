
(function () {
    'use strict';
    angular.module('dCodeApp')
    .controller('ProfileController', ProfileController);

    ProfileController.$inject = ['$scope', '$http', '$rootScope', '$location', 'UserContextService'];

    function ProfileController($scope, $http, $rootScope, $location, UserContextService) {
        $scope.userContext = [];
        $scope.skillSet = [];
        $scope.skillsFromDb = [];
        $scope.profile = [];
        $scope.selectedSkill = null;
        $scope.isEditMode = false;
        $scope.test = { isEditMode: false };
        $scope.isFirstTimeLogin = false;
        $scope.skillModel={ newSkillValue: "",
            successMessage: null,
            errorMessage: null
    }

        $scope.resetSkillValues = function () {
            $scope.skillModel.successMessage = null;
            $scope.skillModel.newSkillValue = "";
            $scope.skillModel.errorMessage = null;
            $("#form").trigger('reset');
            $("#form").find("label").text("").end();
        }

        $scope.insertSkill = function () {
            if ($scope.skillModel.newSkillValue != null) {
                $http({
                    url: "/Common/InsertNewSkill",
                    method: "POST",
                    data: { skillValue: $scope.skillModel.newSkillValue }
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null) {
                            $scope.skillModel.successMessage = null;
                            $scope.skillModel.errorMessage = null;
                            if (data == "Added Skill") {
                                $scope.skillModel.successMessage = data;
                            }
                            else {
                                $scope.skillModel.errorMessage = data;
                            }
                            
                        }
                    }
                }).error(function (error) {
                });
            }
        }
        $scope.changeToEditMode = function () {
            $scope.isEditMode = !$scope.isEditMode;
        }
        $scope.selectedSkill = function (value) {
            if (value != null && value.originalObject != null) {
                if (!$scope.isSkillExist(value.originalObject.Id)) {
                    if ($scope.skillSet == null) {
                        $scope.skillSet = [];
                        $scope.skillSet.push(value.originalObject);
                    }
                    else {
                        $scope.skillSet.push(value.originalObject);
                    }
                    $("#isSkillAdded").removeClass("invalid");
                }
            }
        }
        $scope.updateProfile = function () {
            var isSkillAdded = false;
            if ($scope.skillSet != null) {
                if ($scope.skillSet.length > 0) {
                    isSkillAdded = true;
                    $("#isSkillAdded").removeClass("invalid");
                } else {
                    $("#isSkillAdded").addClass("invalid");
                }

            }
            else {
                $("#isSkillAdded").addClass("invalid");
            }
            
            if (!!$scope.skillSet) {


            $scope.profileRequest = {
                UserId: $scope.userContext.UserId,
                ProjectName: $scope.profile.ProjectName,
                ProjectCode: $scope.profile.ProjectCode,
                ManagerName: $scope.profile.ManagerName,
                ManagerEmailId: $scope.profile.ManagerEmailId,
                SkillSet: $scope.skillSet
            };



            $http({
                url: "/Profile/UpdateProfile",
                method: "POST",
                data: { profileRequest: $scope.profileRequest }
            }).success(function (data, status, headers, config) {
                if (data != undefined) {
                    if (data != null) {
                        UserContextService.InitializeUserContext().then(function (data) {
                            if ($rootScope.userContext != null && $rootScope.userContext.Role == "1") {
                                location.href = "/Requestor/Dashboard";
                            }
                            else {
                                location.href = "/Contributor/Dashboard";
                            }
                        });
                    }
                }
            }).error(function (error) {
            });
        }
             else
                 return false;
        }


        $scope.isSkillExist = function (skill) {
            var result = false;
            angular.forEach($scope.skillSet, function (value, index) {
                if (skill == value.Id) {
                    result = true;
                }
            });
            return result;
        }


        $scope.addSkill = function (skill) {
            angular.forEach($scope.skillsFromDb, function (value, index) {
                if ((index + 1) == skill) {
                    var asdf = $scope.isSkillExist(skill);
                    if (!$scope.isSkillExist(skill)) {
                        $scope.skillSet.push(value);
                    }
                }
            });
        }
        $scope.removeSkill = function (index) {
            var skills = $scope.skillSet.splice(index, 1);
        }
        $scope.populateFieldsFromUserContext = function () {
            $scope.userContext = $rootScope.userContext;
            $scope.profile = $rootScope.userContext;
            $scope.skillSet = $rootScope.userContext.SkillSet;
        }
        $scope.checkIfFirstTimeLogin = function(){
            if ($rootScope.userContext != null && ($rootScope.userContext.ManagerEmailId == null || $rootScope.userContext.ManagerEmailId == "")) {
                $scope.isFirstTimeLogin = true;
                $scope.isEditMode = true;
            }
        }
        $scope.onLoad = function () {
            $scope.populateFieldsFromUserContext();
            $scope.checkIfFirstTimeLogin();
            
        }

        UserContextService.InitializeUserContext().then(function (data) {
            $scope.onLoad();
        });
    }

})();