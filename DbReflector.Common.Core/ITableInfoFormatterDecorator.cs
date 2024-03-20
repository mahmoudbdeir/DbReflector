using System;

namespace DbReflector.Common
{
    public interface ITableInfoFormatterDecorator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>For table Person: PersonId [pk], FirstName [not null], MiddleName [null], DepartmentId [fk, not null], GroupId [fk, null]</returns>
        string CsvColumns { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>FirstName [not null],MiddleName [null],DepartmentId [fk, not null],GroupId [fk, null]</returns>
        string CsvColumnsNoPk { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>PersonId [pk], FirstName [not null], MiddleName [null], DepartmentId [fk, not null], GroupId [fk, null]</returns>
        string CsvColumnsWithAt { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>@FirstName [not null],@MiddleName [null],@DepartmentId [fk, not null],@GroupId [fk, null]</returns>
        string CsvColumnsNoPkWithAt { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>PersonId [pk] INT, FirstName [not null] VARCHAR(50), MiddleName [null] VARCHAR(50), DepartmentId [fk, not null] INT, GroupId [fk, null] INT</returns>
        string CsvColumnsWithAtAndSqlType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>@FirstName [not null] VARCHAR(50), @MiddleName [null] VARCHAR(50), @DepartmentId [fk, not null] INT, @GroupId [fk, null] INT</returns>
        string CsvColumnsNoPkWithAtAndSqlType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>@PersonId PersonId, @FirstName FirstName, @MiddleName MiddleName, @DepartmentId DepartmentId, @GroupId GroupId</returns>
        string CsvColumnsWithAtAndAlias { get; }

        string CSharpFuncParams { get; }

        string CSharpFuncInvokeParams { get; }

        string CSharpEntityNameCamelCase { get; }
    }
}