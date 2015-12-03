using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlideShow : MonoBehaviour
{
    bool playing;
    List<GameObject> images;
    int index;

    void Awake()
    {
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
        if (Input.anyKeyDown && playing /* && !Input.GetKeyDown(KeyCode.Escape)*/)
        {
            index++;
            if (index > images.Count)
            {
                EnableSlide(-1);
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
        playing = true;
        index = 0;
        EnableSlide(index);
    }

    public void StopSlideShow()
    {
        playing = false;
        EnableSlide(-1);
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
