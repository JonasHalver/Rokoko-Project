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
    public List<AudioClip> woodSounds = new List<AudioClip>();
    public List<AudioClip> metalSounds = new List<AudioClip>();
    public List<AudioClip> glassSounds = new List<AudioClip>();

    AudioSource aS;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
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
                clip = woodSounds[Random.Range(0, metalSounds.Count)];
                break;
            case Material.Glass:
                clip = woodSounds[Random.Range(0, glassSounds.Count)];
                break;
        }
        return clip;
    }

    private void OnCollisionEnter(Collision collision)
    {
        aS.PlayOneShot(PickSoundEffect());
    }
}
