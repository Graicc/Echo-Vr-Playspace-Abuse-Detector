using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EchoManager : MonoBehaviour
{
    public GameObject playerPrefab;
    List<GameObject> players;

    public Text abusers;
    public Text timer;

    string oldStatus;

    public Settings settings;

    public void Init() {
        if (players != null) {
            foreach (GameObject g in players) {
                Destroy(g);
            }
        }
        players = new List<GameObject>();
        for (int i = 0; i <=1; i++) {
            Player[] team = EchoApi.instance.data.teams[i].players;
            if (team == null) {
                continue;
            }
            for (int j = 0; j < team.Length; j++) {
                GameObject player = Instantiate(playerPrefab);
                player.transform.position = Utils.FloatToVector(team[j].position);
                PlayerController newController = player.GetComponent<PlayerController>();
                newController.teamIndex = i; // TODO: This should probally be a fucntion in the class
                newController.playerIndex = j;
                Debug.Log("New player: " + team[j].name);
                player.name = team[j].name;
                players.Add(player);
            }
        }
        Reload();
    }

    public void Reload() {
        foreach (GameObject g in players) {
            g.GetComponent<PlayerController>().Reload();
        }
    }

    private void Start() {
        //string ex = JsonUtility.ToJson(settings);
        //Debug.Log(ex);
        //File.WriteAllText(Application.dataPath + "/settings.json", ex);
        string jsonImport = File.ReadAllText(Application.dataPath + "/settings.json");
        settings = JsonUtility.FromJson<Settings>(jsonImport);

        // There's noplace better to put this
        Screen.SetResolution(1280, 720, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Init();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Reload();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (EchoApi.instance.data.game_status != oldStatus)
            Reload();
        oldStatus = EchoApi.instance.data.game_status;

        abusers.text = "";
        if (players != null) {
            foreach (GameObject player in players) {
                if (player.GetComponent<PlayerController>().isAbuse)
                    abusers.text = abusers.text + player.name + "\n";
            }
        }

        timer.text = EchoApi.instance.data.game_clock_display;
    }
}