using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public static class Icons
    {
        public const string GAME_OBJECT_STATS = "UI/GameObject-Stats";
        public const string TRANSFORM_RESET = "UI/Transform-Reset";

        private class ClassParams
        {
            public string IconName { get; set; }
            public Texture IconTexture { get; set; }
        }

        private static readonly Dictionary<string, ClassParams> duClassParams = new Dictionary<string, ClassParams>()
        {
            // Sample
            // ["DustEngine.ClassName"] = new ClassParams { IconName = "Folder/DuIconName" },
        };

        //--------------------------------------------------------------------------------------------------------------

        public static bool IsClassSupported(string className)
        {
            return duClassParams.ContainsKey(className);
        }

        public static Texture GetTextureByClassName(string className)
        {
            if (!duClassParams.ContainsKey(className))
                return null;

            ClassParams classParams = duClassParams[className];

            if (Dust.IsNull(classParams.IconTexture))
                classParams.IconTexture = Resources.Load(classParams.IconName) as Texture;

            return classParams.IconTexture;
        }

        public static Texture GetTextureByComponent(Component component)
        {
            if (Dust.IsNull(component))
                return null;

            string className = component.GetType().ToString();

            if (!IsClassSupported(className))
                return null;

            return GetTextureByClassName(className);
        }
    }
}
#endif
