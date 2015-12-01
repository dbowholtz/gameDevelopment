using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{

	public GameObject block;
	public GameObject spikes;
	public GameObject player;
	public GameObject flag;
	public GameObject winFlag;
	private GameObject curFlag;
	public GameObject curPlayer;
	private float spawnVelocity;
	private int attempts;
	public Text text;
	public Text winText;
	private playerController pc;
	public int end;
	private int room;
	private GameObject finalBlock;
	public AudioClip deathSound;
	public AudioClip flagSound;
	public AudioClip jumpSound;
	private AudioSource audio;
	// Use this for initialization
	void Start ()
	{
		Camera camera = GetComponent<Camera> ();
		Vector3 p = camera.ScreenToWorldPoint (new Vector3 (16, 16, camera.nearClipPlane));
		p.z = 0;
		GameObject go = Instantiate (block);
		go.transform.position = p;
		curFlag = Instantiate (flag);
		curFlag.transform.position = curPlayer.transform.position;
		spawnVelocity = curPlayer.GetComponent<Rigidbody2D> ().velocity.y;
		attempts = 0;
		audio = GetComponent<AudioSource> ();
		text.text = "Attempts: " + attempts;
		GameObject gb;
		Vector3 p1;
		pc = curPlayer.GetComponent<playerController> ();
		int i;
		for (i=0; i<35; i++) {
			gb = Instantiate (block);
			p1 = p;
			p1.x = p1.x + 0.5f * i;
			gb.transform.position = p1;
			if (i == 30) {
				gb = Instantiate (spikes);
				p1 = p;
				p1.x = p1.x + 0.5f * i;
				p1.y = p1.y + 0.65f;
				gb.transform.position = p1;
			}
		}
		while (i<end) {
			gb = Instantiate (block);
			p1 = p;
			p1.x = p1.x + 0.5f * i;
			gb.transform.position = p1;
			i++;
			int start = i;
			for (i=i; i<start+15; i++) {
				gb = Instantiate (block);
				p1 = p;
				p1.x = p1.x + 0.5f * i;
				gb.transform.position = p1;
			}
			gb = Instantiate (block);
			p1 = p;
			p1.x = p1.x + 0.5f * i;
			gb.transform.position = p1;

			//Randomly generate one of the following platform setups
			if (i < (end - 50)) {
				room = Random.Range (1, 5);
			} else {
				room = -1;
			}

			start = i;

			switch (room) {
			case 1:
				for (i=i; i<start+3; i++) {
					gb = Instantiate (block);
					p1 = p;
					p1.x = p1.x + 0.5f * i;
					gb.transform.position = p1;
					gb = Instantiate (spikes);
					p1 = p;
					p1.x = p1.x + 0.5f * i;
					p1.y = p1.y + 0.65f;
					gb.transform.position = p1;
				}
				start = i;
				for (i=i; i<start+3; i++) {
					gb = Instantiate (block);
					p1 = p;
					p1.x = p1.x + 0.5f * i;
					gb.transform.position = p1;
				}
				start = i;
				for (i=i; i<start+3; i++) {
					gb = Instantiate (block);
					p1 = p;
					p1.x = p1.x + 0.5f * i;
					gb.transform.position = p1;
					gb = Instantiate (spikes);
					p1 = p;
					p1.x = p1.x + 0.5f * i;
					p1.y = p1.y + 0.65f;
					gb.transform.position = p1;
				}
				break;
			case 2:
				for (i = i; i<start+20; i++) {
					gb = Instantiate (block);
					p1 = p;
					p1.x = p1.x + 0.5f * i;
					gb.transform.position = p1;
					gb = Instantiate (spikes);
					p1 = p;
					p1.x = p1.x + 0.5f * i;
					p1.y = p1.y + 0.65f;
					gb.transform.position = p1;
					
					if ((i - start) % 8 == 0) {
						gb = Instantiate (block);
						p1 = p;
						p1.x = p1.x + 0.5f * i;
						p1.y = p1.y + 1.35f;
						gb.transform.position = p1;
					}
				}
				break;
			case 3:
				for (i=i; i<start+5; i++) {
					gb = Instantiate (block);
					p1 = p;
					p1.x = p1.x + 0.5f * i;
					gb.transform.position = p1;
					gb = Instantiate (spikes);
					p1 = p;
					p1.x = p1.x + 0.5f * i;
					p1.y = p1.y + 0.65f;
					gb.transform.position = p1;
				}
				break;
			case 4:
				break;
			default:
				break;
			}


		}
		finalBlock = Instantiate (winFlag);
		p1 = p;
		p1.x = p1.x + 0.5f * i;
		p1.y = p1.y + 0.65f;
		finalBlock.transform.position = p1;


	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Restart")) {
			Debug.Log ("Restart");
			pc.isDead = true;
		} else if (Input.GetButtonDown ("Flag")) {
			Debug.Log ("Flag");
			Destroy (curFlag);
			curFlag = Instantiate (flag);
			curFlag.transform.position = curPlayer.transform.position;
			spawnVelocity = curPlayer.GetComponent<Rigidbody2D> ().velocity.y;
			audio.PlayOneShot(flagSound);
		} else if (Input.GetButtonDown ("Cancel")) {
			Application.LoadLevel ("title");
		} else if(Input.GetButtonDown("Jump")){
			audio.PlayOneShot(jumpSound);
		}
		if (curPlayer.GetComponent<playerController> ().isDead) {
			Debug.Log ("DEAD");
			audio.PlayOneShot(deathSound);
			Destroy (curPlayer);
			curPlayer = Instantiate (player);
			curPlayer.transform.position = curFlag.transform.position;
			attempts++;
			text.text = "Attempts: " + attempts;
			pc = curPlayer.GetComponent<playerController> ();
		}
		if (curPlayer.transform.position.x > finalBlock.transform.position.x) {
			pc.speed = 0;
			curPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
			winText.text = "YOU WIN";
			StartCoroutine("winTheGame");
		}
	}
	IEnumerator winTheGame(){
		yield return new WaitForSeconds(2);
		Application.LoadLevel("title");
	}
	void LateUpdate ()
	{
		Camera camera = Camera.main;
		camera.transform.position = new Vector3 (curPlayer.transform.position.x + 7, 0, camera.transform.position.z);
	}

}
