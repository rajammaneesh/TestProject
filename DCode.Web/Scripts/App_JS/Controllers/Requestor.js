(function () {
    'use strict';
    angular.module('dCodeApp')
    .controller('requestorController', RequestorController);

    RequestorController.$inject = ['$scope', '$http', '$rootScope', '$filter', '$window', '$anchorScroll', '$location', 'UserContextService', '$uibModal', '$log'];

    function RequestorController($scope, $http, $rootScope, $filter, $window, $anchorScroll, $location, UserContextService, $uibModal, $log) {
        $scope.userContext = null;
        $scope.taskApplicantsRecordCount = 100;
        $scope.taskApplicantsGlobal = null;
        $scope.taskApplicants = null;
        $scope.taskStatusRecordCount = 100;
        $scope.taskStatusesGlobal = null;
        $scope.taskStatuses = null;
        $scope.activeParentIndex = null;
        $scope.reviewIndex = null;
        $scope.taskStatusCount = 0;
        $scope.taskStatusesTotalRecords = 0;
        $scope.taskApplicantsCount = 0;
        $scope.taskApplicantsTotalRecords = 0;
        $scope.searchBox = { text: null };
        $scope.dashboard = { showApproval: true, showTaskStatus: false, showHistory: false };
        $scope.taskStatusVisibility = { showFirst: true };
        $scope.taskTypes = [{ Id: 1, Description: "Client Service" }, { Id: 2, Description: "Firm Initiative" }];
        $scope.selectedTaskTypes = {
            taskApplications: 1,
            taskStatus: 1,
            taskHistory: 1
        };
        $scope.workAgain = [];
        $scope.ratingValue = [];
        $scope.classValue = null;
        $scope.classApproval = "pull-left margin-left-right-10 letter-spacing-1 link active";//On page load default active
        $scope.classTrackStatus = "pull-left margin-left-right-10 letter-spacing-1 link";
        $scope.classHistory = "pull-left margin-left-right-10 letter-spacing-1 link";

        $scope.controlReviewApplicant = function (value) {
            $scope.taskStatusesGlobal[value].IsWorkAgainClicked = true;
            if ($scope.taskStatusesGlobal[value].IsWorkAgainClicked && $scope.taskStatusesGlobal[value].IsRatingClicked) {
                $scope.taskStatusesGlobal[value].EnableReviewApplicant = true;
            }
        }
        $scope.ratingClicked = function (value) {
            $scope.taskStatusesGlobal[value].IsRatingClicked = true;
            if ($scope.taskStatusesGlobal[value].IsWorkAgainClicked && $scope.taskStatusesGlobal[value].IsRatingClicked) {
                $scope.taskStatusesGlobal[value].EnableReviewApplicant = true;
            }
        }

        $scope.reinitialiseClasses = function () {
            $scope.classApproval = "pull-left margin-left-right-10 letter-spacing-1 link";
            $scope.classTrackStatus = "pull-left margin-left-right-10 letter-spacing-1 link";
            $scope.classHistory = "pull-left margin-left-right-10 letter-spacing-1 link";
        }
        $scope.addActiveClass = function (value) {
            $scope.reinitialiseClasses();
            if (value == "approval") {
                $scope.classApproval = $scope.classApproval + " active";
            }
            else if (value == "taskstatus") {
                $scope.classTrackStatus = $scope.classTrackStatus + " active";
            }
            else if (value == "history") {
                $scope.classHistory = $scope.classHistory + " active";
            }
        }

        $scope.controlTabsMyTasks = function (value) {
            $scope.addActiveClass(value);
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
            //$scope.$apply();
            //$scope.$digest();
        }

        $scope.NavigateToNewTask = function () {
            location.href = '/Requestor/NewTasks'
        }
        $scope.deselectRating = function () {
            $scope.classValueGood = 'pull-left';
            $scope.classValueAverage = 'pull-left';
            $scope.classValueUnS = 'pull-left';
        }
        $scope.changeClassValueOfGood = function (value) {
            $scope.controlReviewApplicant(value);
            if ($scope.classValueGood == 'pull-left' || ($scope.indexValue != null && $scope.indexValue == value)) {
                $scope.indexValue = value;
                $scope.classValueGood = 'pull-left active';
                $scope.classValueAverage = 'pull-left';
                $scope.classValueUnS = 'pull-left';
            }
            else {
                $scope.classValueGood = 'pull-left';
                $scope.indexValue = null;
            }
        }

        $scope.changeClassValueOfAverage = function (value) {
            $scope.controlReviewApplicant(value);
            if ($scope.classValueAverage == 'pull-left' || ($scope.indexValue != null && $scope.indexValue == value)) {
                $scope.indexValue = value;
                $scope.classValueGood = 'pull-left';
                $scope.classValueAverage = 'pull-left active';
                $scope.classValueUnS = 'pull-left';
            }
            else {

                $scope.classValueAverage = 'pull-left';
                $scope.indexValue = null;
            }
        }

        $scope.changeClassValueOfUnS = function (value) {
            $scope.controlReviewApplicant(value);
            if ($scope.classValueUnS == 'pull-left' || ($scope.indexValue != null && $scope.indexValue == value)) {
                $scope.indexValue = value;
                $scope.classValueGood = 'pull-left';
                $scope.classValueAverage = 'pull-left';
                $scope.classValueUnS = 'pull-left active';
            }
            else {
                $scope.classValueUnS = 'pull-left';
                $scope.indexValue = null;
            }
        }

        $scope.filterTaskApplicants = function () {
            var searchOptions = { Task: { ProjectName: null } };
            if ($scope.searchBox != null) {
                searchOptions.Task.ProjectName = $scope.searchBox.text;
                //searchOptions.TaskName = $scope.searchBox;
            }
            $scope.taskApplicants = $filter('filter')(angular.copy($scope.taskApplicantsGlobal), searchOptions);
            $scope.taskStatuses = $filter('filter')(angular.copy($scope.taskStatusesGlobal), searchOptions);
        };

        $scope.showStatusResponse = function (index) {
            if ($scope.activeParentIndex == index) {
                $scope.activeParentIndex = null;
            }
            else {
                $scope.activeParentIndex = index;
            }
        };
        $scope.isShowing = function (index) {
            return $scope.activeParentIndex == index;
        };
        $scope.showReviewOptions = function (index) {
            $scope.reviewIndex = index;
            $scope.workAgain = [];
            $scope.ratingValue = [];
            $scope.taskStatusesGlobal[index].EnableReviewApplicant = false;
            $scope.deselectRating();
        };

        $scope.isShowingReview = function (index) {
            return $scope.reviewIndex == index;
        };

        $scope.changeTaskTypeForTaskApplications = function () {
            $scope.taskApplicants = null;
            $scope.taskApplicantsCount = 0;
            $scope.getApplicants();
        };

        $scope.getApplicants = function () {
            //Make service calls only if fetched records are less than total records
            if ($scope.taskApplicants == null || ($scope.taskApplicants.length < $scope.taskApplicantsTotalRecords)) {
                $scope.taskApplicantsCount++;
                $http({
                    url: "/Requestor/GetTaskApplicantsForApproval?currentPageIndex=" + $scope.taskApplicantsCount + "&recordsCount=" + $scope.taskApplicantsRecordCount + "&selectedTaskTypeId=" + $scope.selectedTaskTypes.taskApplications,
                    method: "GET",
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null) {
                            if ($scope.taskApplicants == null) {
                                $scope.taskApplicants = data.TaskApprovals;
                                $scope.taskApplicantsGlobal = data.TaskApprovals;
                            }
                            else {
                                angular.forEach(data.TaskApprovals, function (value, index) {
                                    $scope.taskApplicants.push(value);
                                });
                            }
                            var count = 0;
                            angular.forEach($scope.taskApplicants, function (value, index) {
                                if (value.Applicants.length > 0) {
                                    count++;
                                }
                            });
                            $scope.applicantsCount = count;
                            $scope.taskApplicantsTotalRecords = data.TotalRecords;
                        }
                    }
                }).error(function (error) {
                });
            }
        }

        $scope.changeTaskTypeForTaskStatus = function () {
            $scope.taskStatuses = null;
            $scope.taskStatusCount = 0;
            $scope.getStatusOftasks();
        };

        $scope.getStatusOftasks = function () {
            //Make service calls only if fetched records are less than total records
            if ($scope.taskStatuses == null || ($scope.taskStatuses.length < $scope.taskStatusesTotalRecords)) {
                $scope.taskStatusCount++;
                $http({
                    url: "/Requestor/GetStatusOftasks?currentPageIndex=" + $scope.taskStatusCount + "&recordsCount=" + $scope.taskStatusRecordCount + "&selectedTaskType=" + $scope.selectedTaskTypes.taskStatus,
                    method: "GET",
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null) {
                            if ($scope.taskStatuses == null) {
                                $scope.taskStatuses = data.TaskStatuses;
                                $scope.taskStatusesGlobal = data.TaskStatuses;
                            }
                            else {
                                angular.forEach(data.TaskStatuses, function (value, index) {
                                    $scope.taskStatuses.push(value);
                                });
                            }
                            $scope.taskStatusVisibility.showFirst = !($scope.taskStatuses.length > 0);
                            $scope.taskStatusesTotalRecords = data.TotalRecords;
                        }
                    }
                }).error(function (error) {
                });
            }
        }

        $scope.assignTaskToApplicant = function (task, applicant, index, taskApplicantId) {
            $scope.AssignTask = {
                TaskId: task.Id,
                ApplicantId: applicant.ApplicantId,
                TaskApplicantId: applicant.TaskApplicantId
            };
            $http({
                method: 'POST',
                url: '/Requestor/AssignTask',
                data: {
                    assignTaskRequest: $scope.AssignTask
                },
                async: true,
                //headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data, status, headers, config) {
                var test = data;
                if (data != null && data > 0) {
                    $scope.applicantsCount--;
                    $scope.AssignedDivModel =
                        {
                            Visible: true,
                            AssigneeName: applicant.Name,
                            AssignedTask: task.ProjectName,
                            AssignedHours: task.Hours + " hrs",
                            StartingDate: task.OnBoardingDate
                        }
                    task.Status = "Assigned";
                    //refresh grid
                    $scope.refreshTasks();
                    $location.hash('divAssigned');
                }
                // handle success things
            }).error(function (data, status, headers, config) {
                // handle error things
            });
        }

        $scope.refreshApplicants = function () {
            $scope.taskApplicantsCount = 0;
            $scope.taskApplicants = null;
            $scope.getApplicants();
        }
        $scope.refreshTasks = function () {
            $scope.taskStatusCount = 0;
            $scope.taskStatuses = null;
            $scope.taskStatusVisibility.showFirst = true;
            $scope.workAgain = [];
            $scope.ratingValue = [];
            $scope.getStatusOftasks();
            $scope.deselectRating();
        }


        $scope.reviewApplicant = function (task, applicant, approvalApplicantId) {
            $http({
                method: 'POST',
                url: '/Requestor/ReviewTask',
                data: {
                    TaskId: task.Id,
                    ApplicantId: applicant.ApplicantId,
                    ApprovedApplicantId: approvalApplicantId
                },
                async: true,
            }).success(function (data, status, headers, config) {
                var test = data;
                if (data != null && data > 0) {

                    $scope.ReviewedDivModel =
                        {
                            Visible: true,
                            AssigneeName: applicant.Name,
                            AssignedTask: task.ProjectName,
                            AssignedHours: task.Hours + " hrs",
                            StartingDate: task.OnBoardingDate
                        }
                    task.Status = "Closed";
                    $scope.refreshTasks();
                    $scope.$broadcast('refresh');
                    //$location.hash('divReviewSuccess');
                }

            }).error(function (data, status, headers, config) {
                // handle error things
            });
        }

        //modal functions
        $scope.animationsEnabled = true;

        $scope.toggleAnimation = function () {
            $scope.animationsEnabled = !$scope.animationsEnabled;
        };

        $scope.onLoad = function () {
            //$scope.getApplicants();
            //$scope.getStatusOftasks();
        }
        $scope.onLoad();
    }
})();

(function () {
    'use strict';
    angular.module('dCodeApp')
    .controller('modalInstanceCtrl', ModalInstanceCtrl);

    ModalInstanceCtrl.$inject = ['$scope', '$uibModal'];

    function ModalInstanceCtrl($scope, $uibModalInstance, items, comments) {
        $scope.comments = $scope.$parent.comments;

        $scope.cancel = function () {
            $scope.$parent.modalInstance.close();
        };
    }
})();


(function () {
    'use strict';
    angular.module('dCodeApp')
    .controller('newTaskController', NewTaskController);

    NewTaskController.$inject = ['$scope', '$http', '$rootScope', '$filter', 'UserContextService'];

    function NewTaskController($scope, $http, $rootScope, $filter, UserContextService) {
        $scope.taskRequest =
            {
                ProjectName: "",
                OnBoardingDate: "",
                DueDate: "",
                WBSCode: "",
                TaskName: "",
                Hours: "",
                SkillSet: [],
                IsRewardsEnabled: false,
                ServiceLineDisplay: ""
            };
        $scope.showDetails = false;
        $scope.showSummary = false;
        $scope.divVisibiltyModel = { showCreate: false, showDetails: false, showSummary: false, showSuccess: false };
        $scope.skills = [];
        $scope.serviceLines = [];
        $scope.taskTypes = [];
        $scope.selectedSkills = [];
        $scope.selectedSkillDesc = null;
        $scope.selectedSkill = null;
        $scope.newSkillValue = null;
        $scope.successMessage = null;
        $scope.errorMessage = null;

        $scope.InitializeTaskRequest = function () {
            $scope.taskRequest =
          {
              ProjectName: "",
              OnBoardingDate: "",
              DueDate: "",
              WBSCode: "",
              TaskName: "",
              Hours: "",
              SkillSet: [],
              IsRewardsEnabled: "",
              SelectedServiceLine: '',
              SelectedTaskType: ''
          };
        }

        $scope.InitializeTaskRequest();

        $scope.resetSkillValues = function () {
            $scope.successMessage = null;
            $scope.errorMessage = null;
            $scope.newSkillValue = null;
        }

        $scope.insertSkill = function () {
            if ($scope.newSkillValue != null) {
                $http({
                    url: "/Common/InsertNewSkill",
                    method: "POST",
                    data: { skillValue: $scope.newSkillValue }
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null) {
                            $scope.successMessage = null;
                            $scope.errorMessage = null;
                            if (data == "Added Skill") {
                                $scope.successMessage = data;
                            }
                            else {
                                $scope.errorMessage = data;
                            }
                        }
                    }
                }).error(function (error) {
                });
            }
        }

        $scope.setSelectedSkillDesc = function () {
            angular.forEach($scope.skills, function (value, index) {
                if ($scope.taskRequest.SkillSet == value.Id) {
                    $scope.selectedSkillDesc = value.Value;
                }
            });
        }

        $scope.selectedSkill = function (value) {
            if (value != null && value.originalObject != null) {
                $("#taskSkill").removeClass("invalid");
                if ($scope.taskRequest == null) {
                    $scope.taskRequest = [];
                }
                if ($scope.taskRequest.SkillSet == null) {
                    $scope.taskRequest.SkillSet = [];
                    $scope.taskRequest.SkillSet = value.originalObject.Id;
                }
                else {
                    $scope.taskRequest.SkillSet = value.originalObject.Id;
                }
                $scope.selectedSkillDesc = value.originalObject.Value;
            }
        }

        $scope.onDateChange = function () {
            console.log($scope.taskRequest.OnBoardingDate);
        };
        $scope.test = function () {
            console.log($scope.taskRequest.DueDate);
        };
        $scope.showDetailsDiv = function () {
            $scope.divVisibiltyModel.showDetails = true;

            $scope.divVisibiltyModel.showCreate = false;
            $scope.divVisibiltyModel.showSummary = false;
            $scope.divVisibiltyModel.showSuccess = false;
            $scope.InitializeTaskRequest();
        };

        $('#txtHr').keydown(function (e) {
            var order = e.which;
            if (order == 187 || order == 189 || order == 69) {
                return false;
            };
        });

        $('#skillsetNewTask').keydown(function (e) {
            var order = e.which;
            $scope.taskRequest.SkillSet = null;
        });
        $('#txtStartDate').keydown(function (e) {
            var order = e.which;
            if (order != 9) {
                return false;
            };
        });
        $('#txtDueDate').keydown(function (e) {
            var order = e.which;
            if (order != 9) {
                return false;
            };
        });

        //removing invalid class on focusin for all the input fields
        $('#txtProjectName').focusout(function () {
            $("#divProjectName").removeClass("invalid");
        });
        $("#txtWBSCode").focusout(function () {
            $("#divWBSCode").removeClass("invalid");
        });
        $("#skillsetNewTask_value").focusout(function () {
            $("#taskSkill").removeClass("invalid");
        });

        $("#ddlServiceLine").change(function () {
            $("#divServiceLine").removeClass("invalid");
        });

        $("#ddlTaskType").change(function () {
            $("#ddlTaskType").removeClass("invalid");
        });

        $('#txtTaskName').focusout(function () {
            $("#divTaskName").removeClass("invalid");
        });

        $scope.RemoveStartDateValidation = function () {
            $("#datetimepicker1").removeClass("invalid");
        };
        $scope.RemoveDueDateValidation = function () {
            $("#datetimepicker2").removeClass("invalid");
        };
        $('#txtHr').focusout(function () {
            $("#divHours").removeClass("invalid");
        });


        //$scope.GetWBSValidation = function () {
        //    var regex = /^[a-zA-Z]{3,}[0-9]{5,}[-]{1,}[0-9]{2,}[-]{1,}[0-9]{2,}[-]{1,}[0-9]{4,}$/;
        //    var val = $("#txtWBSCode").val().toLocaleLowerCase();
        //    if (val.length == 19 && regex.test(val)) {
        //        if (val.substring(0, 3).indexOf("xyi") != -1 || val.substring(0, 3).indexOf("lpx") != -1 || val.substring(0, 3).indexOf("dci") != -1) {
        //            $("#divWBSCode").addClass("invalid");
        //            //return;
        //        }
        //        else {
        //            $("#divWBSCode").removeClass("invalid");
        //        }
        //    } else {
        //        $("#divWBSCode").addClass("invalid");
        //        //return;
        //    }
        //};

        $scope.ValidateNewTaskData = function () {
            var isValid = true;
            var focusSet = false;

            //validation project Name
            if ($('#txtProjectName').val() == '' || $('#txtProjectName').val() == null) {
                $("#divProjectName").addClass("invalid");
                $('#txtProjectName').focus();
                if (!focusSet)
                    focusSet = true;
                isValid = false;
            }

            //Skip wbs validation if task type is firm initiative
            if ($scope.taskRequest.SelectedTaskType != 2) {
                //validating WBS Code
                if ($("#txtWBSCode").val() == '' || $("#txtWBSCode").val() == null) {
                    $("#divWBSCode").addClass("invalid");

                    if (!focusSet) {
                        $('#txtWBSCode').focus();
                        focusSet = true;
                    }
                    isValid = false;
                }
                else {
                    var regex = /^[a-zA-Z]{3,}[0-9]{5,}[-]{1,}[0-9]{2,}[-]{1,}[0-9]{2,}[-]{1,}[0-9]{2,}[-]{1,}[0-9]{4,}$/;
                    var val = $("#txtWBSCode").val().toLocaleLowerCase();
                    if (val.length == 22 && regex.test(val)) {
                        if (val.substring(0, 3).indexOf("xyi") != -1 || val.substring(0, 3).indexOf("lpx") != -1 || val.substring(0, 3).indexOf("dci") != -1) {
                            $("#divWBSCode").addClass("invalid");
                            //return;
                            if (!focusSet) {
                                focusSet = true;
                                $('#txtWBSCode').focus();
                            }
                            isValid = false;
                        }
                        else {
                            $("#divWBSCode").removeClass("invalid");
                        }
                    } else {
                        $("#divWBSCode").addClass("invalid");
                        if (!focusSet) {
                            focusSet = true;
                            $('#txtWBSCode').focus();
                        }
                        isValid = false;
                        //return;
                    }
                }
            }

            //validating skill set
            if ($("#skillsetNewTask_value").val() == '' || $("#skillsetNewTask_value").val() == null) {
                $("#taskSkill").addClass("invalid");
                if (!focusSet) {
                    $('#skillsetNewTask_value').focus();
                    focusSet = true;
                }
                isValid = false;
            }

            //validating service Line
            if ($scope.taskRequest.SelectedServiceLine == null || $scope.taskRequest.SelectedServiceLine == "") {
                $("#divServiceLine").addClass("invalid");
                if (!focusSet) {
                    focusSet = true;
                    $('#ddlServiceLine').focus();
                }
                isValid = false;
                //$scope.ServiceLineValidation = false;
            }

            //validating task Type
            if ($scope.taskRequest.SelectedTaskType == null || $scope.taskRequest.SelectedTaskType == "") {
                $("#divTaskType").addClass("invalid");
                if (!focusSet) {
                    focusSet = true;
                    $('#ddlTaskType').focus();
                }
                isValid = false;
                //$scope.ServiceLineValidation = false;
            }

            //Validating Task Name
            if ($('#txtTaskName').val() == '' || $('#txtTaskName').val() == null) {
                $("#divTaskName").addClass("invalid");
                if (!focusSet) {
                    $('#txtTaskName').focus();
                    focusSet = true;
                }
                isValid = false;
            }

            //validating Start Date
            if ($('#txtStartDate').val() == '' || $('#txtStartDate').val() == null) {
                $("#datetimepicker1").addClass("invalid");
                if (!focusSet) {
                    focusSet = true;
                    $('#txtStartDate').focus();
                }
                isValid = false;
            }
            //validating Due Date
            if ($('#txtDueDate').val() == '' || $('#txtDueDate').val() == null) {
                $("#datetimepicker2").addClass("invalid");
                $("#spanInvalidDate").text("Invalid Due Date");

                if (!focusSet) {
                    focusSet = true;
                    $('#txtDueDate').focus();
                }
                isValid = false;
            }

            if (moment($('#txtStartDate').val()).isAfter($('#txtDueDate').val())) {
                $("#datetimepicker2").addClass("invalid");
                $("#spanInvalidDate").text("Due Date cannot be on or before Start Date");

                if (!focusSet) {
                    $('#txtDueDate').focus();
                    focusSet = true;
                }
                isValid = false;
            }
            //else {
            //    $("#datetimepicker2").removeClass("invalid");
            //    $scope.onBoardingDateReview = true;
            //}

            //validating if due date is more than 2 weeks
            //if ($('#datetimepicker2').attr('class').indexOf("invalid") == -1 ) {
            //    var one_day = 1000 * 60 * 60 * 24;
            //    var date1 = new Date($('#txtStartDate').val()).getTime();
            //    var date2 = new Date($('#txtDueDate').val()).getTime();
            //    var dateDiff = Math.round((date2 - date1) / one_day);
            //    if (dateDiff > 14) {
            //        $("#datetimepicker2").addClass("invalid");
            //        $("#spanInvalidDate").text("Due Date cannot be greater that 2 weeks");
            //        $scope.onBoardingDateReview = false;
            //        if (!focusSet) {
            //            $('#txtDueDate').focus();
            //            focusSet = true;
            //        }
            //        isValid = false;
            //    }

            //}

            //validating Hours
            if ($('#txtHr').val() == '' || $('#txtHr').val() == null) {
                $("#divHours").addClass("invalid");
                if (!focusSet) {
                    $('#txtHr').focus();
                    focusSet = true;
                }
                isValid = false;
            }
            return isValid;
        };

        $scope.reviewClick = function () {
            if ($scope.ValidateNewTaskData()) {
                //$("#spanInvalidDate").text("Due Date cannot be on or before On Boarding Date");
                if ($scope.taskRequest != null) {
                    if ($scope.taskRequest.ProjectName != null && $scope.taskRequest.ProjectName.length > 2) {
                        if ($scope.taskRequest.ProjectName.indexOf(" ") > 0) {
                            var split = $scope.taskRequest.ProjectName.split(" ");
                            $scope.taskRequest.ShortName = split[0].substring(0, 1) + split[1].substring(0, 1);
                        }
                        else {
                            $scope.taskRequest.ShortName = $scope.taskRequest.ProjectName.substring(0, 2);
                        }
                    }
                    else {
                        $scope.taskRequest.ShortName = $scope.taskRequest.ProjectName;
                    }

                    if (document.querySelector("#datetimepicker1 input").value != "") {
                        $scope.taskRequest.OnBoardingDate = document.querySelector("#datetimepicker1 input").value;
                    }
                    if (document.querySelector("#datetimepicker2 input").value != "") {
                        $scope.taskRequest.DueDate = document.querySelector("#datetimepicker2 input").value;
                    }
                    if ($scope.taskRequest != null && $scope.taskRequest.OnBoardingDate != "" && $scope.taskRequest.DueDate != "") {
                        $scope.onBoardingDateReview = true;

                        //if (moment($scope.taskRequest.OnBoardingDate).isAfter($scope.taskRequest.DueDate)) {
                        //    $("#datetimepicker2").addClass("invalid");
                        //} else {
                        //    $("#datetimepicker2").removeClass("invalid");
                        //    $scope.onBoardingDateReview = true;
                        //}

                        var selectedSkill = false;
                        if ($scope.taskRequest.SkillSet != null && $scope.taskRequest.SkillSet != "") {
                            selectedSkill = true;
                            //$("#taskSkill").removeClass("invalid");
                        }
                        else {
                            $("#taskSkill").addClass("invalid");
                            $('#skillsetNewTask_value').focus();
                        }

                        angular.forEach($scope.serviceLines, function (value, index) {
                            if ($scope.taskRequest.SelectedServiceLine == value.Id)
                                $scope.taskRequest.ServiceLineDisplay = value.Name;
                        });

                        //if ($('#datetimepicker2').attr('class').indexOf("invalid") == -1 && $scope.taskRequest.DueDate != null && $scope.taskRequest.OnBoardingDate != null) {
                        //    var one_day = 1000 * 60 * 60 * 24;
                        //    var date1 = new Date($scope.taskRequest.OnBoardingDate).getTime();
                        //    var date2 = new Date($scope.taskRequest.DueDate).getTime();
                        //    var dateDiff = Math.round((date2 - date1) / one_day);
                        //    if (dateDiff > 14) {
                        //        $("#datetimepicker2").addClass("invalid");
                        //        $("#spanInvalidDate").text("Due Date cannot be greater that 2 weeks");
                        //        $scope.onBoardingDateReview = false;
                        //    }

                        //}
                        //if ($("#txtHr").val().indexOf('.') > -1) {
                        //    //$("#spanHrsError").show();
                        //    $("#divHours").addClass("invalid");
                        //    $scope.HrsValidation = false;
                        //}
                        //else {
                        //    $("#divHours").removeClass("invalid");
                        //    //$("#spanHrsError").hide();
                        //    $scope.HrsValidation = true;
                        //}

                        //validation to check service line is selected
                        //if ($scope.taskRequest.SelectedServiceLine == null || $scope.taskRequest.SelectedServiceLine == "") {
                        //    $("#divServiceLine").addClass("invalid");
                        //    $scope.ServiceLineValidation = false;
                        //}
                        //else {
                        //    $("#divServiceLine").removeClass("invalid");
                        //    $scope.ServiceLineValidation = true;
                        //}

                        //$scope.GetWBSValidation();

                        var wbsCheckValue = (!!$scope.taskRequest.WBSCode && $scope.taskRequest.SelectedTaskType == 1)
                            || $scope.taskRequest.SelectedTaskType == 2;

                        var isvalid = !!$scope.taskRequest.ProjectName && wbsCheckValue && selectedSkill
                            && !!$scope.taskRequest.TaskName && !!$scope.taskRequest.DueDate && !!$scope.taskRequest.Hours
                            && $scope.taskRequest.OnBoardingDate && !!$scope.onBoardingDateReview;

                        //var isvalid = !!$scope.taskRequest.ProjectName && !!$scope.taskRequest.WBSCode && selectedSkill
                        //  && !!$scope.taskRequest.TaskName && !!$scope.taskRequest.DueDate && !!$scope.taskRequest.Hours //&& !!$scope.taskRequest.IsRewardsEnabled
                        //  && $scope.taskRequest.OnBoardingDate && !!$scope.onBoardingDateReview && $scope.HrsValidation && $scope.ServiceLineValidation;


                        if (isvalid) {
                            $scope.divVisibiltyModel.showSummary = true;
                            $scope.divVisibiltyModel.showDetails = false;
                            $scope.divVisibiltyModel.showCreate = false;
                            $scope.divVisibiltyModel.showSuccess = false;
                            $scope.onBoardingDateReviewUI = $filter('date')(new Date(document.querySelector("#datetimepicker1 input").value), 'MMM dd, yyyy');
                        }
                    }
                }
            }
        }

        $scope.navigateMyTasks = function () {
            location.href = "/Requestor/Dashboard";
        }

        $scope.cancelClick = function () {
            $scope.divVisibiltyModel.showCreate = true;

            $scope.divVisibiltyModel.showDetails = false;
            $scope.divVisibiltyModel.showSuccess = false;
            $scope.divVisibiltyModel.showSummary = false;
        };

        $scope.editClick = function () {
            $("#taskSkill").removeClass("invalid");
            $scope.divVisibiltyModel.showDetails = true;

            $scope.divVisibiltyModel.showSummary = false;
            $scope.divVisibiltyModel.showSuccess = false;
            $scope.divVisibiltyModel.showCreate = false;

        }

        $scope.successClick = function () {
            $scope.divVisibiltyModel.showSuccess = true;

            $scope.divVisibiltyModel.showDetails = false;
            $scope.divVisibiltyModel.showSummary = false;
            $scope.divVisibiltyModel.showCreate = false;
        }

        $scope.isFirstTimeUser = function () {
            var isFirstTimeUser = false;
            $http({
                url: "/Requestor/IsFirstTimeUserForNewTask",
                method: "GET",
            }).success(function (data, status, headers, config) {
                $scope.divVisibiltyModel.showCreate = data;
                $scope.divVisibiltyModel.showDetails = !data;
            }).error(function (error) {
            });
        };

        //$("#txtWBSCode").focusout(function () {
        //    $scope.GetWBSValidation();
        //});



        $scope.upsertTask = function () {
            $scope.taskRequest.GiftsOrAwards = $scope.taskRequest.IsRewardsEnabled == "true" ? true : false;
            $http({
                url: "/Task/UpsertTask",
                method: "POST",
                data: { taskRequest: $scope.taskRequest }
            }).success(function (data, status, headers, config) {
                var test = data;
                if (data != null) {
                    $scope.successClick();
                    //commented this to show the tast and project values in the success page.
                    //$scope.taskRequest = null;
                    $scope.$broadcast('angucomplete-alt:clearInput', 'skillsetNewTask');

                }
            }).error(function (error) {
            });
        }

        $scope.getAllSkills = function () {
            $http({
                url: "/Task/GetSkills",
                method: "Get",
            }).success(function (data, status, headers, config) {
                var test = data;
                if (data != null) {
                    $scope.skills = data;
                }
            }).error(function (error) {
            });
        }

        $scope.getAllServiceLines = function () {
            var reqObj = $scope.task;
            $http({
                url: "/Common/GetServiceLines",
                method: "GET"
            }).success(function (data, status, config) {

                if (data != null) {
                    $scope.serviceLines = data;
                }
            }).error(function (error) {
            });
        }

        $scope.getAllTaskTypes = function () {
            var reqObj = $scope.task;
            $http({
                url: "/Common/GetTaskTypes",
                method: "GET"
            }).success(function (data, status, config) {

                if (data != null) {
                    $scope.taskTypes = data;
                }
            }).error(function (error) {
            });
        }

        $scope.onLoad = function () {
            $scope.isFirstTimeUser();
            $scope.getAllSkills();
            $scope.getAllServiceLines();
            $scope.getAllTaskTypes();
        }
        $scope.onLoad();

        $scope.images = [1, 2, 3, 4, 5, 6, 7, 8];

        $scope.loadMore = function () {
            var last = $scope.images[$scope.images.length - 1];
            for (var i = 1; i <= 8; i++) {
                $scope.images.push(last + i);
            }
        };

    }
})();

(function () {
    'use strict';
    angular.module('dCodeApp')
    .controller('historyController', HistoryController);

    HistoryController.$inject = ['$scope', '$http', '$rootScope', 'UserContextService'];

    function HistoryController($scope, $http, $rootScope, UserContextService) {
        $scope.historyVisibility = { showFirst: true, showHistory: false };
        $scope.tasksHistory = null;
        $scope.taskHistoryPageIndex = 0;
        $scope.taskHistoryRecordsCount = 20;
        $scope.taskHistoryTotalRecords = null;

        $scope.NavigateToNewTask = function () {
            location.href = '/Requestor/NewTasks'
        }

        $scope.getTaskHistory = function () {
            if ($scope.tasksHistory == null || ($scope.tasksHistory.length < $scope.taskHistoryTotalRecords)) {
                $scope.taskHistoryPageIndex++;
                $http({
                    url: "/Requestor/GetTaskhistory?currentPageIndex=" + $scope.taskHistoryPageIndex + "&recordsCount=" + $scope.taskHistoryRecordsCount,
                    method: "GET"
                }).success(function (data, status, headers, config) {
                    var test = data;
                    if (data != null) {
                        if ($scope.tasksHistory == null) {
                            $scope.tasksHistory = data.TaskHistories;
                        }
                        else {
                            angular.forEach(data.TaskHistories, function (value, index) {
                                $scope.tasksHistory.push(value);
                            });
                        }
                        $scope.taskHistoryTotalRecords = data.TotalRecords;
                        if ($scope.tasksHistory.length > 0) {
                            $scope.historyVisibility.showHistory = true;
                            $scope.historyVisibility.showFirst = false;
                        }
                        else {
                            $scope.historyVisibility.showHistory = false;
                            $scope.historyVisibility.showFirst = true;
                        }
                    }
                }).error(function (error) {
                });
            }
        }

        $scope.$on('refresh', function (e) {
            $scope.tasksHistory = null;
            $scope.taskHistoryPageIndex = 0;
            $scope.taskHistoryRecordsCount = 20;
            $scope.taskHistoryTotalRecords = null;
            $scope.getTaskHistory();
        });
    }
})();


