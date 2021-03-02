namespace DustEngine
{
    public abstract class DuInstantAction : DuAction
    {
        protected override void ActionInnerUpdate(float deltaTime)
        {
            OnActionUpdate(deltaTime);

            ActionInnerStop(false);
        }
    }
}
