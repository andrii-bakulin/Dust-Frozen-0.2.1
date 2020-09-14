using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public static class Icons
    {
        internal const string ADD_FIELD = "DRAFT/UI/Add-Field";

        internal const string GAME_OBJECT_STATS = "UI/GameObject-Stats";
        internal const string TRANSFORM_RESET = "UI/Transform-Reset";


        private class ClassParams
        {
            public string IconName { get; set; }
            public Texture IconTexture { get; set; }
        }

        private static readonly Dictionary<string, ClassParams> duClassParams = new Dictionary<string, ClassParams>()
        {
            // Animations
            ["DustEngine.DuFollow"] = new ClassParams { IconName = "DRAFT/Animation/DuFollow" },
            ["DustEngine.DuRotate"] = new ClassParams { IconName = "DRAFT/Animation/DuRotate" },
            ["DustEngine.DuTarget"] = new ClassParams { IconName = "DRAFT/Animation/DuTarget" },
            ["DustEngine.DuTranslate"] = new ClassParams { IconName = "DRAFT/Animation/DuTranslate" },
            ["DustEngine.DuVibrate"] = new ClassParams { IconName = "DRAFT/Animation/DuVibrate" },

            // Events
            ["DustEngine.DuCollisionEvent"] = new ClassParams { IconName = "DRAFT/Events/DuColliderEvent" },
            ["DustEngine.DuCollision2DEvent"] = new ClassParams { IconName = "DRAFT/Events/DuColliderEvent2D" },
            ["DustEngine.DuTimerEvent"] = new ClassParams { IconName = "DRAFT/Events/DuTimerEvent" },
            ["DustEngine.DuTriggerEvent"] = new ClassParams { IconName = "DRAFT/Events/DuColliderEvent" },
            ["DustEngine.DuTrigger2DEvent"] = new ClassParams { IconName = "DRAFT/Events/DuColliderEvent2D" },

            // Fields
            ["DustEngine.DuFieldsSpace"] = new ClassParams { IconName = "DRAFT/Fields/FieldsSpace" },

            // Fields:Objects
            ["DustEngine.DuConeField"] = new ClassParams { IconName = "DRAFT/Fields/Objects/DuConeField" },
            ["DustEngine.DuCubeField"] = new ClassParams { IconName = "DRAFT/Fields/Objects/DuCubeField" },
            ["DustEngine.DuCylinderField"] = new ClassParams { IconName = "DRAFT/Fields/Objects/DuCylinderField" },
            ["DustEngine.DuDirectionalField"] = new ClassParams { IconName = "DRAFT/Fields/Objects/DuDirectionalField" },
            ["DustEngine.DuRadialField"] = new ClassParams { IconName = "DRAFT/Fields/Objects/DuRadialField" },
            ["DustEngine.DuSphereField"] = new ClassParams { IconName = "DRAFT/Fields/Objects/DuSphereField" },
            ["DustEngine.DuTorusField"] = new ClassParams { IconName = "DRAFT/Fields/Objects/DuTorusField" },

            // Gizmos
            ["DustEngine.DuArrowGizmo"] = new ClassParams { IconName = "DRAFT/Gizmos/DuArrowGizmo" },
            ["DustEngine.DuConeGizmo"] = new ClassParams { IconName = "DRAFT/Gizmos/DuConeGizmo" },
            ["DustEngine.DuCubeGizmo"] = new ClassParams { IconName = "DRAFT/Gizmos/DuCubeGizmo" },
            ["DustEngine.DuCylinderGizmo"] = new ClassParams { IconName = "DRAFT/Gizmos/DuCylinderGizmo" },
            ["DustEngine.DuMeshGizmo"] = new ClassParams { IconName = "DRAFT/Gizmos/DuMeshGizmo" },
            ["DustEngine.DuPyramidGizmo"] = new ClassParams { IconName = "DRAFT/Gizmos/DuPyramidGizmo" },
            ["DustEngine.DuSphereGizmo"] = new ClassParams { IconName = "DRAFT/Gizmos/DuSphereGizmo" },
            ["DustEngine.DuTorusGizmo"] = new ClassParams { IconName = "DRAFT/Gizmos/DuTorusGizmo" },
            ["DustEngine.DuTriggerGizmo"] = new ClassParams { IconName = "DRAFT/Gizmos/DuTriggerGizmo" },

            // Instances
            ["DustEngine.DuDestroyer"] = new ClassParams { IconName = "DRAFT/Instances/DuDestroyer" },
            ["DustEngine.DuSpawner"] = new ClassParams { IconName = "DRAFT/Instances/DuSpawner" },

            // Modifiers
            ["DustEngine.DuRandomTransform"] = new ClassParams { IconName = "DRAFT/Modifiers/DuRandomTransform" },

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
