using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Bepos.Reporting.Engine.Interfaces;
using Bepos.Reporting.Engine.Models;

namespace Bepos.Reporting.Engine
{
    public class ReportEngine : IReportEngine
    {
        private readonly ReportContext _reportContext;

        public ReportEngine(ReportContext reportingContext)
        {
            _reportContext = reportingContext;
        }

        public async Task<T> ExecuteQuery<T>(string sql, params object[] parameters)
        {
            return await _reportContext.SqlQuerySerialized<T>(sql, parameters);
        }

        public BuildQueryResult BuildQuery(ReportCriteria criteria, bool onlyRecordsCount = false)
        {
            var sqlStatement = new StringBuilder();
            var queryParametersList = new List<Object>();
            //Select -------------------------------------------------

            if (onlyRecordsCount)
            {
                sqlStatement.Append(" Select Count(*) ");
            }
            else
            {
                var selectFields = GetCriteriaSelect(criteria);
                if (!string.IsNullOrEmpty(selectFields))
                {
                    sqlStatement.Append(" Select ");
                    sqlStatement.AppendLine(selectFields);
                }
            }

            //From ---------------------------------------------------

            sqlStatement.AppendLine(" From ");
            sqlStatement.AppendLine(criteria.MainTable);

            //Where --------------------------------------------------

            var whereFields = GetCriteriaWhere(criteria, ref queryParametersList);
            if (!string.IsNullOrEmpty(whereFields))
            {
                sqlStatement.AppendLine(" Where ");
                sqlStatement.Append(whereFields);
            }

            if (!onlyRecordsCount)
            {
                //Group By ------------------------------------------------

                var groupFields = GetCriteriaGroup(criteria);
                if (!string.IsNullOrEmpty(groupFields))
                {
                    sqlStatement.AppendLine(" Order By ");
                    sqlStatement.Append(groupFields);
                }

                //Order By ------------------------------------------------

                var orderFields = GetCriteriaOrder(criteria);
                if (!string.IsNullOrEmpty(orderFields))
                {
                    sqlStatement.AppendLine(string.IsNullOrEmpty(groupFields) ? " Order By " : " , ");
                    sqlStatement.Append(orderFields);
                }
                sqlStatement.AppendLine(string.IsNullOrEmpty(groupFields) && string.IsNullOrEmpty(orderFields) ? $" Order By {criteria.MainTablePrimaryKey} " : "");

                //Skip/Take Rows ------------------------------------------

                var currentPage = criteria.CurrentPage <= 0 ? 1 : criteria.CurrentPage;
                var skip = (currentPage - 1) * criteria.RecordsPerPage;
                var take = criteria.RecordsPerPage;
                sqlStatement.AppendLine($" Offset {skip} Rows");
                sqlStatement.AppendLine($" Fetch Next {take} Rows Only");
                sqlStatement.AppendLine($" FOR XML RAW ('record'), ROOT ('records')");
            }

            return new BuildQueryResult() { SqlStatement = sqlStatement.ToString(), SqlParameters = queryParametersList.ToArray() };
        }

        public async Task<PaginationInfo> GetPaginationInfo(ReportCriteria criteria)
        {
            var resultCriteria = BuildQuery(criteria, true);
            var resultQuery = await ExecuteQuery<int>(resultCriteria.SqlStatement, resultCriteria.SqlParameters);
            var paginationInfo = new PaginationInfo();
            paginationInfo.TotalRecCount = resultQuery;
            paginationInfo.TotalPageCount = (int) Math.Ceiling ((double)paginationInfo.TotalRecCount / criteria.RecordsPerPage) ;
            paginationInfo.CurrentPage = criteria.CurrentPage;
            return paginationInfo;
        }

        private string GetCriteriaSelect(ReportCriteria criteria)
        {
            var sbSelectFields = new StringBuilder();
            var commaPlaceHolder = " ";
            foreach (var displayField in criteria.DisplayFields)
            {
                sbSelectFields.Append(commaPlaceHolder);
                sbSelectFields.Append(displayField.Field);
                commaPlaceHolder = ", ";
            }
            return sbSelectFields.ToString();
        }

        private string GetCriteriaWhere(ReportCriteria criteria, ref List<object> queryParametersList)
        {
            var sbWhereFields = new StringBuilder();
            var commaPlaceHolder = " ";
            foreach (var valueFilter in criteria.ValueFilters)
            {
                sbWhereFields.Append(commaPlaceHolder);
                sbWhereFields.Append($"{valueFilter.Field} = @p_{valueFilter.Field}");
                commaPlaceHolder = " and ";
                queryParametersList.Add(new SqlParameter($"p_{valueFilter.Field}", valueFilter.Value));
            }

            foreach (var rangeFilter in criteria.RangeFilters)
            {
                sbWhereFields.Append(commaPlaceHolder);
                sbWhereFields.Append($" {rangeFilter.Field} >= {rangeFilter.FromValue} and {rangeFilter.Field} <= {rangeFilter.ToValue} ");
                commaPlaceHolder = " and ";
            }

            return sbWhereFields.ToString();
        }

        private string GetCriteriaGroup(ReportCriteria criteria)
        {

            var sbGroupFields = new StringBuilder();
            var commaPlaceHolder = " ";
            foreach (var groupField in criteria.GroupBy)
            {
                if (groupField.Field == "default") continue;

                sbGroupFields.Append(commaPlaceHolder);
                sbGroupFields.Append(groupField.Field);
                commaPlaceHolder = ", ";
            }
            return sbGroupFields.ToString();
        }

        private string GetCriteriaOrder(ReportCriteria criteria)
        {
            var sbOrderFields = new StringBuilder();
            var commaPlaceHolder = " ";
            foreach (var orderField in criteria.OrderBy)
            {
                if (orderField.Field == "default") continue;
                sbOrderFields.Append(commaPlaceHolder);
                sbOrderFields.Append(orderField.Field);
                commaPlaceHolder = ", ";
            }
            return sbOrderFields.ToString();
        }
    }
}

