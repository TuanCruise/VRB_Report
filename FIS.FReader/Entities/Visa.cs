using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIS.FReader.Entities
{
    class Visa : FileToRead
    {
        private List<Report> listReport = new List<Report>();

        public List<Report> ListReport
        {
            get { return listReport; }
            set { listReport = value; }
        }
        private List<Report_110> listReport_110 = new List<Report_110>();
        public List<Report_110> ListReport_110
        {
            get { return listReport_110; }
            set { listReport_110 = value; }
        }
        private List<Report_120> listReport_120 = new List<Report_120>();
        public List<Report_120> ListReport_120
        {
            get { return listReport_120; }
            set { listReport_120 = value; }
        }
        private List<Report_130> listReport_130 = new List<Report_130>();
        public List<Report_130> ListReport_130
        {
            get { return listReport_130; }
            set { listReport_130 = value; }
        }
        private List<Report_140> listReport_140 = new List<Report_140>();
        public List<Report_140> ListReport_140
        {
            get { return listReport_140; }
            set { listReport_140 = value; }
        }
        private List<Report_210> listReport_210 = new List<Report_210>();

        public List<Report_210> ListReport_210
        {
            get { return listReport_210; }
            set { listReport_210 = value; }
        }
    }


    public class Report
    {
        private String reportDate;
        public String ReportDate
        {
            get { return reportDate; }
            set { reportDate = value; }
        }
        private String procDate;

        public String ProcDate
        {
            get { return procDate; }
            set { procDate = value; }
        }
        private String reportID;

        public String ReportID
        {
            get { return reportID; }
            set { reportID = value; }
        }
        private String reportFor;

        public String ReportFor
        {
            get { return reportFor; }
            set { reportFor = value; }
        }
        private String rollUp;

        public String RollUp
        {
            get { return rollUp; }
            set { rollUp = value; }
        }
        private String reportingFor;

        public String ReportingFor
        {
            get { return reportingFor; }
            set { reportingFor = value; }
        }
        private String funds;

        public String Funds
        {
            get { return funds; }
            set { funds = value; }
        }
        private String settlementCurrency;

        public String SettlementCurrency
        {
            get { return settlementCurrency; }
            set { settlementCurrency = value; }
        }

        private String clearingCurrency;

        public String ClearingCurrency
        {
            get { return clearingCurrency; }
            set { clearingCurrency = value; }
        }

    }


    #region Report_110
    public class Report_110 : Report
    {
        private List<Detail> listDetail = new List<Detail>();

        public List<Detail> ListDetail
        {
            get { return listDetail; }
            set { listDetail = value; }
        }

    }
    #endregion
    public class Report_120 : Report
    {
        private List<Group> listGroup = new List<Group>();

        public List<Group> ListGroup
        {
            get { return listGroup; }
            set { listGroup = value; }
        }
    }
    public class Group
    {
        private String groupName;

        public String GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }
        private List<Detail> listDetail = new List<Detail>();

        public List<Detail> ListDetail
        {
            get { return listDetail; }
            set { listDetail = value; }
        }
    }

    //public class Acquirer
    //{
    //    public String Name = "ACQUIRER TRANSACTIONS";
    //    private List<Detail> listDetail = new List<Detail>();

    //    public List<Detail> ListDetail
    //    {
    //        get { return listDetail; }
    //        set { listDetail = value; }
    //    }
    //}
    //public class Issuser
    //{
    //    public String Name = "ISSUER TRANSACTIONS";
    //    private List<Detail> listDetail = new List<Detail>();

    //    public List<Detail> ListDetail
    //    {
    //        get { return listDetail; }
    //        set { listDetail = value; }
    //    }
    //}
    public class Detail
    {
        private String colum1;

        public String Colum1
        {
            get { return colum1; }
            set { colum1 = value; }
        }

        private String colum2;

        public String Colum2
        {
            get { return colum2; }
            set { colum2 = value; }
        }
        private String colum3;

        public String Colum3
        {
            get { return colum3; }
            set { colum3 = value; }
        }
        private String colum4;

        public String Colum4
        {
            get { return colum4; }
            set { colum4 = value; }
        }
        private String colum5;

        public String Colum5
        {
            get { return colum5; }
            set { colum5 = value; }
        }
        private String colum6;

        public String Colum6
        {
            get { return colum6; }
            set { colum6 = value; }
        }
    }



    public class Report_130 : Report
    {
        private List<Group> listGroup = new List<Group>();

        public List<Group> ListGroup
        {
            get { return listGroup; }
            set { listGroup = value; }
        }
    }
    public class Report_140 : Report
    {
        private List<Group> listGroup = new List<Group>();

        public List<Group> ListGroup
        {
            get { return listGroup; }
            set { listGroup = value; }
        }
    }
    public class Report_210 : Report
    {
        private List<Group> listGroup = new List<Group>();

        public List<Group> ListGroup
        {
            get { return listGroup; }
            set { listGroup = value; }
        }
    }
    public class Report_900 : Report
    {

    }

}
