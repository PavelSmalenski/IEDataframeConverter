using IE.Parsers.Processors.StateHandling.States;

namespace IE.Parsers.Processors.StateHandling;

class StateHandler
{
    Dictionary<Transition, IState> Transitions = null!;

    public IState CurrentState { get; private set; } = null!;

    public StateHandler()
    {
        RegisterTransitions();
    }

    public bool Transit(Command command)
    {
        var transition = new Transition(CurrentState, command);
        IState nextState;
        if (Transitions.TryGetValue(transition, out nextState!))
        {
            CurrentState = nextState;
            return true;
        }
        else
        {
            System.Console.WriteLine($"Failed to move from '{CurrentState.ToString()}' with command '{command.ToString()}'");
            return false;
        }
    }
    void RegisterTransitions()
    {
        IState stateStart = new StateStart();
        IState stateCallHeader = new StateCallHeader();
        IState stateDataframeHeaderStart = new StateDataframeHeaderStart();
        IState stateDataframeHeaderName = new StateDataframeHeaderName();
        IState stateDataframeHeaderParams = new StateDataframeHeaderParams();
        IState stateDataframeHeaderOrganized = new StateDataframeHeaderOrganized();
        IState stateRecordHeaderHeading = new StateRecordHeaderHeading();
        IState stateRecordHeaderName = new StateRecordHeaderName();
        IState stateRecordHeaderIdentifier = new StateRecordHeaderIdentified();
        IState stateRecordHeaderKey = new StateRecordHeaderKey();
        IState stateRecord = new StateRecord();
        IState stateEnd = new StateEnd();

        Transitions = new Dictionary<Transition, IState>()
        {
            { new Transition(stateStart, Command.NewPage), stateCallHeader },

            { new Transition(stateCallHeader, Command.Advance2), stateCallHeader },
            { new Transition(stateCallHeader, Command.Advance1), stateCallHeader },
            { new Transition(stateCallHeader, Command.Advance3), stateCallHeader },
            { new Transition(stateCallHeader, Command.NewPage),  stateDataframeHeaderStart },

            { new Transition(stateDataframeHeaderStart, Command.Advance2), stateDataframeHeaderName },

            { new Transition(stateDataframeHeaderName, Command.Advance2), stateDataframeHeaderParams },

            { new Transition(stateDataframeHeaderParams, Command.Advance2),           stateDataframeHeaderParams },
            { new Transition(stateDataframeHeaderParams, Command.DataframeOrganized), stateDataframeHeaderOrganized },
            { new Transition(stateDataframeHeaderParams, Command.Advance3),           stateRecordHeaderHeading },

            { new Transition(stateDataframeHeaderOrganized, Command.Advance1), stateDataframeHeaderOrganized },
            { new Transition(stateDataframeHeaderOrganized, Command.Advance3), stateRecordHeaderHeading },

            { new Transition(stateRecordHeaderHeading, Command.Advance1), stateRecordHeaderHeading },
            { new Transition(stateRecordHeaderHeading, Command.Advance3), stateRecordHeaderName },
            { new Transition(stateRecordHeaderHeading, Command.Advance2), stateRecord },

            { new Transition(stateRecordHeaderName, Command.RecordIdentified), stateRecordHeaderIdentifier },
            { new Transition(stateRecordHeaderName, Command.RecordKey),        stateRecordHeaderKey },
            { new Transition(stateRecordHeaderName, Command.Advance2),         stateRecord },

            { new Transition(stateRecordHeaderIdentifier, Command.Advance1),  stateRecordHeaderIdentifier },
            { new Transition(stateRecordHeaderIdentifier, Command.RecordKey), stateRecordHeaderKey },
            { new Transition(stateRecordHeaderIdentifier, Command.Advance2),  stateRecord },

            { new Transition(stateRecordHeaderKey, Command.Advance1), stateRecordHeaderKey },
            { new Transition(stateRecordHeaderKey, Command.Advance2), stateRecord },

            { new Transition(stateRecord, Command.Advance1),      stateRecord },
            { new Transition(stateRecord, Command.RecordHeading), stateRecordHeaderHeading },
            { new Transition(stateRecord, Command.Advance3),      stateRecordHeaderName },
            { new Transition(stateRecord, Command.NewPage),       stateCallHeader },
            { new Transition(stateRecord, Command.Exit),          stateEnd },
        };

        CurrentState = stateStart;
    }
}