using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using WindowsTimer = System.Windows.Forms.Timer;
using Decal.Adapter;
using Decal.Filters;
using Decal.Adapter.Wrappers;
using System.Text;
using VirindiViewService;
using MyClasses.MetaViewWrappers;
using System.Drawing;
using System.Drawing.Imaging;


namespace Mishna
{
    public partial class PluginCore : PluginBase
    {
        void btnGetInventory_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
           
                
                Mishna.PluginCore.Util.WriteToChat("ToonInventoryButton was clicked");
                m = 500;
                xdocToonInventory = new XDocument(new XElement("Objs"));
                doGetInventory();
            
        }
    
       void btnUpdateInventory_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            Mishna.PluginCore.Util.WriteToChat("The button to update inventory was clicked");
                doUpdateInventory();
       }

        void btnGetBurden_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
       {
           Mishna.PluginCore.Util.WriteToChat("The button to update burden was clicked");
           bgetBurden = true;
           doUpdateInventory();
       }

        private void doUpdateInventory()
        {
            try{
 
                //Need a timer for processing inventory
                mWaitingForIDTimer = new WindowsTimer();
                //Need a list to hold the inventory
                mWaitingForID = new List<WorldObject>();

                // If already have an inventory file for a toon, do not need to duplicate already id'd inventory
                // moldObjsID is a list that contains the object IDs of the previous inventory for that toon
                moldObjsID = new List<string>();
                List<string>  mtemps = new List<string>();
                if (File.Exists(inventoryFilename))
                {
                    try
                    {
                        xdocToonInventory = XDocument.Load(inventoryFilename);

                        if (bgetBurden)
                        {
                            IEnumerable<XElement> myelements = xdocToonInventory.Element("Objs").Descendants("Obj");
                            int oldCount = (int)(xdocToonInventory.Element("Objs").Elements("Obj").Count());

                            var obj = from o in xdocToonInventory.Descendants("Obj")
                                      where o.Element("ObjName").Value.Contains("Stipend") ||
                                          o.Element("ObjName").Value.Contains("Crystal") || o.Element("ObjName").Value.Contains("Jewel")
                                          || o.Element("ObjName").Value.Contains("Pearl") || o.Element("ObjName").Value.Contains("Trade Note")
                                          || o.Element("ObjName").Value.Contains("Society Gem") || o.Element("ObjName").Value.Contains("Token")
                                          || o.Element("ObjName").Value.Contains("Field Ration") || o.Element("ObjName").Value.Contains("Pea")
                                          || o.Element("ObjName").Value.Contains("Coin") || o.Element("ObjName").Value.Contains("Sack")
                                          || o.Element("ObjName").Value.Contains("Venom Sac") || o.Element("ObjName").Value.Contains("Glyph")
                                          || o.Element("ObjName").Value.Contains("Arrow")
                                      select o;
                            obj.Remove();
                            int newCount = (int)(xdocToonInventory.Element("Objs").Elements("Obj").Count());
                       //     xdoc.Save(inventoryFilename);


                            Mishna.PluginCore.Util.WriteToChat("Count before removal = " + oldCount + ".  Count after removal = " + newCount);
                        } // end of bgetBurden
                        IEnumerable<XElement> elements = xdocToonInventory.Element("Objs").Descendants("ObjID");
                        foreach (XElement element in elements)
                        {
                            //Create list of the ID's currently in the inventory
                             { moldObjsID.Add(element.Value);}
                        }
                    } // end of try
                    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
                    bmgoon = true; 

                }
                else //if that toon not previously inventoried need to start an inventory file for them
                {
                    xdocToonInventory = new XDocument(new XElement("Objs"));
                    doGetInventory();
                }
                
 
                // if left this subprogram because of exception in update need a way to avoid returning to this program
                if (bmgoon)
                {
                    mCurrID = new List<string>();

                    //loop for checking each obj in the current inventory
                    foreach (Decal.Adapter.Wrappers.WorldObject obj in Core.WorldFilter.GetInventory())
                    {
                        try
                        {
                            //Need to find the current inventory objects and create a list of their ids mCurrID
                            objID = obj.Id;
                            string sobjID = objID.ToString();
                            mCurrID.Add(sobjID);
                            //Need to compare the ids in mCurrID with those of the previous inventory 
                            if (!moldObjsID.Contains(sobjID))
                            {
                                Globals.Host.Actions.RequestId(obj.Id);
                                mWaitingForID.Add(obj); //if the ID not in previous inventory need to get the data
                            }

                        }
                        catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


                    } // endof foreach world object



                    Mishna.PluginCore.Util.WriteToChat("Items being inventoried " + mWaitingForID.Count);
                    //Do one run through saved ids to get all data that is immediately available
                    if (mWaitingForID.Count > 0)
                    {
                        // initialize event timer for processing inventory
                        mWaitingForIDTimer.Tick += new EventHandler(TimerEventProcessor);

                        // Sets the timer interval to 5 seconds.
                        mWaitingForIDTimer.Interval = 10000;

                        ProcessDataInventory();
                    }
                    //Now need to start routines that will continue to get data as becomes available or will end the search and save the files
                    fn = "inventoryFilename";
                    mIsFinished();  //This routine used both by the inventory and by the armor routines

                 }

            } //end of try
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }
        


        // The following code has to do with selection of inventory to display in listbox
        // First it is necessary to choose the class of inventory; ie, weapons, armor etc. 
        [MVControlEvent("cmbSelectClass", "Change")]
        void cmbSelectClass_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {

                objClass = cmbSelectClass.Selected;
                findClassName(objClass);
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }


        // In case of  Weapons will want to find weapons of specific type; e.g., missile
        [MVControlEvent("cmbWieldAttrib", "Change")]
        void cmbWieldAttrib_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {

                int tempeWieldAttrInt = cmbWieldAttrib.Selected;
                findWieldAttrInt(tempeWieldAttrInt);
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        // Need to determine damage type of weapon or wand
        [MVControlEvent("cmbDamageType", "Change")]
        void cmbDamageType_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {

                int tempeDamageTypeInt = cmbDamageType.Selected;
                findDamageTypeInt(tempeDamageTypeInt);                
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        void cmbLevel_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {

                int tempeLevelInt = cmbLevel.Selected;
                findLevelInt(tempeLevelInt);
           }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        void cmbArmorSet_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {

                int tempeArmorSetInt = cmbArmorSet.Selected;
                findArmorSetInt(tempeArmorSetInt);
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        void cmbArmorLevel_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {

                int tempeArmorLevelInt = cmbArmorLevel.Selected;
                findArmorLevelInt(tempeArmorLevelInt);
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        void cmbCoverage_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
           try
            {

                int tempeCoverageInt = cmbCoverage.Selected;
                findArmorCoverage(tempeCoverageInt);
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        void cmbMaterial_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {

                int tempeMaterialInt = cmbMaterial.Selected;
                findMaterial(tempeMaterialInt);
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        void cmbSalvWork_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {

                int tempeSalvWorkInt = cmbSalvWork.Selected;
                findobjSalvWork(tempeSalvWorkInt);
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        void cmbEmbue_Change(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {

                int tempeEmbueInt = cmbEmbue.Selected;
                findEmbueTypeInt(tempeEmbueInt);
           }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }



        // items selected need to be added to listview: lstinventory
        [MVControlEvent("btnLstInventory", "Click")]
        void btnLstInventory_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try{
                Mishna.PluginCore.Util.WriteToChat("I am at The list inventory section");

           XDocument tempDoc = new XDocument(new XElement("Objs"));
            tempDoc.Save(inventorySelect);
            tempDoc = null;
            mySelect = null;

            if (txbSelect.Text != null)
            {
                mySelect = txbSelect.Text.Trim();
                mySelect = mySelect.ToLower();
            }
            else
            { mySelect = null; }
        xdoc = XDocument.Load(genInventoryFilename);
            }//end of try //

            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


            try
            {
                switch (objClass)
                {

                 case 0:
                
                    if (mySelect != null && mySelect != "")
                    {

                         newDoc = new XDocument(new XElement("Objs",
                          from p in xdoc.Element("Objs").Descendants("Obj")
                          where p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                         p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect)
                          select p));

                    }
                    else if (mySelect == null || mySelect == "")
                    { Mishna.PluginCore.Util.WriteToChat("You must choose a class or type something inbox");}

                break;
                case 1:
                case 2:
                case 11:
                    if (mySelect != null && mySelect.Trim() != "")
                    {
                        if (objArmorSet == 0 && objArmorLevel == 1 && objCovers == 0)
                        {
                            newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName)&&
                                 (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                  p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                            select p));

                        }


                        else if (objArmorSet > 0 && objArmorLevel == 1 && objCovers == 0)
                        {
                            newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjSet").Value == objArmorSet.ToString() &&
                                 (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                  p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                            select p));

                        }
                        else if ((objArmorLevel > 1 || objArmorLevel < 1) && objArmorSet == 0 &&  objCovers == 0)
                        {
                            newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldValue").Value == objArmorLevel.ToString() &&
                                  (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                  p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                            select p));
                        }
                        else if (objCovers > 0 && objArmorSet == 0 && objArmorLevel == 1 )
                        {
                            newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                                  p.Element("ObjCovers").Value == objCovers.ToString() &&
                                  (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                  p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                            select p));
                        }
                        else if (objArmorSet > 0 && (objArmorLevel < 1 || objArmorLevel > 1) && objCovers == 0)
                        {
                            newDoc = new XDocument(new XElement("Objs",
                                from p in xdoc.Element("Objs").Descendants("Obj")
                                where p.Element("ObjClass").Value.Contains(objClassName) &&
                                    p.Element("ObjSet").Value == objArmorSet.ToString() &&
                                   p.Element("ObjWieldValue").Value == objArmorLevel.ToString() &&
                                      (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                      p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                                select p));
                         }
                         else if (objArmorSet > 0 &&  objCovers > 0 && objArmorLevel == 1 )
                         {
                                newDoc = new XDocument(new XElement("Objs",
                                from p in xdoc.Element("Objs").Descendants("Obj")
                                where p.Element("ObjClass").Value.Contains(objClassName) &&
                                    p.Element("ObjSet").Value == objArmorSet.ToString() &&
                                      p.Element("ObjCovers").Value == objCovers.ToString() &&
                                      (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                      p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                                 select p));
                          }
                          else if (objArmorSet == 0 && (objArmorLevel > 1 || objArmorLevel < 1) && objCovers > 0)
                            {
                                newDoc = new XDocument(new XElement("Objs",
                                from p in xdoc.Element("Objs").Descendants("Obj")
                                where p.Element("ObjClass").Value.Contains(objClassName) &&
                                   p.Element("ObjWieldValue").Value == objArmorLevel.ToString() &&
                                    p.Element("ObjCovers").Value == objCovers.ToString() &&
                                     (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                      p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                            }
                          else if (objArmorSet > 0 && (objArmorLevel > 1 || objArmorLevel < 1) && objCovers > 0)
                          
                             {
                               newDoc = new XDocument(new XElement("Objs",
                                from p in xdoc.Element("Objs").Descendants("Obj")
                                where p.Element("ObjClass").Value.Contains(objClassName) &&
                                    p.Element("ObjSet").Value == objArmorSet.ToString() &&
                                     p.Element("ObjWieldValue").Value == objArmorLevel.ToString() &&
                                      p.Element("ObjCovers").Value == objCovers.ToString() &&
                                      (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                      p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                                select p));
                             }
                        }
              
                    else
                    {
                       if (objArmorSet == 0 && objArmorLevel == 1 && objCovers == 0)
                        {
                             newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) 
                            select p));
                        }

                        else if (objArmorSet > 0 && objArmorLevel == 1 && objCovers == 0)
                        {
 
                            newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName)  &&
                                p.Element("ObjSet").Value == objArmorSet.ToString()
                            select p)); 
                       }
                        else if ((objArmorLevel > 1 || objArmorLevel < 1) && objArmorSet == 0 && objCovers == 0)
                        {
                            newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                               p.Element("ObjWieldValue").Value == objArmorLevel.ToString()
                            select p));
                        }
                        else if (objCovers > 0 && objArmorSet == 0 && objArmorLevel == 1 )
                        {
                            newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                                  p.Element("ObjCovers").Value == objCovers.ToString() 
                            select p));
                        }
                        else if (objArmorSet > 0 && (objArmorLevel < 1 || objArmorLevel > 1) && objCovers == 0)
                        {
                            newDoc = new XDocument(new XElement("Objs",
                                from p in xdoc.Element("Objs").Descendants("Obj")
                                where p.Element("ObjClass").Value.Contains(objClassName) &&
                                    p.Element("ObjSet").Value == objArmorSet.ToString() &&
                                   p.Element("ObjWieldValue").Value == objArmorLevel.ToString() 
                                select p));
                         }
                         else if (objArmorSet > 0 &&  objCovers > 0 && objArmorLevel == 1 )
                         {
                                newDoc = new XDocument(new XElement("Objs",
                                from p in xdoc.Element("Objs").Descendants("Obj")
                                where p.Element("ObjClass").Value.Contains(objClassName) &&
                                    p.Element("ObjSet").Value == objArmorSet.ToString() &&
                                      p.Element("ObjCovers").Value == objCovers.ToString() 
                                 select p));
                          }
                          else if (objArmorSet == 0 && (objArmorLevel > 1 || objArmorLevel < 1) && objCovers > 0)
                            {
                                newDoc = new XDocument(new XElement("Objs",
                                from p in xdoc.Element("Objs").Descendants("Obj")
                                where p.Element("ObjClass").Value.Contains(objClassName) &&
                                   p.Element("ObjWieldValue").Value == objArmorLevel.ToString() &&
                                    p.Element("ObjCovers").Value == objCovers.ToString() 
                               select p));
                            }
                        else if (objArmorSet > 0 && (objArmorLevel > 1 || objArmorLevel < 1) && objCovers > 0)
                        {
                            newDoc = new XDocument(new XElement("Objs",
                             from p in xdoc.Element("Objs").Descendants("Obj")
                             where p.Element("ObjClass").Value.Contains(objClassName) &&
                                 p.Element("ObjSet").Value == objArmorSet.ToString() &&
                                  p.Element("ObjWieldValue").Value == objArmorLevel.ToString() &&
                                   p.Element("ObjCovers").Value == objCovers.ToString()
                             select p));
                        }


                    }  //end of if spells
                   break;
                   case 5:
                   if (mySelect != null && mySelect != "")
                   {
                       if (objWieldAttrInt == 0 && objDamageTypeInt == 0 && objLevelInt == 1 && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }


                       else if (objWieldAttrInt > 0 && objDamageTypeInt == 0 && objLevelInt == 1 && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }

                       else if (objDamageTypeInt > 0 && objWieldAttrInt == 0 && objLevelInt == 1 && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if ((objLevelInt < 1 || objLevelInt > 1) && objWieldAttrInt == 0 && objDamageTypeInt == 0 && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objEmbueTypeInt > 0 && objWieldAttrInt == 0 && objDamageTypeInt == 1 && objLevelInt == 1 )
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objWieldAttrInt > 0 && objDamageTypeInt > 0 && objLevelInt ==1  && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objWieldAttrInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objDamageTypeInt == 0 &&  objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                                p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objWieldAttrInt > 0 && objEmbueTypeInt > 0 && objDamageTypeInt == 0 && objLevelInt == 1 )
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                                p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objDamageTypeInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objWieldAttrInt == 0 &&  objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                                p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objDamageTypeInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objWieldAttrInt == 0 && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                                p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objDamageTypeInt > 0 && objEmbueTypeInt > 0 && objWieldAttrInt == 0 && (objLevelInt == 1))
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                                p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString()  &&
                               (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if ((objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0 && objWieldAttrInt == 0 && objDamageTypeInt == 0 )
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                                p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objWieldAttrInt > 0 && objDamageTypeInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                                p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objWieldAttrInt > 0 && objDamageTypeInt > 0 && objEmbueTypeInt > 0  && objLevelInt == 1 )
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                                p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objWieldAttrInt > 0 && objDamageTypeInt == 0 && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                                p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                                p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objWieldAttrInt == 0 && objDamageTypeInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                                p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                                p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       else if (objWieldAttrInt > 0 && objDamageTypeInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                                p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                                p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                   } //end of case 5 with spells
                   else 
                   {
                       if (objWieldAttrInt == 0 && objDamageTypeInt == 0 && objLevelInt == 1 && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) 
                               select p));
                       }

   
                    else if (objWieldAttrInt > 0 && objDamageTypeInt == 0 && objLevelInt == 1 && objEmbueTypeInt == 0)
                      {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() 
                               select p));
                        }

                    else if (objDamageTypeInt > 0 && objWieldAttrInt == 0 && objLevelInt == 1 && objEmbueTypeInt == 0)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjDamage").Value == objDamageTypeInt.ToString() 
                            select p));
                    }

                    else if ((objLevelInt < 1 || objLevelInt > 1) && objWieldAttrInt == 0 && objDamageTypeInt == 0 && objEmbueTypeInt == 0)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjWieldValue").Value == objLevelInt.ToString() 
                            select p));
                    }
                    else if (objEmbueTypeInt > 0 && objWieldAttrInt == 0 && objDamageTypeInt == 1 && objLevelInt == 1)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                            select p));
                    }
                    else if (objWieldAttrInt > 0 && objDamageTypeInt > 0 && objLevelInt == 1 && objEmbueTypeInt == 0)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                             p.Element("ObjDamage").Value == objDamageTypeInt.ToString() 
                            select p));
                    }
                    else if (objWieldAttrInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objDamageTypeInt == 0 && objEmbueTypeInt == 0)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                             p.Element("ObjWieldValue").Value == objLevelInt.ToString() 
                            select p));
                    }
                    else if (objWieldAttrInt > 0 && objEmbueTypeInt > 0 && objDamageTypeInt == 0 && objLevelInt == 1)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                             p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                            select p));
                    }
                    else if (objDamageTypeInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objWieldAttrInt == 0 && objEmbueTypeInt == 0)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                             p.Element("ObjWieldValue").Value == objLevelInt.ToString() 
                            select p));
                    }
                    else if (objDamageTypeInt > 0 && objEmbueTypeInt > 0 && objWieldAttrInt == 0 && (objLevelInt == 1))
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                             p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                            select p));
                    }

                    else if ((objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0 && objWieldAttrInt == 0 && objDamageTypeInt == 0)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                             p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                            select p));
                    }
                    else if (objWieldAttrInt > 0 && objDamageTypeInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt == 0)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                             p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                             p.Element("ObjWieldValue").Value == objLevelInt.ToString() 
                            select p));
                    }
                    else if (objWieldAttrInt > 0 && objDamageTypeInt > 0 && objEmbueTypeInt > 0 && objLevelInt == 1)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                             p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                             p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                            select p));
                    }
                    else if (objWieldAttrInt > 0 && objDamageTypeInt == 0 && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                             p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                             p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                            select p));
                    }
                    else if (objWieldAttrInt == 0 && objDamageTypeInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0)
                    {
                        newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                             p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                             p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                             p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                            select p));
                    }
                    else if (objWieldAttrInt > 0 && objDamageTypeInt > 0 && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0)
                      {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                p.Element("ObjWieldAttr").Value == objWieldAttrInt.ToString() &&
                                p.Element("ObjDamage").Value == objDamageTypeInt.ToString() &&
                               p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                               p.Element("ObjEmbue").Value == (objEmbueTypeInt.ToString())
                               select p));
                        }

                     } //end of case 5  no spells
                   
                   break;
                    case 4:
                    case 6:
                   if (mySelect != null && mySelect != "")
                   {
                       if (objDamageTypeInt == 0 && objMagicDamageInt == 0 && objLevelInt == 1 && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                 p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }


                       if ((objDamageTypeInt > 0 || objMagicDamageInt > 0) && (objLevelInt == 1) && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjDamage").Value == objDamageTypeInt.ToString() ||
                                 p.Element("ObjMagicDamage").Value == objDamageTypeInt.ToString()) &&
                                (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                                 p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       if ((objLevelInt < 1 || objLevelInt > 1) && objDamageTypeInt == 0 && objMagicDamageInt == 0 && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                               p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                               (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                               p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       if (objEmbueTypeInt > 0 && objDamageTypeInt == 0 && objMagicDamageInt == 0 && objLevelInt == 1) 
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                               p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                               (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                               p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }

                       if ((objDamageTypeInt > 0 || objMagicDamageInt > 0) && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjDamage").Value == objDamageTypeInt.ToString() ||
                                 p.Element("ObjMagicDamage").Value == objDamageTypeInt.ToString()) &&
                               p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                               (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                               p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       if ((objDamageTypeInt > 0 || objMagicDamageInt > 0) && objEmbueTypeInt > 0 && objLevelInt == 1 )
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjDamage").Value == objDamageTypeInt.ToString() ||
                                 p.Element("ObjMagicDamage").Value == objDamageTypeInt.ToString()) &&
                               p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                               (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                               p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }
                       if ((objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0 && objDamageTypeInt == 0 && objMagicDamageInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                               p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                               p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                               (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                               p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }

                       if ((objDamageTypeInt > 0 || objMagicDamageInt > 0) && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjDamage").Value == objDamageTypeInt.ToString() ||
                                 p.Element("ObjMagicDamage").Value == objDamageTypeInt.ToString()) &&
                               p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                               p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() &&
                               (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                               p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                               select p));
                       }

                   }
                   else
                   {
                       if (objDamageTypeInt == 0 && objMagicDamageInt == 0 && (objLevelInt == 1) && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) 
                               select p));
                       }
 
                       if ((objDamageTypeInt > 0 || objMagicDamageInt > 0) && (objLevelInt == 1) && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjDamage").Value == objDamageTypeInt.ToString() ||
                                 p.Element("ObjMagicDamage").Value == objDamageTypeInt.ToString()) 
                               select p));
                       }
                       if ((objLevelInt < 1 || objLevelInt > 1) && objDamageTypeInt == 0 && objMagicDamageInt == 0 && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                               p.Element("ObjWieldValue").Value == objLevelInt.ToString() 
                               select p));
                       }
                       if (objEmbueTypeInt > 0 && objDamageTypeInt == 0 && objMagicDamageInt == 0 && (objLevelInt == 1))
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                               p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                               select p));
                       }
                       if ((objDamageTypeInt > 0 || objMagicDamageInt > 0) && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt == 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjDamage").Value == objDamageTypeInt.ToString() ||
                                 p.Element("ObjMagicDamage").Value == objDamageTypeInt.ToString()) &&
                               p.Element("ObjWieldValue").Value == objLevelInt.ToString() 
                              select p));
                       }

                       if ((objDamageTypeInt > 0 || objMagicDamageInt > 0) && objEmbueTypeInt > 0 && objLevelInt == 1)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjDamage").Value == objDamageTypeInt.ToString() ||
                                 p.Element("ObjMagicDamage").Value == objDamageTypeInt.ToString()) &&
                               p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                               select p));
                       }
                       if ((objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0 && objDamageTypeInt == 0 && objMagicDamageInt == 0 )
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                               p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                               p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                              select p));
                       }


                       if ((objDamageTypeInt > 0 || objMagicDamageInt > 0) && (objLevelInt < 1 || objLevelInt > 1) && objEmbueTypeInt > 0)
                       {
                           newDoc = new XDocument(new XElement("Objs",
                               from p in xdoc.Element("Objs").Descendants("Obj")
                               where p.Element("ObjClass").Value.Contains(objClassName) &&
                                (p.Element("ObjDamage").Value == objDamageTypeInt.ToString() ||
                                 p.Element("ObjMagicDamage").Value == objDamageTypeInt.ToString()) &&
                               p.Element("ObjWieldValue").Value == objLevelInt.ToString() &&
                               p.Element("ObjEmbue").Value == objEmbueTypeInt.ToString() 
                               select p));
                       }
                   }

                   break;

                   case 7:
                   if ((objClassName.Contains("Salvage")) && (objSalvWork == "None"))
                   {
                       newDoc = new XDocument(new XElement("Objs",
                         from p in xdoc.Element("Objs").Descendants("Obj")
                         where p.Element("ObjClass").Value.Contains(objClassName) &&
                         p.Element("ObjMaterial").Value == objMat.ToString()
                         select p));
                   }

                   else if ((objClassName.Contains("Salvage")) && ((objSalvWork == "1,2,3,4,5,6") || (objSalvWork == "7,8") || (objSalvWork == "9")))
                   {
                       newDoc = new XDocument(new XElement("Objs",
                         from p in xdoc.Element("Objs").Descendants("Obj")
                         where p.Element("ObjClass").Value.Contains(objClassName) &&
                         p.Element("ObjMaterial").Value == objMat.ToString() &&
                         objSalvWork.Contains(p.Element("ObjWork").Value.Substring(0, 1))
                         select p));
                   }

                   else if ((objClassName.Contains("Salvage")) && (objSalvWork == "10"))
                   {
                       newDoc = new XDocument(new XElement("Objs",
                         from p in xdoc.Element("Objs").Descendants("Obj")
                         where p.Element("ObjClass").Value.Contains(objClassName) &&
                         p.Element("ObjMaterial").Value == objMat.ToString() &&
                         objSalvWork.ToString() == p.Element("ObjWork").Value
                         select p));
                   }
              
                   break;
                    default:

                   if (objClassName != null && mySelect != null  && mySelect.Trim() != "")
                   {

                       newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName) &&
                                   (p.Element("ObjName").Value.ToLower().Contains(mySelect) ||
                               p.Element("ObjSpellXml").Value.ToLower().Contains(mySelect))
                            select p));
                   }

                   else if (objClassName != null && (mySelect == null  || mySelect.Trim() == ""))
                   {

                       newDoc = new XDocument(new XElement("Objs",
                            from p in xdoc.Element("Objs").Descendants("Obj")
                            where p.Element("ObjClass").Value.Contains(objClassName)
                            select p));
                   }

                   break;



                } //end of switch
                if ((mySelect != null || mySelect.Trim() != "") && objClassName != "None")
                {

                    xdoc = null;
                    newDoc.Save(inventorySelect);
                    newDoc = null;
                }
                else
                
                    { Mishna.PluginCore.Util.WriteToChat("You must choose a class or type something inbox"); }

                


                } //end of try //

                catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

            newDoc = XDocument.Load(inventorySelect);



            IEnumerable<XElement> childElements = newDoc.Element("Objs").Descendants("Obj");
            foreach (XElement childElement in childElements)
            {
                objIcon = Convert.ToInt32(childElement.Element("ObjIcon").Value);
                objName = childElement.Element("ObjName").Value;

                toonName = childElement.Element("ToonName").Value.ToString();
                long objID = Convert.ToInt32(childElement.Element("ObjID").Value);
                string objIDstr = objID.ToString();
                IListRow newRow = lstInventory.AddRow();
                newRow[0][1] = objIcon;
                newRow[1][0] = objName;
                newRow[2][0] = toonName;
                newRow[3][0] = objIDstr;
            }
            newDoc = null;
        }// end of btnlist

        [MVControlEvent("lstInventory", "Selected")]
        private void lstInventory_Selected(object sender, MyClasses.MetaViewWrappers.MVListSelectEventArgs e)
        {
            try
            {
                xdoc = XDocument.Load(inventorySelect);

                IListRow row = lstInventory[e.Row];

                // the object name is included in the text file on this row
                objID = Convert.ToInt32(row[3][0]);



                elements = xdoc.Element("Objs").Descendants("Obj");
                element = new XElement(new XElement("Objs",
                    from p in xdoc.Element("Objs").Descendants("Obj")
                    where (p.Element("ObjID").Value.ToLower().Contains(objID.ToString()))
                    select p));

                xdoc = null;
                List<XElement> ElementsList = new List<XElement>();


                 childElements = element.Descendants();
                foreach (XElement childElement in childElements)

                { ElementsList.Add(childElement); }


                objName = ElementsList[1].Value;
                toonName = ElementsList[3].Value;
                message = objName + ", " + toonName;
 
                
                switch (objClass)
                {
                    case 0:
                        objAl = ElementsList[8].Value;
                        objWork = ElementsList[15].Value;
                        objTinks = ElementsList[16].Value;
                        objLevel = ElementsList[28].Value;
                        objArmorSet = Convert.ToInt32(ElementsList[9].Value);
                        objCovers = Convert.ToInt32(ElementsList[10].Value);
                        objSpells = ElementsList[41].Value.ToString();
                        findArmorSetName(objArmorSet);
                        findCoversName(objCovers);
                        objMissD = ((Math.Round(Convert.ToDouble(ElementsList[34].Value), 2) - 1) * 100).ToString();
                        if (Convert.ToDouble(objMissD) < 0) { objMissD = "0"; }
                        objManaC = (Math.Round(Convert.ToDouble(ElementsList[33].Value), 2) * 100).ToString();
                        objMagicD = ((Math.Round(Convert.ToDouble(ElementsList[32].Value), 2) - 1) * 100).ToString();
                        if (Convert.ToDouble(objMagicD) < 0) { objMagicD = "0"; }
                        objMelD = Math.Round(((Convert.ToDouble(ElementsList[31].Value) - 1) * 100), 2).ToString();
                        if (Convert.ToDouble(objMelD) < 0) { objMelD = "0"; }
                        objElemvsMons = Math.Round(((Convert.ToDouble(ElementsList[30].Value) - 1) * 100), 2).ToString();
                        if (Convert.ToDouble(objElemvsMons) < 0) { objElemvsMons = "0"; }
                        objEmbueTypeInt = Convert.ToInt32(ElementsList[24].Value);
                        objAttack = Math.Round((Convert.ToDouble(ElementsList[36].Value)-1) * 100).ToString();
                        if (Convert.ToDouble(objAttack) < 0) { objAttack = "0"; }
                        objMaxDam = ElementsList[38].Value.ToString();
                        objVar = Math.Round(Convert.ToDouble(ElementsList[39].Value), 2).ToString();
                        objBurden = ElementsList[43].Value;
                        objStack = ElementsList[44].Value;
                        if (ElementsList[45].Value != "0")
                        {
                            string objAcid = ((Math.Round(Convert.ToDouble(ElementsList[45].Value), 4))).ToString();
                             string objLight = ((Math.Round(Convert.ToDouble(ElementsList[46].Value), 4))).ToString();
                             string objFire = ((Math.Round(Convert.ToDouble(ElementsList[47].Value), 4))).ToString();
                            string objCold = ((Math.Round(Convert.ToDouble(ElementsList[48].Value), 4))).ToString();
                            string objBludg = ((Math.Round(Convert.ToDouble(ElementsList[49].Value), 4))).ToString();
                            string objSlash = ((Math.Round(Convert.ToDouble(ElementsList[50].Value), 4))).ToString();
                            string objPierce = ((Math.Round(Convert.ToDouble(ElementsList[51].Value), 4))).ToString();

                            objProts = objSlash + "/" + objPierce + "/" + objBludg + "/" + objFire + "/" + objCold + "/" + objAcid + "/" + objLight;
                        }
                        else
                        { objProts = ""; }

                        if (objProts != "")
                        {
                            message = message + ", Al: " + objAl + ", Prots: " + objProts + ", Work: " + objWork + ", Burden: " + objBurden + 
                                " , Number: " + objStack + " , Tinks: " + objTinks +
                               ", Level: " + objLevel + ", " + objArmorSetName + " Set, " + objSpells +
                               ", covers: " + objCoversName + ", ManaC: " + objManaC +
                               ", MeleeD: " + objMelD + ", MagicD: " + objMagicD + ", MissileD: " + objMissD +
                               ", ElemVsMonster: " + objElemvsMons + ", Attack: " + objAttack +
                               ", MaxDam: " + objMaxDam + ", Variance: " + objVar + ", Embue: " + objEmbueTypeStr;
                        }
                        else
                        {

                            message = message + ", Al: " + objAl + ", Work: " + objWork + ", Burden: " + objBurden +
                                 " , Number: " + objStack + " , Tinks: " + objTinks +
                                ", Level: " + objLevel + ", " + objArmorSetName + " Set, " + objSpells +
                                ", covers: " + objCoversName + ", ManaC: " + objManaC +
                                ", MeleeD: " + objMelD + ", MagicD: " + objMagicD + ", MissileD: " + objMissD +
                                ", ElemVsMonster: " + objElemvsMons + ", Attack: " + objAttack +
                                ", MaxDam: " + objMaxDam + ", Variance: " + objVar + ", Embue: " + objEmbueTypeStr;
                        }
 
                        break;

    // *                
                    case 1:
                    case 2:
                    case 11:
                        if (objClass == 1)
                        {
                            objAl = ElementsList[8].Value;
                            objWork = ElementsList[15].Value;
                            objTinks = ElementsList[16].Value;


                            if (ElementsList[45].Value != "0" && ElementsList[1].Value.Contains("Covenant"))
                            {
                                string objAcid = ((Math.Round(Convert.ToDouble(ElementsList[45].Value), 4))).ToString();
                                string objLight = ((Math.Round(Convert.ToDouble(ElementsList[46].Value), 4))).ToString();
                                string objFire = ((Math.Round(Convert.ToDouble(ElementsList[47].Value), 4))).ToString();
                                string objCold = ((Math.Round(Convert.ToDouble(ElementsList[48].Value), 4))).ToString();
                                string objBludg = ((Math.Round(Convert.ToDouble(ElementsList[49].Value), 4))).ToString();
                                string objSlash = ((Math.Round(Convert.ToDouble(ElementsList[50].Value), 4))).ToString();
                                string objPierce = ((Math.Round(Convert.ToDouble(ElementsList[51].Value), 4))).ToString();

                                objProts = objSlash + "/" + objPierce + "/" + objBludg + "/" + objFire + "/" + objCold + "/" + objAcid + "/" + objLight;
                            }
                            else
                            { objProts = ""; }

                            if (objProts != "")
                            {

                                message = message + ", Al: " + objAl + " " + objProts + ", Work: " + objWork +
                                   ", Tinks: " + objTinks;
                            }
                            else
                            {
                                message = message + ", Al: " + objAl + ", Work: " + objWork +
                                   ", Tinks: " + objTinks;
                            }
                        }
                        if (objClass == 1 || objClass == 2)
                        {
                            objCovers = Convert.ToInt32(ElementsList[10].Value);
                            findCoversName(objCovers);
                            message = message + ", Covers: " + objCoversName;
                        }
                        if (objClass == 1 || objClass == 2 || objName.Contains("Aetheria"))
                        {
                            objLevel = ElementsList[28].Value;
                            objArmorSet = Convert.ToInt32(ElementsList[9].Value);
                            objSpells = ElementsList[41].Value.ToString();
                            findArmorSetName(objArmorSet);
                            objBurden = ElementsList[43].Value;

                            message = message + ", Level: " + objLevel + ", Set: " + objArmorSetName
                                + ", Spells: " + objSpells;
                        }
                        break; 
                        if (objClass == 11 && !objName.Contains("Aetheria"))
                        {
                            objStack = ElementsList[44].Value;
                            message = message + ", # in Stack: " + objStack;
                            }

                    case 3:
                        objLevel = ElementsList[28].Value;
                        objSpells = ElementsList[41].Value.ToString();
                        message = message + ", Level: " + objLevel + ", " + objSpells;
                        break; 

                    case 4:
                        objWork = ElementsList[15].Value;
                        objTinks = ElementsList[16].Value;
                        objLevel = ElementsList[28].Value;
                        objDamageTypeInt = Convert.ToInt32(ElementsList[42].Value);
                        objMissD = ((Math.Round(Convert.ToDouble(ElementsList[34].Value), 2) - 1) * 100).ToString();
                        if (Convert.ToDouble(objMissD) < 0) { objMissD = "0"; }
                        objManaC = (Math.Round(Convert.ToDouble(ElementsList[33].Value), 2) * 100).ToString();
                        objMagicD = ((Math.Round(Convert.ToDouble(ElementsList[32].Value), 2) - 1) * 100).ToString();
                        if (Convert.ToDouble(objMagicD) < 0) { objMagicD = "0"; }
                        objMelD = Math.Round(((Convert.ToDouble(ElementsList[31].Value) - 1) * 100), 2).ToString();
                        objElemvsMons = Math.Round(((Convert.ToDouble(ElementsList[30].Value) - 1) * 100), 2).ToString();
                        objEmbueTypeInt = Convert.ToInt32(ElementsList[24].Value);
                        findEmbueTypeStr(objEmbueTypeInt);
                        findDamageType();
                        objSpells = ElementsList[41].Value.ToString();

                        message = message + ", Damage: " + objDamageType + ", Wield Level: " + objLevel + 
                            ", ElemVsMonster: " + objElemvsMons +
                            ", ManaC: " + objManaC + ", MeleeD: " + objMelD + ", MagicD: " + objMagicD + 
                            ", MissileD: " + objMissD + ", Embue: " + objEmbueTypeStr + 
                            ", Work: " + objWork + ", Tinks: " + objTinks + ", " + objSpells;
                        break;
                    case 5:
                        objDamageTypeInt = Convert.ToInt32(ElementsList[29].Value);
                        findDamageType();
                        objAttack = Math.Round((Convert.ToDouble(ElementsList[36].Value)-1) * 100).ToString();
                        if (Convert.ToDouble(objAttack) < 0) { objAttack = "0"; }
                        objMaxDam = ElementsList[38].Value.ToString();
                        objMaxDamLong = Convert.ToInt32(objMaxDam);
                        objDVar = (Convert.ToDouble(ElementsList[39].Value));
                        objMinDam = Math.Round(objMaxDamLong - ((objDVar)*(objMaxDamLong)),2).ToString();
                        objEmbueTypeInt = Convert.ToInt32(ElementsList[24].Value);
                        findEmbueTypeStr(objEmbueTypeInt);
                        objWork = ElementsList[15].Value.ToString();
                        objTinks = ElementsList[16].Value.ToString();
                        objLevel = ElementsList[28].Value.ToString();
                        objMissD = ((Math.Round(Convert.ToDouble(ElementsList[34].Value), 2) - 1) * 100).ToString();
                        if (Convert.ToDouble(objMissD) < 0) { objMissD = "0"; }
                        objMagicD = ((Math.Round(Convert.ToDouble(ElementsList[32].Value), 2) - 1) * 100).ToString();
                        if (Convert.ToDouble(objMagicD) < 0) { objMagicD = "0"; }
                        objMelD = Math.Round(((Convert.ToDouble(ElementsList[31].Value) - 1) * 100), 2).ToString();
                        objSpells = ElementsList[41].Value.ToString();
                        message = message + " , Damage: " + objDamageType + ", WieldLevel: " + objLevel +
                            ", Attack: " + objAttack + ", MeleeD: " + objMelD + 
                            " Min-Max Damage: " + objMinDam + "-" + objMaxDam +
                            " , Embue: " + objEmbueTypeStr + ", Work: " + objWork + ", Tinks: " + objTinks + 
                            ", MissD: " + objMissD + ", MagicD " + objMagicD + ", " + objSpells;
                        break;
                    case 6:
                        objDamageTypeInt = Convert.ToInt32(ElementsList[29].Value);
                        findDamageType();
                        objWork = ElementsList[15].Value.ToString();
                        objTinks = ElementsList[16].Value.ToString();
                        objLevel = ElementsList[28].Value.ToString();
                        string objElDam = ElementsList[21].Value.ToString();
                        string objDamBon = ((Math.Round(Convert.ToDouble(ElementsList[37].Value), 2) - 1) * 100).ToString();
                        objMissD = ((Math.Round(Convert.ToDouble(ElementsList[34].Value), 2) - 1) * 100).ToString();
                        if (Convert.ToDouble(objMissD) < 0) { objMissD = "0"; }
                        objMagicD = ((Math.Round(Convert.ToDouble(ElementsList[32].Value), 2) - 1) * 100).ToString();
                        if (Convert.ToDouble(objMagicD) < 0) { objMagicD = "0"; }
                        objMelD = Math.Round(((Convert.ToDouble(ElementsList[31].Value) - 1) * 100), 2).ToString();

                        if (Convert.ToDouble(objDamBon) < 0) { objDamBon = "0"; }


                        objSpells = ElementsList[41].Value.ToString();
                        message = message + ", Damage Type: " + objDamageType + ", WieldLevel: " + objLevel + 
                            ", Elem Dmg: " + objElDam + 
                            ", Damage Bonus: " + objDamBon + "MelD: " + objMelD + 
                            ", MissD: " + objMissD + ", MagicD: " + objMagicD +
                            ", Work: " + objWork + ", Tinks: " + objTinks + ", " + objSpells;
                        break;
                    case 7:
                        string objSalvWork = ElementsList[35].Value.ToString();
                        objBurden = ElementsList[43].Value;
                        long objMat = Convert.ToInt32(ElementsList[7].Value);
                        findMaterialName(objMat);
                        Mishna.PluginCore.Util.WriteToChat("Material" + objMatName);

                        message = message + ", Material: " + objMatName + ", Work: " + objSalvWork + ", Burden: " + objBurden;

                        break;
 
                    default:
                        objBurden = ElementsList[43].Value;
                        objStack = ElementsList[44].Value;
                        objSpells = ElementsList[41].Value.ToString();

                        message = message + ", Burden: " + objBurden + ", Number: " + objStack + ", Spells: " + objSpells;
                        break;


                }



                    Mishna.PluginCore.Util.WriteToChat(message);
                    message = null;
                    elements = null;
                    childElements = null;
                    element = null;
                
            }
            

            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }



        // It helps to clear the list before making new selection
        [MVControlEvent("btnClrInventory", "Click")]
        void btnClrInventory_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            lstInventory.Clear();
             cmbSelectClass.Selected = 0;
            cmbWieldAttrib.Selected = 0;
            cmbDamageType.Selected = 0;
            cmbLevel.Selected = 0;
            cmbArmorSet.Selected = 0;
            cmbMaterial.Selected = 0;
            cmbCoverage.Selected = 0;
            cmbArmorLevel.Selected = 0;
            cmbSalvWork.Selected = 0;
            cmbEmbue.Selected = 0;
            objEmbueTypeInt = 0;
            objDamageTypeInt = 0;
            objLevelInt = 1;
            objWieldAttrInt = 0;
            objSalvWork = "None";
            objClassName = null;
            objMat = 0;
            objCovers = 0;
            objArmorLevel = 1;
            objArmorSet = 0;
            newDoc = null;
            xdoc = null;
            childElements = null;
            elements = null;
            txbSelect.Text = "";
            mySelect = "";
       }
 
    } // end of partial class

} //end of namespace


 