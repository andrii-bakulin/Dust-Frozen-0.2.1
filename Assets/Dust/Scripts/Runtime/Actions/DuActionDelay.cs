using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Action Delay")]
    public class DuActionDelay : DuIntervalAction
    {
        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionUpdate(float deltaTime)
        {
            // Nothing need to do :)
        }
    }
}
