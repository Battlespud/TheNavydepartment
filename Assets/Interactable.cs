namespace DefaultNamespace
{
    public interface IInteractable
    {
        void IEnableInteraction(bool b); //pop up a UI indicator to click in order to start IInteract
        void IInteract(); //Actually do something
    }
}