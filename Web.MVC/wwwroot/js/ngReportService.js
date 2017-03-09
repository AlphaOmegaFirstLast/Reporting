(function () {
    'use strict';
    var myModule = angular.module('reportModule', ['ngRoute']);

    myModule.service('reportService', function ($http) {
        this.criteriaFields = [];
        this.resultSet = {};
        this.title = "";
        this.mainTable = "";
        this.mainTablePrimaryKey = "";
        this.GroupBy = "default";
        this.OrderBy = "default";
        this.selectedId = "";
        this.currentPage = 1;
        this.recordsPerPage = 3;
        var me = this;   //use me to hold a reference to the service inside callbacks as "this" points to different objects in different contexts

        //-- -------------------------------------------- --

        this.init = function (title, mainTable, mainTablePrimaryKey, criteriaFields) {
            this.criteriaFields = criteriaFields;
            this.title = title;
            this.mainTable = mainTable;
            this.mainTablePrimaryKey = mainTablePrimaryKey;
        };
        //-- -------------------------------------------- --

        this.Criteria = function () {
            var objCriteria = { Title: "", mainTable: "", mainTablePrimaryKey: ""
                              , DisplayFields: [], ValueFilters: [], RangeFilters: [], GroupBy: [], OrderBy: []
                              , recordsPerPage:1 , currentPage:3
            };

            objCriteria.Title = this.title;
            objCriteria.mainTable = this.mainTable;
            objCriteria.mainTablePrimaryKey = this.mainTablePrimaryKey;
            objCriteria.recordsPerPage = this.recordsPerPage;
            objCriteria.currentPage = this.currentPage;

            var groupByField = { Field: this.GroupBy.field, Caption: this.GroupBy.caption };
            objCriteria.GroupBy.push(groupByField);

            var orderByField = { Field: this.OrderBy.field , Caption: this.OrderBy.caption};
            objCriteria.OrderBy.push(orderByField);

            $.each(this.criteriaFields, function (i, field) {
                if (field.isDisplayChecked) {
                    var displayField = { Field: field.field, Caption: field.caption };
                    objCriteria.DisplayFields.push(displayField);
                }
            });

            $.each(this.criteriaFields, function (i, field) {
                if (field.isValueFilter && field.isFilterChecked) {
                    var filter = { Field: field.field, Caption: field.caption, Value: field.filterControlValue };
                    objCriteria.ValueFilters.push(filter);
                }
            });

            $.each(this.criteriaFields, function (i, field) {
                if (field.isRangeFilter && field.isFilterChecked) {
                    var filter = { Field: field.field, Caption: field.caption, FromValue: field.filterControlFromValue, ToValue: field.filterControlToValue };
                    objCriteria.RangeFilters.push(filter);
                }
            });

            return objCriteria;
        };
        //-- -------------------------------------------- --

        this.clickValueFilter = function (field) {

            if (field.isFilterChecked) {

                field.isDisplayChecked = false;
                field.isDisplayDisabled = true;
                field.isGroupDisabled = true;
                field.isOrderDisabled = true;
            } else {
                field.isDisplayDisabled = false;
                field.isGroupDisabled = false;
                field.isOrderDisabled = false;
                field.filterControlValue = "";
            }
        };
        //-- -------------------------------------------- --

        this.clickGroupBy = function (field) {

            this.resetGroupBy();
            this.GroupBy = field;                             //in case function is called from other codes
            field.isGroupChecked = true;

            field.isDisplayChecked = false;
            field.isDisplayDisabled = true;

            field.isFilterChecked = false;
            field.isFilterDisabled = true;

            if (field.isOrderChecked) {                             //set order to default
                this.clickOrderBy(this.criteriaFields[0]);
            }
            field.isOrderChecked = false;
            field.isOrderDisabled = true;
        };
        //-- -------------------------------------------- --

        this.clickOrderBy = function (field) {
            this.OrderBy = field;                             //in case function is called from other codes
            field.isOrderChecked = true;
            field.isDisplayChecked = true;
        };
        //-- -------------------------------------------- --

        this.resetGroupBy = function () {
            $.each(this.criteriaFields, function (i, field) {
                if (field.isGroupChecked) {
                    field.isGroupChecked = false;
                    field.isFilterDisabled = false;
                    field.isDisplayDisabled = false;
                    field.isOrderDisabled = false;
                }
            });
        };
        //-- -------------------------------------------- --

        this.ShowInReportHeader = function(data) {   //used to show group by & order by in the report headers
            var isEmpty = data ? data.length === 0 : true;
            if (!isEmpty) {
                return (data[0].field !== "default");
            }
            return false;
        }
        //-- -------------------------------------------- --

        this.preview = function (callerResultset) {
            var req = {
                method: 'POST',
                url: apiPath + "getReports",
                headers: { 'Content-Type': 'application/json' },
                data: this.Criteria()
            }

            $http(req).then(function successCallback(response) {
                me.resultSet = response.data; // response.data["Data"];
                callerResultset(me.resultSet);
                ShowInfo("ngService.success= ", me.resultSet);
            }
            , function errorCallback(response) {
                httpErrorHandler(response);
            }
            );

            // this.resultSet = [{ id: "1", name: "Emp1" }, { id: "2", name: "Emp22" }, { id: "3", name: "Emp333" }];
            ShowInfo("ngService.return= ", me.resultSet);
            return me.resultSet;
        };
        //-- -------------------------------------------- --

        this.callApi = function (apiMethod,successFunc) {
            var req = {
                method: 'POST',
                url: apiPath + apiMethod,
                headers: { 'Content-Type': 'application/json' },
                data: this.Criteria()
            }

            $http(req).then(
                function successCallback(response) {
                    successFunc(response.data);
                }
              , function errorCallback(response) {
                  httpErrorHandler(response);
                }
            );

            return;
        };
        //-- -------------------------------------------- --

        this.select = function (id) {
            this.selectedId = id;
            return id;
        };
        //-- -------------------------------------------- --

    });

})();


