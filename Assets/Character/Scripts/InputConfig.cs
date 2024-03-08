using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

[CreateAssetMenu()]
public class InputConfig : ScriptableObject
{
    [Header("Buffer config, all in ms")]
    public long dodgeBufferDuration = 300;
    public long attackBufferDuration = 100;
    public long specialBufferDuration = 100;
}

}