using UnityEngine;
using System.Collections;

public class enemyScript : MonoBehaviour {

	private float maxY;
	private float minY;
	
	private int direction = 1;
	
	public bool inPlay = true;
	private bool releaseCrate = false;
	
	private SpriteRenderer crateRender;

	void Start () {
		maxY = this.transform.position.y + 0.5f;
		minY = maxY - 1.0f;
		
		crateRender = this.transform.GetComponent<SpriteRenderer> ();
	}
	
	void Update () {
		this.transform.position = new Vector2 (this.transform.position.x, this.transform.position.y + (direction * 0.05f));
		if (this.transform.position.y > maxY)
			direction = -1;
		
		if (this.transform.position.y < minY)
			direction = 1;
		
		if (!inPlay && !releaseCrate)
			Respawn ();
	}
	
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			GameObject tmpPlayer = GameObject.Find("player");
			tmpPlayer.rigidbody2D.AddForce(Vector2.right * 1000);
			tmpPlayer.rigidbody2D.AddForce(Vector2.up * 3000);
			tmpPlayer.collider2D.enabled = false;
			GameObject.Find("Main Camera").GetComponent<playSound>().PlaySound("restart");
			Invoke ("reloadScene", 1.25f);
		}
	}

	private void reloadScene(){
		Application.LoadLevel ("Start");
	}

	void Respawn(){
		releaseCrate = true;
		Invoke("placeCrate", (float)Random.Range(3,10));
	}
	
	void placeCrate(){
		inPlay = true;
		releaseCrate = false;
		
		GameObject tmpTile = GameObject.Find ("Main Camera").GetComponent<levelCreator>().tilePos;
		this.transform.position = new Vector2 (tmpTile.transform.position.x, tmpTile.transform.position.y + 5.5f);
		maxY = this.transform.position.y + 0.5f;
		minY = maxY - 1.0f;
	}
}
