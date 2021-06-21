using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace FIS.AppClient.Utils
{
    public class BufferedResultManager : DataTable
    {
        private readonly int m_MaxPageSize = App.Environment.ClientInfo.UserProfile.MaxPageSize;

        public ModuleInfo ModuleInfo { get; private set; }
        public string LastSearchResultKey { get; private set; }
        public DateTime LastSearchTime { get; private set; }
        public List<ModuleFieldInfo> ColumnFields { get; private set; }

        private int m_BufferSize;
        public int BufferSize
        {
            get { return m_BufferSize; }
        }

        private int m_MaxPage;
        public int MaxPage
        {
            get
            {
                return m_MaxPage;
            }
        }

        private int m_MinPage;
        public int MinPage
        {
            get
            {
                return m_MinPage;
            }
        }

        private int m_StartRow;
        public int StartRow
        {
            get { return m_StartRow; }
        }

        public bool IsNotEmpty
        {
            get { return (Rows.Count > 0); }
        }

        public BufferedResultManager()
        {
        }

        public BufferedResultManager(string tableName) : base(tableName)
        {
        }

        public BufferedResultManager(ModuleInfo moduleInfo, string lastSearchResultKey, DateTime lastSearchTime)
        {
            ModuleInfo = moduleInfo;
            ColumnFields =
                FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleID,
                    CODES.DEFMODFLD.FLDGROUP.SEARCH_COLUMN
                );
            LastSearchResultKey = lastSearchResultKey;
            LastSearchTime = lastSearchTime;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (LastSearchResultKey != null)
                {
                    try
                    {
                        using (var ctrlSA = new SAController())
                        {
                            ctrlSA.DisposeSearchResult(ModuleInfo.ModuleID, ModuleInfo.SubModule, LastSearchResultKey, LastSearchTime);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void GetFullBuffer()
        {
            Clear();
            var fromRow = 0;
            using (var ctrlSA = new SAController())
            {
                DataTable tempTable;
                do
                {
                    DataContainer container;
                    ctrlSA.FetchAllSearchResult(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, LastSearchResultKey, LastSearchTime, fromRow);

                    if (Rows.Count == 0)
                    {
                        container.BuildSchema(this, ColumnFields);
                    }

                    tempTable = container.GetTable(ColumnFields);
                    if (tempTable.Rows.Count > 0)
                    {
                        fromRow = fromRow + tempTable.Rows.Count;
                        foreach (DataRow row in tempTable.Rows)
                        {
                            ImportRow(row);
                        }
                    }
                }
                while (tempTable.Rows.Count > 0);

                m_BufferSize = Rows.Count;
                AcceptChanges();
            }
        }

        public void GetBuffer(int selectedPage)
        {
            Clear();
            using (var ctrlSA = new SAController())
            {
                DataContainer container;
                ctrlSA.FetchSearchResult(
                    out container, out m_BufferSize,
                    out m_MinPage, out m_MaxPage, out m_StartRow,
                    ModuleInfo.ModuleID, ModuleInfo.SubModule,
                    LastSearchResultKey, LastSearchTime,
                    selectedPage, m_MaxPageSize);

                container.FillTable(this, ColumnFields);
            }
        }

        public void GetMoreRows(int selectedPage)
        {
            using (var ctrlSA = new SAController())
            {
                DataContainer container;
                ctrlSA.FetchSearchResult(
                    out container, out m_BufferSize,
                    out m_MinPage, out m_MaxPage, out m_StartRow,
                    ModuleInfo.ModuleID, ModuleInfo.SubModule,
                    LastSearchResultKey, LastSearchTime,
                    selectedPage, m_MaxPageSize);

                container.FillTable(this, ColumnFields);
            }
        }
    }
}
