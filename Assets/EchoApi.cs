using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EchoApi : MonoBehaviour {
    public static EchoApi instance { get; private set; }

    public EchoObject data;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null) { // Singleton stuff
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);

        StartCoroutine(GetData("http://127.0.0.1:6721/session"));
    }

    IEnumerator GetData(string uri) {
        while (true) {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError) {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else {
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    data = JsonUtility.FromJson<EchoObject>(webRequest.downloadHandler.text);
                }
                //yield return new WaitForSeconds(.5f);
            }
        }
    }
}

[Serializable]
public class EchoObject {
    public string game_status;
    public string game_clock_display;
    public Team[] teams;
}

[Serializable]
public class Team {
    public Player[] players;
}

[Serializable]
public class Player {
    public string name;
    public float[] velocity;
    public float[] position;
    public float[] forward;
    public float[] lhand;
    public float[] rhand;
}
