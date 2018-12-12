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
        initializeTaskTypes();
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
                $("#projectNam").attr("placeholder", "Search by project/initiative name");
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
                TaskApplicantId: applicant.TaskApplicantId,
                TaskTypeId: task.TypeId
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

        $scope.closeTheTask = function (task, index) {
            $http({
                method: 'POST',
                url: '/Task/CloseTask',
                data: {
                    taskId: task.Task.Id
                },
                aync: true,
            }).success(function (task, tasks, index) {
                $scope.taskApplicants.splice(index, 1);
                $scope.getTaskHistory();
            })
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

        function initializeTaskTypes() {
            $scope.taskTypes = [{ Id: 1, Description: "Client Service" }, { Id: 2, Description: "Firm Initiative" }, { Id: 3, Description: "Industry Initiative" }];
        }
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
                Description: "",
                DueDate: "",
                WBSCode: "",
                TaskName: "",
                Hours: "",
                SkillSet: [],
                IsRewardsEnabled: false,
                OfferingDisplay: ""
            };
        $scope.showDetails = false;
        $scope.showSummary = false;
        $scope.divVisibiltyModel = { showCreate: false, showDetails: false, showSummary: false, showSuccess: false, showCreateODCTask: false };
        $scope.skills = [];
        $scope.serviceLines = [];
        $scope.offerings = [$scope.offerings = { PortfolioId: -1, OfferingId: -1, OfferingCode: "-- select --", DisplayName: "-- select --" }];
        $scope.portfolios = [];
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
                    Description: "",
                    DueDate: "",
                    WBSCode: "",
                    TaskName: "",
                    Hours: "",
                    SkillSet: [],
                    IsRewardsEnabled: "",
                    SelectedOffering: -1,
                    SelectedTaskType: -1,
                    SelectedSubOffering: -1
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
        //Summary
        //This function is responsible to load either the deloitte TechX fields or the ODC fields
        $scope.showDetailsDiv = function (hasODCAccess) {
            $scope.divVisibiltyModel.showDetails = true;
            $scope.hasODCAccess = hasODCAccess;
            if (hasODCAccess) {
                $scope.filterTaskTypesByODC();
            } else {
                $scope.getAllTaskTypes();
                $rootScope.logoImageName = "tech-x-logo.png";
            }
            $scope.divVisibiltyModel.showCreate = false;
            $scope.divVisibiltyModel.showSummary = false;
            $scope.divVisibiltyModel.showSuccess = false;
            $scope.InitializeTaskRequest();
        };

        //Summary
        //This function is called if the user is in ODC and clicks on the "Create New Task"
        $scope.filterTaskTypesByODC = function () {
            if ($scope.accessibleODCId) { //If ODC access is present
                //FInd the excluded list of TaskTypes.
                var exTaskTypes;
                angular.forEach($scope.ODCList, function (value, key) {
                    if (value.OfferingId == $scope.accessibleODCId) {
                        if (value.ExcludeTaskTypeList.indexOf(',') != -1) {
                            exTaskTypes = value.ExcludeTaskTypeList.split(',');
                        } else {
                            exTaskTypes = value.ExcludeTaskTypeList;
                        }
                        $rootScope.logoImageName = value.Logo;
                    }
                });

                //Change the tasktype dropdown model now
                $scope.taskTypes = $scope.taskTypes.filter(function (e) {
                    return this.indexOf(e.Id.toString()) == -1;
                }, exTaskTypes);
            }
        }

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
            if ($scope.taskRequest.SelectedTaskType < 2) {
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
                    var regex = /^[a-zA-Z]{3,}[0-9]{5,}[-]{1,}[0-9]{2,}[-]{1,}[0-9]{2,}[-][0-9]{2,}[-]{1,}[0-9]{4,}$/;
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
            if ($scope.taskRequest.SelectedTaskType < 2) {
                if ($("#skillsetNewTask_value").val() == '' || $("#skillsetNewTask_value").val() == null) {
                    $("#taskSkill").addClass("invalid");
                    if (!focusSet) {
                        $('#skillsetNewTask_value').focus();
                        focusSet = true;
                    }
                    isValid = false;
                }
            }

            //validating offering
            if ($scope.taskRequest.SelectedOffering == null || $scope.taskRequest.SelectedOffering <= 0) {
                $("#divServiceLine").addClass("invalid");
                if (!focusSet) {
                    focusSet = true;
                    $('#ddlServiceLine').focus();
                }
                isValid = false;
                //$scope.ServiceLineValidation = false;
            }

            if ($scope.divVisibiltyModel.showCreateODCTask && $scope.hasODCAccess) {
                if ($scope.taskRequest.SelectedSubOffering == null || $scope.taskRequest.SelectedSubOffering <= 0) {
                    $("#divSubOffering").addClass("invalid");
                    if (!focusSet) {
                        focusSet = true;
                        $('#ddlSubOffering').focus();
                    }
                    isValid = false;
                    $scope.isValidSubOffering = false;
                } else {
                    $scope.isValidSubOffering = true;
                }
            } else {
                $scope.taskRequest.SelectedSubOffering = null;
            }

            //validating task Type
            if ($scope.taskRequest.SelectedTaskType == null || $scope.taskRequest.SelectedTaskType <= 0) {
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
                        else if ($scope.taskRequest.SelectedTaskType != null && $scope.taskRequest.SelectedTaskType >= 2) {
                            $scope.taskRequest.SkillSet
                            selectedSkill = true;
                            //$("#taskSkill").removeClass("invalid");
                        }
                        else {
                            $("#taskSkill").addClass("invalid");
                            $('#skillsetNewTask_value').focus();
                        }

                        angular.forEach($scope.offerings, function (value, index) {
                            if ($scope.taskRequest.SelectedOffering == value.OfferingId)
                                $scope.taskRequest.OfferingDisplay = value.OfferingCode;
                        });



                        //$scope.GetWBSValidation();

                        var wbsCheckValue = (!!$scope.taskRequest.WBSCode && $scope.taskRequest.SelectedTaskType == 1)
                            || $scope.taskRequest.SelectedTaskType >= 2;

                        var isvalid = !!$scope.taskRequest.ProjectName && wbsCheckValue && selectedSkill
                            && !!$scope.taskRequest.TaskName && !!$scope.taskRequest.DueDate && !!$scope.taskRequest.Hours
                            && $scope.taskRequest.OnBoardingDate && !!$scope.onBoardingDateReview;


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

        $scope.isFirstTimeUser = function (hasODCAccess) {
            //If The requstor belongs to an ODC, then better to show 2 cards
            if (hasODCAccess) {
                $scope.divVisibiltyModel.showCreate = true; //data; TODO: check this.
                $scope.divVisibiltyModel.showDetails = false;//!data;
                return;
            }
            $http({
                url: "/Requestor/IsFirstTimeUserForNewTask",
                method: "GET",
            }).success(function (data, status, headers, config) {
                $scope.divVisibiltyModel.showCreate = data; //data; TODO: check this.
                $scope.divVisibiltyModel.showDetails = !data;//!data;
                
            }).error(function (error) {
            });
        };

        $scope.checkODCAccess = function () {
            $rootScope.$watch('userContext', function () {
                if ($rootScope.userContext != null) {
                    $scope.accessibleODCId = $rootScope.userContext.AccessibleODCId;
                    $scope.hasODCAccess = $rootScope.userContext.HasODCAccess;
                    $scope.divVisibiltyModel.showCreateODCTask = $scope.hasODCAccess;
                    $scope.showTaskCardsOrTaskDetails();
                }
            });
        }

        $scope.showTaskCardsOrTaskDetails = function () {
            $rootScope.logoImageName = "tech-x-logo.png"; //By deafult show the regular logo
            if ($scope.hasODCAccess) {
                $scope.getODCListAndShowCards();
            } else {
                $scope.isFirstTimeUser(false); //has no ODC access
            }
        }

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

        //Summary
        //This function is used to pull out all the subofferings by Offering
        $scope.getSubOfferingsByOfferingId = function (Id) {
            $http({
                url: "/Common/GetAllSubOfferings?offeringId=" + Id,
                method: "GET"
            }).success(function (data, status, config) {
                if (data != null) {
                    $scope.subOfferings = data;
                    $scope.subOfferings.unshift({ OfferingId: -1, Id: -1, Description: "-- select --" });
                    $scope.taskRequest.SelectedSubOffering = -1;
                }
            }).error(function (error) {
                $scope.subOfferings = [];
            });
           
        }
        //SUmmary
        //This function decides whether to show the cards or directly show the task details form
        $scope.getODCListAndShowCards = function () {
            $http({
                url: "/Common/GetODCList",
                method: "GET",
                async : false //intentionally kept
            }).success(function (data, status, config) {
                if (data != null) {
                    $scope.ODCList = data.ODCList;
                    for (var i = 0; i < data.ODCList.length; i++) {
                        if (data.ODCList[i].OfferingId == $scope.accessibleODCId) {
                            $scope.accessibleODCName = data.ODCList[i].Name;
                            $scope.showTechX = data.ODCList[i].ShowTechX;

                            if ($scope.showTechX == '0') {
                                $scope.showDetailsDiv(true);
                            } else {
                                $scope.isFirstTimeUser(true);
                            }
                            break;
                        }
                    }
                }
            }).error(function (error) {
                $scope.ODCList = [];
            });
            $scope.taskRequest.SelectedSubOffering = -1;
        }


        $scope.getAllOfferings = function (Id) {
            $http({
                url: "/Common/GetPortfolioOfferings?taskTypeId=" + Id,
                method: "GET"
            }).success(function (data, status, config) {

                if (data != null) {
                    $scope.offerings = data;
                    if ($scope.offerings != null) {
                        $scope.offerings.unshift({ PortfolioId: -1, OfferingId: -1, OfferingCode: "-- select --", DisplayName: "-- select --" });
                        //Only if ODC access is present and the task is created from ODC section
                        if ($scope.divVisibiltyModel.showCreateODCTask && $scope.hasODCAccess) {
                            $scope.taskRequest.SelectedOffering = $scope.accessibleODCId;
                            $scope.getSubOfferingsByOfferingId($scope.accessibleODCId);
                        }
                    }
                }
            }).error(function (error) {
                $scope.offerings = { PortfolioId: -1, OfferingId: -1, OfferingCode: "-- select --", DisplayName: "-- select --" }
            });
        }

        $scope.getAllTaskTypes = function () {
            var reqObj = $scope.task;
            $http({
                url: "/Common/GetTaskTypes",
                method: "GET",
                async:false
            }).success(function (data, status, config) {

                if (data != null) {
                    $scope.taskTypes = data;
                    if ($scope.taskTypes != null)
                        $scope.taskTypes.unshift({ Id: -1, Description: "-- select --" });
                }
            }).error(function (error) {
                $scope.taskTypes = { Id: -1, Description: "-- select --" };
            });
        }

        $scope.onLoad = function () {
            $scope.getAllSkills();
            $scope.getAllTaskTypes();
            $scope.checkODCAccess();
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


