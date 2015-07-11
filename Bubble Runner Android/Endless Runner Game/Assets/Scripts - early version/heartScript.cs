using UnityEngine;
using System.Collections;

public class heartScript : MonoBehaviour {

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
			switch(crateRender.sprite.name){
			case "HeartBlue":
				GameObject.Find("Main Camera").GetComponent<levelCreator>().gameSpeed -=1.0f;
				break;
			case "HeartRed":
				GameObject.Find("player").rigidbody2D.AddForce(Vector2.up*6000);
				break;
			case "HeartYellow":
				GameObject.Find("Main Camera").GetComponent<scoreHandler>().Points += 10;
				break;
			}	
			inPlay = false;
			this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 30.0f);
			GameObject.Find("Main Camera").GetComponent<playSound>().PlaySound("power");
		}
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
