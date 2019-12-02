using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkTransform : MonoBehaviour {

    [SerializeField]
    private Vector2 oldPosition;

    private NetworkIdentity networkIdentity;
    private Player player;

    private float stillCounter = 0;
    // Start is called before the first frame update
    void Start() {
        networkIdentity = GetComponent<NetworkIdentity>();
        oldPosition = transform.position;
        player = new Player();
        player.position = new Position();
        player.position.x = transform.position.x;
        player.position.y = transform.position.y;
        Debug.Log("Transform"+transform.position);

        if (!networkIdentity.IsControlling()) {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (networkIdentity.IsControlling()) {
            if (oldPosition != (Vector2)transform.position) {
                oldPosition = transform.position;
                stillCounter = 0;
                sendData();
            } else {
                stillCounter += Time.deltaTime;
                if (stillCounter >= 1) {
                    stillCounter = 0;
                    sendData();
                }
            }
        }
    }
    void sendData() {
        player.position.x = Mathf.Round(transform.position.x * 1000.0f) / 1000.0f;
        player.position.y = Mathf.Round(transform.position.y * 1000.0f) / 1000.0f;

        string str = "{\"id\":\"\",\"position\":{\"x\":\""+player.position.x+"\",\"y\":\""+player.position.y+"\"}}";

        JSONObject js = new JSONObject(str);

        networkIdentity.GetSocket().Emit("updatePosition", js);
    }
}
