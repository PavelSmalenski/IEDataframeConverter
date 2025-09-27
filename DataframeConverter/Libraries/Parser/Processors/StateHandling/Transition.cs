using IE.Parsers.Processors.StateHandling.States;

namespace IE.Parsers.Processors.StateHandling;

class Transition
{
    public IState CurrentState { get; init; }

    public Command Command { get; init; }

    public Transition(IState current, Command command)
    {
        CurrentState = current;
        Command = command;
    }

    public override int GetHashCode()
    {
        return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        Transition? other = obj as Transition;
        return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
    }
}