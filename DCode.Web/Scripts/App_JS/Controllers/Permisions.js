(function () {
    'use strict';
    angular.module('dCodeApp')
    .controller('permissionsController', PermissionsController);

    PermissionsController.$inject = ['$scope', '$http', '$rootScope', '$filter', '$window','$anchorScroll','$location', 'UserContextService'];

    function PermissionsController($scope, $http, $rootScope, $filter, $window,$anchorScroll,$location, UserContextService) {
        $scope.userContext = null;
        $scope.taskApplicantsRecordCount = 100;
        $scope.taskApplicantsGlobal = null;
        $scope.taskApplicants = null;
        $scope.taskStatusRecordCount = 100;
        $scope.taskStatusesGlobal = null;
        $scope.taskStatuses = null;
        $scope.activeParentIndex = [];
        $scope.reviewIndex = null;
        $scope.taskStatusCount = 0;
        $scope.taskStatusesTotalRecords = 0;
        $scope.taskApplicantsCount = 0;
        $scope.taskApplicantsTotalRecords = 0;
        $scope.searchBox = null;
        $scope.permissionsVisibility = {showFirst : true}
        $rootScope.permissionsCount--;//set the count on ui

        $scope.NavigateToNewTask = function () {
            location.href = '/Requestor/NewTasks'
        }
        $scope.filterTaskApplicants = function () {
            var searchOptions = { Task: { ProjectName: null } };
            if ($scope.searchBox != null) {
                searchOptions.Task.ProjectName = $scope.searchBox;
                //searchOptions.TaskName = $scope.searchBox;
            }
            $scope.taskApplicants = $filter('filter')(angular.copy($scope.taskApplicantsGlobal), searchOptions);
            $scope.taskStatuses = $filter('filter')(angular.copy($scope.taskStatusesGlobal), searchOptions);
        };

        $scope.showStatusResponse = function (index) {
            if ($scope.activeParentIndex[index])
                $scope.activeParentIndex[index] = false
            else
                $scope.activeParentIndex[index] = true;
        };

        $scope.showReviewOptions = function (index) {
            $scope.reviewIndex = index;
        };

        $scope.isShowingReview = function (index) {
            return $scope.reviewIndex == index;
        };

        $scope.RefreshViewModel = function()
        {
            $scope.taskApplicantsTotalRecords = 0;
            $scope.taskApplicantsCount = 0;
            $scope.taskApplicants = null;
            $scope.getApplicants();
        }


        $scope.getApplicants = function () {
            //Make service calls only if fetched records are less than total records
            if ($scope.taskApplicants == null || ($scope.taskApplicants.length < $scope.taskApplicantsTotalRecords)) {
                $scope.taskApplicantsCount++;
                $http({
                    url: "/Requestor/GetTaskApplicantsForPermissions?currentPageIndex=" + $scope.taskApplicantsCount + "&recordsCount=" + $scope.taskApplicantsRecordCount,
                    method: "GET",
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null) {
                            if ($scope.taskApplicants == null) {
                                $scope.taskApplicants = data.permissionTasks;
                                $scope.taskApplicantsGlobal = data.permissionTasks;
                            }
                            else {
                                angular.forEach(data.permissionTasks, function (value, index) {
                                    $scope.taskApplicants.push(value);
                                });
                            }
                            $scope.taskApplicantsTotalRecords = data.TotalRecords;
                            if ($scope.taskApplicants.length > 0) {
                                $scope.permissionsVisibility.showFirst = false;
                            }
                            else
                            {

                                $scope.permissionsVisibility.showFirst = true;
                            }
                        }
                    }
                }).error(function (error) {
                });
            }
        }

        $scope.AllowTask = function (task, applicant, taskApplicantId) {
            $scope.allowTask = {
                TaskApplicantId: taskApplicantId,
                TaskId: task.Id,
                ApplicantId: applicant.ApplicantId
            };


            $http({
                method: 'POST',
                url: '/Requestor/AllowTask',
                data: {
                    allowTaskRequest: $scope.allowTask
                },
                async: true,
                //headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data, status, headers, config) {
                var test = data;
                if (data != null && data > 0) {

                    $scope.AssignedDivModel =
                        {
                            Visible: true,
                            AssigneeName: applicant.Name,
                            AssignedTask: task.ProjectName,
                            AssignedHours: task.Hours + " hrs",
                            StartingDate: task.OnBoardingDate
                        }
                    $scope.RejectedDivModel = { Visible: false };
                    $scope.RefreshViewModel();
                    $rootScope.permissionsCount--;//set the count on ui
                    $location.hash('divSuccess');
                }
                // handle success things
            }).error(function (data, status, headers, config) {
                // handle error things
            });
        }

        $scope.RejectTask = function (task, applicant, taskApplicantId) {
            $scope.rejectTask = {
                TaskApplicantId: taskApplicantId,
                TaskId: task.Id,
                ApplicantId: applicant.ApplicantId
            };
            $http({
                method: 'POST',
                url: '/Requestor/RejectTask',
                data: {
                    rejecttaskRequest: $scope.rejectTask
                },
                async: true,
            }).success(function (data, status, headers, config) {
                var test = data;
                if (data != null && data > 0) {
                    $scope.RejectedDivModel =
                        {
                            Visible: true,
                            AssigneeName: applicant.Name,
                            AssignedTask: task.ProjectName,
                            AssignedHours: task.Hours + " hrs",
                            StartingDate: task.OnBoardingDate
                        }

                    $scope.AssignedDivModel = { Visible: false };

                    $scope.RefreshViewModel();
                    $rootScope.permissionsCount--;//set the count on ui
                    $location.hash('divReject');
                }

            }).error(function (data, status, headers, config) {
                // handle error things
            });
        }

        $scope.onLoad = function () {
            $scope.getApplicants();
            //$scope.getStatusOftasks();
            //$scope.isFirstTimeUser();
        }
        $scope.onLoad();
    }
})();