///////////////////////////////////////////////////////////////////////////////
//File: MainView.cs
//
//Description: An example plugin using the VVS MetaViewWrappers. When VVS is
//  enabled, the plugin's view appears under the VVS bar. Otherwise, it appears
//  in the regular Decal bar.
//
//This file is Copyright (c) 2009 VirindiPlugins
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//HE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using VirindiViewService;
using MyClasses.MetaViewWrappers;
using System.Drawing;
using Decal.Adapter;
using Decal.Adapter.Wrappers;



namespace Mishna
{

    public partial class PluginCore : PluginBase
    {
        #region Auto-generated view codee
        // Need this line to be able to initialize View below

        MyClasses.MetaViewWrappers.IView View;

        MyClasses.MetaViewWrappers.IButton btnGetInventory;
        MyClasses.MetaViewWrappers.IButton btnUpdateInventory;
        MyClasses.MetaViewWrappers.IButton btnGetBurden;
        //   MyClasses.MetaViewWrappers.IButton btnGetToonArmor;
        MyClasses.MetaViewWrappers.IButton btnGetToonStats;
        MyClasses.MetaViewWrappers.IButton btnLstInventory;
        MyClasses.MetaViewWrappers.IButton btnClrInventory;
 //       MyClasses.MetaViewWrappers.IButton btnLstToonStats;
 //       MyClasses.MetaViewWrappers.IButton btnGetToonArmor;

        MyClasses.MetaViewWrappers.ICombo cmbSelectClass;
        MyClasses.MetaViewWrappers.ICombo cmbWieldAttrib;
        MyClasses.MetaViewWrappers.ICombo cmbDamageType;
        MyClasses.MetaViewWrappers.ICombo cmbLevel;
        MyClasses.MetaViewWrappers.ICombo cmbArmorSet;
        MyClasses.MetaViewWrappers.ICombo cmbMaterial;
        MyClasses.MetaViewWrappers.ICombo cmbCoverage;
        MyClasses.MetaViewWrappers.ICombo cmbArmorLevel;
        MyClasses.MetaViewWrappers.ICombo cmbSalvWork;
        MyClasses.MetaViewWrappers.ICombo cmbEmbue;

        MyClasses.MetaViewWrappers.IList lstInventory;
        MyClasses.MetaViewWrappers.ITextBox txbSelect;

        MyClasses.MetaViewWrappers.ICheckBox chkInventory;
        MyClasses.MetaViewWrappers.ICheckBox chkInventoryWaiting;
        MyClasses.MetaViewWrappers.ICheckBox chkInventoryBurden;
        MyClasses.MetaViewWrappers.ICheckBox chkInventoryComplete;
        MyClasses.MetaViewWrappers.ICheckBox chkToonStats;
      //  MyClasses.MetaViewWrappers.ICheckBox chkToonArmor;
        MyClasses.MetaViewWrappers.ICheckBox chkQuickSlotsv;
        MyClasses.MetaViewWrappers.ICheckBox chkQuickSlotsh;


        MyClasses.MetaViewWrappers.IStaticText lblSetup;
        MyClasses.MetaViewWrappers.IStaticText lblInventoryExpl;
        MyClasses.MetaViewWrappers.IStaticText lblArmor;
        MyClasses.MetaViewWrappers.IStaticText lblSalvage;
        MyClasses.MetaViewWrappers.IStaticText lblWeapons;


        
        //MyClasses.MetaViewWrappers.IList ToonAttributes;

        void ViewInit()
        {
            try
            {

                //Create view here
                View = MyClasses.MetaViewWrappers.ViewSystemSelector.CreateViewResource(PluginCore.host, "Mishna.Views.mainView.xml");


                btnGetInventory = (MyClasses.MetaViewWrappers.IButton)View["btnGetInventory"];
                btnUpdateInventory = (MyClasses.MetaViewWrappers.IButton)View["btnUpdateInventory"];
                btnGetBurden = (MyClasses.MetaViewWrappers.IButton)View["btnGetBurden"];
       //          btnGetToonArmor = (MyClasses.MetaViewWrappers.IButton)View["btnGetToonArmor"];
                btnGetToonStats = (MyClasses.MetaViewWrappers.IButton)View["btnGetToonStats"];
                btnLstInventory = (MyClasses.MetaViewWrappers.IButton)View["btnLstInventory"];
                btnClrInventory = (MyClasses.MetaViewWrappers.IButton)View["btnClrInventory"];
                
                cmbSelectClass = (MyClasses.MetaViewWrappers.ICombo)View["cmbSelectClass"];
                cmbSelectClass.Selected = 0;
                cmbWieldAttrib = (MyClasses.MetaViewWrappers.ICombo)View["cmbWieldAttrib"];
                cmbWieldAttrib.Selected = 0;
                cmbDamageType = (MyClasses.MetaViewWrappers.ICombo)View["cmbDamageType"];
                cmbDamageType.Selected = 0;
               cmbLevel = (MyClasses.MetaViewWrappers.ICombo)View["cmbLevel"];
                cmbLevel.Selected = 0;
                cmbArmorSet = (MyClasses.MetaViewWrappers.ICombo)View["cmbArmorSet"];
                cmbArmorSet.Selected = 0;
                cmbMaterial = (MyClasses.MetaViewWrappers.ICombo)View["cmbMaterial"];
                cmbMaterial.Selected = 0;
                cmbCoverage = (MyClasses.MetaViewWrappers.ICombo)View["cmbCoverage"];
                cmbCoverage.Selected = 0;
                cmbArmorLevel = (MyClasses.MetaViewWrappers.ICombo)View["cmbArmorLevel"];
                cmbArmorLevel.Selected = 0;
                cmbSalvWork = (MyClasses.MetaViewWrappers.ICombo)View["cmbSalvWork"];
                cmbSalvWork.Selected = 0;
                cmbEmbue = (MyClasses.MetaViewWrappers.ICombo)View["cmbEmbue"];
                cmbEmbue.Selected = 0;

                chkInventory = (MyClasses.MetaViewWrappers.ICheckBox)View["chkInventory"];
                chkInventory.Checked = binventoryEnabled;
                chkInventoryBurden = (MyClasses.MetaViewWrappers.ICheckBox)View["chkInventoryBurden"];
                chkInventoryBurden.Checked = binventoryBurdenEnabled;
                chkInventoryComplete = (MyClasses.MetaViewWrappers.ICheckBox)View["chkInventoryComplete"];
                chkInventoryComplete.Checked = binventoryCompleteEnabled;
                chkInventoryWaiting = (MyClasses.MetaViewWrappers.ICheckBox)View["chkInventoryWaiting"];
                chkInventoryWaiting.Checked = binventoryWaitingEnabled;
                chkToonStats = (MyClasses.MetaViewWrappers.ICheckBox)View["chkToonStats"];
                chkToonStats.Checked = btoonStatsEnabled;
                //chkToonArmor = (MyClasses.MetaViewWrappers.ICheckBox)View["chkToonArmor"];
                //chkToonArmor.Checked = btoonArmorEnabled;
                chkQuickSlotsv = (MyClasses.MetaViewWrappers.ICheckBox)View["chkQuickSlotsv"];
                chkQuickSlotsv.Checked = bquickSlotsvEnabled;
                chkQuickSlotsh = (MyClasses.MetaViewWrappers.ICheckBox)View["chkQuickSlotsh"];
                chkQuickSlotsh.Checked = bquickSlotshEnabled;
  
              
                lstInventory = (MyClasses.MetaViewWrappers.IList)View["lstInventory"];

                txbSelect = (MyClasses.MetaViewWrappers.ITextBox)View["txbSelect"];

                lblWeapons = (MyClasses.MetaViewWrappers.IStaticText)View["lblWeapons"];
                lblArmor = (MyClasses.MetaViewWrappers.IStaticText)View["lblArmor"];
                lblSalvage = (MyClasses.MetaViewWrappers.IStaticText)View["lblSalvage"];
                lblSetup = (MyClasses.MetaViewWrappers.IStaticText)View["lblSetup"];
                lblInventoryExpl = (MyClasses.MetaViewWrappers.IStaticText)View["lblInventoryExpl"];



                btnGetInventory.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnGetInventory_Click);
                btnUpdateInventory.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnUpdateInventory_Click);
                btnGetBurden.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnGetBurden_Click);
             //   btnGetToonArmor.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnGetToonArmor_Click);
                btnGetToonStats.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnGetToonStats_Click);
                btnLstInventory.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnLstInventory_Click);
                btnClrInventory.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(btnClrInventory_Click);
                cmbSelectClass.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbSelectClass_Change);
                cmbWieldAttrib.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbWieldAttrib_Change);
                cmbDamageType.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbDamageType_Change);
                cmbLevel.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbLevel_Change);
                cmbMaterial.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbMaterial_Change);
                cmbArmorSet.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbArmorSet_Change);
                cmbArmorLevel.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbArmorLevel_Change);
                cmbCoverage.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbCoverage_Change);
                cmbSalvWork.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbSalvWork_Change);
                cmbEmbue.Change += new EventHandler<MyClasses.MetaViewWrappers.MVIndexChangeEventArgs>(cmbEmbue_Change);
                chkInventory.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkInventory_Change);
                chkInventoryWaiting.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkInventoryWaiting_Change);
                chkInventoryBurden.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkInventoryBurden_Change);
                chkInventoryComplete.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkInventoryComplete_Change);
                chkToonStats.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkToonStats_Change);
              //  chkArmor.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkArmor_Change);
                 chkQuickSlotsv.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkQuickSlotsv_Change);
                 chkQuickSlotsh.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkQuickSlotsh_Change);
 
                lstInventory.Selected += new EventHandler<MVListSelectEventArgs>(lstInventory_Selected);
  //              ServerDispatch.Handler += new EventHandler<ServerDispatch_Handler
               

 //           CoreManager.Current.CharacterFilter.Login += new EventHandler<Decal.Adapter.Wrappers.LoginEventArgs>(CharacterFilter_Login);
 //           CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
 //           CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
 //           CoreManager.Current.WorldFilter.ReleaseObject += new EventHandler<Decal.Adapter.Wrappers.ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);

//              CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);

                
  

            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }

        void chkInventory_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                binventoryEnabled = e.Checked;

                SaveSettings();
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }

        void chkInventoryBurden_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                binventoryBurdenEnabled = e.Checked;

                SaveSettings();
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }

        void chkInventoryComplete_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                binventoryCompleteEnabled = e.Checked;

                SaveSettings();
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }


        void chkInventoryWaiting_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                binventoryWaitingEnabled = e.Checked;

             //   SaveSettings();
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }


        void chkToonStats_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                btoonStatsEnabled = e.Checked;

                SaveSettings();
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }

        void chkToonArmor_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                btoonArmorEnabled = e.Checked;

                SaveSettings();
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }

        void chkQuickSlotsv_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                bquickSlotsvEnabled = e.Checked;

                SaveSettings();
                if (bquickSlotsvEnabled)
                {
                    quickSlotsvEnabled();
                }
                else if (!bquickSlotsvEnabled)
                {
                    quickSlotsvNotEnabled();
                }

            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }

        void chkQuickSlotsh_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                bquickSlotshEnabled = e.Checked;

                SaveSettings();

                if (bquickSlotshEnabled)
                {
                    quickSlotshEnabled();
                }
                else if (!bquickSlotshEnabled)
                {
                    quickSlotshNotEnabled();
                }

            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }


        void ViewDestroy()
        {
            SaveSettings();
            doClearHud(quickiesvHud, xdocQuickSlotsv, quickSlotsvFilename);
            doClearHud(quickieshHud, xdocQuickSlotsh, quickSlotshFilename);

            if (quickiesvHud != null)
            {
                // Host.Render.RemoveHud(quickiesHud);
                quickiesvHud.Dispose();
                quickiesvHud = null;
            }


            if (quickieshHud != null)
            {
                // Host.Render.RemoveHud(quickiesHud);
                quickieshHud.Dispose();
                quickieshHud = null;
            }

            btnGetInventory = null;
           // btnGetToonArmor = null;
            lstInventory = null;
            btnGetToonStats = null;
            ////sldTest = null;
            View.Dispose();
        }
        #endregion Auto-generated view code
    }
}