using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerConnection : SocketIOComponent {
    // Start is called before the first frame update

    [Header("Network Client")]
    [SerializeField]
    private Transform networkContainer;
    [SerializeField]
    public GameObject playerPrefab;
    public GameObject bombaPrefab;
    [SerializeField]
    public GameObject[] spawns=new GameObject[4];

    public static string ClientID { get; private set; }

    private Dictionary<string, NetworkIdentity> serverObjects;
    private int count = 0;

    public override void Start() {
        base.Start();
        initialize();
        setupEvents();
        Debug.Log(url);
        Console.WriteLine(url);
    }

    private void initialize() {
        serverObjects = new Dictionary<string, NetworkIdentity>();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    private void setupEvents() {
        On("open", (E) => {
            Debug.Log("Connection made to the server");
        });

        On("register", (E) => {
            ClientID = RemoveQuotes(E.data["id"].ToString());

            Debug.LogFormat("O nosso client ID é ({0})", ClientID);
        });

        On("spawn", (E) => {
            if (serverObjects.Count < 4) {
                string id = RemoveQuotes(E.data["id"].ToString());

                GameObject go = Instantiate(playerPrefab,spawns[serverObjects.Count].transform.position,Quaternion.identity, networkContainer);
                Debug.Log(spawns[serverObjects.Count].transform.position);
                Debug.Log(count);
                count++;
                go.name = string.Format("PLayer({0})", id);
                NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                ni.setControllerID(id);
                ni.SetSocketReference(this);
                serverObjects.Add(id, ni);
            } else {
                //dizer ao player que nao pode entrar
            }
        });

        On("disconnected", (E) => {
            string id = RemoveQuotes(E.data["id"].ToString());

            GameObject go = serverObjects[id].gameObject;
            Destroy(go);
            serverObjects.Remove(id);
        });

        On("updatePosition", (E) => {
            string id = RemoveQuotes(E.data["id"].ToString());
            float x = float.Parse(E.data["position"]["x"].str);
            float y = float.Parse(E.data["position"]["y"].str);

            NetworkIdentity ni = serverObjects[id];
            ni.transform.position = new Vector2(x, y);
        });

        On("updateBomba", (E) => {

            Instantiate(bombaPrefab, new Vector2(float.Parse(E.data["x"].str), float.Parse(E.data["y"].str)),Quaternion.identity);
        });
    }
    
    private string RemoveQuotes(string Value) {
        return Value.Replace("\"", "");
    }
}
[Serializable]
public class Player {
    public string id;
    public Position position;
}

[Serializable]
public class Position {
    public float x;
    public float y;
}