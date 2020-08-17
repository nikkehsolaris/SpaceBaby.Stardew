using System;
using StardewValley.Menus;
using StardewValley;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;
using StardewModdingAPI;

namespace SpaceBaby.RadialMenu.Framework
{
    public class RadialMenu : IClickableMenu
    {
        List<ClickableComponent> buttons = new List<ClickableComponent>();
        IMonitor Monitor;
        readonly double ring_radius = 150.0f;
        int previousButtonIndex = Game1.player.CurrentToolIndex;

        //the offset in percentage
        // .25 = 25% = 1.5~ rads = 90 degree offset
        double offset = -.25d;

        public RadialMenu(IMonitor monitor):
           base()
        {
            Monitor = monitor;
            buttons = GetCurrentItems();
        }

        List<ClickableComponent> GetCurrentItems()
        {
            List<ClickableComponent> buttonList = new List<ClickableComponent>();
            List<Item> itemList =  Game1.player.Items.ToList().GetRange(0, 12);
            for ( int index = 0; index< itemList.Count(); index++)
            {
                if (itemList[index] != null)
                {
                    buttonList.Add( new ClickableComponent(new Rectangle(0, 1, 2, 3), string.Concat((object)index) ) );
                }
            }

            return buttonList;
        }

        public override void draw(SpriteBatch b)
        {
            if (Game1.activeClickableMenu != null )
                return;

            buttons = GetCurrentItems();
            int FarmerX = (int)Game1.player.getLocalPosition(Game1.viewport).X;
            int FarmerY = (int)Game1.player.getLocalPosition(Game1.viewport).Y - (Game1.player.GetBoundingBox().Height * 2 );

            ClickableComponent CurrentToolButton = buttons.Find(x => (Game1.player.CurrentToolIndex == Convert.ToInt32(x.name)));
            int CurrentToolButtonIndex = (CurrentToolButton != null) ?
            Convert.ToInt32(buttons.Find(x => (Game1.player.CurrentToolIndex == Convert.ToInt32(x.name))).name) :
            Array.FindIndex(Game1.player.Items.ToArray(), (i=> !(i is null) ) ) ;

            for (int i = 0; i < buttons.Count; i++)
            {
                double angle = (double)(i - CurrentToolButtonIndex) / (double)buttons.Count;

                //TODO: Make the ring spin instead of instant transmission
                if (previousButtonIndex != CurrentToolButtonIndex)
                {

                }

                //The angle in percentage from 0
                // see offset for details
                float vecX = (float) ((double)FarmerX + Math.Cos((offset + angle) * (2d * Math.PI) ) * ring_radius);
                float vecY = (float) ((double)FarmerY + Math.Sin((offset + angle) * (2d * Math.PI) ) * ring_radius);

                Vector2 position = new Vector2((float)vecX, (float)vecY );

                int currentItemIndex = Convert.ToInt32(this.buttons[i].name);
                if (Game1.player.items[currentItemIndex] != null)
                    Game1.player.items[currentItemIndex].drawInMenu(b, position, Game1.player.CurrentToolIndex == currentItemIndex ? 0.9f : this.buttons.ElementAt(i).scale * 0.8f, 1f, 0.88f);
            }
            base.draw(b);
        }

    }
}
