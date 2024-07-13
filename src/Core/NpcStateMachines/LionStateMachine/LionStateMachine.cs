using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;

public class LionStateMachine
{
    private static readonly StringName _TRANSITIONS_FILE = new StringName("res://Core/NpcStateMachines/LionStateMachine/Transitions.json");

    public enum LionStates
    {
        Introduction = 1,
        Food = 2,
        Foe = 3,
        EvadedFoodOrFoeQuestion = 4,
        WalkIntoAquarium = 5,
    }

    public enum LionEvents
    {
        GeneralProgreesion = 1,
        AnswerFood = 2,
        AnswerFoe = 3,
        EvadeFoodOrFoeQuestion = 4,
    }

    private SaveStateService _serviceSaveState = null;

    public LionStateMachine(SaveStateService serviceSaveState)
    {
        _serviceSaveState = serviceSaveState;
    }

    public LionStates? GetStateId()
    {
        LionStates? result = null;
        using (var context = new SaveStateService())
        {
            var contextState = context.Load();
            result = contextState.NpcStates.LionState;
        }
        return result;
    }

    public LionStates? GetNextStateId(LionStates stateId, LionEvents eventId)
    {
        LionStates? result = null;
        try
        {
            // Raise Exception if no NextState is found
            var transition = LionTransitions.Where(x => x.StateId == (int)stateId && x.EventId == (int)eventId).First();
            result = (LionStates)transition.NextStateId;
        }
        catch (Exception exception)
        {
            GD.PrintErr($"No Lion Transition was found for state {stateId} and event {eventId}. Exception raised: {exception.Message}.");
            throw;
        }
        return result;
    }

    public void SetStateId(LionStates stateId)
    {
        using (var context = new SaveStateService())
        {
            var contextState = context.Load();
            contextState.NpcStates.LionState = stateId;
            context.Commit(contextState);
        }
    }

    public List<TransitionModel> LionTransitions => GetLionTransitions();

    public List<TransitionModel> GetLionTransitions()
    {
        var result = new List<TransitionModel>();
        try
        {
            using var file = FileAccess.Open(_TRANSITIONS_FILE, FileAccess.ModeFlags.Read);
            string content = file.GetAsText();
            result = JsonConvert.DeserializeObject<List<TransitionModel>>(content);
        }
        catch (Exception exception)
        {
            GD.PrintErr($"Issue parsing Lion's Transition File: {exception.Message}");
        }
        return result;
    }
}