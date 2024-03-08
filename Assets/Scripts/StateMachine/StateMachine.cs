using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.FSM
{
    public class StateMachine
    {
        public State[] states;
        public GameObject obj;
        public int currentState;

        public StateMachine(GameObject obj, System.Type enumStateType)
        {
            this.obj = obj;
            int numStates = System.Enum.GetNames(enumStateType).Length;
            // Debug.Log(("numStates", numStates));
            states = new State[numStates];
        }

        public void RegisterState<T>(System.Enum id)
        where T : State, new()
        {
            T state = new T();
            state.obj = obj;
            state.Init();

            int index = (int)(object)id;
            states[index] = state;
        }

        public State GetState(int stateId)
        {
            int index = (int)stateId;
            return states[index];
        }

        public void FixedUpdate()
        {
            GetState(currentState)?.FixedUpdate();
        }

        public void Update()
        {
            GetState(currentState)?.Update();
        }

        public void ChangeState(System.Enum newState)
        {
            int nextState = (int)(object)newState;
            GetState(currentState)?.Exit(nextState);
            GetState(nextState)?.Enter(currentState);
            currentState = nextState;
        }

        public void ChangeState(int newState)
        {
            GetState(currentState)?.Exit(newState);
            GetState(newState)?.Enter(currentState);
            currentState = newState;
        }
    }
}
