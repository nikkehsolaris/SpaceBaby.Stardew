using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley;
using System.Linq;

namespace SpaceBaby.RadialMenu
{
    public class ModEntry : Mod
    {
        public ModEntry()
        {
        }

        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.GameLoop.SaveLoaded += addRadialMenu;
            this.Helper.Events.Display.RenderingHud += TestHideToolbar;
        }

        private void addRadialMenu(object sender, SaveLoadedEventArgs e)
        {
            Game1.onScreenMenus.Add(new Framework.RadialMenu(this.Monitor) );
        }

        private void TestHideToolbar(object sender, RenderingHudEventArgs e)
        {
            if (Game1.onScreenMenus.Any(x => (x is Toolbar)))
            {
                IClickableMenu Toolbar = Game1.onScreenMenus.First(x => (x is Toolbar));
                Game1.onScreenMenus.Remove(Toolbar);

            }
        }
    }
}
