using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using WindowsTimer = System.Windows.Forms.Timer;
using Decal.Adapter;
using Decal.Filters;
using Decal.Adapter.Wrappers;
using System.Xml.Linq;
using System.IO;

namespace Mishna
{
    public partial class PluginCore : PluginBase
    {

        void btnGetToonArmor_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            doGetArmor();
        }

        private void doGetArmor()
        {
         try{
            Mishna.PluginCore.Util.WriteToChat("ToonArmorButton was clicked");
            mWaitingForArmorID = new List<WorldObject>();

            armorFilename = currDir + @"\" + toonName + "Armor.xml";
            genArmorFilename = world + @"\allToonsArmor.xml";
            holdingArmorFilename = world + @"\holdingArmor.xml";


           xdocArmor = new XDocument(new XElement("Objs"));

 
           if (!File.Exists(genArmorFilename))
           {

              XDocument tempDoc = new XDocument(new XElement("Objs"));
              tempDoc.Save(genArmorFilename);
              tempDoc = null;


           }

           foreach (Decal.Adapter.Wrappers.WorldObject obj in Core.WorldFilter.GetInventory())
           {
              try
              {
                    
                    objName = obj.Name;
 
                    bool shouldAdd = false;

                    if (obj.Values(LongValueKey.Slot) == -1) 
                    {

                        shouldAdd = true;
                        Globals.Host.Actions.RequestId(obj.Id);
                        mWaitingForArmorID.Add(obj);
                      }
              }  //end try
                catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

              } // endof foreach world object
              ProcessArmorDataInventory();
              fn = "armorFilename";
              mArmorIsFinished();
            } //end of try
           catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
       }
       
        private void mArmorIsFinished()
        {
            try
            {
                int ma = 30;
                int na = mWaitingForArmorID.Count;
                if (na < ma)
                {
                    string sna = na.ToString();

                    Mishna.PluginCore.Util.WriteToChat(sna);
                    ma = na;
                    string mname = null;


                    if (mWaitingForArmorID.Count > 0)
                    {
                    
                         mDoWait();
                    }

                    else
                    {
                           try
                            {

                                xdocArmor.Save(armorFilename);
                                xdocArmor = null;

                                removeToonfromArmorFile();

                                var myArmorDoc = XDocument.Load(genArmorFilename);
                                myArmorDoc.Root.Add(XDocument.Load(armorFilename).Root.Elements());

                                myArmorDoc.Save(holdingArmorFilename);
                                myArmorDoc = null;

                                xdocArmor = XDocument.Load(holdingArmorFilename);
                                xdocArmor.Save(genArmorFilename);
                                Mishna.PluginCore.Util.WriteToChat("General Armor file has been saved. ");
                            }
                            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

                        }
 
                        m = 30;
                        k = 0;
                        n = 0;
                        mWaitingForID = null;
                        xdoc = null;
                        fn = null;

                    }
                }
            
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        public void removeToonfromArmorFile()
        {
            try
            {

                xdocArmor = XDocument.Load(genArmorFilename);

                IEnumerable<XElement> myelements = xdocArmor.Element("Objs").Descendants("Obj");

                xdocArmor.Descendants("Obj").Where(x => x.Element("ToonName").Value == toonName).Remove();
             //   xdocArmor.Save(genInventoryFilename);
                xdocArmor = null;

            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }


        //     public void mDoWait()
        //{

        //    mWaitingForIDTimer.Start();

        //}

        //This is routine that puts the data of an obj into the inventory file xml
        private void ProcessArmorDataInventory()
        {
            for (int n = 0; n < mWaitingForArmorID.Count; n++)
            {
                try
                {

                    currentobj = mWaitingForArmorID[n];
       
                    if (mWaitingForArmorID[n].HasIdData)
                    {
                        currentarmorobj = mWaitingForArmorID[n];
                        mWaitingForID.Remove(mWaitingForID[n]);
                        //   if ((fn == "armorFilename") && (currentobj.Values(LongValueKey.Imbued) == 0)) { break; }
                        objClassName = currentarmorobj.ObjectClass.ToString();
                        objName = currentarmorobj.Name;
                        Mishna.PluginCore.Util.WriteToChat("I am in processing data and iding " + objName);
                        objID = currentarmorobj.Id;
        
 //                       goGetProtocol(currentobj);
             
                        objIcon = currentarmorobj.Icon;
                       // Type t = objIcon.GetType();

                        long objDesc = currentarmorobj.Values(LongValueKey.DescriptionFormat);
                        long objMat = currentarmorobj.Values(LongValueKey.Material);
                        long objCatType = (int)currentarmorobj.Values(LongValueKey.Category);
                        long objAtt = (int)currentarmorobj.Values(LongValueKey.Attuned);
                        long objBnd = (int)currentarmorobj.Values(LongValueKey.Bonded);
                        long objToonLevel = (int)currentarmorobj.Values(LongValueKey.MinLevelRestrict);
                        long objLore = (int)currentarmorobj.Values(LongValueKey.LoreRequirement);
                        long objAl = (int)currentarmorobj.Values(LongValueKey.ArmorLevel);
                        long objType = (int)currentarmorobj.Values(LongValueKey.Type);
                        long objTinks = (int)currentarmorobj.Values(LongValueKey.NumberTimesTinkered);
                        long objWork = (int)currentarmorobj.Values(LongValueKey.Workmanship);
                        long objSet = (int)currentarmorobj.Values(LongValueKey.ArmorSet);
                        long objCovers = currentarmorobj.Values(LongValueKey.Coverage);
                        long objEqSlot = currentarmorobj.Values(LongValueKey.EquipableSlots);
                        long objWieldValue = (int)currentarmorobj.Values(LongValueKey.WieldReqValue);
                        long objSkillLevReq = (int)currentarmorobj.Values(LongValueKey.SkillLevelReq);
                        double objSalvWork = currentarmorobj.Values(DoubleValueKey.SalvageWorkmanship);
                        double objAcid = currentarmorobj.Values(DoubleValueKey.AcidProt);
                        double objLight = currentarmorobj.Values(DoubleValueKey.LightningProt);
                        double objFire = currentarmorobj.Values(DoubleValueKey.FireProt);
                        double objCold = currentarmorobj.Values(DoubleValueKey.ColdProt);
                        double objBludg = currentarmorobj.Values(DoubleValueKey.BludgeonProt);
                        double objSlash = currentarmorobj.Values(DoubleValueKey.SlashProt);
                        double objPierce = currentarmorobj.Values(DoubleValueKey.PierceProt);
                        objSpellXml = GoGetSpells(currentarmorobj);
                        long objRareID = currentarmorobj.Values(LongValueKey.RareId);
                        long objBurden = currentarmorobj.Values(LongValueKey.Burden);
                        long objModel = currentarmorobj.Values(LongValueKey.Model);

                        xdocArmor.Element("Objs").Add(new XElement("Obj",
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
                        new XElement("ObjType", objType),
                        new XElement("ObjAtt", objAtt),
                        new XElement("ObjBnd", objBnd),
                        new XElement("ObjWieldAttr", objWieldAttrInt),
                        new XElement("ObjWieldValue", objWieldValue),
                        new XElement("ObjSalvWork", objSalvWork),
                        new XElement("ObjSpellXml", objSpellXml),
                        new XElement("ObjBurden", objBurden),
                        new XElement("ObjAcid", objAcid),
                        new XElement("ObjLight", objLight),
                        new XElement("ObjFire", objFire),
                        new XElement("ObjCold", objCold),
                        new XElement("ObjBludg", objBludg),
                        new XElement("ObjSlash", objSlash),
                        new XElement("ObjPierce", objPierce),
                        new XElement("ObjRareID", objRareID)));


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
                        objType = 0;

                        objAtt = 0;
                        objBnd = 0;
                       objWieldAttrInt = 0;
                        objWieldValue = 0;
                        objSalvWork = 0;
                        objEqSlot = 0;
                        objSpellXml = null;
                       objRareID = 0;
                        objBurden = 0;
                        objAcid = 0;
                        objLight = 0;
                        objFire = 0;
                        objCold = 0;
                        objBludg = 0;
                        objSlash = 0;
                        objPierce = 0;

                        objModel = 0;

                        } // end of if


                } // endof try

                catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

            } // end of for



        } // end of process data





    } // end of partial class

}  // end of namespace