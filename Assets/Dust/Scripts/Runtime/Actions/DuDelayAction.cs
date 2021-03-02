using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Delay Action")]
    public class DuDelayAction : DuIntervalAction
    {
        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            // Nothing need to do :)
        }
    }
}
