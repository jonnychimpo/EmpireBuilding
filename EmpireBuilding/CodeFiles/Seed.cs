using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmpireBuilding.CodeFiles
{
    public class Seed
    {
        private int pvSeedID;
        public int SeedID
        {
            get { return pvSeedID; }
            set { pvSeedID = value; }
        }

        private string pvSeedName;
        public string SeedName
        {
            get { return pvSeedName; }
            set { pvSeedName = value; }
        }

        private int pvSeedGrowthRate;
        public int SeedGrowthRate
        {
            get { return pvSeedGrowthRate; }
            set { pvSeedGrowthRate = value; }
        }

        private int pvSeedGrowthRateModified;
        public int SeedGrowthRateModified
        {
            get { return pvSeedGrowthRateModified; }
            set { pvSeedGrowthRateModified = value; }
        }

        private double pvSeedValue;
        public double SeedValue
        {
            get { return pvSeedValue; }
            set { pvSeedValue = value; }
        }

        private double pvSeedValueModified;
        public double SeedValueModified
        {
            get { return pvSeedValueModified; }
            set { pvSeedValueModified = value; }
        }

        private double pvSeedQuality;
        public double SeedQuality
        {
            get { return pvSeedQuality; }
            set { pvSeedQuality = value; }
        }

        private double pvSeedQualityModified;
        public double SeedQualityModified
        {
            get { return pvSeedQualityModified; }
            set { pvSeedQualityModified = value; }
        }

        private int pvNativePlotType;
        public int NativePlotType
        {
            get { return pvNativePlotType; }
            set { pvNativePlotType = value; }
        }

        private int pvNativeBonus;
        public int NativeBonus
        {
            get { return pvNativeBonus; }
            set { pvNativeBonus = value; }
        }

        private string pvFlowerImage;
        public string FlowerImage
        {
            get { return pvFlowerImage; }
            set { pvFlowerImage = value; }
        }

        private int pvPlantableLevel;
        public int PlantableLevel
        {
            get { return pvPlantableLevel; }
            set { pvPlantableLevel = value; }
        }

        private int pvBonusEntries;
        public int BonusEntries
        {
            get { return pvBonusEntries; }
            set { pvBonusEntries = value; }
        }

        private int pvHarvestPerpoint;
        public int HarvestPerpoint
        {
            get { return pvHarvestPerpoint; }
            set { pvHarvestPerpoint = value; }
        }

        private int pvBaseHPP;
        public int BaseHPP
        {
            get { return pvBaseHPP; }
            set { pvBaseHPP = value; }
        }

        private int pvPlantedEXP;
        public int PlantedEXP
        {
            get { return pvPlantedEXP; }
            set { pvPlantedEXP = value; }
        }

        private int pvMaxPlantSize;
        public int MaxPlantSize
        {
            get { return pvMaxPlantSize; }
            set { pvMaxPlantSize = value; }
        }

        private string pvRarity;
        public string Rarity
        {
            get { return pvRarity; }
            set { pvRarity = value; }
        }

        private int pvFortitude;
        public int Fortitude
        {
            get { return pvFortitude; }
            set { pvFortitude = value; }
        }

        private int pvFortitudeModified;
        public int FortitudeModified
        {
            get { return pvFortitudeModified; }
            set { pvFortitudeModified = value; }
        }

        private int pvDecayRate;
        public int DecayRate
        {
            get { return pvDecayRate; }
            set { pvDecayRate = value; }
        }

        public Seed(XDocument seed2load, int seedid)
        {
            this.SeedID = seedid;
            this.SeedName = seed2load.Element("AllSeeds").Element("Seed" + seedid).Attribute("Name").Value;
            this.SeedGrowthRate = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("SeedGrowthRate").Value);
            this.SeedGrowthRateModified = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("SeedGrowthRateModified").Value);
            this.SeedValue = Convert.ToDouble(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("SeedValue").Value);
            this.SeedValueModified = Convert.ToDouble(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("SeedValueModified").Value);
            this.SeedQuality = Convert.ToDouble(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("SeedQuality").Value);
            this.SeedQualityModified = Convert.ToDouble(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("SeedValueModified").Value);
            this.FlowerImage = seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("FlowerImage").Value;
            this.PlantableLevel = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("PlantableLevel").Value);
            this.BonusEntries = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("BonusEntries").Value);
            this.TotalPlantedCount = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("TotalPlantedCount").Value);
            this.TotalTimesHarvest = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("TotalTimesHarvest").Value);
            this.TotalHarvestYield = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("TotalHarvestYield").Value);
            this.TotalPoints = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("TotalPoints").Value);
            this.PointsUsed = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("PointsUsed").Value);
            this.HarvestPerPoint = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("HarvestPerPoint").Value);
            this.BaseHPP = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("BaseHPP").Value);
            this.Points4Growth = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("Points4Growth").Value);
            this.Points4Value = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("Points4Value").Value);
            this.Points4Quality = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("Points4Quality").Value);
            this.PlantedEXP = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("PlantedEXP").Value);
            this.MaxPlantSize = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("MaxPlantSize").Value);
            this.Rarity = seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("Rarity").Value;
            this.Fortitude = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("Fortitude").Value);
            this.FortitudeModified = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("FortitudeModified").Value);
            this.DecayRate = Convert.ToInt32(seed2load.Element("AllSeeds").Element("Seed" + seedid).Element("DecayRate").Value);
        }

        public Seed() { }

        private int pvTotalPlantedCount;
        public int TotalPlantedCount
        {
            get { return pvTotalPlantedCount; }
            set { pvTotalPlantedCount = value; }
        }

        private int pvTotalTimesHarvest;
        public int TotalTimesHarvest
        {
            get { return pvTotalTimesHarvest; }
            set { pvTotalTimesHarvest = value; }
        }

        private int pvTotalHarvestYield;
        public int TotalHarvestYield
        {
            get { return pvTotalHarvestYield; }
            set { pvTotalHarvestYield = value; }
        }

        private int pvTotalPoints;
        public int TotalPoints
        {
            get { return pvTotalPoints; }
            set { pvTotalPoints = value; }
        }

        private int pvPointsUsed;
        public int PointsUsed
        {
            get { return pvPointsUsed; }
            set { pvPointsUsed = value; }
        }

        private int pvHarvestPerPoint;
        public int HarvestPerPoint
        {
            get { return pvHarvestPerPoint; }
            set { pvHarvestPerPoint = value; }
        }

        private int pvPoints4Growth;
        public int Points4Growth
        {
            get { return pvPoints4Growth; }
            set { pvPoints4Growth = value; }
        }

        private int pvPoints4Quality;
        public int Points4Quality
        {
            get { return pvPoints4Quality; }
            set { pvPoints4Quality = value; }
        }

        private int pvPoints4Value;
        public int Points4Value
        {
            get { return pvPoints4Value; }
            set { pvPoints4Value = value; }
        }

    }
}
