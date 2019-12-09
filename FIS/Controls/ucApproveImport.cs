using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using FIS.Utils;
using FIS.Controllers;
using FIS.Entities;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.Extensions;
using FIS.Common;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.StyleFormatConditions;

namespace FIS.AppClient.Controls
{
    public partial class ucApproveImport : ucModule, IParameterFieldSupportedModule
    {
        public int moduleid; 
        public ucApproveImport()
        {
            InitializeComponent();
        }

    }   
}
