using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Interaction
{

[RequireComponent(typeof(Interactable))]
public class MainDoor : MonoBehaviour
{
    [System.Serializable]
    public struct Step
    {
        public Door door;
        public Material material;
        public GameObject sigilToActivate;
        public Renderer[] renderers;
    }
    [SerializeField] Step[] steps = new Step[4];

    int sigilsToCollect;
    [SerializeField] AudioClip placeSigilSound;

    private void Awake()
    {
        GetComponent<Interactable>().onInteractCallbacks += TryUseSigil;
    }

    private void Start()
    {
        sigilsToCollect = steps.Length;
    }

    public void TryUseSigil(GameObject player)
    {
        Character.SigilManager sm = player.GetComponent<Character.SigilManager>();
        if (sm != null && sm.hasSigil)
        {
            SoundManager.PlaySound(transform.position, placeSigilSound, spatialBlend: 0.5f, volume: 0.6f);

            Debug.Log("Used sigil");
            steps[^sigilsToCollect].door.OpenDoor();
            steps[^sigilsToCollect].sigilToActivate.SetActive(true);
            foreach (var rend in steps[^sigilsToCollect].renderers)
                rend.material = steps[^sigilsToCollect].material;
            sigilsToCollect -= 1;
            sm.hasSigil = false;
            if (sigilsToCollect == 0)
                Debug.Log("Congrats, you got out!");
        }
    }
}

}