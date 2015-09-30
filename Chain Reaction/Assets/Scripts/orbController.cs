using UnityEngine;
using System.Collections;

public class orbController : MonoBehaviour {
    Rigidbody2D orbRB;
    public float speed;
    public bool isBomb;
    public float scale;
    private Vector3 scaleVector;
    private Color[] colorChoice = new Color[] { Color.red, Color.blue, Color.green, Color.magenta, Color.yellow };
    private Renderer renderer;
    private bool blownUp;
    private Vector2 lastVelocity;
    public gameController gc;
    private AudioSource audio;
    // Use this for initialization
    void Start() {
        //print("Orb on Start");
        audio = GetComponent<AudioSource>();
        gc = (gameController) GameObject.Find("Main Camera").GetComponent("gameController");
        blownUp = false;
        renderer = GetComponent<Renderer>();
        int j = Random.RandomRange(0, colorChoice.Length);
        renderer.material.color = colorChoice[j];
        isBomb = false;
        orbRB = GetComponent<Rigidbody2D>();
        orbRB.velocity = new Vector2(Random.Range(-speed,speed), Random.Range(-speed,speed));
        scaleVector = new Vector3(this.gameObject.transform.localScale.x * scale, this.gameObject.transform.localScale.y * scale, 0);
    }

    // Update is called once per frame
    void Update() {
        //this.gameObject.layer = 0;
        if (isBomb && this.gameObject.layer != LayerMask.NameToLayer("blownBalls"))
        {
            //Debug.Log(this.gameObject.layer == LayerMask.NameToLayer("blownBalls"));
            gc.explosion();
            orbRB.velocity = new Vector2(0.0f, 0.0f);
            this.gameObject.layer = LayerMask.NameToLayer("blownBalls");
            orbRB.isKinematic = true;
            StartCoroutine(Fade());
            audio.Play();
        }

        if (isBomb && this.gameObject.transform.localScale.x < scaleVector.x && !blownUp)
        {
            this.gameObject.transform.localScale += new Vector3((Time.deltaTime) * this.gameObject.transform.localScale.x, (Time.deltaTime) * this.gameObject.transform.localScale.y, 0);
        }
        if(this.gameObject.transform.localScale.x > scaleVector.x)
        {
            StartCoroutine(destroyOrb());
            blownUp = true;
            scaleVector = this.gameObject.transform.localScale;
        }

        
        if(blownUp && this.gameObject.transform.localScale.x > 0)
        {
            this.gameObject.transform.localScale -= new Vector3(scaleVector.x * Time.deltaTime, scaleVector.y * Time.deltaTime, 0);
        }
    }
    void FixedUpdate()
    {
        lastVelocity = orbRB.velocity;
    }
    IEnumerator Fade()
    {
        for (float f = 1f; f >= 0; f -= 0.005f)
        {
            Color c = renderer.material.color;
            c.a = f;
            renderer.material.color = c;
            yield return null;
        }
    }
    IEnumerator destroyOrb()
    {
        yield return new WaitForSeconds(.75f);
        Destroy(this.gameObject);
        gc.doneExploding();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        orbRB.velocity = Vector2.Reflect(lastVelocity,collision.contacts[0].normal);
        if(collision.gameObject.name.Contains("orb"))
        {
            this.makeBomb();
        }
    }

    public void makeBomb()
    {
        isBomb = true;
    }
    
}
