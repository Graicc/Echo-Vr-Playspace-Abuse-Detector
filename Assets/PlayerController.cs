using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int teamIndex;
    public int playerIndex;

    public GameObject playerPoint;
    public GameObject playspace;
    public Text text;

    public Material good;
    public Material bad;

    public bool isAbuse;

    EchoManager manager;

    public void Reload() {
        playspace.transform.position = playerPoint.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        text.text = name;
        manager = GameObject.Find("EchoManager").GetComponent<EchoManager>();
        playspace.transform.localScale = Vector3.one * manager.settings.diameter;
    }

    // Update is called once per frame
    void Update()
    {
        Player playerData = EchoApi.instance.data.teams[teamIndex].players[playerIndex];
        playerPoint.transform.position = Utils.FloatToVector(playerData.position);

        playspace.transform.position += Utils.FloatToVector(playerData.velocity) * Time.deltaTime;
        playspace.transform.position = Vector3.MoveTowards(playspace.transform.position, playerPoint.transform.position, manager.settings.speed * Time.deltaTime); // TODO: Settings Static Class with settings that are loaded from json maybee idk

        isAbuse = !playspace.GetComponent<Collider>().bounds.Contains(playerPoint.transform.position);
        if (isAbuse)
            playspace.GetComponent<Renderer>().material = bad;
        else
            playspace.GetComponent<Renderer>().material = good;
    }
}
