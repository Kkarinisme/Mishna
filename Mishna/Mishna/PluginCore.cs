using System;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using VirindiViewService;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using MyClasses.MetaViewWrappers;


/*
 * The template for this was created by Mag-nus and the following notes were compiled by him. 8/19/2011
 * 
 * No license applied, feel free to use as you wish. H4CK TH3 PL4N3T? TR45H1NG 0UR R1GHT5? Y0U D3C1D3!
 * 
 * Notice how I use try/catch on every function that is called or raised by decal (by base events or user initiated events like buttons, etc...).
 * This is very important. Don't crash out your users!
 * 
 * In 2.9.6.4+ Host and Core both have Actions objects in them. They are essentially the same thing.
 * You sould use Host.Actions though so that your code compiles against 2.9.6.0 (even though I reference 2.9.6.5 in this project)
 * 
  * 
 * If you have issues compiling, remove the Decal.Adapater and VirindiViewService references and add the ones you have locally.
 * Decal.Adapter should be in C:\Games\Decal 3.0\
 * VirindiViewService should be in C:\Games\VirindiPlugins\VirindiViewService\
*/

namespace Mishna
{
    // View is the path to the xml file that contains info on how to draw our in-game plugin. The xml contains the name and icon our plugin shows in-game.
    // The other key here is that mainView.xml must be included as an embeded resource. If its not, your plugin will not show up in-game.

    [WireUpBaseEvents]
    public partial class PluginCore : PluginBase
    {
        /// <summary>
        /// This is called when the plugin is started up. This happens only once.
        /// </summary>
        /// 


        static PluginCore Instance;
        static PluginHost host;
        protected override void Startup()
        {
            try
            {
                // This initializes our static Globals class with references to the key objects your plugin will use, Host and Core.
                // The OOP way would be to pass Host and Core to your objects, but this is easier.
                Globals.Init("Mishna", Host, Core);
                Instance = this;
                host = Host;
             }
            catch (Exception ex) { Util.LogError(ex); }
        }

        /// <summary>
        /// This is called when the plugin is shut down. This happens only once.
        /// </summary>

        protected override void Shutdown()
        {
            try
            {
                //Destroy the view.
                MVWireupHelper.WireupEnd(this);
                View.Dispose();

                btnGetInventory.Click -= new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnGetInventory_Click);
                btnUpdateInventory.Click -= new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnUpdateInventory_Click);
                // btnGetToonArmor.Click -= new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnGetToonArmor_Click);
                btnGetToonStats.Click -= new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnGetToonStats_Click);
                btnLstInventory.Click -= new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnLstInventory_Click);
                btnClrInventory.Click -= new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnClrInventory_Click);
                cmbSelectClass.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbSelectClass_Change);
                cmbWieldAttrib.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbWieldAttrib_Change);
                cmbDamageType.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbDamageType_Change);
                cmbLevel.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbLevel_Change);
                cmbMaterial.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbMaterial_Change);
                cmbArmorSet.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbArmorSet_Change);
                cmbArmorLevel.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbArmorLevel_Change);
                cmbCoverage.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbCoverage_Change);
                cmbSalvWork.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbSalvWork_Change);
                cmbEmbue.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbEmbue_Change);
                chkInventory.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkInventory_Change);
                chkInventoryWaiting.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkInventoryWaiting_Change);
                chkInventoryBurden.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkInventoryBurden_Change);
                chkInventoryComplete.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkInventoryComplete_Change);
                chkStats.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkStats_Change);
                
//            CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);

//            CoreManager.Current.CharacterFilter.Login -= new EventHandler<Decal.Adapter.Wrappers.LoginEventArgs>(CharacterFilter_Login);

//            CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
//            CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
 //           CoreManager.Current.WorldFilter.ReleaseObject -= new EventHandler<Decal.Adapter.Wrappers.ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);

            if (quickiesHud != null)
            {
               quickiesHud.Dispose();
               quickiesHud = null;
            }

            if (quickiesHud_Head != null)
            {
               quickiesHud.Dispose();
               quickiesHud_Head = null;
            }
    


                lstInventory.Selected -= new EventHandler<MVListSelectEventArgs>(lstInventory_Selected);
                mWaitingForIDTimer.Tick -= new EventHandler(TimerEventProcessor);



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
                objDamageTypeInt = 0;
                objLevelInt = 1;
                objWieldAttrInt = 0;
                objSalvWork = "None";
                objMat = 0;
                objCovers = 0;
                objArmorLevel = 1;
                newDoc = null;
                xdoc = null;
 


            }
            catch (Exception ex) { Util.LogError(ex); }
        }


        [BaseEvent("LoginComplete", "CharacterFilter")]
        private void CharacterFilter_LoginComplete(object sender, EventArgs e)
        {
            try
            {
                Mishna.PluginCore.Util.WriteToChat("Plugin now online. Server population: " + Core.CharacterFilter.ServerPopulation);

                //Initialize core variables
                toonName = Core.CharacterFilter.Name;
                world = Core.CharacterFilter.Server;
                // need to set path for saving folders to the current world which is called currDir
                setPathToWorld();
                // need a path for the toon for saving files
                setPathToToon();
                // need to be certain above directories exist for saving files or checking them
                chkDirExists();
                //Need to set up names of files for inventory for individual toon and the file to contain all inventory
                inventoryFilename = currDir + @"\" + toonName + "Inventory.xml";

                genInventoryFilename = world + @"\inventory.xml";
                genSettingsFilename = world + @"\settings.xml";
                if(File.Exists(genSettingsFilename))
                {   
                   xdoc = XDocument.Load(genSettingsFilename);
                   mcount = xdoc.Root.Descendants().Count();
               }
                if (!File.Exists(genSettingsFilename) || mcount < 4)
                {
                    Mishna.PluginCore.Util.WriteToChat("I am at creating new file. mcount = " + mcount);

                    inventoryEnabled = false;
                    inventoryBurdenEnabled = false;
                    inventoryCompleteEnabled = false;
                    statsEnabled = false;
                    quickSlotsEnabled = false;
                    xdoc = new XDocument(new XElement("Settings"));

                    xdoc.Element("Settings").Add(new XElement("Setting",
                         new XElement("InventoryEnabled", inventoryEnabled),
                         new XElement("InventoryBurdenEnabled", inventoryBurdenEnabled),
                         new XElement("InventoryCompleteEnabled", inventoryCompleteEnabled),
                         new XElement("StatsEnabled", statsEnabled),
                         new XElement("QuickSlotsEnabled", quickSlotsEnabled)));
                    xdoc.Save(genSettingsFilename);

                }
                toonQuickSlotsFilename = currDir + @"\" + toonName + "QuickSlots.xml";
                if (!File.Exists(toonQuickSlotsFilename))
                {
                    XDocument tempDoc = new XDocument(new XElement("QuickSlots"));
                    tempDoc.Save(toonQuickSlotsFilename);
                    tempDoc = null;
                }
                holdingInventoryFilename = world + @"\holdingInventory.xml";
                 inventorySelect = world + @"\inventorySelected.xml";
                  //// doGetArmor();  Cannot go here if do get inventory() is turned on; two conflict
                 getSettings();
                 ViewInit();
              //   if (quickSlotsEnabled)
              //   {getQuickSlots(); }
                 if (inventoryCompleteEnabled)
                 {
                     inventoryBurdenEnabled = false;
                     inventoryEnabled = false;
                     m = 500;
                     doGetInventory();
               
                 }
                 if (inventoryBurdenEnabled)
                 {
                     inventoryEnabled = false;
                     getBurden = true;
                     doUpdateInventory();
                 }
                 if (inventoryEnabled)
                 { doUpdateInventory(); }
                 if (statsEnabled)
                 { getStats(); }
           } //end of try
           catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        private void getSettings()
        {
            try
            {
                if (System.IO.File.Exists(genSettingsFilename))
                {
                    xdoc = new XDocument();  //(genSettingsFilename);
                   // List<XElement> settingsList = new List<XElement>();
                    List<string> settingsList = new List<string>();
                    xdoc = XDocument.Load(genSettingsFilename);


                    foreach (XElement element in xdoc.Element("Settings").Element("Setting").Descendants())
                    {
                        string mval = element.Value;
                        settingsList.Add(mval);

                   }
 
                    inventoryEnabled = Convert.ToBoolean(settingsList[0]);
                    inventoryBurdenEnabled = Convert.ToBoolean(settingsList[1]);
                     inventoryCompleteEnabled = Convert.ToBoolean(settingsList[2]);
                    statsEnabled = Convert.ToBoolean(settingsList[3]);
                   // quickSlotsEnabled = Convert.ToBoolean(settingsList[5];
                }
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }

        private void SaveSettings()
        {
            try
            {
                xdoc= new XDocument(new XElement("Settings"));
                xdoc.Element("Settings").Add(new XElement("Setting",
                         new XElement("InventoryEnabled", inventoryEnabled),
                         new XElement("InventoryBurdenEnabled", inventoryBurdenEnabled),
                         new XElement("InventoryCompleteEnabled", inventoryCompleteEnabled),
                         new XElement("StatsEnabled", statsEnabled),
                         new XElement("QuickSlotsEnabled", quickSlotsEnabled)));
               xdoc.Save(genSettingsFilename);
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        [BaseEvent("ChangeObject", "WorldFilter")]
        void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
        {
            try
            {
                // This can get very spammy so I filted it to just print on ident received
                if (e.Change == WorldChangeType.IdentReceived)
                { identRecd = true; }
            }
            catch (Exception ex) { Util.LogError(ex); }
        }


 

        [BaseEvent("Logoff", "CharacterFilter")]
        private void CharacterFilter_Logoff(object sender, Decal.Adapter.Wrappers.LogoffEventArgs e)
        {
            try
            {
                // Unsubscribe to events here, but know that this event is not gauranteed to happen. I've never seen it not fire though.
                // This is not the proper place to free up resources, but... its the easy way. It's not proper because of above statement.
                //      Mishna.Globals.Core.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject2);
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }
    }
}


