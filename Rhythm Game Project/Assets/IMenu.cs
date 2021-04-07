public interface IMenu
{
    // Transition in to the menu.
    void TransitionIn();

    // Transition out of the current menu.
    void TransitionOut();

    // Beat timing animations per measure.
    void OnMeasure();

    // Beat timing animations per tick.
    void OnTick();
}