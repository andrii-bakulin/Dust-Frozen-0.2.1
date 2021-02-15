namespace DustEngine
{
    public abstract class DuInstantAction : DuAction
    {
        internal override void ActionInnerUpdate(float deltaTime)
        {
            m_PercentsCompletedLast = 0f;
            m_PercentsCompletedNow = 1f;

            OnActionUpdate(deltaTime);

            ActionInnerStop(false);
        }
    }
}
