using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Interaction
{

[RequireComponent(typeof(Interactable))]
public class Sigil : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSound;

    private void Awake()
    {
        GetComponent<Interactable>().onInteractCallbacks += PickUpSigil;
    }

    public void PickUpSigil(GameObject player)
    {
        Character.SigilManager sm = player.GetComponent<Character.SigilManager>();
        if (sm != null && !sm.hasSigil)
        {
            sm.hasSigil = true;
            Debug.Log("Picked up sigil");
            Destroy(this.gameObject);

            SoundManager.PlaySound(transform.position, pickUpSound, spatialBlend: 0.5f, volume: 0.5f);
        }
    }
}

}