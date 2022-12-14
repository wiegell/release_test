using System;
using System.Threading;
using System.Threading.Tasks;
using AppKit;

using Foundation;


namespace AstroWall
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
        }
        private NSStatusBar statusBar;
        private NSStatusItem statusBarItem;
        private State state;

        #region Override Methods
        public async override void DidFinishLaunching(NSNotification notification)
        {
            // Create a Status Bar Menu
            statusBar = NSStatusBar.SystemStatusBar;
            statusBarItem = statusBar.CreateStatusItem(NSStatusItemLength.Variable);

            MacOShelpers.InitIcon(statusBarItem, this.StatusMenu);


            // Init state
            state = new State(this.StatusMenu, statusBarItem);
            state.SetStateInitializing();
            state.loadOrCreateDB();
            state.loadOrCreatePrefs();
            await state.LoadFromDBOrOnline();
            state.PopulateMenu();
            state.setStateIdle();
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
            Console.WriteLine("term called");
            state.saveDBToDisk();
            state.savePrefsToDisk();
        }
        #endregion



        partial void MenuManualCheckPic(Foundation.NSObject sender)
        {
            state.setStateIdle();
            //MacOShelpers.InitIcon2(statusBarItem, this.StatusMenu);
            //string imgurl = HTMLHelpers.getImgUrl();
            //Task<string> tmpFilePath = FileHelpers.DownloadUrlToTmpPath(imgurl);
            ////MacOShelpers.SetWallpaper(tmpFilePath);
            //Console.WriteLine("file dl");
        }


    }
}

