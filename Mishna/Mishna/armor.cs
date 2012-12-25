using System;
using System.Xml;
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
            mWaitingForID = new List<WorldObject>();

            armorFilename = currDir + @"\" + toonName + "Armor.xml";
            genArmorFilename = world + @"\allToonsArmor.xml";
            holdingArmorFilename = world + @"\holdingArmor.xml";


           xdoc = new XDocument(new XElement("Objs"));

 
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
                    }



                    //if (obj.ObjectClass == ObjectClass.WandStaffOrb || obj.ObjectClass == ObjectClass.MeleeWeapon ||
                    //  obj.ObjectClass == ObjectClass.MissileWeapon)
                    //{

                    //    shouldAdd = true;
                    //}
                    if (shouldAdd)
                    {
                        Globals.Host.Actions.RequestId(obj.Id);
                        mWaitingForID.Add(obj);
                    }
              }  //end try
                catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

              } // endof foreach world object

              fn = "armorFilename";
              mIsFinished();
            } //end of try
           catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
       }




    } // end of partial class

}  // end of namespace