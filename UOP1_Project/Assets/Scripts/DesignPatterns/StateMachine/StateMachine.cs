using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
	/// <summary>
	/// Store the current state
	/// </summary>
	/// <value></value>
	public State currentState { get; private set; }
	/// <summary>
	/// Called on state change
	/// </summary>
	public Action stateChanged;

	private readonly List<Transition> transitionFromAnyState = new List<Transition>();

	public void Tick()
	{
		Transition transition = GetTransitionIfAvailable(currentState);
		if (transition != null)
		{
			SetState(transition.To);
		}

		currentState?.Tick();
	}

	public void SetState(State state)
	{
		if (state == currentState)
		{
			return;
		}

		currentState?.OnExit();
		currentState = state;
		if (stateChanged != null)
		{
			stateChanged();
		}

		currentState.OnEnter();
	}

	public void AddAnyTransition(State state, Func<bool> predicate)
	{
		transitionFromAnyState.Add(new Transition(state, predicate));
	}

	private Transition GetTransitionIfAvailable(State currentState)
	{
		Transition transitionIfAny = GetNextTransitionFromList(transitionFromAnyState, currentState);

		if (transitionIfAny == null || transitionIfAny.To == currentState)
		{
			transitionIfAny = GetNextTransitionFromList(this.currentState.Transitions, null);
		}

		return transitionIfAny;
	}

	private static Transition GetNextTransitionFromList(List<Transition> transitions, State blockState)
	{
		foreach (Transition transition in transitions)
		{
			if (transition.To != blockState && transition.Condition())
			{
				return transition;
			}
		}

		return null;
	}
}
