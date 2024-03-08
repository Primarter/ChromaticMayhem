using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Enemies
{

public abstract class EnemyAgent : MonoBehaviour
{
    public abstract void Disable();

    public abstract void Enable();
}

}