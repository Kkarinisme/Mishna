﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Decal;
using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Decal.Filters;
using Decal.Adapter.NetParser;
using Decal.Adapter.Messages;
using System.Xml.Serialization;
using VirindiViewService;
using MyClasses.MetaViewWrappers;
using WindowsTimer = System.Windows.Forms.Timer;


namespace Mishna
{
    public partial class PluginCore : PluginBase
    {

        public static class Util
        {
            public static void LogError(Exception ex)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\" + Globals.PluginName + " errors.txt", true))
                    {
                        writer.WriteLine("============================================================================");
                        writer.WriteLine(DateTime.Now.ToString());
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("Source: " + ex.Source);
                        writer.WriteLine("Stack: " + ex.StackTrace);
                        if (ex.InnerException != null)
                        {
                            writer.WriteLine("Inner: " + ex.InnerException.Message);
                            writer.WriteLine("Inner Stack: " + ex.InnerException.StackTrace);
                        }
                        writer.WriteLine("============================================================================");
                        writer.WriteLine("");
                        writer.Close();
                    }
                }
                catch
                {
                }
            }


            public static void WriteToChat(string message)
            {
                try
                {
                    Globals.Host.Actions.AddChatText("<{" + Globals.PluginName + "}>: " + message, 5);
                }
                catch (Exception ex) { LogError(ex); }
            }
        }

        private void SaveSettings()
        {
            try
            {
                xdocGenSettings = new XDocument(new XElement("Settings"));
                xdocGenSettings.Element("Settings").Add(new XElement("Setting",
                         new XElement("QuickSlotsvEnabled", bquickSlotsvEnabled),
                         new XElement("QuickSlotshEnabled", bquickSlotshEnabled),
                         new XElement("VpointX", vpt.X),
                         new XElement("VpointY", vpt.Y),
                         new XElement("HpointX", hpt.X),
                         new XElement("HpointY", hpt.Y),
                         new XElement("InventoryEnabled", binventoryEnabled),
                         new XElement("InventoryBurdenEnabled", binventoryBurdenEnabled),
                         new XElement("InventoryCompleteEnabled", binventoryCompleteEnabled),
                         new XElement("ToonStatsEnabled", btoonStatsEnabled)));
                        // new XElement("ToonArmorEnabled", btoonArmorEnabled),
                 xdocGenSettings.Save(genSettingsFilename);

            }
             catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }
		

        // need a path to the world of the current toon
        public void setPathToWorld()
        {
            world = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\" + Globals.PluginName + @"\" + world);
        }

        // need a path for saving Toon files
        public void setPathToToon()
        {
            pathToToon = world + @"\" + toonName;
            currDir = pathToToon;
        }

        // need to be certain above directories exist
        public void chkDirExists()
        {
            if (!Directory.Exists(currDir))
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(currDir);
                //    DirectoryInfo dirInfoToon = Directory.CreateDirectory(pathToToon);
            }
        }

        private void doGetInventory()
        {
            try
            {
              //  xdoc = new XDocument(new XElement("Objs"));
                //Need a list to hold the inventory
               mWaitingForIDTimer = new WindowsTimer();
                mWaitingForID = new List<WorldObject>();
                mCurrID = new List<string>();
                if (xdocGenInventory == null)
                {
                    if (!File.Exists(genInventoryFilename))
                    {
                        xdocGenInventory = new XDocument(new XElement("Objs"));

                    }
                    else
                    { xdocGenInventory = XDocument.Load(genInventoryFilename); }
                }


                // if no toon previously inventoried in this server need to begin a general inventory file (genInventoryFilename)
 
                foreach (Decal.Adapter.Wrappers.WorldObject obj in Core.WorldFilter.GetInventory())
                {
                    try
                    {
                        objID = obj.Id;
                        string sobjID = objID.ToString();
                        mCurrID.Add(sobjID);
                        Globals.Host.Actions.RequestId(obj.Id);
                        mWaitingForID.Add(obj);
                    }
                    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


                } // endof foreach world object

                Mishna.PluginCore.Util.WriteToChat("Items being inventoried " + mWaitingForID.Count);

               ProcessDataInventory();

               // initialize event timer for processing inventory
               mWaitingForIDTimer.Tick += new EventHandler(TimerEventProcessor);

               // Sets the timer interval to 5 seconds.
               mWaitingForIDTimer.Interval = 10000;


                
                //Now need to start routines that will continue to get data as becomes available or will end the search and save the files
                fn = "inventoryFilename";
                mIsFinished();  //This routine used both by the inventory and by the armor routines

            } //end of try
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        } // end of dogetinventory


 

        public void mIsFinished()
        {
            try
            {

                int n = mWaitingForID.Count;
                string s = n.ToString();
                if (n < m)
                {
                    Mishna.PluginCore.Util.WriteToChat(s);
                    m = n;
                    string mname = null;


                    if (mWaitingForID.Count > 0)
                    {
                        if (binventoryWaitingEnabled)
                        {
                            for (int i = 0; i < n; i++)
                            {
                                mname = mWaitingForID[i].Name;
                                Mishna.PluginCore.Util.WriteToChat(mname);

                            }
                        }
                        mDoWait();
                    }

                    else
                    {
                        if (fn == "inventoryFilename")
                        {
                            try
                            {


                                //xdoc.Save(inventoryFilename);
                                //xdoc = null;
                                removeExcessObjsfromFile();


                                removeToonfromFile();

                                xdocGenInventory.Root.Add(XDocument.Load(inventoryFilename).Root.Elements());
                                xdocGenInventory.Save(genInventoryFilename);
                                Mishna.PluginCore.Util.WriteToChat("General Inventory file has been saved. ");
                            }
                            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

                        }
                        else
                        {
                            try
                            {

                                xdoc.Save(armorFilename);
                                xdoc = null;

                             //   removeToonfromFile();

                                var MyDoc = XDocument.Load(genArmorFilename);
                                MyDoc.Root.Add(XDocument.Load(armorFilename).Root.Elements());

                                MyDoc.Save(holdingArmorFilename);
                                MyDoc = null;

                                xdoc = XDocument.Load(holdingArmorFilename);
                                xdoc.Save(genArmorFilename);
                                Mishna.PluginCore.Util.WriteToChat("General Armor file has been saved. ");
                            }
                            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

                        }
                        Mishna.PluginCore.Util.WriteToChat("File " + fn + " saved in directory " + currDir);

                        m = 500;
                        k = 0;
                        n = 0;
                        mWaitingForID = null;
                        xdoc = null;
                        fn = null;

                    }
                }
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }



        public void TimerEventProcessor(Object Sender, EventArgs mWaitingForIDTimer_Tick)

        {
            try
            {


                mWaitingForIDTimer.Stop();



                for (int n = 0; n < mWaitingForID.Count; n++)
                {
 
                    if (mWaitingForID[n].HasIdData)
                    {
                      //  bidentRecd = false;
                        ProcessDataInventory();
                        mIsFinished();
                    }
                    else
                    { mDoWait(); }
                }



                //if (bidentRecd)
                //{

                //    bidentRecd = false;
                //    ProcessDataInventory();
                //    mIsFinished();
                //}
                //else
                //{ mDoWait(); }



            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }



        public void mDoWait()
        {

            mWaitingForIDTimer.Start();

        }

        //This is routine that puts the data of an obj into the inventory file xml
        private void ProcessDataInventory()
        {
            for (int n = 0; n < mWaitingForID.Count; n++)
            {
                try
                {

                    currentobj = mWaitingForID[n];

                    if (mWaitingForID[n].HasIdData)
                    {
                        currentobj = mWaitingForID[n];
                        mWaitingForID.Remove(mWaitingForID[n]);
                        //   if ((fn == "armorFilename") && (currentobj.Values(LongValueKey.Imbued) == 0)) { break; }
                        objClassName = currentobj.ObjectClass.ToString();
                        objName = currentobj.Name;
                        objID = currentobj.Id;
        
 //                       goGetProtocol(currentobj);
             
                        objIcon = currentobj.Icon;
                        Type t = objIcon.GetType();

                        long objDesc = currentobj.Values(LongValueKey.DescriptionFormat);
                        long objMat = currentobj.Values(LongValueKey.Material);
                        long objCatType = (int)currentobj.Values(LongValueKey.Category);
                        long objAtt = (int)currentobj.Values(LongValueKey.Attuned);
                        long objBnd = (int)currentobj.Values(LongValueKey.Bonded);
                        long objToonLevel = (int)currentobj.Values(LongValueKey.MinLevelRestrict);
                        long objLore = (int)currentobj.Values(LongValueKey.LoreRequirement);
                        long objAl = (int)currentobj.Values(LongValueKey.ArmorLevel);
                        long objType = (int)currentobj.Values(LongValueKey.Type);
                        long objTinks = (int)currentobj.Values(LongValueKey.NumberTimesTinkered);
                        long objWork = (int)currentobj.Values(LongValueKey.Workmanship);
                        long objSet = (int)currentobj.Values(LongValueKey.ArmorSet);
                        long objCovers = currentobj.Values(LongValueKey.Coverage);
                        long objEqSlot = currentobj.Values(LongValueKey.EquipableSlots);
                        long objCleaveType = (int)currentobj.Values(LongValueKey.CleaveType);
                        long objElemDmg = (int)currentobj.Values(LongValueKey.ElementalDmgBonus);
                        long objEmbue = (int)currentobj.Values(LongValueKey.Imbued);
                        long objSlayer = (int)currentobj.Values(LongValueKey.SlayerSpecies);
                        long objWieldAttrInt = (int)currentobj.Values(LongValueKey.WieldReqAttribute);
                        long objWieldType = (int)currentobj.Values(LongValueKey.WieldReqType);
                        long objWieldValue = (int)currentobj.Values(LongValueKey.WieldReqValue);
                        long objDamage = (int)currentobj.Values(LongValueKey.DamageType);
                        long objMissType = (int)currentobj.Values(LongValueKey.MissileType);
                        long objSkillLevReq = (int)currentobj.Values(LongValueKey.SkillLevelReq);
                        double objElemvsMons = currentobj.Values(DoubleValueKey.ElementalDamageVersusMonsters);
                        double objMelD = currentobj.Values(DoubleValueKey.MeleeDefenseBonus);
                        double objMagicD = currentobj.Values(DoubleValueKey.MagicDBonus);
                        double objManaC = currentobj.Values(DoubleValueKey.ManaCBonus);
                        double objMissileD = currentobj.Values(DoubleValueKey.MissileDBonus);
                        double objSalvWork = currentobj.Values(DoubleValueKey.SalvageWorkmanship);
                        double objAttack = currentobj.Values(DoubleValueKey.AttackBonus);
                        double objDamageBonus = currentobj.Values(DoubleValueKey.DamageBonus);
                        double objAcid = currentobj.Values(DoubleValueKey.AcidProt);
                        double objLight = currentobj.Values(DoubleValueKey.LightningProt);
                        double objFire = currentobj.Values(DoubleValueKey.FireProt);
                        double objCold = currentobj.Values(DoubleValueKey.ColdProt);
                        double objBludg = currentobj.Values(DoubleValueKey.BludgeonProt);
                        double objSlash = currentobj.Values(DoubleValueKey.SlashProt);
                        double objPierce = currentobj.Values(DoubleValueKey.PierceProt);
                        long objMastery = currentobj.Values(LongValueKey.ActivationReqSkillId);
                        long objMaxDamage = currentobj.Values(LongValueKey.MaxDamage);
                        double objVariance = currentobj.Values(DoubleValueKey.Variance);
                        objSpellXml = GoGetSpells(currentobj);
                        long objMagicDam = currentobj.Values(LongValueKey.WandElemDmgType);
                        long objRareID = currentobj.Values(LongValueKey.RareId);
                        long objBurden = currentobj.Values(LongValueKey.Burden);
                        long objStackCount = currentobj.Values(LongValueKey.StackCount);
                        long objModel = currentobj.Values(LongValueKey.Model);
                        long iconUnderlay = currentobj.Values(LongValueKey.IconUnderlay);
                        long iconOverlay = currentobj.Values(LongValueKey.IconUnderlay);
                        long iconOutline = currentobj.Values(LongValueKey.IconOutline);
                        long objFlags = currentobj.Values(LongValueKey.Flags);
                        long objCreateFlag1 = currentobj.Values(LongValueKey.CreateFlags1);
                        long objCreateFlag2 = currentobj.Values(LongValueKey.CreateFlags2);
                        long objUnknown10 = currentobj.Values(LongValueKey.Unknown10);
                        long objUnknown100000 = currentobj.Values(LongValueKey.Unknown100000);
                        long objUnknown800000 = currentobj.Values(LongValueKey.Unknown800000);
                        long objUnknown8000000 = currentobj.Values(LongValueKey.Unknown8000000);
                        long objUsageMask = currentobj.Values(LongValueKey.UsageMask);
                                           
                        xdocToonInventory.Element("Objs").Add(new XElement("Obj",
                        new XElement("ObjName", objName),
                        new XElement("ObjID", objID),
                        new XElement("ToonName", toonName),
                        new XElement("ObjIcon", objIcon),
                        new XElement("ObjClass", objClassName),
                        new XElement("ObjDesc", objDesc),
                        new XElement("ObjMaterial", objMat),
                        new XElement("ObjAl", objAl),
                        new XElement("ObjSet", objSet),
                        new XElement("ObjCovers", objCovers),
                        new XElement("ObjEqSlot", objEqSlot),
                        new XElement("ObjToonLevel", objToonLevel),
                        new XElement("ObjLoreReq", objLore),
                        new XElement("ObjSkillLevReq", objSkillLevReq),
                        new XElement("ObjWork", objWork),
                        new XElement("ObjTink", objTinks),
                        new XElement("ObjCatType", objCatType),
                        new XElement("ObjCleaveType", objCleaveType),
                        new XElement("ObjMissType", objMissType),
                        new XElement("ObjType", objType),
                        new XElement("ObjElemDmg", objElemDmg),
                        new XElement("ObjAtt", objAtt),
                        new XElement("ObjBnd", objBnd),
                        new XElement("ObjEmbue", objEmbue),
                        new XElement("ObjSlayer", objSlayer),
                        new XElement("ObjWieldAttr", objWieldAttrInt),
                        new XElement("ObjWieldType", objWieldType),
                        new XElement("ObjWieldValue", objWieldValue),
                        new XElement("ObjDamage", objDamage),
                        new XElement("ObjElemvsMons", objElemvsMons),
                        new XElement("ObjMelD", objMelD),
                        new XElement("ObjMagicD", objMagicD),
                        new XElement("ObjManaC", objManaC),
                        new XElement("ObjMissileD", objMissileD),
                        new XElement("ObjSalvWork", objSalvWork),
                        new XElement("ObjAttack", objAttack),
                        new XElement("ObjDamageBonus", objDamageBonus),
                        new XElement("ObjMaxDamage", objMaxDamage),
                        new XElement("ObjVariance", objVariance),
                        new XElement("ObjMastery", objMastery),
                        new XElement("ObjSpellXml", objSpellXml),
                        new XElement("ObjMagicDamage", objMagicDam),
                        new XElement("ObjBurden", objBurden),
                        new XElement("ObjStackCount",objStackCount),
                        new XElement("ObjAcid", objAcid),
                        new XElement("ObjLight", objLight),
                        new XElement("ObjFire", objFire),
                        new XElement("ObjCold", objCold),
                        new XElement("ObjBludg", objBludg),
                        new XElement("ObjSlash", objSlash),
                        new XElement("ObjPierce", objPierce),
                        new XElement("ObjRareID", objRareID),
                        new XElement("IconOverlay", iconOverlay),
                        new XElement("IconOutline", iconOutline),
                        new XElement("IconUnderlay", iconUnderlay),
                        new XElement("ObjFlags", objFlags),
                        new XElement("ObjCreateFlag1", objCreateFlag1),
                        new XElement("ObjCreateFlag2", objCreateFlag2),
                        new XElement("ObjUnknown10", objUnknown10),
                        new XElement("ObjUnknown100000", objUnknown100000),
                        new XElement("ObjUnknown800000", objUnknown800000),
                        new XElement("ObjUnknown8000000", objUnknown8000000),
                        new XElement("ObjUsageMask", objUsageMask)));


                        //  xdoc.Save(inventoryFilename);


                        currentobj = null;
                        objClassName = null;
                        objName = null;
                        objDesc = 0;
                        objID = 0;
                        objIcon = 0;

                        objAl = 0;
                        objSet = 0;
                        objMat = 0;
                        objCovers = 0;
                        objToonLevel = 0;
                        objLore = 0;
                        objSkillLevReq = 0;
                        objTinks = 0;
                        objWork = 0;
                        objCatType = 0;
                        objCleaveType = 0;
                        objMissType = 0;
                        objType = 0;
                        objElemDmg = 0;
                        objMastery = 0;

                        objAtt = 0;
                        objBnd = 0;
                        objEmbue = 0;
                        objSlayer = 0;
                        objWieldAttrInt = 0;
                        objWieldType = 0;
                        objWieldValue = 0;
                        objDamage = 0;
                        objElemvsMons = 0;
                        objMelD = 0;
                        objMagicD = 0;
                        objManaC = 0;
                        objMissileD = 0;
                        objSalvWork = 0;
                        objAttack = 0;
                        objDamageBonus = 0;
                        objEqSlot = 0;
                        objVariance = 0;
                        objMaxDamage = 0;
                        objSpellXml = null;

                        objMagicDam = 0;
                        objRareID = 0;
                        objBurden = 0;
                        objStackCount = 0;
                        objAcid = 0;
                        objLight = 0;
                        objFire = 0;
                        objCold = 0;
                        objBludg = 0;
                        objSlash = 0;
                        objPierce = 0;

                        objModel = 0;


                        iconOverlay = 0;
                        iconUnderlay = 0;
                        iconOutline = 0;

                        objFlags = 0;
                        objCreateFlag1 = 0;
                        objCreateFlag2 = 0;
                        objUnknown10 = 0;
                        objUnknown100000 = 0;
                        objUnknown800000 = 0;
                        objUnknown8000000 = 0;
                        objUsageMask = 0; 
                        } // end of if


                } // endof try

                catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

            } // end of for



        } // end of process data

        private string GoGetSpells(Decal.Adapter.Wrappers.WorldObject o)
        {
            FileService fs = (FileService)Core.FileService;
            int intspellcnt = o.SpellCount;
            string oXmlSpells = "";
            for (int i = 0; i < intspellcnt; i++)
            {
                int spellId = o.Spell(i);

                Spell spell = fs.SpellTable.GetById(spellId);

                string spellName = spell.Name;
                if (spellName.Contains("Major") || spellName.Contains("Epic") ||
                  spellName.Contains("Incantation")  || spellName.Contains("Surge")
                    || spellName.Contains("Cloaked in Skill") || spellName.Contains("Legendary"))
                {
                    oXmlSpells = oXmlSpells + ", " + spellName;

                }

            }
            return oXmlSpells;
        }  //endof gogetspells

 

        

        public void removeExcessObjsfromFile()
        {
            try
            {
                List<string> holding = new List<string>();
             //   xdoc = XDocument.Load(inventoryFilename);
                IEnumerable<XElement> elements = xdocToonInventory.Element("Objs").Descendants("Obj");

                int oldCount = (int)(xdocToonInventory.Element("Objs").Elements("Obj").Count());
                foreach (string val in moldObjsID)
                {
                    if (!mCurrID.Contains(val))
                    {
                        holding.Add(val); 
                    }

                }


                var obj = from o in xdocToonInventory.Descendants("Obj") where holding.Contains(o.Element("ObjID").Value) select o;
                obj.Remove();
                int newCount = (int)(xdocToonInventory.Element("Objs").Elements("Obj").Count());
                int count = oldCount - newCount;
                Mishna.PluginCore.Util.WriteToChat(count + " objects removed from inventory of " + toonName);
                xdocToonInventory.Save(inventoryFilename);
                xdocToonInventory = null;
                moldObjsID = null;
                mWaitingForID = null;
                mCurrID = null;
                holding = null;


            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


        }




        public void removeToonfromFile()
        {
            try
            {

                 IEnumerable<XElement> elements = xdocGenInventory.Element("Objs").Descendants("Obj");
                xdocGenInventory.Descendants("Obj").Where(x => x.Element("ToonName").Value == toonName).Remove();

            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }
    }
}// end of namespace




