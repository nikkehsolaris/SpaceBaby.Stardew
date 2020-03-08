using System;
using SpaceBaby.AdjustableFarmWaterColor.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using xTile.Dimensions;
using StardewValley;
using StardewValley.Locations;
using System.Linq;

namespace SpaceBaby.AdjustableFarmWaterColor
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        private BuildableGameLocation Farm;

        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.SaveLoaded += GetFarm;
            helper.Events.Display.Rendering += ChangeWater;
        }

        private void ChangeWater(object sender, RenderingEventArgs e)
        {
            if(Farm.waterColor.Value != this.Config.waterColor)
                this.Farm.waterColor.Value = this.Config.waterColor;
        }

        private void GetFarm(object sender, SaveLoadedEventArgs e)
        {
            this.Farm = (StardewValley.Locations.BuildableGameLocation)Game1.locations.ToList().Find(x => x is Farm);
        }
    }
}
