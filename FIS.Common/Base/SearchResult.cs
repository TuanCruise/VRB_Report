using FIS.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace FIS.Base
{
    public class SearchResult : IDisposable
    {
        private DataTable m_TableCachedResult;
        private OracleDataReader m_AdapDataReader;
        private OracleDataReader m_AdapDataReader2;
        public string SessionKey { get; set; }
        public string SearchKey { get; set; }
        public DateTime TimeSearch { get; set; }
        public DataTable CachedResult
        {
            get
            {
                return m_TableCachedResult;
            }
            set
            {
                m_TableCachedResult = value;
                if (m_TableCachedResult != null) IsBufferMode = true;
            }
        }
        public OracleDataReader DataReader
        {
            get
            {
                return m_AdapDataReader;
            }
            set
            {
                m_AdapDataReader = value;
                var field = typeof(OracleDataReader).GetField("m_rowSize", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    var rowSize = (long)field.GetValue(value);
                    if (rowSize > 0)
                    {
                        value.FetchSize = rowSize * CONSTANTS.MAX_ROWS_IN_BUFFER;
                    }
                }

                CachedResult = new DataTable("RESULT");
                var tblSchema = m_AdapDataReader.GetSchemaTable();
                if (tblSchema != null)
                {
                    foreach (DataRow rowSchema in tblSchema.Rows)
                    {
                        CachedResult.Columns.Add((string)rowSchema["ColumnName"], (Type)rowSchema["DataType"]);
                    }
                    if (m_AdapDataReader != null) IsBufferMode = false;
                }
            }
        }
        public OracleDataReader DataReader2
        {
            get
            {
                return m_AdapDataReader2;
            }
            set
            {
                m_AdapDataReader2 = value;

                var field = typeof(OracleDataReader).GetField("m_rowSize", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    var rowSize = (long)field.GetValue(value);
                    if (rowSize > 0)
                    {
                        value.FetchSize = rowSize * CONSTANTS.MAX_ROWS_IN_BUFFER;
                    }
                }

                var tblSchema = m_AdapDataReader2.GetSchemaTable();
                if (tblSchema != null)
                {
                    foreach (DataRow rowSchema in tblSchema.Rows)
                    {
                        if (!CachedResult.Columns.Contains((string)rowSchema["ColumnName"]))
                            CachedResult.Columns.Add((string)rowSchema["ColumnName"], (Type)rowSchema["DataType"]);
                    }
                    if (m_AdapDataReader2 != null) IsBufferMode = false;
                }
            }
        }
        public IDbConnection DBConnection { get; set; }
        public bool IsBufferMode { get; set; }

        public void BufferData(int highestRow)
        {
            if (!IsBufferMode)
            {
                var isStop = false;
                while (CachedResult.Rows.Count < highestRow && !isStop)
                {
                    isStop = true;
                    DataRow row = null;
                    if (DataReader != null && DataReader.Read())
                    {
                        row = CachedResult.NewRow();
                        CachedResult.Rows.Add(row);

                        var count = DataReader.FieldCount;
                        for (var i = 0; i < count; i++)
                        {
                            row[DataReader.GetName(i)] = DataReader.GetValue(i);
                        }

                        isStop = false;
                    }

                    if (DataReader2 != null && DataReader2.Read())
                    {
                        if (row == null)
                        {
                            row = CachedResult.NewRow();
                            CachedResult.Rows.Add(row);
                        }

                        var count = DataReader2.FieldCount;
                        for (var i = 0; i < count; i++)
                        {
                            row[DataReader2.GetName(i)] = DataReader2.GetValue(i);
                        }

                        isStop = false;
                    }
                }
            }
        }

        public DataTable GetSearchResult(int startRow, int rowCount)
        {
            return CachedResult.Rows.OfType<DataRow>()
                .Skip(startRow)
                .Take(rowCount)
                .CopyToDataTable();
        }

        #region IDisposable Members

        public void Dispose()
        {
            SearchKey = null;
            if (IsBufferMode)
                CachedResult.Dispose();
            else
            {
                DBConnection.Dispose();
                DataReader.Dispose();
                if (DataReader2 != null) DataReader2.Dispose();
            }
        }

        #endregion
    }
}
