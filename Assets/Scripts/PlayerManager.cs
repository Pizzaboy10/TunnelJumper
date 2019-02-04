using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerManager : MonoBehaviour {

    public TerrainGenerator terrGen;

    public bool isGrounded = true;
    public bool isAlive = true;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject dragAnchor, dragHandle, water, loseUI;
    public float waterRiseSpeed = 0.5f;

    public float highestHeight = 0;
    public float lastLandHeight = -5;
    [SerializeField] float waterHeight;
    [SerializeField] float pickupsGained = 0;
    [SerializeField] float coinsGained = 0;
    [SerializeField] TextMeshProUGUI scoretext, starsText, coinsText, waterText, failScoreText, failHighScoreText;
    [SerializeField] Animator anim;

    public float distanceToWater;
    public Slider waterHeightSlider;
    public Slider playerHeightSlider;
    AudioSource source;



    private void Start()
    {
        scoretext.text = "Score: 0";
        starsText.text = "Stars: 0";
        coinsText.text = "Stars: 0";
        loseUI.SetActive(false);
        source = GetComponent<AudioSource>();
    }

    void Update () {
        water.transform.Translate(0, waterRiseSpeed * Time.deltaTime, 0);
        waterText.text = "Water: " + waterRiseSpeed * 10;
        distanceToWater = lastLandHeight - water.transform.position.y;
        waterHeightSlider.value = distanceToWater;
        playerHeightSlider.value = transform.position.y - lastLandHeight;


        if (rb.velocity == Vector2.zero && isGrounded == false){
            StartCoroutine(GroundedCheck());
        }
        if (transform.position.y > highestHeight)
        {
            highestHeight = transform.position.y;
            scoretext.text = "SCORE: " + (highestHeight + pickupsGained*3 + coinsGained).ToString("F2");
        }
        else if (transform.position.y < water.transform.position.y)
        {
            Died();
        }
        anim.SetBool("IsGrounded", isGrounded);
    }


    IEnumerator GroundedCheck()
    {
        yield return new WaitForSeconds(0.2f);
        if (rb.velocity == Vector2.zero && isAlive)                         //Don't ground the player if they're dead
        {
            isGrounded = true;                                              //Confirms after a short delay if we are grounded

            if (lastLandHeight < transform.position.y)                      //If our last "landing position" is lower than out current one, update that.
            {
                anim.SetBool("Celebrate", true);
                lastLandHeight = transform.position.y;


                waterHeight = lastLandHeight - 8;
                if (water.transform.position.y < waterHeight)
                {
                    water.transform.position = new Vector2(-10, waterHeight);
                }

                yield return new WaitForSeconds(1f);
                anim.SetBool("Celebrate", false);
            }
        }
        else
        {
            yield return null;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("OneWayPlatform") && collision.transform.position.y < transform.position.y)
        {
            collision.GetComponent<PolygonCollider2D>().isTrigger = false;
            terrGen.NewRoomGeneration();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            source.Play();
            pickupsGained++;
            starsText.text = "Stars: " + pickupsGained.ToString();
            Destroy(collision.gameObject);
            waterRiseSpeed -= 0.6f;
            if (waterRiseSpeed < 0.5f)
            {
                waterRiseSpeed = 0.5f;
            }
        }
        else if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coinsGained++;
            coinsText.text = "Coins: " + coinsGained.ToString();
        }
    }

    public void Died()
    {
        isAlive = false;
        loseUI.SetActive(true);
        anim.SetTrigger("Died");

        //Set records

        failScoreText.text = (highestHeight + pickupsGained).ToString("F2");
        failHighScoreText.text = PlayerPrefs.GetFloat("RecordHeight", 0).ToString();

        PlayerPrefs.SetFloat("PreviousHeight", lastLandHeight);
        if (PlayerPrefs.GetFloat("RecordHeight") < lastLandHeight)
        {
            PlayerPrefs.SetFloat("RecordHeight", lastLandHeight);
        }
    }
}
