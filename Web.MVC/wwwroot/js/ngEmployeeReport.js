﻿
    var app = angular.module('appReportEmployee', ['ngRoute', 'reportModule']);

    app.controller('controllerReportEmployee', ['reportService', function (reportService) {
        var me = this;
        this.title = "Department Report";
        this.mainTable = "Employees";
        this.mainTablePrimaryKey = "id";

        this.CriteriaFields = [];
        this.resultSet = [];
        this.resultCriteria = {}; //will be populated if there is a returned resultset
        this.selectedId = "";
        this.currentPage = 1;
        this.recordsPerPage = 3;

        this.init = function () {
            
            var field;
            field = CriteriaField( "default", "default");
            field.isDisplay = false;
            field.isGroupChecked = true;
            field.isOrderChecked = true;
            this.CriteriaFields.push(field);

            field = CriteriaField("Id", "Id");
            field.isValueFilter = false;
            this.CriteriaFields.push(field);

            field = CriteriaField("Name", "Name");
            field.isRangeFilter = false;
            this.CriteriaFields.push(field);

            field = CriteriaField("Address", "Address");
            field.isRangeFilter = false;
            this.CriteriaFields.push(field);

            field = CriteriaField("Experience", "Experience");
            field.isRangeFilter = false;
            this.CriteriaFields.push(field);

            field = CriteriaField("Level", "Level");
            field.isValueFilter = false;
            this.CriteriaFields.push(field);

            field = CriteriaField("Salary", "Salary");
            field.isValueFilter = false;
            this.CriteriaFields.push(field);
            reportService.init(this.title, this.mainTable, this.mainTablePrimaryKey, this.CriteriaFields);
        };

        //-- -------------------------------------------- --

        this.Criteria = function () {
            return reportService.Criteria();
        };
        //-- -------------------------------------------- --

        this.clickValueFilter = function (field) {
            reportService.clickValueFilter(field);
        };
        //-- -------------------------------------------- --

        this.clickGroupBy = function (field) {
            reportService.clickGroupBy(field);
        };
        //-- -------------------------------------------- --

        this.clickOrderBy = function (field) {
            reportService.clickOrderBy(field);
        };
        //-- -------------------------------------------- --

        this.resetGroupBy = function () {
            reportService.resetGroupBy();
        };
        //-- -------------------------------------------- --
        this.preview = function () {
            reportService.preview(me.populateResultset);
        };
        //-- -------------------------------------------- --
        this.select = function (id) {
            this.selectedId = reportService.select(id);
        };
        //-- -------------------------------------------- --

        this.not = function (boolValue) {
            return !boolValue;
        };
        //-- -------------------------------------------- --
        this.ShowInReportHeader = function(data) {
            return reportService.ShowInReportHeader(data);
        }
        //-- -------------------------------------------- --
        this.paginationChanged = function() {
            reportService.recordsPerPage = me.recordsPerPage;
            reportService.currentPage = me.currentPage;
        }
        this.populateResultset = function (obj) {
            me.resultSet = obj['Data'];
            me.resultCriteria = obj['ReportCriteria'];
        };



        //-- -------------------------------------------- --
        this.showCriteria = function () {
            reportService.callApi("GetCriteria", me.showInTestPane);
        };
        //-- -------------------------------------------- --
        this.showSql = function () {
            reportService.callApi("GetSql", me.showInTestPane);
        };
        //-- -------------------------------------------- --
        this.showXml = function () {
            reportService.callApi("GetReportsInXml", me.showInTestPane);
        };
        //-- -------------------------------------------- --
        this.showJson = function () {
            reportService.callApi("GetReports", me.showInTestPane);
        };
        //-- -------------------------------------------- --
        this.showReport = function () {
            reportService.callApi("GetReports", me.showAsReport);
        };
        //-- -------------------------------------------- --

        this.showInTestPane = function (data) {
            var s = JSON.stringify(data, null, 3);
            s = s.replace(/\\r\\n/g, "\n");
            $("#testPane").val(s);
        };
        //-- -------------------------------------------- --
        this.showAsReport = function (apiResponse) {

            me.resultCriteria = apiResponse.reportCriteria;

            if (apiResponse.status.ok) {
                var data = JSON.parse(apiResponse.data) ;
                if (Array.isArray(data.records.record)) {
                    me.resultSet = data.records;
                } else {
                    me.resultSet = { record: [] };
                    me.resultSet.record.push(data.records.record);
                }

            } else {

                ShowInfo("Api Error:", apiResponse.status.info);
            }

            var s = JSON.stringify(me.resultSet, null, 3);
            s = s.replace(/\\r\\n/g, "\n");
            $("#testPane").val(s);
        };
        //-- -------------------------------------------- --

      this.init();

    } ] );
