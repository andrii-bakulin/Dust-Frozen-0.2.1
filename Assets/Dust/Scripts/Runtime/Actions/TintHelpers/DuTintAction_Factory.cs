using System;
using UnityEngine;

namespace DustEngine
{
    public partial class DuTintAction
    {
        private static TintMode[] autoDetectTintsSequence = new[]
        {
            TintMode.MeshRenderer,

            TintMode.UIImage,
            TintMode.UIText,
        };

        public static Type GetUpdaterTypeByTintMode(TintMode tintMode)
        {
            switch (tintMode)
            {
                case TintMode.Auto:
                    return null;

                case TintMode.MeshRenderer:
                    return typeof(MeshRendererUpdater);

                case TintMode.UIImage:
                    return typeof(UIImageUpdater);
                case TintMode.UIText:
                    return typeof(UITextUpdater);
            }
            
            return null;
        } 
        
        protected static TintUpdater FactoryUpdater(DuTintAction tintAction, TintMode tintMode)
        {
            switch (tintMode)
            {
                case TintMode.Auto:
                    foreach (var tryTintMode in autoDetectTintsSequence)
                    {
                        TintUpdater updater = FactoryUpdater(tintAction, tryTintMode);

                        if (Dust.IsNotNull(updater))
                            return updater;
                    }
                    return null;
                    
                case TintMode.MeshRenderer:
                    return MeshRendererUpdater.Create(tintAction);

                case TintMode.UIImage:
                    return UIImageUpdater.Create(tintAction);
                case TintMode.UIText:
                    return UITextUpdater.Create(tintAction);
            }

            return null;
        }
    }
}
