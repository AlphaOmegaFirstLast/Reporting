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

            var fromClause = GetCriteriaFromClause(criteria);
            sqlStatement.AppendLine(fromClause);

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
                sqlStatement.AppendLine(string.IsNullOrEmpty(groupFields) && string.IsNullOrEmpty(orderFields) ? $" Order By [{criteria.MainTable}].[{criteria.MainTablePrimaryKey}] " : "");

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
            paginationInfo.TotalPageCount = (int)Math.Ceiling((double)paginationInfo.TotalRecCount / criteria.RecordsPerPage);
            paginationInfo.CurrentPage = criteria.CurrentPage;
            return paginationInfo;
        }

        private string GetCriteriaSelect(ReportCriteria criteria)
        {
            var sbSelectFields = new StringBuilder();
            var commaPlaceHolder = string.Empty;
            foreach (var displayField in criteria.DisplayFields)
            {
                if (displayField.Field == "default") continue;
                sbSelectFields.Append(commaPlaceHolder);
                if (!string.IsNullOrEmpty(displayField.LookupTable) && !string.IsNullOrEmpty(displayField.LookupField))
                    sbSelectFields.Append($"[{displayField.LookupTable}].[{displayField.LookupField}] as [{displayField.Caption}]");
                else
                    sbSelectFields.Append($"[{criteria.MainTable}].[{displayField.Field}] as [{displayField.Caption}]");

                commaPlaceHolder = ", ";
            }
            return sbSelectFields.ToString();
        }

        private string GetCriteriaFromClause(ReportCriteria criteria)
        {
            var sbFromClause = new StringBuilder();
            sbFromClause.AppendLine(" From ");
            sbFromClause.AppendLine(criteria.MainTable);

            //to avoid adding the joined table more than once, in the case more than lookupfield required fronm the same lookup table
            var joinClauseList = new List<string>();
            foreach (var field in criteria.DisplayFields)
            {
                if (!string.IsNullOrEmpty(field.LookupTable) && !string.IsNullOrEmpty(field.LookupFieldValue))
                {
                    var joinClause = $" Left Join [{field.LookupTable}] on [{field.LookupTable}].[{field.LookupFieldValue}] = [{criteria.MainTable}].[{field.Field}] ";
                    if (!joinClauseList.Contains(joinClause))
                        joinClauseList.Add(joinClause);
                }
            }
            //----------------------------------------------------------
            foreach (var filter in criteria.ValueFilters)
            {
                if (!string.IsNullOrEmpty(filter.LookupTable) && !string.IsNullOrEmpty(filter.LookupFieldValue))
                {
                    // naming confusion ...need to be revised... filTer.LookupField & field.LookupField
                    var joinClause = $" Left Join [{filter.LookupTable}] on [{filter.LookupTable}].[{filter.LookupFieldValue}] = [{criteria.MainTable}].[{filter.Field}] ";
                    if (!joinClauseList.Contains(joinClause))
                        joinClauseList.Add(joinClause);
                }
            }
            //----------------------------------------------------------
            foreach (var joinClause in joinClauseList)
            {
                sbFromClause.AppendLine(joinClause);
            }
            return sbFromClause.ToString();
        }

        private string GetCriteriaWhere(ReportCriteria criteria, ref List<object> queryParametersList)
        {
            var sbWhereFields = new StringBuilder();
            var commaPlaceHolder = string.Empty;
            foreach (var valueFilter in criteria.ValueFilters)
            {
                sbWhereFields.Append(commaPlaceHolder);
                sbWhereFields.Append($" {criteria.MainTable}.{valueFilter.Field} = @p_{valueFilter.Field}");
                commaPlaceHolder = " and ";
                queryParametersList.Add(new SqlParameter($"p_{valueFilter.Field}", valueFilter.Value));
            }

            foreach (var rangeFilter in criteria.RangeFilters)
            {
                sbWhereFields.Append(commaPlaceHolder);
                sbWhereFields.Append($"  {criteria.MainTable}.{rangeFilter.Field} >= {rangeFilter.FromValue} and {criteria.MainTable}.{rangeFilter.Field} <= {rangeFilter.ToValue} ");
                commaPlaceHolder = " and ";
            }

            return sbWhereFields.ToString();
        }

        private string GetCriteriaGroup(ReportCriteria criteria)
        {
            var sbGroupFields = new StringBuilder();
            var commaPlaceHolder = string.Empty;
            foreach (var groupField in criteria.GroupBy)
            {
              if (!string.IsNullOrEmpty(groupField.Field))
              {
                if (groupField.Field == "default") continue;

                  sbGroupFields.Append(commaPlaceHolder);
                  if (!string.IsNullOrEmpty(groupField.LookupTable) && !string.IsNullOrEmpty(groupField.LookupField))
                    sbGroupFields.Append($"[{groupField.LookupTable}].[{groupField.LookupField}]");
                  else
                    sbGroupFields.Append(groupField.Field);

                  commaPlaceHolder = ", ";
              }
            }
            return sbGroupFields.ToString();
        }

        private string GetCriteriaOrder(ReportCriteria criteria)
        {
            var sbOrderFields = new StringBuilder();
            var commaPlaceHolder = string.Empty;
            foreach (var orderField in criteria.OrderBy)
            {
              if (!string.IsNullOrEmpty(orderField.Field))
              {
                if (orderField.Field == "default") continue;

                sbOrderFields.Append(commaPlaceHolder);
                if (!string.IsNullOrEmpty(orderField.LookupTable) && !string.IsNullOrEmpty(orderField.LookupField))
                  sbOrderFields.Append($"[{orderField.LookupTable}].[{orderField.LookupField}]");
                else
                  sbOrderFields.Append($"[{criteria.MainTable}].[{orderField.Field}]");

                commaPlaceHolder = ", ";
              }
            }
            return sbOrderFields.ToString();
        }
    }
}

