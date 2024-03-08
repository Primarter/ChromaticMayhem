using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

namespace Knossos.Character
{

public enum BufferedInput
{
    Dodge,
    Attack,
    Special
}

[RequireComponent(typeof(AnimationController))]
public class InputManager : MonoBehaviour
{
    public struct Inputs
    {
        public float horizontal;
        public float vertical;
        public bool dodge;
        public bool attack;
        public bool special;
        public bool interact;
    }

    public static Inputs inputs;

    public delegate void OnUnbufferedInput(BufferedInput input);
    public static OnUnbufferedInput onUnbufferedInput;

    public delegate void OnClearedInput(BufferedInput input);
    public static OnClearedInput onClearedInput;

    [SerializeField]
    private InputConfig config;

    private static Stopwatch sw = new();
    private static Queue<(BufferedInput, long)> inputBuffer = new();
    private static Dictionary<BufferedInput, int> inputCounts = new();

    private void Start()
    {
        sw.Start();
        foreach(var i in Enum.GetValues(typeof(BufferedInput)))
        {
            inputCounts[(BufferedInput)(int)i] = 0;
        }
    }

    private void Update()
    {
        (BufferedInput, long) buffered;
        while (inputBuffer.TryPeek(out buffered) && buffered.Item2 < sw.ElapsedMilliseconds)
        {
            var (input, _) = inputBuffer.Dequeue();
            if (onUnbufferedInput != null)
            {
                onUnbufferedInput(input);
            }
            inputCounts[input] -= 1;
            if (inputCounts[input] == 0)
            {
                onClearedInput(input);
            }
        }

        inputs = new Inputs
        {
            horizontal = Input.GetAxisRaw("Horizontal"),
            vertical = Input.GetAxisRaw("Vertical"),
            dodge = Input.GetButtonDown("Dash"),
            attack = Input.GetButtonDown("Attack"),
            special = Input.GetButtonDown("Special"),
            interact = Input.GetButtonDown("Interact"),
        };

        if (inputs.dodge)
        {
            inputBuffer.Enqueue((BufferedInput.Dodge, sw.ElapsedMilliseconds + config.dodgeBufferDuration));
            inputCounts[BufferedInput.Dodge] += 1;
        }
        else if (inputs.special)
        {
            inputBuffer.Enqueue((BufferedInput.Special, sw.ElapsedMilliseconds + config.specialBufferDuration));
            inputCounts[BufferedInput.Special] += 1;
        }
        else if (inputs.attack)
        {
            inputBuffer.Enqueue((BufferedInput.Attack, sw.ElapsedMilliseconds + config.attackBufferDuration));
            inputCounts[BufferedInput.Attack] += 1;
        }
    }

    public static bool CheckBuffer(BufferedInput input, bool consume = true)
    {
        (BufferedInput, long) buffered;
        if (inputBuffer.TryPeek(out buffered) && buffered.Item1 == input)
        {
            if (consume) {
                var (deq, _) = inputBuffer.Dequeue();
                inputCounts[deq] -= 1;
                if (inputCounts[deq] == 0)
                {
                    onClearedInput(input);
                }
            }
            return true;
        }

        return false;
    }

    public static void EmptyBuffer()
    {
        inputBuffer.Clear();
        foreach(var i in Enum.GetValues(typeof(BufferedInput)))
        {
            inputCounts[(BufferedInput)(int)i] = 0;
        }
    }
}

}