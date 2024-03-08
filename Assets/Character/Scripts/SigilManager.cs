using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

public class SigilManager : MonoBehaviour
{
    bool _hasSigil = false;
    public bool hasSigil
    {
        get => _hasSigil;
        set => _hasSigil = value;
    }
}

}