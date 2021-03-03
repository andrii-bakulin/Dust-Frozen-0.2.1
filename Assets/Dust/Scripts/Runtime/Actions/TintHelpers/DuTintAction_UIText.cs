using UnityEngine;
using UnityEngine.UI;

namespace DustEngine
{
    public partial class DuTintAction
    {
        protected class UITextUpdater : TintUpdater
        {
            protected Text m_Target;

            //----------------------------------------------------------------------------------------------------------

            public static TintUpdater Create(DuTintAction parentTintAction)
            {
                var target = parentTintAction.m_TargetTransform.GetComponent<Text>();

                if (Dust.IsNull(target))
                    return null;

                var tintUpdater = new UITextUpdater();
                tintUpdater.m_Target = target;
                tintUpdater.Init(parentTintAction);
                return tintUpdater;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            public override void Init(DuTintAction parentTintAction)
            {
                base.Init(parentTintAction);
                
                m_StartColor = m_Target.color;
            }

            public override void Update(float deltaTime, Color color)
            {
                m_Target.color = color;
            }

            public override void Release(bool isActionTerminated)
            {
                m_Target = null;

                base.Release(isActionTerminated);
            }
        }
    }
}
