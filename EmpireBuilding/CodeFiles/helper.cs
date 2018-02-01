using Microsoft.Phone.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace EmpireBuilding.CodeFiles
{
    class helper
    {
        public static string CheckFiles(string fn, int WhichOne, double replace, string returnxml)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (WhichOne == 1)
                {
                    if (replace == 1.0) store.DeleteFile("User.xml");
                    if (store.FileExists(fn) == false)
                    {
                        var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
                        using (var file = appStorage.OpenFile(fn, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                        {
                            using (var writer = new StreamWriter(file))
                            {
                                XDocument loadedData = XDocument.Load("XMLData/" + fn);
                                //XDocument loadedData = XDocument.Load(fn);
                                writer.Write(loadedData);
                                return loadedData.ToString();
                            }
                        }
                    }
                    else if (store.FileExists(fn)) // Check if file exists
                    {
                        using (StreamReader sr = new StreamReader(store.OpenFile(fn, FileMode.OpenOrCreate, FileAccess.Read)))
                        {
                            string tempxml = sr.ReadToEnd();
                            return tempxml;
                        }
                    }
                    else return null;
                }
                else if (WhichOne == 2)
                {
                    store.DeleteFile(fn);
                    var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
                    using (var file = appStorage.OpenFile(fn, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                    {
                        using (var writer = new StreamWriter(file))
                        {
                            writer.Write(returnxml);
                            return null;
                        }
                    }
                }
                else if (WhichOne == 3)
                {
                    if (store.FileExists(fn) == true)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else return null;
            }
        }

        public static User LoadDevice(string userxml)
        {
            User ebUsers = new User();
            TextReader tr = new StringReader(userxml);
            XDocument xdoc = XDocument.Load(tr);

            ebUsers.DeviceID = xdoc.Element("UserInfo").Element("DeviceID").Value;

            if (ebUsers.DeviceID == "")
            {
                //object DeviceUniqueID;
                //byte[] DeviceIDbyte = null;
                byte[] DeviceIDbyte = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
                //string DeviceIDAsString = Convert.ToBase64String(myDeviceID);
                //if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out DeviceUniqueID))
                //    DeviceIDbyte = (byte[])DeviceUniqueID;
                string tDeviceID = Convert.ToBase64String(DeviceIDbyte);
                ebUsers.DeviceID = tDeviceID;
            }

            return ebUsers;
        }

        public static string CreateNewUser(string userxml, string version, string uname, int uid, double tempfc, double temppc, int cexp, double tempms, int bfu, int tig)
        {
            string newUserXML = string.Empty;
            string tempUserName = uname;
            int tempUserID = uid;
            double tempFreeCurrency = tempfc;
            double tempPaidCurrency = temppc;
            double tempMoneySpent = tempms;
            int tempCurrentExploredPlots = cexp;
            int tempBFUsed = bfu;
            int tempTIG = tig;
            string tempuserxml = string.Empty;

            // Create a new user
            newUserXML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<UserInfo version=\"" + version + "\">";
            newUserXML += "\n<User>";
            newUserXML += "\n<Name>" + tempUserName + "</Name>";
            newUserXML += "\n<ID>" + tempUserID + "</ID>";
            newUserXML += "\n<FreeCurrency>" + tempFreeCurrency + "</FreeCurrency>";
            newUserXML += "\n<PaidCurrency>" + tempPaidCurrency + "</PaidCurrency>";
            newUserXML += "\n<CurrentExploredPlots>" + tempCurrentExploredPlots + "</CurrentExploredPlots>";
            newUserXML += "\n<MoneySpent>" + tempCurrentExploredPlots + "</MoneySpent>";
            newUserXML += "\n<BioFuelUsed>" + tempCurrentExploredPlots + "</BioFuelUsed>";
            newUserXML += "\n<TimeInGame>" + tempCurrentExploredPlots + "</TimeInGame>";
            newUserXML += "\n<PlotsPlowed>0</PlotsPlowed>";
            newUserXML += "\n<PlotsPlanted>0</PlotsPlanted>";
            newUserXML += "\n<PlotsHarvested>0</PlotsHarvested>";
            newUserXML += "\n<MillEXP>0</MillEXP>";
            newUserXML += "\n<MillCount>0</MillCount>";
            newUserXML += "\n<MillPrice>0</MillPrice>";
            newUserXML += "\n<MillTime>240</MillTime>";
            newUserXML += "\n<MillCompleteDate></MillCompleteDate>";
            newUserXML += "\n<MillProcessing>0</MillProcessing>";
            newUserXML += "\n<MillCurrentSeed>0</MillCurrentSeed>";
            newUserXML += "\n<MillSeedProcessingCount>0</MillSeedProcessingCount>";
            newUserXML += "\n<GlobalBaseLocation>0|0</GlobalBaseLocation>";
            newUserXML += "\n<HomeTile>0</HomeTile>";
            newUserXML += "\n<HomeSector>0</HomeSector>";
            newUserXML += "\n</User>";
            newUserXML += "\n</UserInfo>";

            //string InsertStringtoFind = "</UserInfo>";
            //int InsertPoint = userxml.IndexOf(InsertStringtoFind);
            //string tempfirstxml = userxml.Substring(0, InsertPoint);
            //string tempsecondxml = userxml.Substring(InsertPoint);

            //string TheGeneratedXML = tempfirstxml + newUserXML + tempsecondxml;

            CheckFiles("User.xml", 2, 0, newUserXML); // Update the XML file on Phone
            tempuserxml = CheckFiles("User.xml", 1, 0, "");

            return tempuserxml;
        }

        public static User LoadUser(string userxml)
        {
            User ebUsers = new User();
            TextReader tr = new StringReader(userxml);
            XDocument xdoc = XDocument.Load(tr);

            ebUsers.UserID = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("ID").Value);
            ebUsers.UserName = xdoc.Element("UserInfo").Element("User").Element("Name").Value;

            return ebUsers;
        }

        public static XDocument LoadSeeds(string seedxml, int seedid)
        {
            XDocument tempSeed = new XDocument();



            return tempSeed;
        }

        public static string SaveUpdatedXML(XDocument xdoc, string tempXML)
        {
            string tString = string.Empty;

            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //if (replace == 1.0) store.DeleteFile("Gameboard.xml");
                store.DeleteFile(tempXML);

                using (Stream stream = store.CreateFile(tempXML))
                {
                    xdoc.Save(stream);
                }
            }

            tString = CheckFiles(tempXML, 1, 0, "");
            return tString;
        }

        public static void ManageFullMapXML(int WhichOption, string sectorxml)
        {
            // WhichOption: 1 => Create Full Map XML
            // WhichOption: 2 => Find and Update a Sector
            string tempfullmapxml = string.Empty;
            if (WhichOption == 1)
            {
                tempfullmapxml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
                tempfullmapxml += "<FullMap>";
                for (int i = 0; i < 100; i++)
                {
                    tempfullmapxml += "<Sector" + i + " IsLoaded=\"" + 0 + "\" LastModified=\"\"></Sector" + i + ">";
                }
                tempfullmapxml += "</FullMap>";

                CheckFiles("FullMap.xml", 2, 0, tempfullmapxml); // Update the XML file on Phone
            }
            else if (WhichOption == 2)
            {
                TextReader tr = new StringReader(sectorxml);
                XDocument xdoc = XDocument.Load(tr);

                int SectorID = Convert.ToInt32(xdoc.Element("Sector").Attribute("SectorID").Value);

                string fullmapxml = helper.CheckFiles("FullMap.xml", 1, 0, "");
                TextReader trFM = new StringReader(fullmapxml);
                XDocument FMxdoc = XDocument.Load(trFM);

                FMxdoc.Element("FullMap").SetElementValue("Sector" + SectorID, sectorxml);
                SaveUpdatedXML(FMxdoc, "FullMap.xml");
            }
        }

        public static void UpdateTile(XDocument XDoc, int PlotID, string nodename, string value, int AttributeORElement)
        {
            //XDoc.Element("Sector").Element("Tile")
        }

        public static XDocument LoadSectorXDoc(int SectorID)
        {
            XDocument tempXdoc = new XDocument();
            string tempstring = string.Empty;

            // Load Full Map String
            string tempfullmapxml = helper.CheckFiles("FullMap.xml", 1, 0, "");
            TextReader tr = new StringReader(tempfullmapxml);
            XDocument xdoc = XDocument.Load(tr);

            tempstring = xdoc.Element("FullMap").Element("Sector" + SectorID).Value;
            TextReader Newtr = new StringReader(tempstring);
            tempXdoc = XDocument.Load(Newtr);

            return tempXdoc;
        }

        public static Machine LoadMachine(XDocument TheMachinesXDoc, int WhichMachine)
        {
            Machine TempMachine = new Machine();

            TempMachine.Name = TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Attribute("Name").Value;
            TempMachine.ID = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Attribute("MachineID").Value);

            TempMachine.Level = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("Level").Value);
            TempMachine.TotalEXP = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("TotalEXP").Value);
            TempMachine.UseCount = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("UseCount").Value);
            TempMachine.Available = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("Available").Value);
            TempMachine.Price = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("Price").Value);
            TempMachine.ExecuteTime = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("ExecuteTime").Value);
            TempMachine.Title = TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("Title").Value;
            TempMachine.Image = TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("Image").Value;
            TempMachine.Speed = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("Speed").Value);
            TempMachine.Efficiency = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("Efficiency").Value);
            TempMachine.Equipment = Convert.ToInt32(TheMachinesXDoc.Element("Machinery").Element("Machine" + WhichMachine).Element("Equipment").Value);

            return TempMachine;

        }

        public static int GetLevel(int skill, int curEXP)
        {
            // Skill
            // 1 - Harvest
            // 2 - Plant
            // 3 - Explore
            // 4 - Plow
            // 5 - Mill

            double levelmultiplier = 1.25;
            long[] arrLevels = new long[100];
            arrLevels[0] = 0;
            int tLevel1 = 60;
            int tLevel2 = 400;
            int tReturnLevel = 1;

            if (skill == 1)
            {
                arrLevels[1] = 1000;
                arrLevels[2] = 100000;
            }
            else if (skill == 2)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 3)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 4)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 5)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }

            for (int i = 3; i < 100; i++)
            {
                arrLevels[i] = Convert.ToInt64(arrLevels[i - 1] + (arrLevels[i - 1] - arrLevels[i - 2]) * levelmultiplier);
            }

            for (int i = 0; i < 100; i++)
            {
                if (curEXP < arrLevels[i])
                {
                    tReturnLevel = i;
                    //if (skill == 1) this.HarvestLevel = i;
                    //if (skill == 2) this.PlantLevel = i;
                    //if (skill == 3) this.ExploreLevel = i;
                    //if (skill == 4) this.PlowLevel = i;
                    //if (skill == 5) this.MillLevel = i;
                    i = 100;
                }
            }

            return tReturnLevel;
        }

        public static long GetNextLevelXP(int skill, int CurLvl)
        {
            long tempNextLevelXP = 0;

            // Skill
            // 1 - Harvest
            // 2 - Plant
            // 3 - Explore
            // 4 - Plow
            // 5 - Mill

            double levelmultiplier = 1.25;
            long[] arrLevels = new long[100];
            arrLevels[0] = 0;
            int tLevel1 = 60;
            int tLevel2 = 400;

            if (skill == 1)
            {
                arrLevels[1] = 1000;
                arrLevels[2] = 100000;
            }
            else if (skill == 2)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 3)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 4)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 5)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }

            for (int i = 3; i < 100; i++)
            {
                arrLevels[i] = Convert.ToInt64(arrLevels[i - 1] + (arrLevels[i - 1] - arrLevels[i - 2]) * levelmultiplier);
            }

            tempNextLevelXP = arrLevels[CurLvl];

            return tempNextLevelXP;
        }

        public static Refinery LoadRefinery(XDocument tempXRef, int tBaseRefineTime, XDocument tempUpgradeXDoc)
        {
            Refinery tempRef = new Refinery();
            tempRef.MaxmG = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("MaxmG").Value);
            tempRef.RefineRate = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("RefineRate").Value);
            tempRef.UsedmG = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("miniGallonsUsed").Value);
            tempRef.RefinedmG = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("miniGallonsRefined").Value);
            tempRef.TotalCropRefined = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("CropRefinedLvl1").Value);
            tempRef.CropPermG = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("CropPermG").Value);
            tempRef.OutputmG = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("OutputmG").Value);
            tempRef.RefineRateLvl = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("RefineRateLvl").Value);
            tempRef.MaxmGLvl = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("MaxmGLvl").Value);
            tempRef.CropPermGLvl = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("CropPermGLvl").Value);
            tempRef.CropsinthisBatch = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("CropsinthisBatch").Value);
            tempRef.IsRefineryMaxed = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("IsRefineryMaxed").Value);
            
            string tempRefineStartDate = tempXRef.Element("Refinery").Element("Setting").Element("RefineStartDate").Value;
            if (tempRefineStartDate != "") { tempRef.RefineStartDate = Convert.ToDateTime(tempXRef.Element("Refinery").Element("Setting").Element("RefineStartDate").Value); }
            string tempRefineCompleteDate = tempXRef.Element("Refinery").Element("Setting").Element("RefineCompleteDate").Value;
            if (tempRefineStartDate != "") { tempRef.RefineCompleteDate = Convert.ToDateTime(tempXRef.Element("Refinery").Element("Setting").Element("RefineCompleteDate").Value); }
            string tempLastRefineDate = tempXRef.Element("Refinery").Element("Setting").Element("LastRefineDate").Value;
            if (tempLastRefineDate != "") { tempRef.LastRefineDate = Convert.ToDateTime(tempXRef.Element("Refinery").Element("Setting").Element("LastRefineDate").Value); }
            //tempRef.mG = (tempRef.RefinedmG - tempRef.UsedmG);
            tempRef.RefineryStatus = "Idle2";
            tempRef.mG = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("mG").Value);
            tempRef.mGDisplay = tempRef.mG + " mG";
            tempRef.CropinRefinery = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element("CropinRefinery").Value);
            int tRefineTime = Convert.ToInt32(Math.Ceiling((double)(tBaseRefineTime / tempRef.RefineRate)));
            tempRef.CropPerSec = (tempRef.CropPermG / tRefineTime);
            TimeSpan TSRefinery1 = new TimeSpan(0, 0, (tempRef.CropPermG / tempRef.CropPerSec));
            tempRef.SecsPerBatch = TSRefinery1.ToString(@"hh\:mm\:ss");
            tempRef.SecsPerBatchLeft = (tempRef.CropPermG / tempRef.CropPerSec).ToString();

            TimeSpan TSRefinery = new TimeSpan(0, 0, tempRef.CropPerSec);
            tempRef.CropPerSecDisplay = TSRefinery.ToString(@"hh\:mm\:ss");

            // Show Upgrade Text
            string WhichProp = "RefineRate";
            int tRRLvl = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element(WhichProp + "Lvl").Value);
            long UpgradeFree = Convert.ToInt64(tempUpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (tRRLvl + 1)).Attribute("UpgradeFree").Value);
            long UpgradeValue = Convert.ToInt64(tempUpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (tRRLvl + 1)).Attribute("UpgradeValue").Value);
            tempRef.RefineRateFCC = (UpgradeFree).ToString("C");
            tempRef.RefineRateNextLvlDisplay = WhichProp + " + " + UpgradeValue;
            WhichProp = "MaxmG";
            int tMMLvl = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element(WhichProp + "Lvl").Value);
            UpgradeFree = Convert.ToInt64(tempUpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (tMMLvl + 1)).Attribute("UpgradeFree").Value);
            UpgradeValue = Convert.ToInt64(tempUpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (tMMLvl + 1)).Attribute("UpgradeValue").Value);
            tempRef.MaxmGFCC = (UpgradeFree).ToString("C");
            tempRef.MaxmGNextLvlDisplay = WhichProp + " + " + UpgradeValue;
            WhichProp = "CropPermG";
            int tCPM = Convert.ToInt32(tempXRef.Element("Refinery").Element("Setting").Element(WhichProp + "Lvl").Value);
            UpgradeFree = Convert.ToInt64(tempUpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (tCPM + 1)).Attribute("UpgradeFree").Value);
            UpgradeValue = Convert.ToInt64(tempUpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (tCPM + 1)).Attribute("UpgradeValue").Value);
            tempRef.CropPermGFCC = (UpgradeFree).ToString("C");
            tempRef.CropPermGNextLvlDisplay = WhichProp + " + " + UpgradeValue;

            return tempRef;
        }

    }
}
