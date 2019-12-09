using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIS.FReader.Utilities
{
    public class Constant
    {
        public class FeeOverduePayment
        {
            public class ReportConstant
            {
                public const string REPORTDATE = "Report Date:";
                public const string REPORTCREATIONTIME = "Report Creation Time:";
                public const string CURRENCY = "Currency: ";
                public const string CONTRACTTYPE = "Contract Type";
                public const string TOTAL = "|Total ";
                public const string ROWSEPARATE = "--+--";

                public const char COLUMNSEPARATE = '|';
                public const char COLON = ':';
                public const char SEPARATEDASH = '-';
                public const char SPACE = ' ';
            }
        }

        public class ContractOverduePayment
        {
            public class ReportConstant
            {
                public const string REPORTDATE = "Report Date:";
                public const string REPORTCREATIONTIME = "Report Creation Time:";
                public const string CURRENCY = "Currency: ";
                public const string CONTRACTTYPE = "Contract Type";
                public const string TOTAL = "|Total ";
                public const string ROWSEPARATE = "+--------------------+--------------------+--------+-------------------+-------------+";
                public const string TABLEHEADER = "|      Contract      |      Account       |Currency|  Min Payment Due  | Due Period  |";

                public const char COLUMNSEPARATE = '|';
                public const char COLON = ':';
                public const char SEPARATEDASH = '-';
                public const char SPACE = ' ';
            }
        }

        public class DebtAutomation
        {
            public class ReportConstant
            {
                public const int COLUMN_1_START = 0;
                public const int COLUMN_1_LENGTH = 25;
                public const int COLUMN_2_START = 25;
                public const int COLUMN_2_LENGTH = 30;
                public const int COLUMN_3_START = 55;
                public const int COLUMN_3_LENGTH = 20;
                public const int COLUMN_4_START = 75;
                public const int COLUMN_4_LENGTH = 20;
                public const int COLUMN_5_START = 95;
                public const int COLUMN_5_LENGTH = 2;

                public const int COLUMN_6_START = 97;
                public const int COLUMN_6_LENGTH = 3;
                public const int COLUMN_7_START = 100;
                public const int COLUMN_7_LENGTH = 12;
                public const int COLUMN_8_START = 112;
                public const int COLUMN_8_LENGTH = 1;
                public const int COLUMN_9_START = 113;
                public const int COLUMN_9_LENGTH = 8;
                public const int COLUMN_10_START = 121;
                public const int COLUMN_10_LENGTH = 0;
            }
        }

        public class AccruedCreditInterest
        {
            public class ReportConstant
            {
                public const string REPORTCREATIONTIME = "Report creation date ";
                public const string CURRENCY = "Currency: ";
                public const string CONTRACTTYPE = "Contract Type";
                public const string CONTRACTTYPE1 = "Contract type:";
                public const string CONTRACTTYPETOTAL = "Contract type total:";
                public const string CURRENCYTOTAL = "|Currency total:";

                public const char COLUMNSEPARATE = '|';
                public const char COLON = ':';
                public const char SEPARATEDASH = '-';
                public const char SPACE = ' ';

                public const string IGNORE_ROW_1 = "|--------------------|----------------------------------------|----------------|";
                public const string IGNORE_ROW_2 = "|    Contract no     |              Client name               |    Interests   |";
             }
        }

        public class Loan
        {
            public class ReportConstant
            {
                public const string REPORTCREATIONTIME = "Report Creation Time:";
                public const string ACCT = "Acct";

                public const char COLUMNSEPARATE = '|';
                public const char COLON = ':';
                public const char SEPARATEDASH = '-';
                public const char SPACE = ' ';

                public const string IGNORE_ROW_1 = "+--------------------+----------------------------------------+----------------------------------------+---------------+-------------------------+";
                public const string IGNORE_ROW_2 = "|      Acct No       |              Branch Part               |                  Name                  |  Passport No  |     Opening balance     |";
                public const string IGNORE_ROW_3 = "|                    |                                        |                                        |               |           VND           |";
                public const string IGNORE_ROW_4 = "|--------------------|----------------------------------------|----------------------------------------|---------------|-------------------------|";
            }
        }

        public class Visa
        {
            public class ReportConstant
            {
                public const string REPORT_110_START = "REPORT ID:  VSS-110 ";
                public const string REPORT_120_START = "REPORT ID:  VSS-120";
                public const string REPORT_130_START = "REPORT ID:  VSS-130";
                public const string REPORT_140_START = "REPORT ID:  VSS-140";
                public const string REPORT_210_START = "REPORT ID:  VSS-210";
                public const string REPORT_900_START = "REPORT ID:  VSS-900-S";

                public const string REPORT_110_END = "*** END OF VSS-110 REPORT ***";
                public const string REPORT_120_END = "***  END OF VSS-120 REPORT  ***";
                public const string REPORT_130_END = "*** END OF VSS-130 REPORT ***";
                public const string REPORT_140_END = "*** END OF VSS-140 REPORT ***";
                public const string REPORT_210_END = "*** END OF VSS-210 REPORT ***";
                public const string REPORT_900_END = "*** END OF VSS-900-S REPORT ***";

                public const string IGNORE_ROW_1 = "                                                                                                                                     ";
                public const string IGNORE_ROW_2 = "*** NO DATA FOR THIS REPORT ***";
                public const string IGNORE_ROW_3 = "                                                                       CREDIT                     DEBIT                     TOTAL   ";
                public const string IGNORE_ROW_4 = "                                              COUNT                    AMOUNT                    AMOUNT                    AMOUNT   ";
                public const string IGNORE_ROW_5 = "                                      CURRENCY               COUNT               CLEARING          INTERCHANGE          INTERCHANGE ";
                public const string IGNORE_ROW_6 = "                                       TABLE                                      AMOUNT              VALUE                VALUE    ";
                public const string IGNORE_ROW_7 = "                                        DATE                                                         CREDITS               DEBITS   ";
                public const string IGNORE_ROW_8 = "                                                                       CREDIT                     DEBIT                     TOTAL    ";
                public const string IGNORE_ROW_9 = "                                              COUNT                    AMOUNT                    AMOUNT                    AMOUNT    ";
                public const string IGNORE_ROW_10 = "                                                                                                REIMBURSEMENT         REIMBURSEMENT  ";
                public const string IGNORE_ROW_11 = "                                                        COUNT              INTERCHANGE               FEE                   FEE       ";
                public const string IGNORE_ROW_12 = "                                                                              AMOUNT               CREDITS                DEBITS    ";



                public const int REPORTING_FOR_START = 16;
                public const int REPORTING_FOR_LENGTH = 42;
                public const int PROC_DATE_START = 122;
                public const int PROC_DATE_LENGTH = 11;
                public const int ROLLUP_TO_START = 12;
                public const int ROLLUP_TO_LENGTH = 42;
                public const int REPORT_DATE_START = 124;
                public const int REPORT_DATE_LENGTH = 9;
                public const int FUNDS_START = 20;
                public const int FUNDS_LENGTH = 65;
                public const int SETTLEMENT_CURRENCTY_START = 22;
                public const int SETTLEMENT_CURRENCTY_LENGTH = 50;
                public const int CLEARING_CURRENCTY_START = 20;
                public const int CLEARING_CURRENCTY_LENGTH = 50;
            }
        }

    }
}
