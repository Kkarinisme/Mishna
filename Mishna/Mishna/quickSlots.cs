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
        XDocument xdocQuickSlotsv = null;
        XDocument xdocQuickSlotsh = null;


        private static VirindiViewService.HudView quickiesvHud = null;
        private static VirindiViewService.HudView quickieshHud = null;
        private static VirindiViewService.HudView quickiesHud = null;

        private static VirindiViewService.Controls.HudFixedLayout quickiesvHud_Head = null;
        private static VirindiViewService.Controls.HudFixedLayout quickieshHud_Head = null;
        WorldObject quickie = null;
        bool baddItem = false;
        bool bremoveItem = false;
        int nquickiev = 0;
        int nquickieh = 0;
        QuickSlotData thisQuickie = null;


        HudImageStack mQuickStackv0 = new HudImageStack();
        HudImageStack mQuickStackv1 = new HudImageStack();
        HudImageStack mQuickStackv2 = new HudImageStack();
        HudImageStack mQuickStackv3 = new HudImageStack();
        HudImageStack mQuickStackv4 = new HudImageStack();
        HudImageStack mQuickStackv5 = new HudImageStack();
        HudImageStack mQuickStackv6 = new HudImageStack();
        HudImageStack mQuickStackv7 = new HudImageStack();
        HudImageStack mQuickStackv8 = new HudImageStack();
        HudImageStack mQuickStackv9 = new HudImageStack();
        HudImageStack mQuickStackv10 = new HudImageStack();
        HudImageStack mQuickStackv11 = new HudImageStack();

        HudImageStack mQuickStackh0 = new HudImageStack();
        HudImageStack mQuickStackh1 = new HudImageStack();
        HudImageStack mQuickStackh2 = new HudImageStack();
        HudImageStack mQuickStackh3 = new HudImageStack();
        HudImageStack mQuickStackh4 = new HudImageStack();
        HudImageStack mQuickStackh5 = new HudImageStack();
        HudImageStack mQuickStackh6 = new HudImageStack();
        HudImageStack mQuickStackh7 = new HudImageStack();
        HudImageStack mQuickStackh8 = new HudImageStack();
        HudImageStack mQuickStackh9 = new HudImageStack();
        HudImageStack mQuickStackh10 = new HudImageStack();
        HudImageStack mQuickStackh11 = new HudImageStack();

        int nQuickieIDv0 = 0;
        int nQuickieIDv1 = 0;
        int nQuickieIDv2 = 0;
        int nQuickieIDv3 = 0;
        int nQuickieIDv4 = 0;
        int nQuickieIDv5 = 0;
        int nQuickieIDv6 = 0;
        int nQuickieIDv7 = 0;
        int nQuickieIDv8 = 0;
        int nQuickieIDv9 = 0;
        int nQuickieIDv10 = 0;
        int nQuickieIDv11 = 0;

        int nQuickieIDh0 = 0;
        int nQuickieIDh1 = 0;
        int nQuickieIDh2 = 0;
        int nQuickieIDh3 = 0;
        int nQuickieIDh4 = 0;
        int nQuickieIDh5 = 0;
        int nQuickieIDh6 = 0;
        int nQuickieIDh7 = 0;
        int nQuickieIDh8 = 0;
        int nQuickieIDh9 = 0;
        int nQuickieIDh10 = 0;
        int nQuickieIDh11 = 0;

        private HudCheckBox chkQuickiev0 = new HudCheckBox();
        private HudCheckBox chkQuickiev1 = new HudCheckBox();
        private HudCheckBox chkQuickiev2 = new HudCheckBox();
        private HudCheckBox chkQuickiev3 = new HudCheckBox();
        private HudCheckBox chkQuickiev4 = new HudCheckBox();
        private HudCheckBox chkQuickiev5 = new HudCheckBox();
        private HudCheckBox chkQuickiev6 = new HudCheckBox();
        private HudCheckBox chkQuickiev7 = new HudCheckBox();
        private HudCheckBox chkQuickiev8 = new HudCheckBox();
        private HudCheckBox chkQuickiev9 = new HudCheckBox();
        private HudCheckBox chkQuickiev10 = new HudCheckBox();
        private HudCheckBox chkQuickiev11 = new HudCheckBox();

        private HudCheckBox chkQuickieh0 = new HudCheckBox();
        private HudCheckBox chkQuickieh1 = new HudCheckBox();
        private HudCheckBox chkQuickieh2 = new HudCheckBox();
        private HudCheckBox chkQuickieh3 = new HudCheckBox();
        private HudCheckBox chkQuickieh4 = new HudCheckBox();
        private HudCheckBox chkQuickieh5 = new HudCheckBox();
        private HudCheckBox chkQuickieh6 = new HudCheckBox();
        private HudCheckBox chkQuickieh7 = new HudCheckBox();
        private HudCheckBox chkQuickieh8 = new HudCheckBox();
        private HudCheckBox chkQuickieh9 = new HudCheckBox();
        private HudCheckBox chkQuickieh10 = new HudCheckBox();
        private HudCheckBox chkQuickieh11 = new HudCheckBox();

        List<HudCheckBox> vchk = new List<HudCheckBox>();
        List<HudCheckBox> hchk = new List<HudCheckBox>();
        List<Int32> vID = new List<Int32>();
        List<Int32> hID = new List<Int32>();
        List<HudImageStack> vst = new List<HudImageStack>();
        List<HudImageStack> hst = new List<HudImageStack>();

        private static Point vpt = new Point();
        private static Point hpt = new Point();

        private VirindiViewService.Controls.HudButton btnQuickiesvAdd = new HudButton();
        private VirindiViewService.Controls.HudButton btnQuickiesvRemove = new HudButton();
        private VirindiViewService.Controls.HudButton btnQuickieshAdd = new HudButton();
        private VirindiViewService.Controls.HudButton btnQuickieshRemove = new HudButton();
        //private VirindiViewService.Controls.HudButton btnQuickiesAdd = new HudButton();
        //private VirindiViewService.Controls.HudButton btnQuickiesRemove = new HudButton();


        public class QuickSlotData
        {
            public string Name;
            public int Guid;
            public ObjectClass ObjectClass;
            public int ImbueId;
            public int Icon;
            public int IconUnderlay;
            public int IconOverlay;
        }

        private void createQuickies(VirindiViewService.HudView hudview)
        {
            try
            {
                

                if (hudview == quickiesvHud)
                {
                    quickiesvHud = new VirindiViewService.HudView("VQuickie", 25, 300, new ACImage(Color.Transparent), false);
                    //quickiesvHud.Width = 25;
                    //quickiesvHud.Height = 300;
                    //quickiesvHud.Title = "VQuickie";
                    quickiesvHud_Head = new HudFixedLayout();

                    btnQuickiesvAdd = new HudButton();
                    btnQuickiesvRemove = new HudButton();
                    if (vpt.X == 0) { vpt.X = 200; }
                    if (vpt.Y == 0) { vpt.Y = 200; }

                    doCreateHud(quickiesvHud, vpt, quickiesvHud_Head, btnQuickiesvAdd, btnQuickiesvRemove);

                    try
                    {
                        vst.Add(mQuickStackv0);
                        vst.Add(mQuickStackv1);
                        vst.Add(mQuickStackv2);
                        vst.Add(mQuickStackv3);
                        vst.Add(mQuickStackv4);
                        vst.Add(mQuickStackv5);
                        vst.Add(mQuickStackv6);
                        vst.Add(mQuickStackv7);
                        vst.Add(mQuickStackv8);
                        vst.Add(mQuickStackv9);
                        vst.Add(mQuickStackv10);
                        vst.Add(mQuickStackv11);

                        vID.Add(nQuickieIDv0);
                        vID.Add(nQuickieIDv1);
                        vID.Add(nQuickieIDv2);
                        vID.Add(nQuickieIDv3);
                        vID.Add(nQuickieIDv4);
                        vID.Add(nQuickieIDv5);
                        vID.Add(nQuickieIDv6);
                        vID.Add(nQuickieIDv7);
                        vID.Add(nQuickieIDv8);
                        vID.Add(nQuickieIDv9);
                        vID.Add(nQuickieIDv10);
                        vID.Add(nQuickieIDv11);

                        chkQuickiev0 = new HudCheckBox(); vchk.Add(chkQuickiev0);
                        chkQuickiev1 = new HudCheckBox(); vchk.Add(chkQuickiev1);
                        chkQuickiev2 = new HudCheckBox(); vchk.Add(chkQuickiev2);
                        chkQuickiev3 = new HudCheckBox(); vchk.Add(chkQuickiev3);
                        chkQuickiev4 = new HudCheckBox(); vchk.Add(chkQuickiev4);
                        chkQuickiev5 = new HudCheckBox(); vchk.Add(chkQuickiev5);
                        chkQuickiev6 = new HudCheckBox(); vchk.Add(chkQuickiev6);
                        chkQuickiev7 = new HudCheckBox(); vchk.Add(chkQuickiev7);
                        chkQuickiev8 = new HudCheckBox(); vchk.Add(chkQuickiev8);
                        chkQuickiev9 = new HudCheckBox(); vchk.Add(chkQuickiev9);
                        chkQuickiev10 = new HudCheckBox(); vchk.Add(chkQuickiev10);
                        chkQuickiev11 = new HudCheckBox(); vchk.Add(chkQuickiev11);

                        quickiesvHud_Head.AddControl(chkQuickiev0, new Rectangle(0, 15, 10, 10));
                        quickiesvHud_Head.AddControl(chkQuickiev1, new Rectangle(0, 30, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev2, new Rectangle(0, 45, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev3, new Rectangle(0, 60, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev4, new Rectangle(0, 75, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev5, new Rectangle(0, 90, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev6, new Rectangle(0, 105, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev7, new Rectangle(0, 120, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev8, new Rectangle(0, 135, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev9, new Rectangle(0, 150, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev10, new Rectangle(0, 165, 12, 12));
                        quickiesvHud_Head.AddControl(chkQuickiev11, new Rectangle(0, 180, 12, 12));

                        quickiesvHud.Moved += (sender, obj) => quickiesvHud_Moved(sender, obj);
                        btnQuickiesvAdd.Hit += (sender, obj) => btnQuickiesvAdd_Hit(sender, obj);
                        btnQuickiesvRemove.Hit += (sender, obj) => btnQuickiesvRemove_Hit(sender, obj);
                        chkQuickiev0.Change += (sender, obj) => chkQuickiev0_Change(sender, obj);
                        chkQuickiev1.Change += (sender, obj) => chkQuickiev1_Change(sender, obj);
                        chkQuickiev2.Change += (sender, obj) => chkQuickiev2_Change(sender, obj);
                        chkQuickiev3.Change += (sender, obj) => chkQuickiev3_Change(sender, obj);
                        chkQuickiev4.Change += (sender, obj) => chkQuickiev4_Change(sender, obj);
                        chkQuickiev5.Change += (sender, obj) => chkQuickiev5_Change(sender, obj);
                        chkQuickiev6.Change += (sender, obj) => chkQuickiev6_Change(sender, obj);
                        chkQuickiev7.Change += (sender, obj) => chkQuickiev7_Change(sender, obj);
                        chkQuickiev8.Change += (sender, obj) => chkQuickiev8_Change(sender, obj);
                        chkQuickiev9.Change += (sender, obj) => chkQuickiev9_Change(sender, obj);
                        chkQuickiev10.Change += (sender, obj) => chkQuickiev10_Change(sender, obj);
                        chkQuickiev11.Change += (sender, obj) => chkQuickiev11_Change(sender, obj);

                        if (xdocQuickSlotsv.Root.HasElements)
                        {
                            doGetData(xdocQuickSlotsv, quickSlotsvFilename);
                        }
                    }
                    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

                }
                else if (hudview == quickieshHud)
                {
                   quickieshHud = new VirindiViewService.HudView("HQuickie", 300, 30, new ACImage(Color.Transparent), false);
                    //quickieshHud.Width = 300;
                    //quickieshHud.Height = 25;
                    //quickieshHud.Title = "HQuickie";
                    quickieshHud_Head = new HudFixedLayout();
                     btnQuickieshAdd = new HudButton();
                    btnQuickieshRemove = new HudButton();
                    if (hpt.X == 0) { hpt.X = 240; }
                    if (hpt.Y == 0) { hpt.Y = 300; }

                    doCreateHud(quickieshHud, hpt, quickieshHud_Head, btnQuickieshAdd, btnQuickieshRemove);

                    hst.Add(mQuickStackh0);
                    hst.Add(mQuickStackh1);
                    hst.Add(mQuickStackh2);
                    hst.Add(mQuickStackh3);
                    hst.Add(mQuickStackh4);
                    hst.Add(mQuickStackh5);
                    hst.Add(mQuickStackh6);
                    hst.Add(mQuickStackh7);
                    hst.Add(mQuickStackh8);
                    hst.Add(mQuickStackh9);
                    hst.Add(mQuickStackh10);
                    hst.Add(mQuickStackh11);

                    hID.Add(nQuickieIDh0);
                    hID.Add(nQuickieIDh1);
                    hID.Add(nQuickieIDh2);
                    hID.Add(nQuickieIDh3);
                    hID.Add(nQuickieIDh4);
                    hID.Add(nQuickieIDh5);
                    hID.Add(nQuickieIDh6);
                    hID.Add(nQuickieIDh7);
                    hID.Add(nQuickieIDh8);
                    hID.Add(nQuickieIDh9);
                    hID.Add(nQuickieIDh10);
                    hID.Add(nQuickieIDh11);

                    chkQuickieh0 = new HudCheckBox(); hchk.Add(chkQuickieh0);
                    chkQuickieh1 = new HudCheckBox(); hchk.Add(chkQuickieh1);
                    chkQuickieh2 = new HudCheckBox(); hchk.Add(chkQuickieh2);
                    chkQuickieh3 = new HudCheckBox(); hchk.Add(chkQuickieh3);
                    chkQuickieh4 = new HudCheckBox(); hchk.Add(chkQuickieh4);
                    chkQuickieh5 = new HudCheckBox(); hchk.Add(chkQuickieh5);
                    chkQuickieh6 = new HudCheckBox(); hchk.Add(chkQuickieh6);
                    chkQuickieh7 = new HudCheckBox(); hchk.Add(chkQuickieh7);
                    chkQuickieh8 = new HudCheckBox(); hchk.Add(chkQuickieh8);
                    chkQuickieh9 = new HudCheckBox(); hchk.Add(chkQuickieh9);
                    chkQuickieh10 = new HudCheckBox(); hchk.Add(chkQuickieh10);
                    chkQuickieh11 = new HudCheckBox(); hchk.Add(chkQuickieh11);

                    quickieshHud_Head.AddControl(chkQuickieh0, new Rectangle(30, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh1, new Rectangle(45, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh2, new Rectangle(60, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh3, new Rectangle(75, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh5, new Rectangle(95, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh6, new Rectangle(110, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh7, new Rectangle(125, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh8, new Rectangle(140, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh9, new Rectangle(155, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh10, new Rectangle(170, 15, 12, 12));
                    quickieshHud_Head.AddControl(chkQuickieh11, new Rectangle(185, 15, 12, 12));

                    quickieshHud.Moved += (sender, obj) => quickieshHud_Moved(sender, obj);
                    btnQuickieshAdd.Hit += (sender, obj) => btnQuickieshAdd_Hit(sender, obj);
                    btnQuickieshRemove.Hit += (sender, obj) => btnQuickieshRemove_Hit(sender, obj);
                    chkQuickieh0.Change += (sender, obj) => chkQuickieh0_Change(sender, obj);
                    chkQuickieh1.Change += (sender, obj) => chkQuickieh1_Change(sender, obj);
                    chkQuickieh2.Change += (sender, obj) => chkQuickieh2_Change(sender, obj);
                    chkQuickieh3.Change += (sender, obj) => chkQuickieh3_Change(sender, obj);
                    chkQuickieh4.Change += (sender, obj) => chkQuickieh4_Change(sender, obj);
                    chkQuickieh5.Change += (sender, obj) => chkQuickieh5_Change(sender, obj);
                    chkQuickieh6.Change += (sender, obj) => chkQuickieh6_Change(sender, obj);
                    chkQuickieh7.Change += (sender, obj) => chkQuickieh7_Change(sender, obj);
                    chkQuickieh8.Change += (sender, obj) => chkQuickieh8_Change(sender, obj);
                    chkQuickieh9.Change += (sender, obj) => chkQuickieh9_Change(sender, obj);
                    chkQuickieh10.Change += (sender, obj) => chkQuickieh10_Change(sender, obj);
                    chkQuickieh11.Change += (sender, obj) => chkQuickieh11_Change(sender, obj);

                    if (xdocQuickSlotsh.Root.HasElements)
                    {
                        doGetData(xdocQuickSlotsh, quickSlotshFilename);
                    }

                }

                Decal.Adapter.CoreManager.Current.ItemSelected += new EventHandler<ItemSelectedEventArgs>(Current_ItemSelected);
                CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);



            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

        private void doCreateHud(VirindiViewService.HudView hud, Point p, HudFixedLayout head, HudButton badd, HudButton bremove)
        {
            try
            {
                hud.ShowInBar = false;
                hud.SpookyTabs = false;
                hud.Visible = true;
                hud.UserGhostable = false;
                //Do not know what this does;
                hud.Ghosted = false;
                hud.UserMinimizable = false;
                // ??--Don't know what useralphachangeable does
                hud.UserAlphaChangeable = false;
                hud.ShowIcon = false;
                //  hud.ClickThrough = true;
                hud.Theme = HudViewDrawStyle.GetThemeByName("Minimalist Transparent");
                hud.Location = p;
                hud.Controls.HeadControl = head;

                badd.Text = "+";
                badd.Visible = true;
                Rectangle recAdd = new Rectangle(0, 0, 12, 12);
                head.AddControl(badd, recAdd);

                bremove.Text = "-";
                bremove.Visible = true;
                Rectangle recRemove = new Rectangle(15, 0, 12, 12);
                head.AddControl(bremove, recRemove);

            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }


        private void doClearHud(VirindiViewService.HudView hud,XDocument xdoc,string filename)
        {
            //btnQuickiesAdd = null;
            //btnQuickiesRemove = null;
            try
            {
                xdoc.Save(filename);
                hud.ClearWindowButtons();

                if (hud == quickiesvHud)
                {
                    nquickiev = 0;
                    try
                    {

                        for (int i = 0; i < 12; i++)
                        { vst[i] = null; }
                    }
                    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

                    try
                    {

                        for (int i = 0; i < 12; i++)
                        { vID[i] = 0; }
                    }
                    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

                    // btnQuickiesvAdd.Hit -= (sender, obj) => btnQuickiesvAdd_Hit(sender, obj);  
                    // btnQuickiesvRemove.Hit -= (sender, obj) => btnQuickiesvRemove_Hit(sender, obj);

                    chkQuickiev0.Change -= (sender, obj) => chkQuickiev0_Change(sender, obj);
                    chkQuickiev1.Change -= (sender, obj) => chkQuickiev1_Change(sender, obj);
                    chkQuickiev2.Change -= (sender, obj) => chkQuickiev2_Change(sender, obj);
                    chkQuickiev3.Change -= (sender, obj) => chkQuickiev3_Change(sender, obj);
                    chkQuickiev4.Change -= (sender, obj) => chkQuickiev4_Change(sender, obj);
                    chkQuickiev5.Change -= (sender, obj) => chkQuickiev5_Change(sender, obj);
                    chkQuickiev6.Change -= (sender, obj) => chkQuickiev6_Change(sender, obj);
                    chkQuickiev7.Change -= (sender, obj) => chkQuickiev7_Change(sender, obj);
                    chkQuickiev8.Change -= (sender, obj) => chkQuickiev8_Change(sender, obj);
                    chkQuickiev9.Change -= (sender, obj) => chkQuickiev9_Change(sender, obj);
                    chkQuickiev10.Change -= (sender, obj) => chkQuickiev10_Change(sender, obj);
                    chkQuickiev11.Change -= (sender, obj) => chkQuickiev11_Change(sender, obj);
                    btnQuickiesvAdd = null;
                    btnQuickiesvRemove = null;

                    for (int i = 0; i < 12; i++)
                    { vchk[i] = null; }

                }

                else if (hud == quickieshHud)
                {
                    nquickieh = 0;
                    try
                    {
                        for (int i = 0; i < 12; i++)
                        { hst[i] = null; }
                    }
                    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

                    try
                    {
                        for (int i = 0; i < 12; i++)
                        { hID[i] = 0; }
                    }
                    catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

                    // btnhQuickiesAdd.Hit -= (sender, obj) => btnhQuickiesAdd_Hit(sender, obj);  
                    // btnQuickieshRemove.Hit -= (sender, obj) => btnQuickieshRemove_Hit(sender, obj);
                    chkQuickieh0.Change -= (sender, obj) => chkQuickieh0_Change(sender, obj);
                    chkQuickieh1.Change -= (sender, obj) => chkQuickieh1_Change(sender, obj);
                    chkQuickieh2.Change -= (sender, obj) => chkQuickieh2_Change(sender, obj);
                    chkQuickieh3.Change -= (sender, obj) => chkQuickieh3_Change(sender, obj);
                    chkQuickieh4.Change -= (sender, obj) => chkQuickieh4_Change(sender, obj);
                    chkQuickieh5.Change -= (sender, obj) => chkQuickieh5_Change(sender, obj);
                    chkQuickieh6.Change -= (sender, obj) => chkQuickieh6_Change(sender, obj);
                    chkQuickieh7.Change -= (sender, obj) => chkQuickieh7_Change(sender, obj);
                    chkQuickieh8.Change -= (sender, obj) => chkQuickieh8_Change(sender, obj);
                    chkQuickieh9.Change -= (sender, obj) => chkQuickieh9_Change(sender, obj);
                    chkQuickieh10.Change -= (sender, obj) => chkQuickieh10_Change(sender, obj);
                    chkQuickieh11.Change -= (sender, obj) => chkQuickieh11_Change(sender, obj);
                    btnQuickieshAdd = null;
                    btnQuickieshRemove = null;

                    for (int i = 0; i < 12; i++)
                    { hchk[i] = null; }
                    nquickieh = 0;
                }
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }


        private void doGetData(XDocument xdoc, string filename)
        {

            try
            {
                if (xdoc == xdocQuickSlotsv) { nquickiev = 0; }
                else if (xdoc == xdocQuickSlotsh) { nquickieh = 0; }

                IEnumerable<XElement> elements = xdoc.Element("Objs").Descendants("Obj");

                foreach (XElement elem in elements)
                {
                    thisQuickie = new QuickSlotData();
                    thisQuickie.Guid = Convert.ToInt32(elem.Element("QID").Value);
                    thisQuickie.Icon = Convert.ToInt32(elem.Element("QIcon").Value);
                    thisQuickie.IconOverlay = Convert.ToInt32(elem.Element("QIconOverlay").Value);
                    thisQuickie.IconUnderlay = Convert.ToInt32(elem.Element("QIconUnderlay").Value);
                    fillHud(xdoc, filename, thisQuickie);

                }
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }

        private void btnQuickiesvAdd_Hit(object sender, System.EventArgs e)
        {
            btnAdd(quickiesvHud);
        }

        private void btnQuickieshAdd_Hit(object sender, System.EventArgs e)
        {
            btnAdd(quickieshHud);
        }


        private void btnAdd(VirindiViewService.HudView hud)
        {
            try
            {
                quickiesHud = hud;
                baddItem = true;
                bremoveItem = false;
            }

            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }

        private void btnQuickiesvRemove_Hit(object sender, System.EventArgs e)
        {
            btnRemove(quickiesvHud);
        }

        private void btnQuickieshRemove_Hit(object sender, System.EventArgs e)
        {
            btnRemove(quickieshHud);
        }

        private void btnRemove(VirindiViewService.HudView hud)
        {
            try
            {
                bremoveItem = true;
                baddItem = false;
            }

            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }

        }


        private void Current_ItemSelected(object sender, ItemSelectedEventArgs e)
        {

            if (baddItem)
            {
                try
                {
                    // The following adds the icon of an item selected to the hudview

                    int objSelectedID = e.ItemGuid;

                    foreach (Decal.Adapter.Wrappers.WorldObject obj in Core.WorldFilter.GetInventory())
                    {
                        if (obj.Id == objSelectedID)
                        {
                            quickie = obj;

                            break;

                        }

                    }

                    QuickSlotData thisQuickie = new QuickSlotData();
                    thisQuickie.Guid = objSelectedID;
                    thisQuickie.Icon = quickie.Icon;
                    thisQuickie.IconOverlay = quickie.Values(LongValueKey.IconOverlay);
                    thisQuickie.IconUnderlay = quickie.Values(LongValueKey.IconUnderlay);
                    if (quickiesHud == quickiesvHud)
                    {
                        fillHud(xdocQuickSlotsv, quickSlotsvFilename, thisQuickie);
                        writeToQuickSlots(xdocQuickSlotsv, quickSlotsvFilename, thisQuickie);
                    }
                    else if (quickiesHud == quickieshHud)
                    {
                        fillHud(xdocQuickSlotsh, quickSlotshFilename, thisQuickie);
                        writeToQuickSlots(xdocQuickSlotsh, quickSlotshFilename, thisQuickie);
                    }

                }
                catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
                baddItem = false;
            }

        }


        private void doQuickieChkWork(Int32 qid, XDocument xdoc, string filename, Int32 n, VirindiViewService.HudView hud)
        {
            if (bremoveItem)
            {
                try
                {
                    IEnumerable<XElement> elements = xdoc.Element("Objs").Descendants("Obj");

                    xdoc.Descendants("Obj").Where(x => x.Element("QID").Value == qid.ToString()).Remove();
                    xdoc.Save(filename);
                }
                catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
                bremoveItem = false;

                if (filename == quickSlotsvFilename)
                {
                    doClearHud(quickiesvHud, xdocQuickSlotsv, quickSlotsvFilename);
                    hud.Dispose();
                    hud = null;
                    xdocQuickSlotsv = XDocument.Load(quickSlotsvFilename);
                    quickiesvHud = new HudView(); 
                    hud = quickiesvHud;
                }
                else if (filename == quickSlotshFilename)              
                {
                    doClearHud(quickieshHud, xdocQuickSlotsh, quickSlotshFilename);
                    hud.Dispose();
                    hud = null;
                    xdocQuickSlotsh = XDocument.Load(quickSlotshFilename);
                    quickieshHud = new HudView(); 
                    hud = quickieshHud;
                }

                createQuickies(hud);
                if (xdoc == xdocQuickSlotsv) { doGetData(xdocQuickSlotsv, quickSlotsvFilename); }
                else if (xdoc == xdocQuickSlotsh) { doGetData(xdocQuickSlotsh, quickSlotshFilename); }
            }
            else if (!bremoveItem)
            {

                CoreManager.Current.Actions.UseItem(qid, 0);

            }

        }

        private void chkQuickiev0_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev0.Checked && nQuickieIDv0 != 0)
            {
                doQuickieChkWork(nQuickieIDv0, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev0.Checked = false;
            }
        }

        private void chkQuickiev1_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev1.Checked && nQuickieIDv1 != 0)
            {
                doQuickieChkWork(nQuickieIDv1, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev1.Checked = false;
            }

        }

        private void chkQuickiev2_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev2.Checked && nQuickieIDv2 != 0)
            {
                doQuickieChkWork(nQuickieIDv2, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev2.Checked = false;
            }

        }

        private void chkQuickiev3_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev3.Checked && nQuickieIDv3 != 0)
            {
                doQuickieChkWork(nQuickieIDv3, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev3.Checked = false;
            }

        }

        private void chkQuickiev4_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev4.Checked && nQuickieIDv4 != 0)
            {
                doQuickieChkWork(nQuickieIDv4, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev4.Checked = false;
            }

        }
        private void chkQuickiev5_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev5.Checked && nQuickieIDv5 != 0)
            {
                doQuickieChkWork(nQuickieIDv5, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev5.Checked = false;
            }

        }
        private void chkQuickiev6_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev6.Checked && nQuickieIDv6 != 0)
            {
                doQuickieChkWork(nQuickieIDv6, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev6.Checked = false;
            }

        }
        private void chkQuickiev7_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev7.Checked && nQuickieIDv7 != 0)
            {
                doQuickieChkWork(nQuickieIDv7, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev7.Checked = false;
            }
        }
        private void chkQuickiev8_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev8.Checked && nQuickieIDv8 != 0)
            {
                doQuickieChkWork(nQuickieIDv8, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev8.Checked = false;
            }

        }
        private void chkQuickiev9_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev9.Checked && nQuickieIDv9 != 0)
            {
                doQuickieChkWork(nQuickieIDv9, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev9.Checked = false;
            }

        }
        private void chkQuickiev10_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev10.Checked && nQuickieIDv10 != 0)
            {
                doQuickieChkWork(nQuickieIDv10, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev10.Checked = false;
            }

        }
        private void chkQuickiev11_Change(object sender, System.EventArgs e)
        {
            if (chkQuickiev11.Checked && nQuickieIDv11 != 0)
            {
                doQuickieChkWork(nQuickieIDv11, xdocQuickSlotsv, quickSlotsvFilename, nquickiev, quickiesvHud);
                chkQuickiev11.Checked = false;
            }

        }

        private void chkQuickieh0_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh0.Checked && nQuickieIDh0 != 0)
            {
                doQuickieChkWork(nQuickieIDh0, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh0.Checked = false;
            }
        }

        private void chkQuickieh1_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh1.Checked && nQuickieIDh1 != 0)
            {
                doQuickieChkWork(nQuickieIDh1, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh1.Checked = false;
            }

        }

        private void chkQuickieh2_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh2.Checked && nQuickieIDh2 != 0)
            {
                doQuickieChkWork(nQuickieIDh2, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh2.Checked = false;
            }

        }

        private void chkQuickieh3_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh3.Checked && nQuickieIDh3 != 0)
            {
                doQuickieChkWork(nQuickieIDh3, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh3.Checked = false;
            }

        }

        private void chkQuickieh4_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh4.Checked && nQuickieIDh4 != 0)
            {
                doQuickieChkWork(nQuickieIDh4, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh4.Checked = false;
            }

        }
        private void chkQuickieh5_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh5.Checked && nQuickieIDh5 != 0)
            {
                doQuickieChkWork(nQuickieIDh5, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh5.Checked = false;
            }

        }
        private void chkQuickieh6_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh6.Checked && nQuickieIDh6 != 0)
            {
                doQuickieChkWork(nQuickieIDh6, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh6.Checked = false;
            }

        }
        private void chkQuickieh7_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh7.Checked && nQuickieIDh7 != 0)
            {
                doQuickieChkWork(nQuickieIDh7, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickiev7.Checked = false;
            }
        }
        private void chkQuickieh8_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh8.Checked && nQuickieIDh8 != 0)
            {
                doQuickieChkWork(nQuickieIDh8, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh8.Checked = false;
            }

        }
        private void chkQuickieh9_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh9.Checked && nQuickieIDh9 != 0)
            {
                doQuickieChkWork(nQuickieIDh9, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh9.Checked = false;
            }

        }
        private void chkQuickieh10_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh10.Checked && nQuickieIDh10 != 0)
            {
                doQuickieChkWork(nQuickieIDh10, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh10.Checked = false;
            }

        }
        private void chkQuickieh11_Change(object sender, System.EventArgs e)
        {
            if (chkQuickieh11.Checked && nQuickieIDh11 != 0)
            {
                doQuickieChkWork(nQuickieIDh11, xdocQuickSlotsh, quickSlotshFilename, nquickieh, quickieshHud);
                chkQuickieh11.Checked = false;
            }

        }

        private void fillHud(XDocument xdoc, string filename, QuickSlotData thisQuickie)
        {
            ACImage mQuickSlots;
            Rectangle rec = new Rectangle(0, 0, 15, 15);
            HudImageStack mQuickStacks = new HudImageStack();
            try
            {
                if (thisQuickie.IconUnderlay != 0)
                {
                    mQuickSlots = new ACImage(thisQuickie.IconUnderlay);

                    mQuickStacks.Add(rec, mQuickSlots);
                }

                mQuickSlots = new ACImage(thisQuickie.Icon);
                mQuickStacks.Add(rec, mQuickSlots);

                if (thisQuickie.IconOverlay != 0)
                {
                    mQuickSlots = new ACImage(0x6000000 + thisQuickie.IconOverlay);
                    mQuickStacks.Add(rec, mQuickSlots);
                }
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


            if (xdoc == xdocQuickSlotsv)
            {
                switch (nquickiev)
                {
                    case 0:
                        mQuickStackv0 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv0, new Rectangle(13, 15, 20, 20));
                        nQuickieIDv0 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 1:
                        mQuickStackv1 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv1, new Rectangle(13, 30, 20, 20));
                        nQuickieIDv1 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 2:
                        mQuickStackv2 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv2, new Rectangle(15, 45, 20, 20));
                        nQuickieIDv2 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 3:
                        mQuickStackv3 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv3, new Rectangle(15, 60, 20, 20));
                        nQuickieIDv3 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 4:
                        mQuickStackv4 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv4, new Rectangle(15, 75, 20, 20));
                        nQuickieIDv4 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 5:
                        mQuickStackv5 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv5, new Rectangle(15, 90, 12, 12));
                        nQuickieIDv5 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 6:
                        mQuickStackv6 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv6, new Rectangle(15, 105, 20, 20));
                        nQuickieIDv6 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 7:
                        mQuickStackv7 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv7, new Rectangle(15, 120, 20, 20));
                        nQuickieIDv7 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 8:
                        mQuickStackv8 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv8, new Rectangle(15, 135, 20, 20));
                        nQuickieIDv8 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 9:
                        mQuickStackv9 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv9, new Rectangle(15, 150, 20, 20));
                        nQuickieIDv9 = thisQuickie.Guid;
                        nquickiev++;
                        break;

                    case 10:
                        mQuickStackv10 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv10, new Rectangle(15, 165, 20, 20));
                        nQuickieIDv10 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    case 11:
                        mQuickStackv11 = mQuickStacks;
                        quickiesvHud_Head.AddControl(mQuickStackv11, new Rectangle(15, 180, 20, 20));
                        nQuickieIDv11 = thisQuickie.Guid;
                        nquickiev++;
                        break;
                    default:
                        Mishna.PluginCore.Util.WriteToChat("There are no more slots available.");
                        break;

                }
            }
            else if (xdoc == xdocQuickSlotsh)
            {
                try
                {
                    switch (nquickieh)
                    {
                        case 0:
                            mQuickStackh0 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh0, new Rectangle(30, 0, 20, 20));
                            nQuickieIDh0 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 1:
                            mQuickStackh1 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh1, new Rectangle(45, 0, 20, 20));
                            nQuickieIDh1 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 2:
                            mQuickStackh2 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh2, new Rectangle(60, 0, 20, 20));
                            nQuickieIDh2 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 3:
                            mQuickStackh3 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh3, new Rectangle(75, 0, 20, 20));
                            nQuickieIDh3 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 4:
                            mQuickStackh4 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh4, new Rectangle(90, 0, 20, 20));
                            nQuickieIDh4 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 5:
                            mQuickStackh5 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh5, new Rectangle(105, 0, 12, 12));
                            nQuickieIDh5 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 6:
                            mQuickStackh6 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh6, new Rectangle(120, 0, 20, 20));
                            nQuickieIDh6 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 7:
                            mQuickStackh7 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh7, new Rectangle(135, 0, 20, 20));
                            nQuickieIDh7 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 8:
                            mQuickStackh8 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh8, new Rectangle(150, 0, 20, 20));
                            nQuickieIDh8 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 9:
                            mQuickStackh9 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh9, new Rectangle(165, 0, 20, 20));
                            nQuickieIDh9 = thisQuickie.Guid;
                            nquickieh++;
                            break;

                        case 10:
                            mQuickStackv10 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh10, new Rectangle(180, 0, 20, 20));
                            nQuickieIDv10 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        case 11:
                            mQuickStackv11 = mQuickStacks;
                            quickieshHud_Head.AddControl(mQuickStackh11, new Rectangle(195, 0, 20, 20));
                            nQuickieIDh11 = thisQuickie.Guid;
                            nquickieh++;
                            break;
                        default:
                            Mishna.PluginCore.Util.WriteToChat("There are no more slots available.");
                            break;

                    }
                }
                catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
            }
        }


        private void writeToQuickSlots(XDocument xdoc, string filename, QuickSlotData thisQuickie)
        {
            xdoc.Element("Objs").Add(new XElement("Obj",
                new XElement("QID", thisQuickie.Guid),
                new XElement("QIcon", thisQuickie.Icon),
                new XElement("QIconOverlay", thisQuickie.IconOverlay),
                new XElement("QIconUnderlay", thisQuickie.IconUnderlay)));
        }


        private void Current_RenderFrame(object sender, System.EventArgs e)
        {
            try
            {
            }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }
        }

       private void  quickiesvHud_Moved(object sender, System.EventArgs e)
        {
            try
            {

                vpt = quickiesvHud.Location; SaveSettings();
           }
            catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


        }

       private void quickieshHud_Moved(object sender, System.EventArgs e)
       {
           try
           {
              hpt = quickieshHud.Location; SaveSettings();
           }
           catch (Exception ex) { Mishna.PluginCore.Util.LogError(ex); }


       }


    }
}//end of namespace



