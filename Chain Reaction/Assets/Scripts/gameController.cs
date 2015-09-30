using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gameController : MonoBehaviour {
    public GameObject orb;
    private bool loadBlown;
    public orbController orbC;
    Camera camera;
    private int explosionCount;
    private int orbsBlownUp;
    private static int level =0;
    private AudioSource audio;
    public Text text;
    private GameObject orbDumpster;
    private GameObject temp;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        Time.timeScale=1;
        orbDumpster = GameObject.Find("orbDumpster");
        level++;
        StartCoroutine(displayText("Welecome to level " + level + "\nPop " + (Mathf.FloorToInt(level*5f*.5f)-1)+ " balls"));
        Debug.Log(level);
        explosionCount = 0;
        orbsBlownUp = 0;
        camera = GetComponent<Camera>();
        camera.backgroundColor = Color.black;
        loadBlown = false;
        for (int i = 0; i < level*5; i++)
        {
            orb.transform.position = new Vector2(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 4.0f));
            
            temp = Instantiate(orb);
            temp.transform.parent = orbDumpster.transform;
        }
       
    }
    IEnumerator displayText(string textToDisplay)
    {
        text.text = textToDisplay;
        yield return new WaitForSeconds(1);
        text.text = "";
    }
	// Update is called once per frame
	void Update () {
        if((explosionCount==0 && loadBlown)||((int)Mathf.FloorToInt(level*5*0.5f))<=orbsBlownUp)
        {
            if (((int)Mathf.FloorToInt(level * 5 * 0.4f)) > orbsBlownUp)
            {
                text.text = "Try Again";
                level--;
            }
            //Debug.Log("To Pop: "+((int)Mathf.Round(level* 5 * 0.5f)) + " " + "Popped: " + orbsBlownUp);
            audio.Play();
            
            
            Time.timeScale = 0;
            System.Threading.Thread.Sleep(1000);
            Application.LoadLevel("theScene");
        }
        if (Input.GetMouseButtonDown(0) && !loadBlown) 
        {
            loadBlown = true;
            Camera camera = GetComponent<Camera>();
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            Vector3 test = camera.ScreenToWorldPoint(new Vector3(x, y, 0));
            test.z = 0;
            test.x += .2f;
            test.y -= .2f;
            orbC.gameObject.transform.position = test;
            //orbC = orb.GetComponent<orbController>();
            orbC.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            orbC.makeBomb();
        }
	}
    public void explosion()
    {
        //Debug.Log("EXPLOSION");
        orbsBlownUp++;
        explosionCount++;
    }
    public void doneExploding()
    {
        explosionCount--;
    }
}
