namespace DustEngine
{
    public abstract class DuInstantAction : DuActionWithCallbacks
    {
        protected override void ActionInnerUpdate(float deltaTime)
        {
            OnActionUpdate(deltaTime);

            ActionInnerStop(false);
        }
    }
}
