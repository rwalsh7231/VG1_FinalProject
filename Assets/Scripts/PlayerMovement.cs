using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    int sprintMult = 1;
    public Image stamBar;
    public Image healthBar;
    public TMP_Text textScore;
    public TMP_Text finalScore;
    public TMP_Text ammoCount;

    public int currAmmo;
    public float currStam, maxStam;
    public float currHealth, maxHealth;
    public int score;
    public float sprintCost;
    public float stamRecharge;
    public float scoreDelay;

	Rigidbody2D _rigidbody2D;
	public Transform aimPivot;
	public GameObject projectilePrefab;

    public bool isPaused;

    void Awake()
    {
        instance = this;
    }

    void Start() {
        score = 0;
        currHealth = maxHealth;
        scoreDelay = 1f;
        StartCoroutine("ScoreTimer");
        ammoCount.text = currAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if(isPaused) {
            return;
        }

        textScore.text = score.ToString();
        ammoCount.text = currAmmo.ToString();

        if(currHealth <= 0) {
            MenuController.instance.GameOver();
            currHealth = 0;
            finalScore.text = "Final Score: " + score.ToString();
        }

        //sprinting capability
        if (Input.GetKeyDown(KeyCode.LeftShift) && currStam > 0)
        {
            sprintMult = 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || currStam < 0)
        {
            sprintMult = 1;
        }


        //go up
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            transform.position += new Vector3(0, 0.5f, 0) * Time.deltaTime*sprintMult;
            if (sprintMult == 2)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }

        //go down
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            transform.position += new Vector3(0, -0.5f, 0) * Time.deltaTime*sprintMult;
            if (sprintMult == 2)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }

        //go left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            transform.position += new Vector3(-0.5f, 0, 0) * Time.deltaTime*sprintMult;
            if (sprintMult == 2)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }

        //go right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            transform.position += new Vector3(0.5f, 0, 0) * Time.deltaTime * sprintMult;
            if (sprintMult == 2)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }

        if (sprintMult == 1) {
            if(currStam < maxStam) {
                currStam += stamRecharge * Time.deltaTime;
            }
            if(currStam > maxStam) {
                currStam = maxStam;
            }
            stamBar.fillAmount = currStam / maxStam;
        }

		Vector3 mousePosition = Input.mousePosition;
		Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
		Vector3 directionFromPlayerToMouse = mousePositionInWorld - transform.position;
		
		float radiansToMouse = Mathf.Atan2(directionFromPlayerToMouse.y, directionFromPlayerToMouse.x);
		float angleToMouse = radiansToMouse * Mathf.Rad2Deg;

		aimPivot.rotation = Quaternion.Euler(0,0, angleToMouse);

		if(Input.GetMouseButtonDown(0) && currAmmo > 0) {
			GameObject newProjectile = Instantiate(projectilePrefab);
			newProjectile.transform.position = transform.position;
			newProjectile.transform.rotation = aimPivot.rotation;
            currAmmo--;
            ammoCount.text = currAmmo.ToString();
		}

        if(Input.GetKeyDown(KeyCode.Escape)) {
            MenuController.instance.Show();
        }
    }

    public void EarnPoints(int pointAmount) {
        score += pointAmount;
    }

    IEnumerator ScoreTimer() {
        // Wait
        yield return new WaitForSeconds(scoreDelay);

        // Add to score
        EarnPoints(1);

        // Repeat
        StartCoroutine("ScoreTimer");
    }

    
}
