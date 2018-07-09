using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManagerScript : MonoBehaviour {

    public static EffectsManagerScript Instance;

    public List<GameObject> explodeList;
    public List<AudioClip> soundList;

    private void Awake()
    {
        Instance = this;
    }

    public float PlayExplode(Vector3 point, int explodeNum, AudioSource source)
    {
        GameObject boom = Instantiate(explodeList[explodeNum]);
        source.PlayOneShot(soundList[explodeNum]);
        boom.transform.position = point;
        Destroy(boom, 0.3f);
        return soundList[explodeNum].length;
    }
}
