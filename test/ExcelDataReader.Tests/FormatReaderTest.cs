﻿using NUnit.Framework;
using ExcelDataReader.Core.NumberFormat;

#if EXCELDATAREADER_NET20
namespace ExcelDataReader.Net20.Tests
#elif NET45
namespace ExcelDataReader.Net45.Tests
#elif NETCOREAPP1_0
namespace ExcelDataReader.Netstandard13.Tests
#elif NETCOREAPP2_0
namespace ExcelDataReader.Netstandard20.Tests
#else
#error "Tests do not support the selected target platform"
#endif
{
    [TestFixture]
    public class FormatReaderTest
    {
        bool IsDateFormatString(string formatString)
        {
            var format = new NumberFormatString(formatString);
            return format?.IsDateTimeFormat ?? false;
        }

        [Test]
        public void NumberFormat_TestIsDateFormatString()
        {
            Assert.IsTrue(IsDateFormatString("dd/mm/yyyy"));
            Assert.IsTrue(IsDateFormatString("dd-mmm-yy"));
            Assert.IsTrue(IsDateFormatString("dd-mmmm"));
            Assert.IsTrue(IsDateFormatString("mmm-yy"));
            Assert.IsTrue(IsDateFormatString("h:mm AM/PM"));
            Assert.IsTrue(IsDateFormatString("h:mm:ss AM/PM"));
            Assert.IsTrue(IsDateFormatString("hh:mm"));
            Assert.IsTrue(IsDateFormatString("hh:mm:ss"));
            Assert.IsTrue(IsDateFormatString("dd/mm/yyyy hh:mm"));
            Assert.IsTrue(IsDateFormatString("mm:ss"));
            Assert.IsTrue(IsDateFormatString("mm:ss.0"));
            Assert.IsTrue(IsDateFormatString("[$-809]dd mmmm yyyy"));
            Assert.IsFalse(IsDateFormatString("#,##0;[Red]-#,##0"));
            Assert.IsFalse(IsDateFormatString("0_);[Red](0)"));
            Assert.IsFalse(IsDateFormatString(@"0\h"));
            Assert.IsFalse(IsDateFormatString("0\"h\""));
            Assert.IsFalse(IsDateFormatString("0%"));
            Assert.IsFalse(IsDateFormatString("General"));
            Assert.IsFalse(IsDateFormatString(@"_-* #,##0\ _P_t_s_-;\-* #,##0\ _P_t_s_-;_-* "" - ""??\ _P_t_s_-;_-@_- "));
        }

        void TestValid(string format)
        {
            var to = new NumberFormatString(format);
            Assert.IsTrue(to.IsValid, "Invalid format: {0}", format);
        }

        [Test]
        public void NumberFormat_TestValid()
        {
            // Mostly adapted from the SheetJS/ssf project:
            TestValid("\" Excellent\"");
            TestValid("\" Fair\"");
            TestValid("\" Good\"");
            TestValid("\" Poor\"");
            TestValid("\" Very Good\"");
            TestValid("\"$\"#,##0");
            TestValid("\"$\"#,##0.00");
            TestValid("\"$\"#,##0.00_);[Red]\\(\"$\"#,##0.00\\)");
            TestValid("\"$\"#,##0.00_);\\(\"$\"#,##0.00\\)");
            TestValid("\"$\"#,##0;[Red]\\-\"$\"#,##0");
            TestValid("\"$\"#,##0_);[Red]\\(\"$\"#,##0\\)");
            TestValid("\"$\"#,##0_);\\(\"$\"#,##0\\)");
            TestValid("\"Haha!\"\\ @\\ \"Yeah!\"");
            TestValid("\"TRUE\";\"TRUE\";\"FALSE\"");
            TestValid("\"True\";\"True\";\"False\";@");
            TestValid("\"Years: \"0");
            TestValid("\"Yes\";\"Yes\";\"No\";@");
            TestValid("\"kl \"hh:mm:ss;@");
            TestValid("\"£\"#,##0.00");
            TestValid("\"£\"#,##0;[Red]\\-\"£\"#,##0");
            TestValid("\"€\"#,##0.00");
            TestValid("\"€\"\\ #,##0.00_-");
            TestValid("\"上午/下午 \"hh\"時\"mm\"分\"ss\"秒 \"");
            TestValid("\"￥\"#,##0.00;\"￥\"\\-#,##0.00");
            TestValid("#");
            TestValid("# ?/?");
            TestValid("# ??/??");
            TestValid("#\" \"?/?");
            TestValid("#\" \"??/??");
            TestValid("#\"abded\"\\ ??/??");
            TestValid("###0.00;-###0.00");
            TestValid("###0;-###0");
            TestValid("##0.0E+0");
            TestValid("#,##0");
            TestValid("#,##0 ;(#,##0)");
            TestValid("#,##0 ;[Red](#,##0)");
            TestValid("#,##0\"р.\";[Red]\\-#,##0\"р.\"");
            TestValid("#,##0.0");
            TestValid("#,##0.00");
            TestValid("#,##0.00 \"�\"");
            TestValid("#,##0.00 €;-#,##0.00 €");
            TestValid("#,##0.00\"р.\";[Red]\\-#,##0.00\"р.\"");
            TestValid("#,##0.000");
            TestValid("#,##0.0000");
            TestValid("#,##0.00000");
            TestValid("#,##0.000000");
            TestValid("#,##0.0000000");
            TestValid("#,##0.00000000");
            TestValid("#,##0.000000000");
            TestValid("#,##0.00000000;[Red]#,##0.00000000");
            TestValid("#,##0.0000_ ");
            TestValid("#,##0.000_ ");
            TestValid("#,##0.000_);\\(#,##0.000\\)");
            TestValid("#,##0.00;(#,##0.00)");
            TestValid("#,##0.00;(#,##0.00);0.00");
            TestValid("#,##0.00;[Red](#,##0.00)");
            TestValid("#,##0.00;[Red]\\(#,##0.00\\)");
            TestValid("#,##0.00;\\(#,##0.00\\)");
            TestValid("#,##0.00[$₹-449]_);\\(#,##0.00[$₹-449]\\)");
            TestValid("#,##0.00\\ \"р.\"");
            TestValid("#,##0.00\\ \"р.\";[Red]\\-#,##0.00\\ \"р.\"");
            TestValid("#,##0.00\\ [$€-407]");
            TestValid("#,##0.00\\ [$€-40C]");
            TestValid("#,##0.00_);\\(#,##0.00\\)");
            TestValid("#,##0.00_р_.;[Red]\\-#,##0.00_р_.");
            TestValid("#,##0.00_р_.;\\-#,##0.00_р_.");
            TestValid("#,##0.0;[Red]#,##0.0");
            TestValid("#,##0.0_ ;\\-#,##0.0\\ ");
            TestValid("#,##0.0_);[Red]\\(#,##0.0\\)");
            TestValid("#,##0.0_);\\(#,##0.0\\)");
            TestValid("#,##0;\\-#,##0;0");
            TestValid("#,##0\\ \"р.\";[Red]\\-#,##0\\ \"р.\"");
            TestValid("#,##0\\ \"р.\";\\-#,##0\\ \"р.\"");
            TestValid("#,##0\\ ;[Red]\\(#,##0\\)");
            TestValid("#,##0\\ ;\\(#,##0\\)");
            TestValid("#,##0_ ");
            TestValid("#,##0_ ;[Red]\\-#,##0\\ ");
            TestValid("#,##0_);[Red]\\(#,##0\\)");
            TestValid("#,##0_р_.;[Red]\\-#,##0_р_.");
            TestValid("#,##0_р_.;\\-#,##0_р_.");
            TestValid("#.0000,,");
            TestValid("#0");
            TestValid("#0.00");
            TestValid("#0.0000");
            TestValid("#\\ ?/10");
            TestValid("#\\ ?/2");
            TestValid("#\\ ?/4");
            TestValid("#\\ ?/8");
            TestValid("#\\ ?/?");
            TestValid("#\\ ??/100");
            TestValid("#\\ ??/100;[Red]\\(#\\ ??/16\\)");
            TestValid("#\\ ??/16");
            TestValid("#\\ ??/??");
            TestValid("#\\ ??/?????????");
            TestValid("#\\ ???/???");
            TestValid("**\\ #,###,#00,000.00,**");
            TestValid("0");
            TestValid("0\"abde\".0\"??\"000E+00");
            TestValid("0%");
            TestValid("0.0");
            TestValid("0.0%");
            TestValid("0.00");
            TestValid("0.00\"°\"");
            TestValid("0.00%");
            TestValid("0.000");
            TestValid("0.000%");
            TestValid("0.0000");
            TestValid("0.000000");
            TestValid("0.00000000");
            TestValid("0.000000000");
            TestValid("0.000000000%");
            TestValid("0.00000000000");
            TestValid("0.000000000000000");
            TestValid("0.00000000E+00");
            TestValid("0.0000E+00");
            TestValid("0.00;[Red]0.00");
            TestValid("0.00E+00");
            TestValid("0.00_);[Red]\\(0.00\\)");
            TestValid("0.00_);\\(0.00\\)");
            TestValid("0.0_ ");
            TestValid("00.00.00.000");
            TestValid("00.000%");
            TestValid("0000");
            TestValid("00000");
            TestValid("00000000");
            TestValid("000000000");
            TestValid("00000\\-0000");
            TestValid("00000\\-00000");
            TestValid("000\\-00\\-0000");
            TestValid("0;[Red]0");
            TestValid("0\\-00000\\-00000\\-0");
            TestValid("0_);[Red]\\(0\\)");
            TestValid("0_);\\(0\\)");
            TestValid("@");
            TestValid("A/P");
            TestValid("AM/PM");
            TestValid("AM/PMh\"時\"mm\"分\"ss\"秒\";@");
            TestValid("D");
            TestValid("DD");
            TestValid("DD/MM/YY;@");
            TestValid("DD/MM/YYYY");
            TestValid("DD/MM/YYYY;@");
            TestValid("DDD");
            TestValid("DDDD");
            TestValid("DDDD\", \"MMMM\\ DD\", \"YYYY");
            TestValid("GENERAL");
            TestValid("General");
            TestValid("H");
            TestValid("H:MM:SS\\ AM/PM");
            TestValid("HH:MM");
            TestValid("HH:MM:SS\\ AM/PM");
            TestValid("HHM");
            TestValid("HHMM");
            TestValid("HH[MM]");
            TestValid("HH[M]");
            TestValid("M/D/YYYY");
            TestValid("M/D/YYYY\\ H:MM");
            TestValid("MM/DD/YY");
            TestValid("S");
            TestValid("SS");
            TestValid("YY");
            TestValid("YYM");
            TestValid("YYMM");
            TestValid("YYMMM");
            TestValid("YYMMMM");
            TestValid("YYMMMMM");
            TestValid("YYYY");
            TestValid("YYYY-MM-DD HH:MM:SS");
            TestValid("YYYY\\-MM\\-DD");
            TestValid("[$$-409]#,##0");
            TestValid("[$$-409]#,##0.00");
            TestValid("[$$-409]#,##0.00_);[Red]\\([$$-409]#,##0.00\\)");
            TestValid("[$$-C09]#,##0.00");
            TestValid("[$-100042A]h:mm:ss\\ AM/PM;@");
            TestValid("[$-1010409]0.000%");
            TestValid("[$-1010409]General");
            TestValid("[$-1010409]d/m/yyyy\\ h:mm\\ AM/PM;@");
            TestValid("[$-1010409]dddd, mmmm dd, yyyy");
            TestValid("[$-1010409]m/d/yyyy");
            TestValid("[$-1409]h:mm:ss\\ AM/PM;@");
            TestValid("[$-2000000]h:mm:ss;@");
            TestValid("[$-2010401]d/mm/yyyy\\ h:mm\\ AM/PM;@");
            TestValid("[$-4000439]h:mm:ss\\ AM/PM;@");
            TestValid("[$-4010439]d/m/yyyy\\ h:mm\\ AM/PM;@");
            TestValid("[$-409]AM/PM\\ hh:mm:ss;@");
            TestValid("[$-409]d/m/yyyy\\ hh:mm;@");
            TestValid("[$-409]d\\-mmm;@");
            TestValid("[$-409]d\\-mmm\\-yy;@");
            TestValid("[$-409]d\\-mmm\\-yyyy;@");
            TestValid("[$-409]dd/mm/yyyy\\ hh:mm;@");
            TestValid("[$-409]dd\\-mmm\\-yy;@");
            TestValid("[$-409]h:mm:ss\\ AM/PM;@");
            TestValid("[$-409]h:mm\\ AM/PM;@");
            TestValid("[$-409]m/d/yy\\ h:mm\\ AM/PM;@");
            TestValid("[$-409]mmm\\-yy;@");
            TestValid("[$-409]mmmm\\ d\\,\\ yyyy;@");
            TestValid("[$-409]mmmm\\-yy;@");
            TestValid("[$-409]mmmmm;@");
            TestValid("[$-409]mmmmm\\-yy;@");
            TestValid("[$-40E]h\\ \"óra\"\\ m\\ \"perckor\"\\ AM/PM;@");
            TestValid("[$-412]AM/PM\\ h\"시\"\\ mm\"분\"\\ ss\"초\";@");
            TestValid("[$-41C]h:mm:ss\\.AM/PM;@");
            TestValid("[$-449]hh:mm:ss\\ AM/PM;@");
            TestValid("[$-44E]hh:mm:ss\\ AM/PM;@");
            TestValid("[$-44F]hh:mm:ss\\ AM/PM;@");
            TestValid("[$-D000409]h:mm\\ AM/PM;@");
            TestValid("[$-D010000]d/mm/yyyy\\ h:mm\\ \"น.\";@");
            TestValid("[$-F400]h:mm:ss\\ AM/PM");
            TestValid("[$-F800]dddd\\,\\ mmmm\\ dd\\,\\ yyyy");
            TestValid("[$AUD]\\ #,##0.00");
            TestValid("[$RD$-1C0A]#,##0.00;[Red]\\-[$RD$-1C0A]#,##0.00");
            TestValid("[$SFr.-810]\\ #,##0.00_);[Red]\\([$SFr.-810]\\ #,##0.00\\)");
            TestValid("[$£-809]#,##0.00;[Red][$£-809]#,##0.00");
            TestValid("[$¥-411]#,##0.00");
            TestValid("[$¥-804]#,##0.00");
            TestValid("[<0]\"\";0%");
            TestValid("[<=9999999]###\\-####;\\(###\\)\\ ###\\-####");
            TestValid("[=0]?;#,##0.00");
            TestValid("[=0]?;0%");
            TestValid("[=0]?;[<4.16666666666667][hh]:mm:ss;[hh]:mm");
            TestValid("[>999999]#,,\"M\";[>999]#,\"K\";#");
            TestValid("[>999999]#.000,,\"M\";[>999]#.000,\"K\";#.000");
            TestValid("[>=100000]0.000\\ \\\";[Red]0.000\\ \\<\\ \\>\\ \\\"\\ \\&\\ \\'\\ ");
            TestValid("[>=100000]0.000\\ \\<;[Red]0.000\\ \\>");
            TestValid("[BLACK]@");
            TestValid("[BLUE]GENERAL");
            TestValid("[Black]@");
            TestValid("[Blue]General");
            TestValid("[CYAN]@");
            TestValid("[Cyan]@");
            TestValid("[DBNum1][$-804]AM/PMh\"时\"mm\"分\";@");
            TestValid("[DBNum1][$-804]General");
            TestValid("[DBNum1][$-804]h\"时\"mm\"分\";@");
            TestValid("[ENG][$-1004]dddd\\,\\ d\\ mmmm\\,\\ yyyy;@");
            TestValid("[ENG][$-101040D]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-101042A]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-140C]dddd\\ \"YeahWoo!\"\\ ddd\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-2C0A]dddd\\ d\" de \"mmmm\" de \"yyyy;@");
            TestValid("[ENG][$-402]dd\\ mmmm\\ yyyy\\ \"г.\";@");
            TestValid("[ENG][$-403]dddd\\,\\ d\" / \"mmmm\" / \"yyyy;@");
            TestValid("[ENG][$-405]d\\.\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-408]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-409]d\\-mmm;@");
            TestValid("[ENG][$-409]d\\-mmm\\-yy;@");
            TestValid("[ENG][$-409]d\\-mmm\\-yyyy;@");
            TestValid("[ENG][$-409]dd\\-mmm\\-yy;@");
            TestValid("[ENG][$-409]mmm\\-yy;@");
            TestValid("[ENG][$-409]mmmm\\ d\\,\\ yyyy;@");
            TestValid("[ENG][$-409]mmmm\\-yy;@");
            TestValid("[ENG][$-40B]d\\.\\ mmmm\\t\\a\\ yyyy;@");
            TestValid("[ENG][$-40C]d/mmm/yyyy;@");
            TestValid("[ENG][$-40E]yyyy/\\ mmmm\\ d\\.;@");
            TestValid("[ENG][$-40F]dd\\.\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-410]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-415]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-416]d\\ \\ mmmm\\,\\ yyyy;@");
            TestValid("[ENG][$-418]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-41A]d\\.\\ mmmm\\ yyyy\\.;@");
            TestValid("[ENG][$-41B]d\\.\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-41D]\"den \"\\ d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-420]dddd\\,\\ dd\\ mmmm\\,\\ yyyy;@");
            TestValid("[ENG][$-421]dd\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-424]dddd\\,\\ d\\.\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-425]dddd\\,\\ d\\.\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-426]dddd\\,\\ yyyy\". gada \"d\\.\\ mmmm;@");
            TestValid("[ENG][$-427]yyyy\\ \"m.\"\\ mmmm\\ d\\ \"d.\";@");
            TestValid("[ENG][$-42B]dddd\\,\\ d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-42C]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-42D]yyyy\"(e)ko\"\\ mmmm\"ren\"\\ d\"a\";@");
            TestValid("[ENG][$-42F]dddd\\,\\ dd\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-437]yyyy\\ \\წ\\ლ\\ი\\ს\\ dd\\ mm\\,\\ dddd;@");
            TestValid("[ENG][$-438]d\\.\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-43F]d\\ mmmm\\ yyyy\\ \"ж.\";@");
            TestValid("[ENG][$-444]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-449]dd\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-44E]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-44F]dd\\ mmmm\\ yyyy\\ dddd;@");
            TestValid("[ENG][$-457]dd\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-813]dddd\\ d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-81A]dddd\\,\\ d\\.\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-82C]d\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-843]yyyy\\ \"й\"\"и\"\"л\"\\ d/mmmm;@");
            TestValid("[ENG][$-C07]dddd\\,\\ dd\\.\\ mmmm\\ yyyy;@");
            TestValid("[ENG][$-FC19]yyyy\\,\\ dd\\ mmmm;@");
            TestValid("[ENG][$-FC22]d\\ mmmm\\ yyyy\" р.\";@");
            TestValid("[ENG][$-FC23]d\\ mmmm\\ yyyy;@");
            TestValid("[GREEN]#,###");
            TestValid("[Green]#,###");
            TestValid("[HH]");
            TestValid("[HIJ][$-2060401]d/mm/yyyy\\ h:mm\\ AM/PM;@");
            TestValid("[HIJ][$-2060401]d\\ mmmm\\ yyyy;@");
            TestValid("[H]");
            TestValid("[JPN][$-411]gggyy\"年\"m\"月\"d\"日\"\\ dddd;@");
            TestValid("[MAGENTA]0.00");
            TestValid("[Magenta]0.00");
            TestValid("[RED]#.##");
            TestValid("[Red]#.##");
            TestValid("[Red][<-25]General;[Blue][>25]General;[Green]General;[Yellow]General\\ ");
            TestValid("[Red][<=-25]General;[Blue][>=25]General;[Green]General;[Yellow]General");
            TestValid("[Red][<>50]General;[Blue]000");
            TestValid("[Red][=50]General;[Blue]000");
            TestValid("[SS]");
            TestValid("[S]");
            TestValid("[TWN][DBNum1][$-404]y\"年\"m\"月\"d\"日\";@");
            TestValid("[WHITE]0.0");
            TestValid("[White]0.0");
            TestValid("[YELLOW]@");
            TestValid("[Yellow]@");
            TestValid("[h]");
            TestValid("[h]:mm:ss");
            TestValid("[h]:mm:ss;@");
            TestValid("[h]\\.mm\" Uhr \";@");
            TestValid("[hh]");
            TestValid("[s]");
            TestValid("[ss]");
            TestValid("\\#\\r\\e\\c");
            TestValid("\\$#,##0_);[Red]\"($\"#,##0\\)");
            TestValid("\\$0.00");
            TestValid("\\C\\O\\B\\ \\o\\n\\ @");
            TestValid("\\C\\R\\O\\N\\T\\A\\B\\ \\o\\n\\ @");
            TestValid("\\R\\e\\s\\u\\l\\t\\ \\o\\n\\ @");
            TestValid("\\S\\Q\\L\\ \\:\\ @");
            TestValid("\\S\\Q\\L\\ \\R\\e\\q\\u\\e\\s\\t\\ \\f\\o\\r\\ @");
            TestValid("\\c\\c\\c?????0\"aaaa\"0\"bbbb\"000000.00%");
            TestValid("\\u\\n\\t\\i\\l\\ h:mm;@");
            TestValid("_ \"￥\"* #,##0.00_ \"Positive\";_ \"￥\"* \\-#,##0.00_ ;_ \"￥\"* \"-\"??_ \"Negtive\";_ @_ \\ \"Zero\"");
            TestValid("_ * #,##0.00_)[$﷼-429]_ ;_ * \\(#,##0.00\\)[$﷼-429]_ ;_ * \"-\"??_)[$﷼-429]_ ;_ @_ ");
            TestValid("_ * #,##0_ ;_ * \\-#,##0_ ;[Red]_ * \"-\"_ ;_ @_ ");
            TestValid("_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)");
            TestValid("_(\"$\"* #,##0_);_(\"$\"* \\(#,##0\\);_(\"$\"* \"-\"??_);_(@_)");
            TestValid("_(\"$\"* #,##0_);_(\"$\"* \\(#,##0\\);_(\"$\"* \"-\"_);_(@_)");
            TestValid("_(* #,##0.0000_);_(* \\(#,##0.0000\\);_(* \"-\"??_);_(@_)");
            TestValid("_(* #,##0.000_);_(* \\(#,##0.000\\);_(* \"-\"??_);_(@_)");
            TestValid("_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)");
            TestValid("_(* #,##0.0_);_(* \\(#,##0.0\\);_(* \"-\"??_);_(@_)");
            TestValid("_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"??_);_(@_)");
            TestValid("_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"_);_(@_)");
            TestValid("_([$ANG]\\ * #,##0.0_);_([$ANG]\\ * \\(#,##0.0\\);_([$ANG]\\ * \"-\"?_);_(@_)");
            TestValid("_-\"€\"\\ * #,##0.00_-;_-\"€\"\\ * #,##0.00\\-;_-\"€\"\\ * \"-\"??_-;_-@_-");
            TestValid("_-* #,##0.00\" TL\"_-;\\-* #,##0.00\" TL\"_-;_-* \\-??\" TL\"_-;_-@_-");
            TestValid("_-* #,##0.00\" €\"_-;\\-* #,##0.00\" €\"_-;_-* \\-??\" €\"_-;_-@_-");
            TestValid("_-* #,##0.00\\ \"р.\"_-;\\-* #,##0.00\\ \"р.\"_-;_-* \"-\"??\\ \"р.\"_-;_-@_-");
            TestValid("_-* #,##0.00\\ \"€\"_-;\\-* #,##0.00\\ \"€\"_-;_-* \"-\"??\\ \"€\"_-;_-@_-");
            TestValid("_-* #,##0.00\\ [$€-407]_-;\\-* #,##0.00\\ [$€-407]_-;_-* \\-??\\ [$€-407]_-;_-@_-");
            TestValid("_-* #,##0.0\\ _F_-;\\-* #,##0.0\\ _F_-;_-* \"-\"??\\ _F_-;_-@_-");
            TestValid("_-* #,##0\\ \"€\"_-;\\-* #,##0\\ \"€\"_-;_-* \"-\"\\ \"€\"_-;_-@_-");
            TestValid("_-* #,##0_-;\\-* #,##0_-;_-* \"-\"??_-;_-@_-");
            TestValid("_-\\$* #,##0.0_ ;_-\\$* \\-#,##0.0\\ ;_-\\$* \"-\"?_ ;_-@_ ");
            TestValid("d");
            TestValid("d-mmm");
            TestValid("d-mmm-yy");
            TestValid("d/m");
            TestValid("d/m/yy;@");
            TestValid("d/m/yyyy;@");
            TestValid("d/mm/yy;@");
            TestValid("d/mm/yyyy;@");
            TestValid("d\\-mmm");
            TestValid("d\\-mmm\\-yyyy");
            TestValid("dd");
            TestValid("dd\"-\"mmm\"-\"yyyy");
            TestValid("dd/m/yyyy");
            TestValid("dd/mm/yy");
            TestValid("dd/mm/yy;@");
            TestValid("dd/mm/yy\\ hh:mm");
            TestValid("dd/mm/yyyy");
            TestValid("dd/mm/yyyy\\ hh:mm:ss");
            TestValid("dd/mmm");
            TestValid("dd\\-mm\\-yy");
            TestValid("dd\\-mmm\\-yy");
            TestValid("dd\\-mmm\\-yyyy\\ hh:mm:ss.000");
            TestValid("dd\\/mm\\/yy");
            TestValid("dd\\/mm\\/yyyy");
            TestValid("ddd");
            TestValid("dddd");
            TestValid("dddd, mmmm dd, yyyy");
            TestValid("h");
            TestValid("h\"时\"mm\"分\"ss\"秒\";@");
            TestValid("h\"時\"mm\"分\"ss\"秒\";@");
            TestValid("h:mm");
            TestValid("h:mm AM/PM");
            TestValid("h:mm:ss");
            TestValid("h:mm:ss AM/PM");
            TestValid("h:mm:ss;@");
            TestValid("h:mm;@");
            TestValid("h\\.mm\" Uhr \";@");
            TestValid("h\\.mm\" h\";@");
            TestValid("h\\.mm\" u.\";@");
            TestValid("hh\":\"mm AM/PM");
            TestValid("hh:mm:ss");
            TestValid("hh:mm:ss\\ AM/PM");
            TestValid("hh\\.mm\" h\";@");
            TestValid("hhm");
            TestValid("hhmm");
            TestValid("m\"月\"d\"日\"");
            TestValid("m/d/yy");
            TestValid("m/d/yy h:mm");
            TestValid("m/d/yy;@");
            TestValid("m/d/yy\\ h:mm");
            TestValid("m/d/yy\\ h:mm;@");
            TestValid("m/d/yyyy");
            TestValid("m/d/yyyy;@");
            TestValid("m/d/yyyy\\ h:mm:ss;@");
            TestValid("m/d;@");
            TestValid("m\\/d\\/yyyy");
            TestValid("mm/dd");
            TestValid("mm/dd/yy");
            TestValid("mm/dd/yy;@");
            TestValid("mm/dd/yyyy");
            TestValid("mm:ss");
            TestValid("mm:ss.0;@");
            TestValid("mmm d, yyyy");
            TestValid("mmm\" \"d\", \"yyyy");
            TestValid("mmm-yy");
            TestValid("mmm-yy;@");
            TestValid("mmm/yy");
            TestValid("mmm\\-yy");
            TestValid("mmm\\-yy;@");
            TestValid("mmm\\-yyyy");
            TestValid("mmmm\\ d\\,\\ yyyy");
            TestValid("mmmm\\ yyyy");
            TestValid("mmss.0");
            TestValid("s");
            TestValid("ss");
            TestValid("yy");
            TestValid("yy/mm/dd");
            TestValid("yy\\.mm\\.dd");
            TestValid("yym");
            TestValid("yymm");
            TestValid("yymmm");
            TestValid("yymmmm");
            TestValid("yymmmmm");
            TestValid("yyyy");
            TestValid("yyyy\"년\"\\ m\"월\"\\ d\"일\";@");
            TestValid("yyyy-m-d h:mm AM/PM");
            TestValid("yyyy-mm-dd");
            TestValid("yyyy/mm/dd");
            TestValid("yyyy\\-m\\-d\\ hh:mm:ss");
            TestValid("yyyy\\-mm\\-dd");
            TestValid("yyyy\\-mm\\-dd;@");
            TestValid("yyyy\\-mm\\-dd\\ h:mm");
            TestValid("yyyy\\-mm\\-dd\\Thh:mm");
            TestValid("yyyy\\-mm\\-dd\\Thhmmss.000");
        }
    }
}
