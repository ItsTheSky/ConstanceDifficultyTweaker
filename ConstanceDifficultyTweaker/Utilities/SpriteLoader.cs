using System.IO;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace ConstanceDifficultyTweaker.Utilities
{
    public static class SpriteLoader
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix + ".SpriteLoader");
        
        public static Sprite LoadFromFile(string filename)
        {
            var path = Path.Combine(Paths.PluginPath, "ConstanceDifficultyTweaker", "icons", filename);

            if (!File.Exists(path))
            {
                Logger.LogError($"Sprite file not found at path: {path}");
                return null;
            }
        
            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            
        
            return Sprite.Create(texture, 
                new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f), 
                100f);
        }
    }

    public static class SpriteRegistry
    {

        public const string SETTINGS_CAROUSEL_ICON = "difficulty.png";

    }
}