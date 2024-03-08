using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos.Enemies;

/*
Minotaur AI

Sleeping:
    do nothing
    if [trigger]
        -> Alert
        go into follow player

Alert:
    Pathfind to Sound
    if [isAtSoundSource]
        -> Patrol

Follow player:
    follow the player
    if [inRangeOfPlayer]
        attack player

Patrol:
    while [amountOfTime]
        go to intersection node
        if [atIntersection]
            find new intersection node
    -> GoHome

GoHome:
    go to nearest lair
    if [inRangeOfPlayer]
        attack player
*/

namespace Knossos.Minotaur
{
    [RequireComponent(typeof(PathSystem))]
    [RequireComponent(typeof(LocomotionSystem))]
    [RequireComponent(typeof(VisionSystem))]
    [RequireComponent(typeof(SoundSensorSystem))]
    [RequireComponent(typeof(AttackSystem))]
    [RequireComponent(typeof(AnimationSystem))]
    [RequireComponent(typeof(StaggerSystem))]
    [RequireComponent(typeof(OnHitEventSystem))]
    [RequireComponent(typeof(PlayerMinotaurVisibilitySystem))]
    public class MinotaurAgent : MonoBehaviour
    {
        public FSM.StateMachine stateMachine;
        [SerializeField] public State currentState;

        [HideInInspector] public PathSystem pathSystem;
        [HideInInspector] public LocomotionSystem locomotionSystem;
        [HideInInspector] public VisionSystem visionSystem;
        [HideInInspector] public SoundSensorSystem soundSensorSystem;
        [HideInInspector] public AttackSystem attackSystem;
        [HideInInspector] public AnimationSystem animationSystem;
        [HideInInspector] public StaggerSystem staggerSystem;
        [HideInInspector] public OnHitEventSystem onHitEventSystem;
        [HideInInspector] public PlayerMinotaurVisibilitySystem playerMinotaurVisibilitySystem;

        void Awake()
        {
            pathSystem = GetComponent<PathSystem>();
            locomotionSystem = GetComponent<LocomotionSystem>();
            visionSystem = GetComponent<VisionSystem>();
            soundSensorSystem = GetComponent<SoundSensorSystem>();
            attackSystem = GetComponent<AttackSystem>();
            animationSystem = GetComponent<AnimationSystem>();
            staggerSystem = GetComponent<StaggerSystem>();
            onHitEventSystem = GetComponent<OnHitEventSystem>();
            playerMinotaurVisibilitySystem = GetComponent<PlayerMinotaurVisibilitySystem>();
        }

        void Start()
        {
            stateMachine = new FSM.StateMachine(this.gameObject, typeof(State));
            stateMachine.RegisterState<StateSleep>(State.Sleep);
            stateMachine.RegisterState<StateAlert>(State.Alert);
            stateMachine.RegisterState<StatePatrol>(State.Patrol);
            stateMachine.RegisterState<StateFollow>(State.Follow);
            stateMachine.RegisterState<StateGoHome>(State.GoHome);
            stateMachine.RegisterState<StateAttack>(State.Attack);
            stateMachine.RegisterState<StateStaggered>(State.Staggered);
            stateMachine.ChangeState(State.Sleep);

            gameObject.SetActive(false); // TODO: TEMPORARY CODE, REMOVE!! USED SO THE BELL CAN GET THE MINOTAUR BEFORE BEING DISABLED
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        void Update()
        {
            stateMachine.Update();
            currentState = (State)stateMachine.currentState;
        }
    }
}
