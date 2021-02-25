namespace DustEngine
{
    public abstract class DuInstantAction : DuAction
    {
        internal override void ActionInnerUpdate(float deltaTime)
        {
            OnActionUpdate(deltaTime);

            ActionInnerStop(false);
        }
    }
}
