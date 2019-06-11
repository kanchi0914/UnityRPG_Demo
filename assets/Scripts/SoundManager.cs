using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    //public AudioSource purchased;
    //public AudioSource battle;
    //public AudioSource dangeon;

    public Dictionary<string, AudioSource> sounds = new Dictionary<string, AudioSource>();



    // Start is called before the first frame update
    void Start()
    {







        //var sound = Resources.Load("Sounds/Purchased", typeof(AudioClip)) as AudioClip;
        //purchased = gameObject.AddComponent<AudioSource>();
        //purchased.clip = sound;
        //purchased.volume = 0.5f;

        //sound = Resources.Load("Sounds/Battle", typeof(AudioClip)) as AudioClip;
        //battle = gameObject.AddComponent<AudioSource>();
        //battle.clip = sound;
        //battle.volume = 0.5f;

        //sound = Resources.Load("Sounds/Dangeon", typeof(AudioClip)) as AudioClip;
        //dangeon = gameObject.AddComponent<AudioSource>();
        //dangeon.clip = sound;
        //dangeon.volume = 0.5f;

        //bbb.Play();
        //aa.Play();
    }

    public void Load()
    {
        var soundsList = new List<string>() { "Purchased", "Battle", "Dangeon", "Hit", "Step" };
        soundsList.ForEach(s =>
        {
            var clip = Resources.Load("Sounds/" + s, typeof(AudioClip)) as AudioClip;
            var audio = gameObject.AddComponent<AudioSource>();
            audio.clip = clip;
            audio.volume = 0.5f;
            sounds.Add(s, audio);
        });
    }

    public void Play(string name)
    {
        sounds[name].Play();
    }

    public void StartBattle()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
