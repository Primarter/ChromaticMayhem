using UnityEngine;

namespace Knossos.FSM
{
    public abstract class State
    {
        public GameObject obj;
        protected int id;

        // Called when State is registered
        public abstract void Init();

        // Called when State become the active
        public abstract void Enter(int previousState);

        // call each Update
        public abstract void Update();

        // call each FixedUpdate
        public abstract void FixedUpdate();

        // Called when State become the active
        public abstract void Exit(int nextState);
    }
}
