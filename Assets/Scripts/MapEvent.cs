using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CreativeSpore.RpgMapEditor
{
    public class MapEvent : MonoBehaviour
    {
        public int tileID; // Id number of the tile the position will change to. Found in the top right when editing a map
        public AudioClip audio;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ActivateEvent();
            }
        }

        // Changes the tiles of the given positions to the given tile ID
        public void ActivateEvent()
        {
            foreach (Transform child in transform)
            {
                RpgMapHelper.SetAutoTileByPosition(child.position, tileID, 0);
            }

            if (audio)
            {
                AudioSource.PlayClipAtPoint(audio, transform.position);
            }
        }
    }
}
