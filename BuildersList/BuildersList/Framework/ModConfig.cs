using System;
namespace SpaceBaby.BuildersList
{
    public class ModConfig
    {
        public bool isActive { get; set; } = true;
        public string currentRecipe { get; set; } = null;
        public bool isCooking { get; set; } = false;

    }
}
