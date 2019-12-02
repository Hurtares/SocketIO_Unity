using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private NetworkIdentity networkIdentity;
    public float speed = .3f;
    public GameObject bombPrefab;

    private Rigidbody2D rb;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        tileMap = GameObject.Find("Walls").GetComponent<Tilemap>();
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (networkIdentity.IsControlling()) {
            rb.MovePosition(new Vector2(transform.position.x + Input.GetAxis("Horizontal") * speed, transform.position.y + Input.GetAxis("Vertical") * speed));

            if (Input.GetKey(KeyCode.Space)&&timer<=0) {
                
                Vector3 worldPosition = transform.position;
                Vector3Int cell = tileMap.WorldToCell(worldPosition);
                Vector3 cellCenterPosition = tileMap.GetCellCenterWorld(cell);
                string str = "{\"x\":\""+cellCenterPosition.x+"\",\"y\":\""+cellCenterPosition.y+"\"}";
                GetComponent<NetworkIdentity>().GetSocket().Emit("updateBomba", new JSONObject(str));

                Instantiate(bombPrefab, cellCenterPosition, Quaternion.identity);
                timer = 2;
            } else {
                timer -= Time.deltaTime;
            }

        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Boom") {
            Debug.Log("Boom");
            //string str = "{\"id\":\"" + ServerConnection.ClientID + "\"}";
            //GetComponent<NetworkIdentity>().GetSocket().Emit("getTFOut", new JSONObject(str));
            GetComponent<NetworkIdentity>().GetSocket().Emit("disconnect");
            Application.Quit();
        }
    }

}
