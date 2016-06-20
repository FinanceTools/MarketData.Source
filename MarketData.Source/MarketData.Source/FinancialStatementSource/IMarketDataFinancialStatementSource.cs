using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Newtonsoft.Json.Linq;

namespace MarketData.Source.FinancialStatementSource
{
    public interface IMarketDataFinancialStatementSource
    {
        IEnumerable<FinancialStatement> GetFinancialStatements(string ticker);
    }


    public class EdgarMarketDataFinancialStatementSource :IMarketDataFinancialStatementSource
    {

        private readonly string _apiKey;

        public EdgarMarketDataFinancialStatementSource(string apiKey)
        {
            _apiKey = apiKey;
        }

        public IEnumerable<FinancialStatement> GetFinancialStatements(string ticker)
        {
            try
            {
                string jsonData;

                using (WebClient web = new WebClient())
                {
                    jsonData = web.DownloadString(string.Format("http://edgaronline.api.mashery.com/v2/corefinancials/ann.json?primarysymbols={0}&appkey={1}", ticker, _apiKey));
                }

                var results = ParseFinancialStatements(jsonData);
                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IEnumerable<FinancialStatement> ParseFinancialStatements(string jsonData)
        {
            dynamic statements = JObject.Parse(jsonData);
            var results = new List<FinancialStatement>();
            if (statements != null && statements.result != null && statements.result.rows != null)
            {
                foreach (var row in statements.result.rows.Children())
                {
                    FinancialStatement statement = new FinancialStatement();
                    foreach (var entry in row.values.Children())
                    {
                        string field = entry.field;
                        string value = entry.value;
                        if (Setters.ContainsKey(field) && value != null && value != "null")
                        {
                            Setters[field](statement, value);
                        }
                    }
                    results.Add(statement);
                }
            }
            return results;
        }

        private static readonly Dictionary<string, Action<FinancialStatement, string>> Setters =
            new Dictionary<string, Action<FinancialStatement, string>>
            {
                #region Metadata
                {"cik", (f,s)=>f.Cik=s },
                {"companyname", (f,s)=>f.CompanyName=s },
                {"entityid", (f,s)=>f.EntityId=s },
                {"primaryexchange", (f,s)=>f.PrimaryExchange=s },
                {"primarysymbol", (f,s)=>f.PrimarySymbol=s },
                {"siccode", (f,s)=>f.SicCode=s },
                {"sicdescription", (f,s)=>f.SicDescription=s },
                {"usdconversionrate", (f,s)=>f.UsdConversionRate=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"restated", (f,s)=>f.Restated=Convert.ToBoolean(s) },
                {"receiveddate", (f,s)=>f.ReceivedDate=Convert.ToDateTime(s, CultureInfo.InvariantCulture) },
                {"preliminary", (f,s)=>f.Preliminary=Convert.ToBoolean(s) },
                {"periodlengthcode", (f,s)=>f.PeriodLengthCode=s },
                {"periodlength", (f,s)=>f.PeriodLength=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"periodenddate", (f,s)=>f.PeriodEnd=Convert.ToDateTime(s,CultureInfo.InvariantCulture) },
                {"original", (f,s)=>f.Original=Convert.ToBoolean(s) },
                {"formtype", (f,s)=>f.FormType=s },
                {"fiscalyear", (f,s)=>f.FiscalYear=Convert.ToInt32(s) },
                {"fiscalquarter", (f,s)=>f.FiscalQuarter=Convert.ToInt32(s) },
                {"dcn", (f,s)=>f.Dcn=s },
                {"currencycode", (f,s)=>f.CurrencyCode=s },
                {"crosscalculated", (f,s)=>f.CrossCalulated=Convert.ToBoolean(s) },
                {"audited", (f,s)=>f.Audited=Convert.ToBoolean(s) },
                {"amended", (f,s)=>f.Amended=Convert.ToBoolean(s) },
                #endregion Metadata
                {"changeincurrentassets", (f,s)=>f.ChangeInCurrentAsset=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"changeincurrentliabilities", (f,s)=>f.ChangeInCurrentLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"changeininventories", (f,s)=>f.ChangeInInventories=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"dividendspaid", (f,s)=>f.DividendsPaid=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"effectofexchangerateoncash", (f,s)=>f.EffectOfExchangeRateOnCash=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"capitalexpenditures", (f,s)=>f.CapitalExpanditure=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },


                {"cashfromfinancingactivities", (f,s)=>f.CashFromFinancingActivities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"cashfrominvestingactivities", (f,s)=>f.CashFromInvestingActivities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"cashfromoperatingactivities", (f,s)=>f.CashFromOperatingActivities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"cfdepreciationamortization", (f,s)=>f.CfDepreciationAmortization=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"changeinaccountsreceivable", (f,s)=>f.ChangeInAccountReceivable=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"investmentchangesnet", (f,s)=>f.InvestmentChangesNet=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"netchangeincash", (f,s)=>f.NetChangeInCash=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totaladjustments", (f,s)=>f.TotalAdjustments=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"ebit", (f,s)=>f.Ebit=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"costofrevenue", (f,s)=>f.CostOfRevenue=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"discontinuedoperations", (f,s)=>f.DiscontinuedOperation=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"equityearnings", (f,s)=>f.EquityEarnings=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"accountingchange", (f,s)=>f.AccountingChange=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"extraordinaryitems", (f,s)=>f.ExtraordinaryItems=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"grossprofit", (f,s)=>f.GrossProfit=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"incomebeforetaxes", (f,s)=>f.IncomeBeforeTaxes=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"interestexpense", (f,s)=>f.InterestExpense=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"netincome", (f,s)=>f.NetIncome=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"netincomeapplicabletocommon", (f,s)=>f.NetIncomeApplicableToCommon=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"researchdevelopmentexpense", (f,s)=>f.ResearchDevelopementExpense=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalrevenue", (f,s)=>f.TotalRevenue=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"sellinggeneraladministrativeexpenses", (f,s)=>f.SellingGeneralAdministrativeExpense=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"commonstock", (f,s)=>f.CommonStock=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"deferredcharges", (f,s)=>f.DeferredCHarges=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },


                {"cashandcashequivalents", (f,s)=>f.BalanceSheet.CashAndCashEquivalent=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"cashcashequivalentsandshortterminvestments", (f,s)=>f.BalanceSheet.CashCashEquivalentAndShortTermInvestments=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"goodwill", (f,s)=>f.BalanceSheet.Goodwill=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"intangibleassets", (f,s)=>f.BalanceSheet.IntangibleAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"inventoriesnet", (f,s)=>f.BalanceSheet.InventoriesNet=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"minorityinterest", (f,s)=>f.IncomeStatement.MinorityInterest=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"otherassets", (f,s)=>f.BalanceSheet.OtherAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"othercurrentassets", (f,s)=>f.BalanceSheet.OtherCurrentAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"othercurrentliabilities", (f,s)=>f.BalanceSheet.OtherCurrentLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"otherequity", (f,s)=>f.BalanceSheet.OtherEquity=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"otherliabilities", (f,s)=>f.BalanceSheet.OtherLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"preferredstock", (f,s)=>f.BalanceSheet.PreferredStock=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"propertyplantequipmentnet", (f,s)=>f.BalanceSheet.PropertyPlantEquipmentNet=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"retainedearnings", (f,s)=>f.BalanceSheet.RetainedEarnings=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalassets", (f,s)=>f.BalanceSheet.TotalAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalcurrentassets", (f,s)=>f.BalanceSheet.TotalCurrentAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalcurrentliabilities", (f,s)=>f.BalanceSheet.TotalCurrentLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalliabilities", (f,s)=>f.BalanceSheet.TotalLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"totallongtermdebt", (f,s)=>f.BalanceSheet.TotalLongTermDebt=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalreceivablesnet", (f,s)=>f.BalanceSheet.TotalReceivableNet=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalshorttermdebt", (f,s)=>f.BalanceSheet.TotalShortTermDebt=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalstockholdersequity", (f,s)=>f.BalanceSheet.TotalStockHolderEquity=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"treasurystock", (f,s)=>f.BalanceSheet.TreasuryStock=Convert.ToDecimal(s, CultureInfo.InvariantCulture) }
            };
    }

    public class BalanceSheet
    {
        public Decimal CashAndCashEquivalent { get; set; }
        public Decimal CashCashEquivalentAndShortTermInvestments { get; set; }
        public Decimal Goodwill{ get; set; }
        public Decimal IntangibleAssets { get; set; }
        public Decimal InventoriesNet { get; set; }
        public Decimal OtherAssets { get; set; }
        public Decimal OtherCurrentAssets { get; set; }
        public Decimal OtherCurrentLiabilities { get; set; }
        public Decimal OtherEquity { get; set; }
        public Decimal OtherLiabilities { get; set; }
        public Decimal PreferredStock { get; set; }
        public Decimal PropertyPlantEquipmentNet { get; set; }
        public Decimal RetainedEarnings { get; set; }
        public Decimal TotalAssets { get; set; }
        public Decimal TotalCurrentAssets { get; set; }
        public Decimal TotalCurrentLiabilities { get; set; }
        public Decimal TotalLiabilities { get; set; }
        public Decimal TotalLongTermDebt { get; set; }
        public Decimal TotalReceivableNet { get; set; }
        public Decimal TotalShortTermDebt { get; set; }
        public Decimal TotalStockHolderEquity { get; set; }
        public Decimal TreasuryStock { get; set; }
    }

    public class FinancialStatement
    {
        public BalanceSheet BalanceSheet { get; set; }
        public IncomeStatement IncomeStatement { get; set; }
        public CashFlowStatement CashFlowStatement { get; set; }
        #region MetaData
        public string Cik { get; set; }
        public string CompanyName { get; set; }
        public string EntityId { get; set; }
        public string PrimaryExchange { get; set; }
        public string PrimarySymbol { get; set; }
        public string SicCode { get; set; }
        public string SicDescription { get; set; }
        public decimal UsdConversionRate { get; set; }
        public bool Restated { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool Preliminary { get; set; }
        public string PeriodLengthCode { get; set; }
        public decimal PeriodLength { get; set; }
        public DateTime PeriodEnd { get; set; }
        public bool Original { get; set; }
        public string FormType { get; set; }
        public int FiscalYear { get; set; }
        public int FiscalQuarter { get; set; }
        public string Dcn { get; set; }
        public string CurrencyCode { get; set; }
        public bool CrossCalulated { get; set; }
        public bool Audited { get; set; }
        public bool Amended { get; set; }
        #endregion MetaData
       
        public Decimal ChangeInCurrentAsset { get; set; }
        public Decimal ChangeInCurrentLiabilities { get; set; }
        public Decimal ChangeInInventories { get; set; }
        public Decimal DividendsPaid { get; set; }
        public Decimal EffectOfExchangeRateOnCash { get; set; }
        public Decimal CapitalExpanditure { get; set; }
        public Decimal CashFromFinancingActivities { get; set; }
        public Decimal CashFromInvestingActivities { get; set; }
        public Decimal CashFromOperatingActivities { get; set; }
        public Decimal CfDepreciationAmortization { get; set; }
        public Decimal ChangeInAccountReceivable { get; set; }
        public Decimal InvestmentChangesNet { get; set; }
        public Decimal NetChangeInCash { get; set; }
        public Decimal TotalAdjustments { get; set; }
        public Decimal Ebit { get; set; }
        public Decimal CostOfRevenue { get; set; }
        public Decimal DiscontinuedOperation { get; set; }
        public Decimal EquityEarnings { get; set; }
        public Decimal AccountingChange { get; set; }
        public Decimal ExtraordinaryItems { get; set; }
        public Decimal GrossProfit { get; set; }
        public Decimal IncomeBeforeTaxes { get; set; }
        public Decimal InterestExpense { get; set; }
        public Decimal NetIncome { get; set; }
        public Decimal NetIncomeApplicableToCommon { get; set; }
        public Decimal ResearchDevelopementExpense { get; set; }
        public Decimal TotalRevenue { get; set; }
        public Decimal SellingGeneralAdministrativeExpense { get; set; }
        public Decimal CommonStock { get; set; }
        public Decimal DeferredCHarges { get; set; }

      




        public FinancialStatement()
        {
            BalanceSheet = new BalanceSheet();
            IncomeStatement = new IncomeStatement();
            CashFlowStatement = new CashFlowStatement();
        }
    }


   

    public class IncomeStatement
    {
        public Decimal MinorityInterest { get; set; }
    }

    public class CashFlowStatement
    {

    }



}



