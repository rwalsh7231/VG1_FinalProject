using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    float sprintMult = 1f;
    public Image stamBar;
    public Image healthBar;
    public TMP_Text textScore;
    public TMP_Text finalScore;
    public TMP_Text ammoCount;
    public TMP_Text moneyText;
    public TMP_Text stamText;
    public TMP_Text healthText;
    public TMP_Text shotSpeedText;
    public TMP_Text sprintText;
    public TMP_Text healthRegenText;
    public TMP_Text baseSpeedText;

    SpriteRenderer sprite;
    Animator animator;

    public float currStam, maxStam;
    public float currHealth, maxHealth;
    public int score;
    public int money;
    public float sprintCost;
    public float speedMultiplier;
    public float baseSpeed;
    public float stamRecharge;
    public float healthRegen;
    public float scoreDelay;
    public float shotMultiplier;

	Rigidbody2D _rigidbody2D;
	public Transform aimPivot;

    public bool isPaused;
    private bool isAnimatedX;
    private bool isAnimatedY;

    public GameObject explosionPrefab;

    //weapon in slot 1
    public Weapon currentWeapon;

    //weapon in slot 2
    public Weapon secondaryWeapon;

    void Awake()
    {
        instance = this;
    }

    void Start() {
        score = 0;
        shotMultiplier = PlayerPrefs.GetFloat("Shot Speed", 10f);
        money = PlayerPrefs.GetInt("Money", 0);
        maxHealth = PlayerPrefs.GetFloat("Health", 2f);
        maxStam = PlayerPrefs.GetFloat("Stamina", 100f);
        healthRegen = PlayerPrefs.GetFloat("Health Regen", 0);
        currHealth = maxHealth;
        currStam = maxStam;
        scoreDelay = 1f;
        speedMultiplier = PlayerPrefs.GetFloat("Sprint", 2f);
        baseSpeed = PlayerPrefs.GetFloat("Base Speed", 1f);
        sprintMult = baseSpeed;
        StartCoroutine("ScoreTimer");


        isAnimatedY = false;
        isAnimatedX = false;

        ammoCount.text = currentWeapon.currAmmo.ToString();


        UpdateShopText();

        sprite = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();


        secondaryWeapon.weaponPickup(-1);

    }

    // Update is called once per frame
    void Update()
    {

        if(isPaused) {
            return;
        }

        textScore.text = "Score: " + score.ToString();
        ammoCount.text = currentWeapon.weaponName + " " + currentWeapon.currAmmo.ToString();

        if(currHealth <= 0) {
            MenuController.instance.GameOver();
            currHealth = 0;
            finalScore.text = "Final Score: " + score.ToString();
            PlayerPrefs.SetInt("Money", score + money);
        }

        //sprinting capability
        if (Input.GetKeyDown(KeyCode.LeftShift) && currStam > 0)
        {
            sprintMult = speedMultiplier * baseSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || currStam < 0)
        {
            sprintMult = baseSpeed;
        }


        //go up
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            transform.position += new Vector3(0, 0.5f, 0) * Time.deltaTime*sprintMult;
            animator.SetFloat("moveY", 1);
            isAnimatedY = true;
            
            if (sprintMult > baseSpeed)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }
        else{
            animator.SetFloat("moveY", 0);
        }

        //go down
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            transform.position += new Vector3(0, -0.5f, 0) * Time.deltaTime*sprintMult;
            animator.SetFloat("moveY", -1);
            
            if (sprintMult > baseSpeed)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }
        else {
            if(!isAnimatedY) {
                animator.SetFloat("moveY", 0);
            }
        }

        //go left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            transform.position += new Vector3(-0.5f, 0, 0) * Time.deltaTime*sprintMult;
            animator.SetFloat("moveX", -1);
            isAnimatedX = true;

            if (sprintMult > baseSpeed)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }
        else {
            animator.SetFloat("moveX", 0);
        }

        //go right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            transform.position += new Vector3(0.5f, 0, 0) * Time.deltaTime * sprintMult;
            animator.SetFloat("moveX", 1);
            
            if (sprintMult > baseSpeed)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }
        else {
            if(!isAnimatedX) {
                animator.SetFloat("moveX", 0);
            }
        }

        if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))) {
            animator.SetFloat("moveX", 0);
        }
        if((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))) {
            animator.SetFloat("moveY", 0);
        }

        if (sprintMult == baseSpeed) {
            if(currStam < maxStam) {
                currStam += stamRecharge * Time.deltaTime;
            }
            if(currStam > maxStam) {
                currStam = maxStam;
            }
            stamBar.fillAmount = currStam / maxStam;
        }

        if (currHealth < maxHealth && healthRegen > 0) {
            if(currHealth < maxHealth) {
                currHealth += healthRegen * Time.deltaTime;
            }
            if(currHealth > maxHealth) {
                currHealth = maxHealth;
            }
            healthBar.fillAmount = currHealth / maxHealth;
        }

		Vector3 mousePosition = Input.mousePosition;
		Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
		Vector3 directionFromPlayerToMouse = mousePositionInWorld - transform.position;
		
		float radiansToMouse = Mathf.Atan2(directionFromPlayerToMouse.y, directionFromPlayerToMouse.x);
		float angleToMouse = radiansToMouse * Mathf.Rad2Deg;

		aimPivot.rotation = Quaternion.Euler(0,0, angleToMouse);

        //if player clicks or presses space and ammo is present, fire
		if((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))) {

            currentWeapon.fireWeapon(aimPivot.rotation);

            ammoCount.text = currentWeapon.weaponName + " " + currentWeapon.currAmmo.ToString();
		}

        //pause game
        if(Input.GetKeyDown(KeyCode.Escape)) {
            MenuController.instance.Show();
        }

        //change from primary to secondary weapon
        if (Input.GetKeyDown(KeyCode.Q)) {

            //if the player has a secondary weapon, do some swapping
            if (secondaryWeapon.ID != -1) {
                int currID = currentWeapon.ID;
                int secondaryID = secondaryWeapon.ID;

                int currAmmo = currentWeapon.currAmmo;
                int secondaryAmmo = secondaryWeapon.currAmmo;

                currentWeapon.weaponPickup(secondaryID);
                currentWeapon.currAmmo = secondaryAmmo;

                secondaryWeapon.weaponPickup(currID);
                secondaryWeapon.currAmmo = currAmmo;

            }
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

    public void UpgradeStamina() {
        int cost = 10;
        if(money >= cost) {
            money -= cost;

            currStam += 10;
            maxStam += 10;
            PlayerPrefs.SetFloat("Stamina", maxStam);

            UpdateShopText();
        }
    }

    public void UpgradeHealth() {
        int cost = 10;

        if(money >= cost) {
            money -= cost;

            currHealth += 2;
            maxHealth += 2;
            PlayerPrefs.SetFloat("Health", maxHealth);

            UpdateShopText();
        }
    }

    public void UpgradeHealthRegen() {
        int cost = 10;

        if(money >= cost) {
            money -= cost;

            healthRegen += 0.01f;
            PlayerPrefs.SetFloat("Health Regen", healthRegen);

            UpdateShopText();
        }
    }

    public void UpgradeSprint() {
        int cost = 10;

        if(money >= cost && speedMultiplier < 5) {
            money -= cost;

            speedMultiplier += 0.1f;
            PlayerPrefs.SetFloat("Sprint", speedMultiplier);

            UpdateShopText();
        }
    }

    public void UpgradeBaseMovement() {
        int cost = 10;

        if(money >= cost && baseSpeed < 2) {
            money -= cost;

            baseSpeed += 0.01f;
            PlayerPrefs.SetFloat("Base Speed", baseSpeed);

            UpdateShopText();
        }
    }

    public void UpgradeBlastSpeed() {
        int cost = 10;

        if(money >= cost) {
            money -= cost;

            shotMultiplier += 1f;
            PlayerPrefs.SetFloat("Shot Speed", shotMultiplier);

            UpdateShopText();
        }
    }

    public void UpdateShopText() {
        moneyText.text = "Money: " + money;
        stamText.text = "Max Stamina: " + (int)maxStam;
        healthText.text = "Max Health: " + (int)maxHealth;
        shotSpeedText.text = "Shot Speed: x" + (int)shotMultiplier;
        if(speedMultiplier < 5) {
            sprintText.text = "Sprint Speed: " + (int)((speedMultiplier)*100 + 0.1) + "%";
        }
        else {
            sprintText.text = "Sprint Speed: \nMAX (500%)";
        }
        if(baseSpeed < 2) {
            baseSpeedText.text = "Base Speed: " + (int)((baseSpeed)*100 + 0.1) + "%";
        }
        else {
            baseSpeedText.text = "Base Speed: \nMAX (200%)";
        }
        healthRegenText.text = "Health Regen: " + ((int)(healthRegen / maxHealth * 1000 + 0.1) / 10.0) + "%";
    }

    public void Reset() {
        money = 0;
        PlayerPrefs.DeleteAll();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<Projectile>()) {
            currHealth -= other.gameObject.GetComponent<Projectile>().damage;
            healthBar.fillAmount = currHealth / maxHealth;
        }
    }
}
