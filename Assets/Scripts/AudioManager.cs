using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Music");

        if (objects.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
