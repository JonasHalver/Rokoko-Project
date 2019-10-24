using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SoundFX : MonoBehaviour
{
    public SoundLibrary soundLibrary;
    public enum Material { Wood, Glass, Metal }
    public Material material = Material.Wood;
    List<AudioClip> woodSounds = new List<AudioClip>();
    List<AudioClip> metalSounds = new List<AudioClip>();
    List<AudioClip> glassSounds = new List<AudioClip>();

    AudioSource aS;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Interactable";
        aS = GetComponent<AudioSource>();
        aS.spatialBlend = 1;
        rb = GetComponent<Rigidbody>();

        woodSounds.AddRange(soundLibrary.woodSounds);
        metalSounds.AddRange(soundLibrary.metalSounds);
        glassSounds.AddRange(soundLibrary.glassSounds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip PickSoundEffect()
    {
        AudioClip clip = null;
        switch (material)
        {
            case Material.Wood:
                clip = woodSounds[Random.Range(0, woodSounds.Count)];
                break;
            case Material.Metal:
                clip = metalSounds[Random.Range(0, metalSounds.Count)];
                break;
            case Material.Glass:
                clip = glassSounds[Random.Range(0, glassSounds.Count)];
                break;
        }
        return clip;
    }

    private void OnCollisionEnter(Collision collision)
    {
        aS.pitch += Random.Range(-0.2f, 0.2f);
        aS.PlayOneShot(PickSoundEffect());
        aS.pitch = 1;
    }
}
