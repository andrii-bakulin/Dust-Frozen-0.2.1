using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    public abstract class DuFlowAction : DuAction
    {
        protected override void ActionInnerUpdate(float deltaTime)
        {
            OnActionUpdate(deltaTime);

            ActionInnerStop(false);
        }
    }
}
