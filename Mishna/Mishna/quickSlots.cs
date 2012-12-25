using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VirindiViewService;
using VirindiViewService.Controls;
using VirindiHUDs;
using MyClasses.MetaViewWrappers;
using MyClasses.MetaViewWrappers.VirindiViewServiceHudControls;
using System.Drawing;
using Decal.Adapter;
using Decal.Adapter.Wrappers;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using System.IO;



namespace Mishna
{

    public partial class PluginCore : PluginBase
    {

        //public void InitializeRawXML(Decal.Adapter.Wrappers.PluginHost p, string pXML)
        //{
        //    VirindiViewService.XMLParsers.Decal3XMLParser ps = new VirindiViewService.XMLParsers.Decal3XMLParser();
        //    ViewProperties iprop;
        //    ControlGroup igroup;
        //    ps.Parse(pXML, out iprop, out igroup);
        //    quickies = new VirindiViewService.HudView(iprop, igroup);
        //}


        private void getQuickSlots()
        {
            try
            {
             Mishna.PluginCore.Util.WriteToChat("At QuickSlots");
             createQuickies();
               

             }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        private void fillList()
        {
            try
            {
                foreach (WorldObject obj in Core.WorldFilter.GetInventory())
                {
                    ObjectClass objClass = obj.ObjectClass;
                    if (objClass == ObjectClass.MeleeWeapon || objClass == ObjectClass.MissileWeapon || objClass == ObjectClass.WandStaffOrb)
                    {
                        Int32 objIcon = obj.Icon;
                        string objIDstr = obj.Id.ToString();
                    //    IListRow newRow = lstQuickies.AddRow();
                    //    newRow[0][1] = objIcon;
                    //    newRow[1][0] = obj.ToString();
                      //  newRow[2][0] = objIDstr;
                    }

                }
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }


        //private void createvQuickies()
        //{
        //    try
        //    {
        //    // if (inactive || pluginSettings == null) return;

        //        if (vQuickies == null)
        //        {
        //            vQuickies = new HudView("Quickslots", 40, 200, new ACImage(System.Drawing.Color.Black), false);
        //            vQuickies.Alpha = 10; //vQuickies.Alpha = pluginSettings.alpha;
        //            // vQuickies.Icon = 0x64F8;
        //            vQuickies.Visible = true;
        //            vQuickies.UserGhostable = true;
        //            vQuickies.Ghosted = true;
        //            vQuickies.UserMinimizable = true;
        //            vQuickies.UserAlphaChangeable = true;
        //            // vQuickSlots.ShowIcon = true;
        //            vQuickies.Location = mLocation;
        //            //cHud.LoadUserSettings();
                    
                    

        //            vStats = new HudFixedLayout();
        //            vQuickies.Controls.HeadControl = vStats;

        //            vQuickies.ThemeChanged += new EventHandler(vQuickies_ThemeChanged);
        //            vQuickies.Moved += new EventHandler(vQuickies_Moved);

        //            //   MouseMovingHud += new EventHandler(MouseMovingHud);
        //        }
        //    }
        //    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        //}


        //void vQuickies_ThemeChanged(object sender, EventArgs e)
        //{
        //    redrawvQuickies();
        //}

        //[MVControlEvent("vQuickies", "Moved")]
        //void vQuickies_Moved(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Mishna.PluginCore.Util.WriteToChat("I am at vQuickies_Moved");
        //     //   int xr = mMouseHudOffset.X;
        //     //   int yr = mMouseHudOffset.Y;
        //        mLocation = new Point(150 + 20, 150 - 20);
        //        redrawvQuickies();
        //    }
        //    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


        //}

        //private void redrawvQuickies()
        //{
        //    try
        //    {
        //        createvQuickies();
        //        // if (inactive || pluginSettings == null) return;

        //        //if (vQuickies == null) createvQuickies();

        //        //vQuickies.Visible = true;

        //        //int h = 10;

        //        //foreach (HudControl ctrl in vQuickies.Controls.ChildrenOf(vStats))
        //        //{
        //        //    vStats.RemovedChild(ctrl);
        //        //    ctrl.Dispose();
        //        //}
        //    }
        //    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


        //    //foreach (string s in vQuickSlots.Theme.GetValList())
        //    //{
        //    //    if (s.ToLower().Contains("color") || s.ToLower().Contains("text"))
        //    //    {
        //    //        try
        //    //        {
        //    //            Color c = vQuickSlots.Theme.GetColor(s);
        //    //            HudStaticText ax = new HudStaticText();
        //    //            ax.FontHeight = TEXTSIZE;
        //    //            ax.Text = s;
        //    //            ax.TextColor = c;

        //    //            vStats.AddControl(ax, new Rectangle(0, h, 200, LINEHEIGHT + LINEHEIGHT - TEXTSIZE));

        //    //            h += LINEHEIGHT + LINEHEIGHT - TEXTSIZE;
        //    //        }
        //    //        catch (Exception ex) { };                    
        //    //    }
        //    //}
        //    //vQuickSlots.Height = h;
        //}







        //public class quickSlots
        //{


        //    //public quickSlots()
        //    //{
        //    //    try
        //    //    {
        //    //        Rectangle mregion = new Rectangle();
        //    //        mregion.X = 400;
        //    //        mregion.Y = 50;
        //    //        mregion.Width = 50;
        //    //        mregion.Height = 800;
        //    //        //   WindowHud quickies = new WindowHud(mregion);

        //    //        //          MyClasses.MetaViewWrappers.IView Quickies;

        //    //        //          Quickies = MyClasses.MetaViewWrappers.ViewSystemSelector.CreateViewResource(PluginCore.host, "Mishna.Views.quickSlots.xml");
        //    //        //         MVWireupHelper.WireupStart(Quickies,PluginCore.host);
        //    //        //    lstQuickies = (MyClasses.MetaViewWrappers.IList)quickies["lstQuickies"];
        //    //        // lstQuickies.Selected += new EventHandler<MVListSelectEventArgs>(lstQuickies_Selected);

        //    //    }
        //    //    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        //    //}//end public quickslots


        //    [MVControlEvent("lstQuickies", "Selected")]
        //    private void lstQuickies_Selected(object sender, MyClasses.MetaViewWrappers.MVListSelectEventArgs e)
        //    {

        //    }
        //}
    }
}



//private void setUpIcons()
// {
//     try
//     {
//         //     XDocument iconDoc = new XDocument();
//         iconDoc = XDocument.Load(toonQuickSlotsFilename);
//         IEnumerable<XElement> myelements = iconDoc.Element("QuickSlots").Descendants("Obj");
//         // myListViews lstQuickies = new myListViews();
//         foreach (XElement element in myelements)
//         {
//             objIcon = Convert.ToInt32(element.Element("ObjIcon").Value);
//             long objID = Convert.ToInt32(element.Element("ObjID").Value);
//             string objIDstr = objID.ToString();
//             IListRow newRow = lstQuickies.AddRow();
//             //     MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.List.ListRow newRow = (MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.List.ListRow)lstQuickies.AddRow();
//             //         lstQuickies.AddRow();
//             newRow[0][1] = objIcon;
//             newRow[3][0] = objIDstr;
//         }
//     }
//     catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


// }

//class myListViews : MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.List
//{
//    myListRow newRow;
//}

//  myListViews();

//    public myListRow AddRow()
//    {

//        newRow = new myListRow();
//         return newRow;

//    }



//}

////  class myListRow : MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.List.ListRow
//  {
//      //public myListRow(int row, MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.List myListViews)
//      //{
//      //    myListRow get; myListRow set;
//      //}

//  }
//  }


