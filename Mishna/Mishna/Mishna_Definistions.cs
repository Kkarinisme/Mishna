using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Decal.Filters;

using Decal.Interop.Net;
using System;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using WindowsTimer = System.Windows.Forms.Timer;
using System.Drawing;

namespace Mishna
{
    public partial class PluginCore : PluginBase
    {
        private const int CanvasWidth = 400;
        private const int CanvasHeight = 175;
  
        private string currDir;
        private string inventoryFilename;
        private string armorFilename;
        private string genArmorFilename;
        private string holdingArmorFilename;
        private string genInventoryFilename;
        private string holdingInventoryFilename;
        private string inventorySelect;
        private string genSettingsFilename;
        private string toonQuickSlotsFilename;

        private static bool inventoryEnabled;
        private static bool inventoryBurdenEnabled;
        private static bool inventoryCompleteEnabled;
        private static bool inventoryWaitingEnabled = false;
        private static bool statsEnabled;
        private static bool quickSlotsEnabled;
        private static bool identRecd = false;
        private static bool mgoon = true;
        private static bool getBurden = false;

        private string toonName;
        private string world;
        private string pathToToon;

        private XmlDocument inventory = new XmlDocument();
        private XmlDocument armor = new XmlDocument();
        private XmlDocument doc = new XmlDocument();
        private XDocument xdoc = new XDocument();
        private XDocument newDoc = new XDocument();
        private XDocument iconDoc = new XDocument();

        private static XElement element = null;
        private static IEnumerable<XElement> childElements = null;
        private static IEnumerable<XElement> elements = null;


        private XmlDocument genInventory = new XmlDocument();
        private XDocument Xinventory = new XDocument();

        //used by both the inventory and armor programs to hold current object being processed
        private WorldObject currentobj;
 
        private string fn;
        private List<string> moldObjsID = new List<string>();
        private List<WorldObject> mWaitingForID;
        private List<WorldObject> mIdNotNeeded = new List<WorldObject>();
        private List<long> mwaitingforChangedEvent = new List<long>();
        private List<string> mCurrID = new List<string>();
        private List<string> mIcons = new List<string>();

        private static WindowsTimer mWaitingForIDTimer = new WindowsTimer();
        private int m = 500;
        private int n = 0;
        private int k = 0;
        private int mcount = 0;


        private static string objSpellXml = null;
         private static string message = null;
        private static string mySelect = null;
        private static string objSalvWork = "None";
        private static string objMatName = null;
        private static long objEmbueTypeInt = 0;
        private static string objEmbueTypeStr = null;
        private static long objWieldAttrInt = 0;
        private static long objDamageTypeInt = 0;
        private static long objLevelInt = 1;
        private static long objCovers = 0;
        private static string objCoversName = null;
        private static string objSpells = null;
        private static Int32 objIcon;
        private static long objArmorLevel = 1;
        private static long objArmorSet = 0;
        private static string objArmorSetName = null;
        private static long objMat = 0;
        private static long objMagicDamageInt = 0;
        private static string objDamageType = null;
        private static double objDVar = 0;
        private static long objMaxDamLong = 0;
        private static string objMinDam = null;

        // private static MyClasses.MetaViewWrappers.IList lstQuickies = null;
        private static VirindiViewService.HudView  quickiesHud; 
        private static VirindiViewService.Controls.HudFixedLayout quickiesHud_Head;


       private static string objClassName = "None";
        private static int objClass = 0;
        private string objName = null;
        private static int objID = 0;

        string objProts;
        string objAl;
        string objWork;
        string objTinks;
        string objLevel;
        string objMissD;
        string objManaC;
        string objMagicD;
        string objMelD;
        string objElemvsMons;
         string objMaxDam;
        string objAttack;
        string objVar;
        string objBurden;
        string objStack;



    }
}