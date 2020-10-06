using System.Collections.Generic;
using UnityEngine;

namespace DustEngine.DustEditor.UI
{
    public static class Icons
    {
        internal const string ACTION_DELETE = "UI/Action-Delete";
        internal const string ACTION_ADD_DEFORMER = "Deformers/Add";
        internal const string ACTION_ADD_FACTORY_MACHINE = "Factory/Add-Machine";
        internal const string ACTION_ADD_FIELD = "Fields/Add";

        internal const string STATE_ENABLED = "UI/State-Enabled";
        internal const string STATE_DISABLED = "UI/State-Disabled";

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
            ["DustEngine.DuFollow"] = new ClassParams { IconName = "Animation/DuFollow" },
            ["DustEngine.DuLookAt"] = new ClassParams { IconName = "Animation/DuLookAt" },
            ["DustEngine.DuPulsate"] = new ClassParams { IconName = "Animation/DuPulsate" },
            ["DustEngine.DuRotate"] = new ClassParams { IconName = "Animation/DuRotate" },
            ["DustEngine.DuShake"] = new ClassParams { IconName = "Animation/DuShake" },
            ["DustEngine.DuTranslate"] = new ClassParams { IconName = "Animation/DuTranslate" },

            // Deformers:Core
            ["DustEngine.DuDeformMesh"] = new ClassParams { IconName = "Deformers/Core/DuDeformMesh" },

            // Deformers
            ["DustEngine.DuTwistDeformer"] = new ClassParams { IconName = "Deformers/DuTwistDeformer" },
            ["DustEngine.DuWaveDeformer"] = new ClassParams { IconName = "Deformers/DuWaveDeformer" },

            // Events
            ["DustEngine.DuCollisionEvent"] = new ClassParams { IconName = "Events/DuColliderEvent" },
            ["DustEngine.DuCollision2DEvent"] = new ClassParams { IconName = "Events/DuColliderEvent2D" },
            ["DustEngine.DuTimerEvent"] = new ClassParams { IconName = "Events/DuTimerEvent" },
            ["DustEngine.DuTriggerEvent"] = new ClassParams { IconName = "Events/DuColliderEvent" },
            ["DustEngine.DuTrigger2DEvent"] = new ClassParams { IconName = "Events/DuColliderEvent2D" },

            // Factory @todo!
            ["DustEngine.DuFactoryInstance"] = new ClassParams { IconName = "Factory/DuFactoryInstance" },

            ["DustEngine.DuGridFactory"] = new ClassParams { IconName = "Factory/DuGridFactory" },
            ["DustEngine.DuHoneycombFactory"] = new ClassParams { IconName = "Factory/DuHoneycombFactory" },
            ["DustEngine.DuLinearFactory"] = new ClassParams { IconName = "Factory/DuLinearFactory" },
            ["DustEngine.DuRadialFactory"] = new ClassParams { IconName = "Factory/DuRadialFactory" },

            // Factory-Machines
            ["DustEngine.DuLookAtFactoryMachine"] = new ClassParams { IconName = "Factory/Machines/DuLookAtFactoryMachine" },

            // Fields
            ["DustEngine.DuFieldsSpace"] = new ClassParams { IconName = "Fields/FieldsSpace" },

            // Fields:Basic
            ["DustEngine.DuConstantField"] = new ClassParams { IconName = "Fields/Basic/DuConstantField" },
            ["DustEngine.DuCoordinatesField"] = new ClassParams { IconName = "Fields/Basic/DuCoordinatesField" },
            ["DustEngine.DuRadialField"] = new ClassParams { IconName = "Fields/Basic/DuRadialField" },
            ["DustEngine.DuTextureField"] = new ClassParams { IconName = "Fields/Basic/DuTextureField" },
            ["DustEngine.DuTimeField"] = new ClassParams { IconName = "Fields/Basic/DuTimeField" },

            // Fields:Math
            ["DustEngine.DuClampField"] = new ClassParams { IconName = "Fields/Math/DuClampField" },
            ["DustEngine.DuCurveField"] = new ClassParams { IconName = "Fields/Math/DuCurveField" },
            ["DustEngine.DuFitField"] = new ClassParams { IconName = "Fields/Math/DuFitField" },
            ["DustEngine.DuInvertField"] = new ClassParams { IconName = "Fields/Math/DuInvertField" },
            ["DustEngine.DuRemapField"] = new ClassParams { IconName = "Fields/Math/DuRemapField" },
            ["DustEngine.DuRoundField"] = new ClassParams { IconName = "Fields/Math/DuRoundField" },

            // Fields:Objects
            ["DustEngine.DuConeField"] = new ClassParams { IconName = "Fields/Objects/DuConeField" },
            ["DustEngine.DuCubeField"] = new ClassParams { IconName = "Fields/Objects/DuCubeField" },
            ["DustEngine.DuCylinderField"] = new ClassParams { IconName = "Fields/Objects/DuCylinderField" },
            ["DustEngine.DuDirectionalField"] = new ClassParams { IconName = "Fields/Objects/DuDirectionalField" },
            ["DustEngine.DuSphereField"] = new ClassParams { IconName = "Fields/Objects/DuSphereField" },
            ["DustEngine.DuTorusField"] = new ClassParams { IconName = "Fields/Objects/DuTorusField" },
            ["DustEngine.DuWaveField"] = new ClassParams { IconName = "Fields/Objects/DuWaveField" },

            // Gizmos
            ["DustEngine.DuArrowGizmo"] = new ClassParams { IconName = "Gizmos/DuArrowGizmo" },
            ["DustEngine.DuConeGizmo"] = new ClassParams { IconName = "Gizmos/DuConeGizmo" },
            ["DustEngine.DuCubeGizmo"] = new ClassParams { IconName = "Gizmos/DuCubeGizmo" },
            ["DustEngine.DuCylinderGizmo"] = new ClassParams { IconName = "Gizmos/DuCylinderGizmo" },
            ["DustEngine.DuFieldsSpaceGizmo"] = new ClassParams { IconName = "Gizmos/DuFieldsSpaceGizmo" },
            ["DustEngine.DuMeshGizmo"] = new ClassParams { IconName = "Gizmos/DuMeshGizmo" },
            ["DustEngine.DuPyramidGizmo"] = new ClassParams { IconName = "Gizmos/DuPyramidGizmo" },
            ["DustEngine.DuSphereGizmo"] = new ClassParams { IconName = "Gizmos/DuSphereGizmo" },
            ["DustEngine.DuTorusGizmo"] = new ClassParams { IconName = "Gizmos/DuTorusGizmo" },
            ["DustEngine.DuTriggerGizmo"] = new ClassParams { IconName = "Gizmos/DuTriggerGizmo" },

            // Instances
            ["DustEngine.DuDestroyer"] = new ClassParams { IconName = "Instance/DuDestroyer" },
            ["DustEngine.DuParallax"] = new ClassParams { IconName = "Instance/DuParallax" },
            ["DustEngine.DuParallaxController"] = new ClassParams { IconName = "Instance/DuParallaxController" },
            ["DustEngine.DuParallaxInstance"] = new ClassParams { IconName = "Instance/DuParallaxInstance" },
            ["DustEngine.DuSpawner"] = new ClassParams { IconName = "Instance/DuSpawner" },

            // Modifiers
            ["DustEngine.DuRandomTransform"] = new ClassParams { IconName = "Modifiers/DuRandomTransform" },

        };

        //--------------------------------------------------------------------------------------------------------------

        public static bool IsClassSupported(string className)
        {
            return duClassParams.ContainsKey(className);
        }

        public static Texture GetTextureByClassName(string className)
            => GetTextureByClassName(className, "");

        public static Texture GetTextureByClassName(string className, string suffix)
        {
            if (!duClassParams.ContainsKey(className))
                return null;

            ClassParams classParams = duClassParams[className];

            // texture with suffix load directly from Resources, without caching
            if (suffix != "")
                return Resources.Load(classParams.IconName + "-" + suffix) as Texture;

            if (Dust.IsNull(classParams.IconTexture))
                classParams.IconTexture = Resources.Load(classParams.IconName) as Texture;

            return classParams.IconTexture;
        }

        public static Texture GetTextureByComponent(Component component)
            => GetTextureByComponent(component, "");

        public static Texture GetTextureByComponent(Component component, string suffix)
        {
            if (Dust.IsNull(component))
                return null;

            string className = component.GetType().ToString();

            if (!IsClassSupported(className))
                return null;

            return GetTextureByClassName(className, suffix);
        }
    }
}
