using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkIdentity : MonoBehaviour {
    [Header("NetWork Info")]
    [SerializeField]
    private string id;
    [SerializeField]
    private bool isControlling;

    private SocketIOComponent socket;

    // Start is called before the first frame update
    void Awake() {
        isControlling = false;
    }

    // Update is called once per frame
    public void setControllerID(string ID) {
        id = ID;
        isControlling = (ServerConnection.ClientID == ID) ? true : false;

    }

    public void SetSocketReference(SocketIOComponent Socket) {
        socket = Socket;
    }

    public string GetID() {
        return id;
    }

    public bool IsControlling() {
        return isControlling;
    }

    public SocketIOComponent GetSocket() {
        return socket;
    }
}
