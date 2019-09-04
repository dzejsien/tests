using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CheckSomeCode
{
    /*
     *                                                                                  
     *|          Method |        Level |      Mean |     Error |    StdDev |    Median |
     *|---------------- |------------- |----------:|----------:|----------:|----------:|
     *| GetFromHashSets | Amortization | 67.668 ns | 2.3091 ns | 6.6252 ns | 65.438 ns |
     *|     GetFromDict | Amortization |  7.518 ns | 0.3745 ns | 1.0865 ns |  7.142 ns |
     *| GetFromHashSets |      Product | 79.147 ns | 1.5187 ns | 2.6994 ns | 78.484 ns |
     *|     GetFromDict |      Product |  6.921 ns | 0.1707 ns | 0.3602 ns |  6.788 ns |
     *| GetFromHashSets |     Tag test | 59.069 ns | 1.1780 ns | 1.2604 ns | 58.989 ns |
     *|     GetFromDict |     Tag test |  7.568 ns | 0.2424 ns | 0.6837 ns |  7.519 ns |
     *| GetFromHashSets |  Vendor Name | 37.379 ns | 0.7780 ns | 2.0630 ns | 36.978 ns |
     *|     GetFromDict |  Vendor Name |  7.016 ns | 0.1719 ns | 0.1524 ns |  7.013 ns |
     *
     */
    public class CheckManySetsVsDict
    {
        private static IList<LevelInfo> _levelInfos;
        private static IList<LevelInfo2> _levelInfos2;

        static CheckManySetsVsDict()
        {
            _levelInfos = new List<LevelInfo>();

            for (int i = 0; i < 1000; i++)
            {
                _levelInfos.Add(new LevelInfo(GroupStructurePropertyNames.Amortization, StructureItems.GetLevelType(GroupStructurePropertyNames.Amortization), StructureItems.GetWbsLevel(GroupStructurePropertyNames.Amortization)));
                _levelInfos.Add(new LevelInfo(GroupStructurePropertyNames.VendorName, StructureItems.GetLevelType(GroupStructurePropertyNames.VendorName), StructureItems.GetWbsLevel(GroupStructurePropertyNames.VendorName)));
                _levelInfos.Add(new LevelInfo(GroupStructurePropertyNames.Product, StructureItems.GetLevelType(GroupStructurePropertyNames.Product), StructureItems.GetWbsLevel(GroupStructurePropertyNames.Product)));
                _levelInfos.Add(new LevelInfo("tag coll", StructureItems.GetLevelType("tag coll"), StructureItems.GetWbsLevel("tag coll")));
            }

            _levelInfos2 = new List<LevelInfo2>();

            for (int i = 0; i < 1000; i++)
            {
                _levelInfos2.Add(new LevelInfo2(GroupStructurePropertyNames.Amortization));
                _levelInfos2.Add(new LevelInfo2(GroupStructurePropertyNames.VendorName));
                _levelInfos2.Add(new LevelInfo2(GroupStructurePropertyNames.Product));
                _levelInfos2.Add(new LevelInfo2("tag coll"));
            }
        }

        //[Params(GroupStructurePropertyNames.Amortization, GroupStructurePropertyNames.VendorName, GroupStructurePropertyNames.Product, "Tag test")]
        public string Level;

        //[Benchmark]
        public LevelType GetFromHashSets()
        {
            return StructureItems.GetLevelType(Level);
        }

        //[Benchmark]
        public LevelType GetFromDict()
        {
            return StructureItems.GetLevelType2(Level);
        }

        [Benchmark]
        public IList<LevelInfo> FilterLevelInfo()
        {
            return _levelInfos.Where(x => x.LevelType == LevelType.CommodityProperty).ToList();
        }

        [Benchmark]
        public  IList<LevelInfo2> FilterLevelInfo2()
        {
            return _levelInfos2.Where(x => x.GetLevelType() == LevelType.CommodityProperty).ToList();
        }
    }

    public readonly struct LevelInfo
    {
        public LevelInfo(string level, LevelType levelType, WbsLevel wbsLevel)
        {
            Level = level;
            LevelType = levelType;
            WbsLevel = wbsLevel;
        }

        public string Level { get; }
        public LevelType LevelType { get; }
        public WbsLevel WbsLevel { get; }
    }

    public readonly struct LevelInfo2
    {
        public LevelInfo2(string level)
        {
            Level = level;
        }

        public string Level { get; }

        public LevelType GetLevelType()
        {
            return StructureItems.GetLevelType2(Level);
        }

        public WbsLevel GetWbsLevel()
        {
            return StructureItems.GetWbsLevel(Level);
        }
    }

    public enum WbsLevel : byte
    {
        Unknown = 0,
        Bid = 1,
        ClientCountry = 2,
        Business = 3,
        Family = 4,
        Offering = 5,
        Case = 6,
        Cluster = 7,
        Pool = 8,
        CostGroup = 9,
        Element = 10,
        CostAttribute = 11
    }

    public enum LevelType : byte
    {
        Unknown = 0,
        Tag = 1,
        CommodityProperty = 2,
        ElementProperty = 3
    }

    public static class StructureItems
    {
        private static readonly ISet<string> CommodityLevels = new HashSet<string>(GroupStructurePropertyNames.CommodityLevels);
        private static readonly ISet<string> ElementLevels = new HashSet<string>(GroupStructurePropertyNames.ElementLevels);
        private static readonly ISet<string> OtherLevels = new HashSet<string>(GroupStructurePropertyNames.All.Except(CommodityLevels.Concat(ElementLevels)));

        private static readonly IReadOnlyDictionary<string, WbsLevel> WbsLevelsByName = new Dictionary<string, WbsLevel>
        {
            { GroupStructurePropertyNames.Bid, WbsLevel.Bid },
            { GroupStructurePropertyNames.ClientCountry, WbsLevel.ClientCountry },
            { GroupStructurePropertyNames.Business, WbsLevel.Business },
            { GroupStructurePropertyNames.Family, WbsLevel.Family },
            { GroupStructurePropertyNames.Product, WbsLevel.Offering },
            { GroupStructurePropertyNames.Service, WbsLevel.Case },
            { GroupStructurePropertyNames.Activity, WbsLevel.Cluster },
            { GroupStructurePropertyNames.Feature, WbsLevel.Pool },
            { GroupStructurePropertyNames.Commodity, WbsLevel.CostGroup },
            { GroupStructurePropertyNames.Element, WbsLevel.Element },
            { GroupStructurePropertyNames.CostAttribute, WbsLevel.CostAttribute },
        };

        private static readonly IReadOnlyDictionary<string, LevelType> LevelTypeByName =
            new Dictionary<string, LevelType>();

        static StructureItems()
        {
            var dict = new Dictionary<string, LevelType>();

            foreach (var level in GroupStructurePropertyNames.CommodityLevels)
            {
                dict.Add(level, LevelType.CommodityProperty);
            }

            foreach (var level in GroupStructurePropertyNames.ElementLevels)
            {
                dict.Add(level, LevelType.ElementProperty);
            }

            foreach (var level in GroupStructurePropertyNames.All.Except(CommodityLevels.Concat(ElementLevels)))
            {
                dict.Add(level, LevelType.Unknown);
            }

            LevelTypeByName = dict;
        }

        public static LevelType GetLevelType(string levelName)
        {
            if (ElementLevels.Contains(levelName))
                return LevelType.ElementProperty;

            if (CommodityLevels.Contains(levelName))
                return LevelType.CommodityProperty;

            if (OtherLevels.Contains(levelName))
                return LevelType.Unknown;

            return LevelType.Tag;
        }

        public static LevelType GetLevelType2(string levelName)
        {
            if (LevelTypeByName.TryGetValue(levelName, out var value))
                return value;

            return LevelType.Tag;
        }

        public static WbsLevel GetWbsLevel(string levelName)
        {
            if (WbsLevelsByName.TryGetValue(levelName, out var value))
                return value;

            return WbsLevel.Unknown;
        }
    }

    public static class GroupStructurePropertyNames
    {
        public static IEnumerable<string> Wbs
        {
            get
            {
                yield return Bid;
                yield return Country;
                yield return Business;
                yield return Family;
                yield return Product;
                yield return Service;
                yield return Activity;
                yield return Feature;
                yield return Commodity;
            }
        }

        public static IEnumerable<string> CommodityLevels
        {
            get
            {
                yield return Amortization;
                yield return ApplyAmortization;
                yield return BillingException;
                yield return CommodityDataSource;
                yield return CommodityStartDate;
                yield return CommodityEndDate;
                yield return CommodityType;
                yield return CostClass;
                yield return DeliveryPhase;
                yield return QuantityType;
            }
        }

        public static IEnumerable<string> ElementLevels
        {
            get
            {
                yield return DeliverFromCountry;
                yield return DeliverFromCurrency;
                yield return DeliveryFromCountryDxcRegion;
                yield return Source;
                yield return VendorName;
                yield return ElementDataSource;
                yield return ProductiveHoursPerDay;
                yield return ProductiveHoursPerYear;
                yield return MovementType;
                yield return ElementName;
                yield return JobCode;
                yield return DabVersion;
            }
        }

        public static IEnumerable<string> All
        {
            get
            {
                yield return Bid;
                yield return Country;
                yield return Business;
                yield return Family;
                yield return Product;
                yield return Service;
                yield return Activity;
                yield return Feature;
                yield return Commodity;

                yield return Element;
                yield return CostAttribute;

                yield return ClientCountry;
                yield return ClientCountryCurrency;
                yield return CostClass;
                yield return CommodityType;
                yield return QuantityType;
                yield return DeliveryPhase;
                yield return DeliverFromCountry;
                yield return DeliverFromCurrency;
                yield return ElementName;
                yield return MovementType;
                yield return JobCode;
                yield return CommodityStartDate;
                yield return CommodityEndDate;
                yield return Shoring;
                yield return InOutOfScope;
                yield return InvoiceCountry;
                yield return InvoiceCountryCurrency;
                yield return OwningCountry;
                yield return OwningCountryCurrency;
                yield return ClientCountryDxcRegion;
                yield return InvoicingCountryDxcRegion;
                yield return OwningCountryDxcRegion;
                yield return DeliveryFromCountryDxcRegion;
                yield return CommodityDataSource;
                yield return AgencyLease;
                yield return Amortization;
                yield return ApplyAmortization;
                yield return BillingException;
                yield return ProcurementMethod;
                yield return GoLiveDate;
                yield return HardwareLifespan;
                yield return UpgradeCycle;
                yield return Source;
                yield return VendorName;
                yield return ElementDataSource;
                yield return ProductiveHoursPerDay;
                yield return ProductiveHoursPerYear;
                yield return Domain;
            }
        }
        public static IEnumerable<string> Levels
        {
            get
            {
                yield return Bid;
                yield return Country;
                yield return Business;
                yield return Family;
                yield return Product;
                yield return Service;
                yield return Activity;
                yield return Feature;
                yield return Commodity;

                yield return Element;
                yield return CostAttribute;
            }
        }

        public const string Bid = "Bid";
        public const string Country = "Country Name";
        public const string Business = "Business";
        public const string Family = "Family";
        public const string Product = "Product";
        public const string Service = "Service";
        public const string Activity = "Activity";
        public const string Feature = "Feature";
        public const string Commodity = "Commodity/Entity";

        public const string Element = "Element Description";
        public const string CostAttribute = "Cost Attribute";

        public const string ClientCountry = "Client Country";
        public const string ClientCountryCurrency = "Client Country Currency";
        public const string CostClass = "Cost Class";
        public const string CommodityType = "Commodity/Entity Type";
        public const string QuantityType = "Quantity Type";
        public const string DeliveryPhase = "Delivery Phase";
        public const string DeliverFromCountry = "Deliver From Country";
        public const string DeliverFromCurrency = "Deliver From Country Currency";
        public const string JobCode = "Job Code";
        public const string ElementName = "Element Name";
        public const string DabVersion = "DAB Version";
        public const string MovementType = "Movement Type";
        public const string CommodityStartDate = "Commodity Start Date";
        public const string CommodityEndDate = "Commodity End Date";
        public const string Shoring = "Shoring";
        public const string InOutOfScope = "In/Out of scope";
        public const string InvoiceCountry = "Invoice Country";
        public const string InvoiceCountryCurrency = "Invoice Country Currency";
        public const string OwningCountry = "Owning Country";
        public const string OwningCountryCurrency = "Owning Country Currency";
        public const string ClientCountryDxcRegion = "Client Country - Corp Region";
        public const string InvoicingCountryDxcRegion = "Invoice Country - Corp Region";
        public const string OwningCountryDxcRegion = "Owning Country - Corp Region";
        public const string DeliveryFromCountryDxcRegion = "Deliver From Country - Corp Region";
        public const string CommodityDataSource = "Data Source (Commodity)";
        public const string AgencyLease = "Agency Lease";
        public const string Amortization = "Amortization";
        public const string ApplyAmortization = "Apply Amortization";
        public const string BillingException = "Billing Exception";
        public const string ProcurementMethod = "Procurement Method";
        public const string GoLiveDate = "Go Live Date";
        public const string HardwareLifespan = "Hardware Lifespan";
        public const string UpgradeCycle = "Upgrade Cycle";
        public const string Source = "Source";
        public const string VendorName = "Vendor Name";
        public const string ElementDataSource = "Data Source (Element)";
        public const string ProductiveHoursPerDay = "Productive Hours/Day";
        public const string ProductiveHoursPerYear = "Productive Hours/Year";
        public const string Domain = "Domain";

        public const string Baseline = "Baseline";
    }
}