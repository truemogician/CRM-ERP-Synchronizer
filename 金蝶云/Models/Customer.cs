// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using KingdeeCloud.Utilities;
using Newtonsoft.Json;

namespace KingdeeCloud.Models {
	public class FNumberConverter : StringIdConverter {
		protected override string IdName => "FNumber";
	}

	public class FNUMBERConverter : StringIdConverter {
		protected override string IdName => "FNUMBER";
	}

	public class FFreezeOperator {
		[JsonProperty("FUserID")]
		public string UserID { get; set; }
	}

	public class FTBDCUSTOMEREXT {
		[JsonProperty("FEntryId")]
		public int EntryId { get; set; }

		[JsonProperty("FEnableSL")]
		public string EnableSL { get; set; }

		[JsonProperty("FFreezeLimit")]
		public string FreezeLimit { get; set; }

		[JsonProperty("FFreezeOperator")]
		public FFreezeOperator FreezeOperator { get; set; }

		[JsonProperty("FFreezeDate")]
		public string FreezeDate { get; set; }

		[JsonProperty("FPROVINCE")]
		[JsonConverter(typeof(FNumberConverter))]
		public string PROVINCE { get; set; }

		[JsonProperty("FCITY")]
		[JsonConverter(typeof(FNumberConverter))]
		public string CITY { get; set; }

		[JsonProperty("FDefaultConsiLoc")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string DefaultConsiLoc { get; set; }

		[JsonProperty("FDefaultSettleLoc")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string DefaultSettleLoc { get; set; }

		[JsonProperty("FDefaultPayerLoc")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string DefaultPayerLoc { get; set; }

		[JsonProperty("FDefaultContact")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string DefaultContact { get; set; }

		[JsonProperty("FMarginLevel")]
		public int MarginLevel { get; set; }

		[JsonProperty("FDebitCard")]
		public string DebitCard { get; set; }

		[JsonProperty("FSettleId")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string SettleId { get; set; }

		[JsonProperty("FChargeId")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string ChargeId { get; set; }

		[JsonProperty("FALLOWJOINZHJ")]
		public string ALLOWJOINZHJ { get; set; }
	}

	public class FMARACTIVITYID {
		[JsonProperty("FBILLNO")]
		public string BILLNO { get; set; }
	}

	public class FWeiXinMarketingId {
		[JsonProperty("FBILLNO")]
		public string BILLNO { get; set; }
	}

	public class FCRMCustomer {
		[JsonProperty("FEntryId")]
		public int EntryId { get; set; }

		[JsonProperty("FSUPERIORCUSTID")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string SUPERIORCUSTID { get; set; }

		[JsonProperty("FLastContactDate")]
		public string LastContactDate { get; set; }

		[JsonProperty("FYearReceive")]
		public int YearReceive { get; set; }

		[JsonProperty("FTotalReceive")]
		public int TotalReceive { get; set; }

		[JsonProperty("FExpireReceive")]
		public int ExpireReceive { get; set; }

		[JsonProperty("FSrcId")]
		public int SrcId { get; set; }

		[JsonProperty("FSrcFormId")]
		public string SrcFormId { get; set; }

		[JsonProperty("FMainContact")]
		public string MainContact { get; set; }

		[JsonProperty("FPhone")]
		public string Phone { get; set; }

		[JsonProperty("FCUSTValue")]
		public string CUSTValue { get; set; }

		[JsonProperty("FCustType")]
		[JsonConverter(typeof(FNumberConverter))]
		public string CustType { get; set; }

		[JsonProperty("FYearOrder")]
		public int YearOrder { get; set; }

		[JsonProperty("FTotalOrder")]
		public int TotalOrder { get; set; }

		[JsonProperty("FAverageOrder")]
		public int AverageOrder { get; set; }

		[JsonProperty("FYearOutStock")]
		public int YearOutStock { get; set; }

		[JsonProperty("FTotalOutStock")]
		public int TotalOutStock { get; set; }

		[JsonProperty("FMARACTIVITYID")]
		public FMARACTIVITYID MARACTIVITYID { get; set; }

		[JsonProperty("FWeiXinMarketingId")]
		public FWeiXinMarketingId WeiXinMarketingId { get; set; }
	}

	public class FTBDCUSTLOCATION {
		[JsonProperty("FContactId")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string ContactId { get; set; }

		[JsonProperty("FIsDefaultConsigneeCT")]
		public string IsDefaultConsigneeCT { get; set; }

		[JsonProperty("FIsCopy")]
		public string IsCopy { get; set; }
	}

	public class FTBDCUSTBANK {
		[JsonProperty("FENTRYID")]
		public int ENTRYID { get; set; }

		[JsonProperty("FCOUNTRY1")]
		[JsonConverter(typeof(FNumberConverter))]
		public string COUNTRY1 { get; set; }

		[JsonProperty("FBANKCODE")]
		public string BANKCODE { get; set; }

		[JsonProperty("FACCOUNTNAME")]
		public string ACCOUNTNAME { get; set; }

		[JsonProperty("FBankTypeRec")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string BankTypeRec { get; set; }

		[JsonProperty("FTextBankDetail")]
		public string TextBankDetail { get; set; }

		[JsonProperty("FBankDetail")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string BankDetail { get; set; }

		[JsonProperty("FOpenAddressRec")]
		public string OpenAddressRec { get; set; }

		[JsonProperty("FOPENBANKNAME")]
		public string OPENBANKNAME { get; set; }

		[JsonProperty("FCNAPS")]
		public string CNAPS { get; set; }

		[JsonProperty("FCURRENCYID")]
		[JsonConverter(typeof(FNumberConverter))]
		public string CURRENCYID { get; set; }

		[JsonProperty("FISDEFAULT1")]
		public string ISDEFAULT1 { get; set; }
	}

	public class FTBDCUSTCONTACT {
		[JsonProperty("FENTRYID")]
		public int ENTRYID { get; set; }

		[JsonProperty("FNUMBER1")]
		public string NUMBER1 { get; set; }

		[JsonProperty("FNAME1")]
		public string NAME1 { get; set; }

		[JsonProperty("FADDRESS1")]
		public string ADDRESS1 { get; set; }

		[JsonProperty("FTRANSLEADTIME1")]
		public int TRANSLEADTIME1 { get; set; }

		[JsonProperty("FMOBILE")]
		public string MOBILE { get; set; }

		[JsonProperty("FIsDefaultConsignee")]
		public string IsDefaultConsignee { get; set; }

		[JsonProperty("FIsDefaultSettle")]
		public string IsDefaultSettle { get; set; }

		[JsonProperty("FIsDefaultPayer")]
		public string IsDefaultPayer { get; set; }

		[JsonProperty("FIsUsed")]
		public string IsUsed { get; set; }
	}

	public class FTBDCUSTORDERORG {
		[JsonProperty("FEntryID")]
		public int EntryID { get; set; }

		[JsonProperty("FOrderOrgId")]
		[JsonConverter(typeof(FNumberConverter))]
		public string OrderOrgId { get; set; }

		[JsonProperty("FIsDefaultOrderOrg")]
		public string IsDefaultOrderOrg { get; set; }
	}

	public class FEmployee {
		[JsonProperty("FSTAFFNUMBER")]
		public string STAFFNUMBER { get; set; }
	}

	public class FAllocUser {
		[JsonProperty("FUSERACCOUNT")]
		public string USERACCOUNT { get; set; }
	}

	public class FCRMAllocation {
		[JsonProperty("FCooperationType")]
		public string CooperationType { get; set; }

		[JsonProperty("FEmployee")]
		public FEmployee Employee { get; set; }

		[JsonProperty("FDept")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string Dept { get; set; }

		[JsonProperty("FRead")]
		public string Read { get; set; }

		[JsonProperty("FModify")]
		public string Modify { get; set; }

		[JsonProperty("FDelete")]
		public string Delete { get; set; }

		[JsonProperty("FAllocation")]
		public string Allocation { get; set; }

		[JsonProperty("FMerger")]
		public string Merger { get; set; }

		[JsonProperty("FTrade")]
		public string Trade { get; set; }

		[JsonProperty("FAllocUser")]
		public FAllocUser AllocUser { get; set; }

		[JsonProperty("FAllocTime")]
		public string AllocTime { get; set; }
	}

	public class FReleventCUST {
		[JsonProperty("FEntryID")]
		public int EntryID { get; set; }

		[JsonProperty("FRelation")]
		public string Relation { get; set; }

		[JsonProperty("FRelaCustomer")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string RelaCustomer { get; set; }

		[JsonProperty("FRelaContact")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string RelaContact { get; set; }

		[JsonProperty("FRelaTel")]
		public string RelaTel { get; set; }

		[JsonProperty("FRelaRemark")]
		public string RelaRemark { get; set; }
	}

	public class Customer {
		[JsonProperty("FCUSTID")]
		public int CUSTID { get; set; }

		[JsonProperty("FCreateOrgId")]
		[JsonConverter(typeof(FNumberConverter))]
		public string CreatorOrgId { get; set; }

		[JsonProperty("FNumber")]
		public string Id { get; set; }

		[JsonProperty("FUseOrgId")]
		[JsonConverter(typeof(FNumberConverter))]
		public string UserOrgId { get; set; }

		[JsonProperty("FName")]
		public string Name { get; set; }

		[JsonProperty("FShortName")]
		public string ShortName { get; set; }

		[JsonProperty("FSELLER")]
		[JsonConverter(typeof(FNumberConverter))]
		public string Seller { get; set; }

		[JsonProperty("FCOUNTRY")]
		[JsonConverter(typeof(FNumberConverter))]
		public string COUNTRY { get; set; }

		[JsonProperty("FPROVINCIAL")]
		[JsonConverter(typeof(FNumberConverter))]
		public string PROVINCIAL { get; set; }

		[JsonProperty("FADDRESS")]
		public string ADDRESS { get; set; }

		[JsonProperty("FZIP")]
		public string ZIP { get; set; }

		[JsonProperty("FWEBSITE")]
		public string WEBSITE { get; set; }

		[JsonProperty("FTEL")]
		public string TEL { get; set; }

		[JsonProperty("FFAX")]
		public string FAX { get; set; }

		[JsonProperty("FCompanyClassify")]
		[JsonConverter(typeof(FNumberConverter))]
		public string CompanyClassify { get; set; }

		[JsonProperty("FCompanyNature")]
		[JsonConverter(typeof(FNumberConverter))]
		public string CompanyNature { get; set; }

		[JsonProperty("FCompanyScale")]
		[JsonConverter(typeof(FNumberConverter))]
		public string CompanyScale { get; set; }

		[JsonProperty("FINVOICETITLE")]
		public string INVOICETITLE { get; set; }

		[JsonProperty("FTAXREGISTERCODE")]
		public string TAXREGISTERCODE { get; set; }

		[JsonProperty("FINVOICEBANKNAME")]
		public string INVOICEBANKNAME { get; set; }

		[JsonProperty("FINVOICETEL")]
		public string INVOICETEL { get; set; }

		[JsonProperty("FINVOICEBANKACCOUNT")]
		public string INVOICEBANKACCOUNT { get; set; }

		[JsonProperty("FINVOICEADDRESS")]
		public string INVOICEADDRESS { get; set; }

		[JsonProperty("FSUPPLIERID")]
		[JsonConverter(typeof(FNumberConverter))]
		public string SUPPLIERID { get; set; }

		[JsonProperty("FIsGroup")]
		public string IsGroup { get; set; }

		[JsonProperty("FIsDefPayer")]
		public string IsDefPayer { get; set; }

		[JsonProperty("FCustTypeId")]
		[JsonConverter(typeof(FNumberConverter))]
		public string CustTypeId { get; set; }

		[JsonProperty("FGROUPCUSTID")]
		[JsonConverter(typeof(FNumberConverter))]
		public string GROUPCUSTID { get; set; }

		[JsonProperty("FGroup")]
		[JsonConverter(typeof(FNUMBERConverter))]
		public string Group { get; set; }

		[JsonProperty("FTRADINGCURRID")]
		[JsonConverter(typeof(FNumberConverter))]
		public string TRADINGCURRID { get; set; }

		[JsonProperty("FCorrespondOrgId")]
		[JsonConverter(typeof(FNumberConverter))]
		public string CorrespondOrgId { get; set; }

		[JsonProperty("FDescription")]
		public string Description { get; set; }

		[JsonProperty("FSETTLETYPEID")]
		[JsonConverter(typeof(FNumberConverter))]
		public string SETTLETYPEID { get; set; }

		[JsonProperty("FRECCONDITIONID")]
		[JsonConverter(typeof(FNumberConverter))]
		public string RECCONDITIONID { get; set; }

		[JsonProperty("FDISCOUNTLISTID")]
		[JsonConverter(typeof(FNumberConverter))]
		public string DISCOUNTLISTID { get; set; }

		[JsonProperty("FPRICELISTID")]
		[JsonConverter(typeof(FNumberConverter))]
		public string PRICELISTID { get; set; }

		[JsonProperty("FTRANSLEADTIME")]
		public int TRANSLEADTIME { get; set; }

		[JsonProperty("FInvoiceType")]
		public string InvoiceType { get; set; }

		[JsonProperty("FTaxType")]
		[JsonConverter(typeof(FNumberConverter))]
		public string TaxType { get; set; }

		[JsonProperty("FRECEIVECURRID")]
		[JsonConverter(typeof(FNumberConverter))]
		public string Currency { get; set; }

		[JsonProperty("FPriority")]
		public int Priority { get; set; }

		[JsonProperty("FTaxRate")]
		[JsonConverter(typeof(FNumberConverter))]
		public string TaxRate { get; set; }

		[JsonProperty("FISCREDITCHECK")]
		public string ISCREDITCHECK { get; set; }

		[JsonProperty("FIsTrade")]
		public string IsTrade { get; set; }

		[JsonProperty("FT_BD_CUSTOMEREXT")]
		public FTBDCUSTOMEREXT TBDCUSTOMEREXT { get; set; }

		[JsonProperty("FCRMCustomer")]
		public FCRMCustomer CRMCustomer { get; set; }

		[JsonProperty("FT_BD_CUSTLOCATION")]
		public List<FTBDCUSTLOCATION> FTBDCUSTLOCATION { get; set; }

		[JsonProperty("FT_BD_CUSTBANK")]
		public List<FTBDCUSTBANK> FTBDCUSTBANK { get; set; }

		[JsonProperty("FT_BD_CUSTCONTACT")]
		public List<FTBDCUSTCONTACT> FTBDCUSTCONTACT { get; set; }

		[JsonProperty("FT_BD_CUSTORDERORG")]
		public List<FTBDCUSTORDERORG> FTBDCUSTORDERORG { get; set; }

		[JsonProperty("FCRMAllocation")]
		public List<FCRMAllocation> FCRMAllocation { get; set; }

		[JsonProperty("FReleventCUST")]
		public List<FReleventCUST> FReleventCUST { get; set; }
	}
}