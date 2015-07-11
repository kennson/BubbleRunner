using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class levelCreator : MonoBehaviour {

	public GameObject tilePos;
	private float startUpPosY;
	private const float tileWidth = 1.25f;
	private int heightLevel = 0;
	private GameObject tmpTile;

	private GameObject collectedTiles;
	private GameObject gameLayer;
	private GameObject bgLayer;
	
	public float gameSpeed = 6.0f;
	private float outOfBounceX;
	private int blankCounter = 0;
	private int middleCounter = 0;
	private string lastTile = "right";
	private float startTime;

	private float outOfBounceY;
	private GameObject _player;

	private bool playerDead = false;
	private const int counterReset = 3;
	public static int counterForAds = counterReset;

	void Awake(){
		Application.targetFrameRate = 60;

		if (Advertisement.isSupported) {
			Advertisement.allowPrecache = true;
			Advertisement.Initialize("37030", false);
		}
	}

	void Start () 
	{
		gameLayer = GameObject.Find("gameLayer");
		collectedTiles = GameObject.Find ("tiles");

		for (int i = 0; i < 21; i++) {
			GameObject tmpg1 = Instantiate(Resources.Load("ground_left", typeof(GameObject))) as GameObject;
			tmpg1.transform.parent = collectedTiles.transform.FindChild ("gLeft").transform;
			GameObject tmpg2 = Instantiate(Resources.Load("ground_middle", typeof(GameObject))) as GameObject;
			tmpg2.transform.parent = collectedTiles.transform.FindChild ("gMiddle").transform;
			GameObject tmpg3 = Instantiate(Resources.Load("ground_right", typeof(GameObject))) as GameObject;
			tmpg3.transform.parent = collectedTiles.transform.FindChild ("gRight").transform;
			GameObject tmpg4 = Instantiate(Resources.Load("blank", typeof(GameObject))) as GameObject;
			tmpg4.transform.parent = collectedTiles.transform.FindChild ("gBlank").transform;
		}

		collectedTiles.transform.position = new Vector2 (-60.0f, -20.0f);

		tilePos = GameObject.Find("startTilePosition");
		startUpPosY = tilePos.transform.position.y;
		outOfBounceX = tilePos.transform.position.x - 5.0f;
		outOfBounceY = startUpPosY - 3.0f;
		_player = GameObject.Find ("player");

		fillScene();
		startTime = Time.time;
	}

	void FixedUpdate () 
	{
		if (startTime - Time.time % 5 == 0) {
			gameSpeed += 0.5f;		
		}

		gameLayer.transform.position = new Vector2 (gameLayer.transform.position.x - gameSpeed * Time.deltaTime, 0);
	
		foreach (Transform child in gameLayer.transform) {
			if(child.position.x < outOfBounceX){
				switch(child.gameObject.name){

				case "ground_left(Clone)" :
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gLeft").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gLeft").transform;
					break;
				case "ground_middle(Clone)" :
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gMiddle").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gMiddle").transform;
					break;
				case "ground_right(Clone)" :
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gRight").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gRight").transform;
					break;
				case "blank(Clone)" :
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gBlank").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gBlank").transform;
					break;
				case "Reward":
					GameObject.Find("Reward").GetComponent<heartScript>().inPlay = false;
					break;
				case "Enemy":
					GameObject.Find("Enemy").GetComponent<enemyScript>().inPlay = false;
					break;
				default:
					Destroy(child.gameObject);
					break;
				}
			}		
		}

		if (gameLayer.transform.childCount < 25)
			spawnTile ();

		if (_player.transform.position.y < outOfBounceY)
			killPlayer ();
	}

	private void killPlayer(){
		counterForAds--;

		if (counterForAds <= 0) {
			resetCounter();
			Advertisement.Show(null, new ShowOptions {
				pause = true,
				resultCallback = result => {

				}
			});
		}
		if (playerDead)
			return;
		playerDead = true;
		this.GetComponent<playSound> ().PlaySound ("restart");
		Invoke ("reloadScene", 1);
	}

	public static void resetCounter() {
		counterForAds = counterReset;
	}

	private void reloadScene(){
		Application.LoadLevel ("Start");
	}
	
	private void fillScene()
	{
		for (int i = 0; i < 15; i++) {
			setTile("middle");		
		}
		setTile ("right");
		
	}
	
	public void setTile(string type){

		switch (type) {
			case "left" :
				tmpTile = collectedTiles.transform.FindChild("gLeft").transform.GetChild(0).gameObject;
				break;
			case "right" :
				tmpTile = collectedTiles.transform.FindChild("gRight").transform.GetChild(0).gameObject;
				break;
			case "middle" :
				tmpTile = collectedTiles.transform.FindChild("gMiddle").transform.GetChild(0).gameObject;
				break;
			case "blank" :
				tmpTile = collectedTiles.transform.FindChild("gBlank").transform.GetChild(0).gameObject;
				break;
		}

		tmpTile.transform.parent = gameLayer.transform;
		tmpTile.transform.position = new Vector2 (tilePos.transform.position.x + (tileWidth), startUpPosY + (heightLevel) * tileWidth);

		tilePos = tmpTile;
		lastTile = type;
	}
	
	private void spawnTile(){
		if (blankCounter > 0) {
			setTile("blank");
			blankCounter--;
			return;
		}

		if (middleCounter > 0) {
			setTile("middle");
			middleCounter--;
			return;
		}

		if (lastTile == "blank") {
			changeHeight ();
			setTile ("left");
			middleCounter = (int)Random.Range (1, 0);
		} else if (lastTile == "right") {
			this.GetComponent<scoreHandler>().Points++;
			blankCounter = (int)Random.Range (1, 3);	
		} else if (lastTile == "middle") {
			setTile("right");	
		}

	}

	private void changeHeight(){
		int newHeightLevel = (int)Random.Range (0, 4);

		if (newHeightLevel < heightLevel)
			heightLevel--;
		else if (newHeightLevel > heightLevel)
			heightLevel++;
	}
}
