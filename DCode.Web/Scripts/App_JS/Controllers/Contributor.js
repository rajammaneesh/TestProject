﻿(function () {
    'use strict';
    angular.module('dCodeApp')
        .controller('contributorController', ContributorController);

    ContributorController.$inject = ['$scope', '$http', '$rootScope', '$filter', '$window', '$anchorScroll', '$location', 'UserContextService'];

    function ContributorController($scope, $http, $rootScope, $filter, $window, $anchorScroll, $location, UserContextService) {
        $scope.userContext = null;
        $scope.taskApplicantsRecordCount = 100;
        $scope.tasksGlobal = null;
        $scope.tasks = null;
        $scope.taskStatusVisibility = { showFirst: true };
        $scope.assignedTasksCount = 0;
        $scope.taskStatusRecordCount = 100;
        $scope.taskStatuses = null;
        $scope.activeParentIndex = null;
        $scope.reviewIndex = null;
        $scope.taskStatusCount = 0;
        $scope.taskStatusesTotalRecords = 0;
        $scope.tasksCount = 0;
        $scope.taskApplicantsTotalRecords = 0;
        $scope.searchBox = { text: null };
        $scope.taskSearch = { text: null, searchFilter: "A", selectedTaskType: 1 };
        $scope.dashboard = { showApproval: true, showTaskStatus: false, showHistory: false, showCreate: false };
        $scope.divVisibiltyModel = { showDetails: true, showSummary: true, showSuccess: false, showApply: true };
        $scope.workAgain = [];
        $scope.ratingValue = [];
        $scope.trackStatus = { Hours: null };
        $scope.tasksPageIndex = 0;
        $scope.tasksRecordCount = 20;
        $scope.tasksTotalRecords = null;
        $scope.tasksAssignedPageIndex = 0;
        $scope.tasksAssignedRecordCount = 20;
        $scope.tasksAssignedTotalRecordsCount = null;
        $scope.assignedTasks = null;
        $scope.assignedTasksGlobal = null;
        $scope.managersEmailId = "";
        $scope.searchFilters = [{ Id: "M", Description: "My Offering" },  // HotFix. Need to review later.
        { Id: "R", Description: "Recommended Tasks" },
        { Id: "A", Description: "All Portfolios" }];

        $scope.selectTaskTypes = [
            { Id: 1, Description: "Client Service" },
            { Id: 2, Description: "Firm Initiative" },
            { Id: 3, Description: "Industry Initiative" }];

        //$scope.SelectedProficiencyType = [
        //    { Id: 1, Description: "Beginner" },
        //    { Id: 2, Description: "Intermediate" },
        //    { Id: 3, Description: "Expert" }];
        $scope.SelectedProficiencyType = [];
        $scope.GetAllProficiencies = function () {

            if ($scope.SelectedProficiencyType.length == 0) {
                var url = "/Contributor/GetAllProficiencies";

                $http({
                    url: url,
                    method: "POST",
                }).success(function (data) {
                    if (data != null) {
                        $scope.SelectedProficiencyType = data;
                    }
                }).error(function (error) {
                });
            }
        }

        $scope.controlTabsMyTasks = function (value) {
            if (value == 'approval') {
                $scope.dashboard.showApproval = true;
                $scope.dashboard.showTaskStatus = false;
                $scope.dashboard.showHistory = false;
            }
            else if (value == 'taskstatus') {
                $scope.dashboard.showApproval = false;
                $scope.dashboard.showTaskStatus = true;
                $scope.dashboard.showHistory = false;
            }
            else if (value == 'history') {
                $scope.dashboard.showApproval = false;
                $scope.dashboard.showTaskStatus = false;
                $scope.dashboard.showHistory = true;
            }
        }

        $scope.NavigateToProfile = function () {
            location.href = '/Profile/Profile'
        }

        $scope.NavigateToApplications = function () {
            location.href = '/Contributor/Dashboard'
        }

        $scope.showStatusResponse = function (index) {
            $scope.activeParentIndex = index;
        };
        $scope.isShowing = function (index) {
            return $scope.activeParentIndex == index;
        };
        $scope.showReviewOptions = function (index, task) {
            if (task.TypeId == 1) {

                $scope.divVisibiltyModel.showSummary = true;
                $scope.divVisibiltyModel.showSuccess = false;
                $scope.reviewIndex = index;
                setTimeout(function () { $('#txtManagerEmailId' + index).focus() }, 1);

            }
            else if (task.TypeId >= 2) {
                $("[id=ddlProfType]").css("border-color", "");
                $scope.reviewIndex = index;
                if (task.SelectedProficiencyType == undefined || task.SelectedProficiencyType == '') {
                    $("[id=ddlProfType]:eq(" + index + ")").css("border-color", "red");
                }
                else {
                    $scope.applyFITask(task);
                }
            }
        };

        $scope.showTimeAddBar = function (index) {
            $scope.divVisibiltyModel.showSummary = true;
            $scope.divVisibiltyModel.showSuccess = false;
            $scope.reviewIndex = index;
            setTimeout(function () { $('#txtManagerEmailId' + index).focus() }, 1);

        }

        $scope.isShowingReview = function (index) {
            return $scope.reviewIndex == index;
        };

        $scope.filterTasks = function () {
            var searchOptions2 = { Task: { ProjectName: null } };
            if ($scope.searchBox != null) {
                searchOptions2.Task.ProjectName = $scope.searchBox.text;
            }
            $scope.assignedTasks = $filter('filter')(angular.copy($scope.assignedTasksGlobal), searchOptions2);
        };
        $scope.reinitialiseAssignedTasksVariables = function () {
            $scope.tasksAssignedPageIndex = 0;
            $scope.assignedTasks = null;
        }
        $scope.postTime = function (approvedApplicant) {
            if ($scope.trackStatus.Hours != null) {
                $http({
                    url: "/Contributor/updatehours",
                    method: "POST",
                    data: { approvedApplicantId: approvedApplicant, hours: $scope.trackStatus.Hours }
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null) {
                            $scope.reinitialiseAssignedTasksVariables();
                            $scope.getAssignedTasks()
                            $scope.trackStatus.Hours = null;
                            $scope.$emit('updateBanner', {});
                        }
                    }

                }).error(function (error) {
                });
            }
        }
        $scope.getAssignedTasks = function () {
            if ($scope.assignedTasks == null || ($scope.assignedTasks.length < $scope.tasksAssignedTotalRecordsCount)) {
                $scope.tasksAssignedPageIndex++
                $http({
                    url: "/Contributor/GetAssignedTasks?currentPageIndex=" + $scope.tasksAssignedPageIndex + "&recordsCount=" + $scope.tasksAssignedRecordCount,
                    method: "GET",
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null) {
                            if ($scope.assignedTasks == null) {
                                $scope.assignedTasks = data.AssignedTasks;
                                $scope.assignedTasksGlobal = data.AssignedTasks;
                            }
                            else {
                                angular.forEach($scope.assignedTasks, function (value, index) {
                                    $scope.assignedTasks.push(value);
                                    $scope.assignedTasksGlobal.push(value);
                                });
                            }
                            $scope.assignedTasksCount = data.TotalRecords;
                            if ($scope.assignedTasks.length > 0) {
                                $scope.taskStatusVisibility.showFirst = false;
                            }
                            else {
                                $scope.taskStatusVisibility.showFirst = true;
                            }
                        }
                    }
                }).error(function (error) {
                });
            }
        }

        $scope.reinitialiseVariables = function () {
            $scope.tasksPageIndex = 0;
            $scope.tasks = null;
        }

        $scope.refreshTasksBasedonInput = function () {
            $scope.refreshTasks();
        }

        $scope.getTasksOnSearchClick = function () {
            $scope.refreshTasks();
        }

        $scope.getTasks = function () {
            $scope.GetAllProficiencies();
            if ($scope.tasks == null || ($scope.tasks.length < $scope.tasksTotalRecords)) {             
                $scope.tasksPageIndex++;

                var searchKey = $scope.taskSearch.text != null ? $scope.taskSearch.text : '';

                var url = "/Contributor/GetAllTasks?searchKey=" + searchKey + "&currentPageIndex=" + $scope.tasksPageIndex + "&recordsCount=" + $scope.tasksRecordCount
                    + "&searchFilter=" + $scope.taskSearch.searchFilter + "&selectedTaskType=" + $scope.taskSearch.selectedTaskType;

                $http({
                    url: url,
                    method: "POST",
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null) {
                            if ($scope.tasks == null) {
                                $scope.tasks = data.Tasks;
                                $scope.tasksGlobal = data.Tasks;
                                $scope.tasksCount = data.Tasks.length;

                            }
                            else {
                                angular.forEach(data.Tasks, function (value, index) {
                                    $scope.tasks.push(value);
                                    $scope.tasksGlobal.push(value);
                                });
                            }
                            $scope.tasksTotalRecords = data.TotalRecords;
                        }
                    }
                }).error(function (error) {
                });
            }
        }

        $scope.refreshTasks = function () {
            $scope.reinitialiseVariables();
            $scope.getTasks();
        }

        $scope.ValidatePermissionDetails = function (index) {
            var isValid = true;
            var userEmail = "";
            if ($rootScope.userContext != null) {
                userEmail = $rootScope.userContext.EmailId;
            }
            if ($("#txtManagerEmailId" + index).val() == "" || $("#txtManagerEmailId" + index).val() == null || $("#txtManagerEmailId" + index).val() == userEmail) {
                $("#divManagerEmailId" + index).addClass("invalid");
                isValid = false;
            }

            else {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((deloitte.com))$/igm;
                if (!re.test($("#txtManagerEmailId" + index).val())) {
                    $("#divManagerEmailId" + index).addClass("invalid");
                    isValid = false;
                }
                else {
                    $("#divManagerEmailId" + index).removeClass("invalid");
                }
            }
            if ($("#txtSOP" + index).val() == "" || $("#txtSOP" + index).val() == null) {
                $("#divSOP" + index).addClass("invalid");
                isValid = false;
            }
            else {
                $("#divSOP" + index).removeClass("invalid");
            }
            return isValid;
        };

        $('#txtCompletedHrs').keydown(function (e) {
            var order = e.which;
            if (order == 187 || order == 189 || order == 69) {
                return false;
            };
        });

        $scope.applyTask = function (task, managersEmailID, statementOfPurpose, indexVal) {
            if ($scope.ValidatePermissionDetails(indexVal)) {
                var managerEmailAddress = "";
                if (managersEmailID != null && managersEmailID != "") {
                    managerEmailAddress = managersEmailID;
                }
                $http({
                    url: "/Contributor/ApplyTask",
                    method: "POST",
                    data: {
                        taskId: task.Id,
                        emailAddress: managerEmailAddress,
                        statementOfPurpose: statementOfPurpose,
                        proficiency: task.SelectedProficiencyType
                    }
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null && data > 0) {

                            $scope.taskRequest =
                                {
                                    TaskName: task.TaskName,
                                    ProjectName: task.ProjectName,
                                    Hours: task.Hours,
                                    StartingDate: task.OnBoardingDate,
                                    TaskType: task.TypeId
                                }

                            $scope.divVisibiltyModel.showSuccess = true;
                            $scope.divVisibiltyModel.showSummary = false;
                            $scope.refreshTasks();
                            $('#divCongrats').modal('show');
                        }
                    }

                }).error(function (error) {
                });
            }
        }

        $scope.applyFITask = function (task) {
            var userEmail = "";
            if ($rootScope.userContext != null) {
                userEmail = $rootScope.userContext.EmailId;
            }
            if (userEmail != task.RequestorEmailId) {
                $http({
                    url: "/Contributor/ApplyFITask",
                    method: "POST",
                    data: {
                        taskId: task.Id,
                        requestor: task.RequestorEmailId,
                        proficiency: task.SelectedProficiencyType
                    }
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null && data > 0) {

                            $scope.taskRequest =
                                {
                                    TaskName: task.TaskName,
                                    ProjectName: task.ProjectName,
                                    Hours: task.Hours,
                                    StartingDate: task.OnBoardingDate,
                                    TaskType: task.TypeId
                                }

                            $scope.divVisibiltyModel.showSuccess = true;
                            $scope.divVisibiltyModel.showSummary = false;
                            $scope.refreshTasks();
                            $('#divCongrats').modal('show');
                        }
                    }

                }).error(function (error) {
                });
            }
        }

        $scope.cancelPermission = function () {
            $scope.divVisibiltyModel.showSummary = false;
        }



        //will be handled by ng-infinite scroll
        $scope.onLoad = function () {
            //$scope.getTasks();
            //$scope.getAssignedTasks();
        }
        $scope.onLoad();

        $scope.CloseModal = function () {
            $('#divCongrats').modal('toggle');
        };

    }
})();

(function () {
    'use strict';
    angular.module('dCodeApp')
        .controller('contributorHistoryController', HistoryController);

    HistoryController.$inject = ['$scope', '$http', '$rootScope', 'UserContextService'];

    function HistoryController($scope, $http, $rootScope, UserContextService) {
        $scope.historyVisibility = { showFirst: true, showHistory: false };
        $scope.tasksHistory = null;
        $scope.historyPageIndex = 0;
        $scope.historyRecordsCount = 20;
        $scope.historyTotalRecords = 0;

        $scope.getTaskHistory = function () {
            if ($scope.tasksHistory == null || ($scope.tasksHistory.length < $scope.historyTotalRecords)) {
                $scope.historyPageIndex++;
                $http({
                    url: "/Contributor/GetTaskHistories?currentPageIndex=" + $scope.historyPageIndex + "&recordsCount=" + $scope.historyRecordsCount,
                    method: "GET"
                }).success(function (data, status, headers, config) {
                    var test = data;
                    if (data != null) {
                        if ($scope.tasksHistory == null) {
                            $scope.tasksHistory = data.TaskHistories;
                        }
                        else {
                            angular.forEach(data.TaskHistories, function (value, indes) {
                                $scope.tasksHistory.push(value);
                            });
                        }
                        $scope.historyTotalRecords = data.TotalRecords;
                        if ($scope.tasksHistory.length > 0) {
                            $scope.historyVisibility.showFirst = false;
                            $scope.historyVisibility.showHistory = true;
                        }
                        else {
                            $scope.historyVisibility.showFirst = true;
                            $scope.historyVisibility.showHistory = false;
                        }

                    }
                }).error(function (error) {
                });
            }
        }

        $scope.NavigateToApplications = function () {
            location.href = '/Contributor/Dashboard'
        }

        //On load is taken care by ng-infinite scroll
        //$scope.onLoad = function () {
        //    $scope.getTaskHistory();
        //}
        //$scope.onLoad();

    }
})();


