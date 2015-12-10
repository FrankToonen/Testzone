using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SlideShow : MonoBehaviour
{
    bool playing;
    List<GameObject> images;
    int index;
    RotateBots rotatingBots;
    Image overlay;
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();

        GameObject bots = GameObject.Find("Rotating Bots");
        if (bots != null)
        {
            rotatingBots = bots.GetComponent<RotateBots>();
        }

        GameObject ol = GameObject.Find("HowToPlay Overlay");
        if (ol != null)
        {
            overlay = ol.GetComponent<Image>();
            overlay.enabled = false;
        }

        images = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            images.Add(transform.GetChild(i).gameObject);
        }
        EnableSlide(-1);
        playing = false;
    }

    void Update()
    {
        if (overlay != null)
        {
            overlay.enabled = playing;
        }

        if (Input.anyKeyDown && playing /* && !Input.GetKeyDown(KeyCode.Escape)*/)
        {
            source.Play();

            index++;
            if (index >= images.Count)
            {
                StopSlideShow();
            } else
            {
                EnableSlide(index);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopSlideShow();
        }
    }

    public void StartSlideShow()
    {
        
        source.Play();

        playing = true;
        index = 0;
        EnableSlide(index);

        if (rotatingBots != null)
        {
            rotatingBots.Deactivate();
        }
    }

    public void StopSlideShow()
    {
        playing = false;
        EnableSlide(-1);

        if (rotatingBots != null)
        {
            rotatingBots.Activate();
        }
    }

    void EnableSlide(int i)
    {
        for (int j = 0; j < images.Count; j++)
        {
            images [j].SetActive(j == i);
        }
        playing = i != -1;
    }
}
