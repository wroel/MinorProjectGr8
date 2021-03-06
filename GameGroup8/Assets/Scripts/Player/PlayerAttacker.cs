﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttacker : MonoBehaviour {
	
	public GameObject bullet;
	public float bulletSpeed = 1000f;
	public static Weapon currentWeapon;

	private WeaponFactory weaponFactory = new WeaponFactory();

	private float nextAttack = 0.0f;
	
	public GameObject weaponPanel;

    public Image[] weaponImages = new Image[8];
	
	public static bool[] unlocked = new bool[8];
	public Text[] lockedUnlockedTexts = new Text[8];
	public Text[] unitCostWeaponTexts = new Text[8];
	public int currentWeaponInt = 1;

	public Text typeOfWunderWaffenText;

	public GameObject weaponUnlockScreen;

    public ParticleSystem pSys1;
    public ParticleSystem pSys2;

	private Animator playerAnimator;

    private int[] weaponCost = new int[] { 0, 0, 10, 15, 30, 10, 10, 10 };
    public Text unitCount;

	public GameObject[] weapons = new GameObject[8];

    private SoundsPlayer PlayerSounds;
    private SoundsWeapons WeaponsSounds;

    Score _score;

	/// <summary>
	/// Called when a new game is started. Sets all fields to default.
	/// </summary>
	public void FirstLoad () {
        _score = Camera.main.GetComponent<Score>();
        currentWeapon = weaponFactory.getPistol ();
		currentWeaponInt = 1;
	    unlocked [0] = true;
		for (int i = 1; i < unlocked.Length; i++) {
			unlocked [i] = false;
		}
		setUnActive ();
		setActive ();
		setTextOfLockUnlock ();
		for (int i = 0; i < 8; i ++) {
			unitCostWeaponTexts[i].text = weaponCost[i] + " Units";
		}
		playerAnimator = gameObject.GetComponent<Animator> ();
        pSys1.enableEmission = false;
        pSys2.enableEmission = false;

        PlayerSounds = gameObject.GetComponent<SoundsPlayer>();
        WeaponsSounds = gameObject.GetComponent<SoundsWeapons>();

        WeaponsSounds.loadGunSounds(gameObject);
        if (currentWeaponInt == 1)
        {
            currentWeapon = weaponFactory.getPistol();
            playerAnimator.SetInteger("weapon", 1);
            currentWeaponInt = 1;
            setAllWeaponsUnactive();
            weapons[0].SetActive(true);
        }
        if (currentWeaponInt == 2)
        {
            currentWeapon = weaponFactory.getShrimpPistol();
            currentWeaponInt = 2;
            playerAnimator.SetInteger("weapon", 1);
            setAllWeaponsUnactive();
            weapons[1].SetActive(true);
        }
        if (currentWeaponInt == 3)
        {
            currentWeapon = weaponFactory.getStingerGun();
            currentWeaponInt = 3;
            playerAnimator.SetInteger("weapon", 1);
            setAllWeaponsUnactive();
            weapons[2].SetActive(true);
        }
        if (currentWeaponInt == 4)
        {
            currentWeapon = weaponFactory.getWeaponizedEel();
            currentWeaponInt = 4;
            playerAnimator.SetInteger("weapon", 3);
            setAllWeaponsUnactive();
            weapons[3].SetActive(true);
        }
        if (currentWeaponInt == 5)
        {
            currentWeapon = weaponFactory.getWunderwuffen();
            currentWeapon.setType(new Type(1));
            currentWeaponInt = 5;
            playerAnimator.SetInteger("weapon", 1);
            setAllWeaponsUnactive();
            weapons[4].SetActive(true);
        }
        if (currentWeaponInt == 6)
        {
            currentWeapon = weaponFactory.getBatteringRam();
            currentWeaponInt = 6;
            playerAnimator.SetInteger("weapon", 3);
            setAllWeaponsUnactive();
            weapons[5].SetActive(true);
        }
        if (currentWeaponInt == 7)
        {
            currentWeapon = weaponFactory.getSwordfish();
            currentWeaponInt = 7;
            playerAnimator.SetInteger("weapon", 2);
            setAllWeaponsUnactive();
            weapons[6].SetActive(true);
        }
        if (currentWeaponInt == 8)
        {
            currentWeapon = weaponFactory.getBaseballBat();
            currentWeaponInt = 8;
            playerAnimator.SetInteger("weapon", 2);
            setAllWeaponsUnactive();
            weapons[7].SetActive(true);
        }
    }

	/// <summary>
	/// Loads the fields according to the save file.
	/// </summary>
	public void LoadFromSave(){
		weaponFactory = new WeaponFactory();
		_score = Camera.main.GetComponent<Score>();
		setUnActive ();
		setActive ();
		setTextOfLockUnlock ();
		for (int i = 0; i < 8; i ++) {
			unitCostWeaponTexts[i].text = weaponCost[i] + " Units";
		}
		playerAnimator = gameObject.GetComponent<Animator> ();
		pSys1.enableEmission = false;
		pSys2.enableEmission = false;

		PlayerSounds = gameObject.GetComponent<SoundsPlayer>();
		WeaponsSounds = gameObject.GetComponent<SoundsWeapons>();

		WeaponsSounds.loadGunSounds(gameObject);

		if (currentWeaponInt == 1){
			currentWeapon = weaponFactory.getPistol();
			playerAnimator.SetInteger("weapon", 1);
			currentWeaponInt = 1;
			setAllWeaponsUnactive();
			weapons[0].SetActive(true);
		}
		if (currentWeaponInt == 2){
			currentWeapon = weaponFactory.getShrimpPistol();
			currentWeaponInt = 2;
			playerAnimator.SetInteger("weapon", 1);
			setAllWeaponsUnactive();
			weapons[1].SetActive(true);
		}
		if (currentWeaponInt == 3){
			currentWeapon = weaponFactory.getStingerGun();
			currentWeaponInt = 3;
			playerAnimator.SetInteger("weapon", 1);
			setAllWeaponsUnactive();
			weapons[2].SetActive(true);
		}
		if (currentWeaponInt == 4){
			currentWeapon = weaponFactory.getWeaponizedEel();
			currentWeaponInt = 4;
			playerAnimator.SetInteger("weapon", 3);
			setAllWeaponsUnactive();
			weapons[3].SetActive(true);
		}
		if (currentWeaponInt == 5){
			currentWeapon = weaponFactory.getWunderwuffen();
			currentWeapon.setType(new Type(1));
			currentWeaponInt = 5;
			playerAnimator.SetInteger("weapon", 1);
			setAllWeaponsUnactive();
			weapons[4].SetActive(true);
		}
		if (currentWeaponInt == 6){
			currentWeapon = weaponFactory.getBatteringRam();
			currentWeaponInt = 6;
			playerAnimator.SetInteger("weapon", 3);
			setAllWeaponsUnactive();
			weapons[5].SetActive(true);
		}
		if (currentWeaponInt == 7){
			currentWeapon = weaponFactory.getSwordfish();
			currentWeaponInt = 7;
			playerAnimator.SetInteger("weapon", 2);
			setAllWeaponsUnactive();
			weapons[6].SetActive(true);
		}
		if (currentWeaponInt == 8){
			currentWeapon = weaponFactory.getBaseballBat();
			currentWeaponInt = 8;
			playerAnimator.SetInteger("weapon", 2);
			setAllWeaponsUnactive();
			weapons[7].SetActive(true);
		}
	}

	/// <summary>
	/// Number of instances that are called every frame. Like: the particle system for the weaponized eel, attacking and switching weapons
	/// and switching weapons.
	/// </summary>
	void Update () {

        pSys1.startRotation = (-gameObject.transform.rotation.eulerAngles.y + 90) * Mathf.Deg2Rad;
        pSys2.startRotation = (-gameObject.transform.rotation.eulerAngles.y + 90) * Mathf.Deg2Rad;


        if (Time.time > nextAttack){
			playerAnimator.SetBool("attack", false);
		}
		bool Base = BaseController.pause;
		setUnActive ();
		setActive ();
		setTextOfLockUnlock ();
		
		if (!Base){
			if(currentWeapon.getIfElectric() && Input.GetMouseButton(0)){
				if (!pSys1.enableEmission) {
					pSys1.enableEmission = true;
					pSys2.enableEmission = true;
				} else {
					pSys1.enableEmission = false;
					pSys2.enableEmission = false;
				}
                WeaponsSounds.playWeaponSound(currentWeaponInt - 1);                            //Sound
                GameObject[] nearbyEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                Vector3 placeOfLightning = pSys1.transform.position;
                for (int i = 0; i < nearbyEnemies.Length; i++) {
                    if (Vector3.Distance(placeOfLightning, nearbyEnemies[i].transform.position) < 2){
                        nearbyEnemies[i].GetComponent<EnemyController>().setHealth((int)(nearbyEnemies[i].GetComponent<EnemyController>().getHealth() - 2 * currentWeapon.getType().damageMultiplierToType(nearbyEnemies[i].GetComponent<EnemyController>().getType()) * PlayerAttributes.getAttackMultiplier()));
                        if (nearbyEnemies[i].GetComponent<EnemyController>().getHealth() <= 0)
                        {
                            EnemySpawner.enemiesDefeaten++;
                            nearbyEnemies[i].GetComponent<Seeker>().StopAllCoroutines();
                            nearbyEnemies[i].GetComponent<Seeker>().destroyed = true;
                            nearbyEnemies[i].GetComponent<EnemyController>().destroyed = true;
                            _score.addScoreEnemy(nearbyEnemies[i].GetComponent<EnemyController>().getLevel());
                            nearbyEnemies[i].GetComponent<EnemyController>().StartCoroutine(nearbyEnemies[i].GetComponent<EnemyController>().die());
                            MiniMapScript.enemies.Remove(nearbyEnemies[i].GetComponent<EnemyController>());
                            if (!nearbyEnemies[i].GetComponent<EnemyController>().dead)
                            {
                                PlayerAttributes.getExperience(nearbyEnemies[i].GetComponent<EnemyController>().getLevel());
                            }
                        }
                    }
                }
            }
            if (currentWeapon.getIfElectric() && Input.GetMouseButtonUp(0))
            {
                pSys1.enableEmission = false;
                pSys2.enableEmission = false;
            }
            if (!currentWeapon.getIfElectric() && currentWeapon.getIfAutomatic() && Input.GetMouseButton(0) && Time.time > nextAttack && !currentWeapon.getIfMelee()){
				nextAttack = Time.time + currentWeapon.getAttackSpeed();
				GameObject bulletClone = GameObject.Instantiate(bullet, transform.position + (transform.forward), transform.rotation) as GameObject;
				bulletClone.tag = currentWeapon.getType().toString ();
				bulletClone.GetComponent<Bullet>().dmg = currentWeapon.getWeaponDamage();
				bulletClone.GetComponent<Bullet>().poisonous = currentWeapon.getIfPoisonous();
				bulletClone.GetComponent<Bullet>().stun = currentWeapon.getIfStuns();
                bulletClone.GetComponent<Bullet>().shotByPlayer = true;
                bulletClone.transform.Rotate(90, 0, 0);
				bulletClone.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
                Analytics.fireShot();
                WeaponsSounds.playWeaponSound(currentWeaponInt - 1);                            //Sound
            }
           
            if (!currentWeapon.getIfAutomatic() && Input.GetMouseButtonDown(0) && Time.time > nextAttack && !currentWeapon.getIfMelee()){
				nextAttack = Time.time + currentWeapon.getAttackSpeed();
				GameObject bulletClone = GameObject.Instantiate(bullet, transform.position + (transform.forward), transform.rotation) as GameObject;
				bulletClone.tag = currentWeapon.getType().toString ();
				bulletClone.GetComponent<Bullet>().dmg = currentWeapon.getWeaponDamage();
				bulletClone.GetComponent<Bullet>().poisonous = currentWeapon.getIfPoisonous();
				bulletClone.GetComponent<Bullet>().stun = currentWeapon.getIfStuns();
                bulletClone.GetComponent<Bullet>().shotByPlayer = true;
                bulletClone.transform.Rotate(90, 0, 0);
				bulletClone.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
                Analytics.fireShot();
                WeaponsSounds.playWeaponSound(currentWeaponInt - 1);                            //Sound
            }
			if (Input.GetMouseButtonDown(0) && Time.time > nextAttack && currentWeapon.getIfMelee()){
				playerAnimator.SetBool ("attack", true);
				nextAttack = Time.time + currentWeapon.getAttackSpeed();
				WeaponsSounds.playWeaponSound(currentWeaponInt - 1);        					//Sound
				Vector3 placeOfWeapon = GameObject.Find ("player").transform.position;
				GameObject[] nearbyEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
				for (int i = 0; i < nearbyEnemies.Length; i++) {
					if (Vector3.Distance (placeOfWeapon + transform.forward * 2, nearbyEnemies [i].transform.position) < 2) {
						int damage = (int)(Random.Range (currentWeapon.getWeaponDamage (), currentWeapon.getWeaponDamage () + 10) * currentWeapon.getType ().damageMultiplierToType (nearbyEnemies [i].GetComponent<EnemyController> ().getType ()) * PlayerAttributes.getAttackMultiplier ());
						nearbyEnemies [i].GetComponent<EnemyController> ().setHealth (nearbyEnemies [i].GetComponent<EnemyController> ().getHealth () - damage);
						if (Vector3.Distance (GameObject.FindGameObjectWithTag ("BASE").transform.position, nearbyEnemies [i].transform.position + transform.forward * currentWeapon.getKnockBack ()) > 11) {
							nearbyEnemies [i].transform.position = nearbyEnemies [i].transform.position + transform.forward * currentWeapon.getKnockBack ();
						}
						if (nearbyEnemies [i].GetComponent<EnemyController> ().getHealth () <= 0) {
							EnemySpawner.enemiesDefeaten++;
							if (!nearbyEnemies [i].name.Equals ("MeepMeepPrefab(Clone)")) {
								nearbyEnemies [i].GetComponent<Seeker> ().StopAllCoroutines ();
								nearbyEnemies [i].GetComponent<Seeker> ().destroyed = true;
							}
							nearbyEnemies [i].GetComponent<EnemyController> ().destroyed = true;
							_score.addScoreEnemy (nearbyEnemies [i].GetComponent<EnemyController> ().getLevel ());
							nearbyEnemies [i].GetComponent<EnemyController> ().StartCoroutine (nearbyEnemies [i].GetComponent<EnemyController> ().die ());
							MiniMapScript.enemies.Remove (nearbyEnemies [i].GetComponent<EnemyController> ());
							if (!nearbyEnemies [i].GetComponent<EnemyController> ().dead) {
								PlayerAttributes.getExperience (nearbyEnemies [i].GetComponent<EnemyController> ().getLevel ());
							}
						}
					}
				}            
			}

            if ((currentWeaponInt == 3 || currentWeaponInt == 4) && Input.GetMouseButtonUp(0))
            {
                WeaponsSounds.StopWeaponsound(currentWeaponInt - 1);
            }

			if ((Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1) || currentWeaponInt == 1) && unlocked[0]){
				currentWeapon = weaponFactory.getPistol();
				playerAnimator.SetInteger ("weapon", 1);
				currentWeaponInt = 1;
				setAllWeaponsUnactive ();
				weapons [0].SetActive (true);
			}
			if ((Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2) || currentWeaponInt == 2) && unlocked[1]){
				currentWeapon = weaponFactory.getShrimpPistol();
				currentWeaponInt = 2;
				playerAnimator.SetInteger ("weapon", 1);
				setAllWeaponsUnactive ();
				weapons [1].SetActive (true);
			}
			if ((Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)  || currentWeaponInt == 3) && unlocked[2]){
				currentWeapon = weaponFactory.getStingerGun();
				currentWeaponInt = 3;
				playerAnimator.SetInteger ("weapon", 1);
				setAllWeaponsUnactive ();
				weapons [2].SetActive (true);
			}
			if ((Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)  || currentWeaponInt == 4) && unlocked[3]){
				currentWeapon = weaponFactory.getWeaponizedEel();
				currentWeaponInt = 4;
				playerAnimator.SetInteger ("weapon", 3);
				setAllWeaponsUnactive ();
				weapons [3].SetActive (true);
			}
			if ((Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5)  || currentWeaponInt == 5) && unlocked[4]){
				bool getIfWunderwaffen = currentWeapon.getIfChangeable ();
				Type type = currentWeapon.getType ();
				currentWeapon = weaponFactory.getWunderwuffen();
				if (getIfWunderwaffen) {
					currentWeapon.setType (type);
				}
				currentWeaponInt = 5;
				playerAnimator.SetInteger ("weapon", 1);
				setAllWeaponsUnactive ();
				weapons [4].SetActive (true);
			}
			if ((Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6)  || currentWeaponInt == 6) && unlocked[5]){
				currentWeapon = weaponFactory.getBatteringRam();
				currentWeaponInt = 6;
				playerAnimator.SetInteger ("weapon", 3);
				setAllWeaponsUnactive ();
				weapons [5].SetActive (true);
			}
			if ((Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7) || currentWeaponInt == 7) && unlocked[6]){
				currentWeapon = weaponFactory.getSwordfish();
				currentWeaponInt = 7;
				playerAnimator.SetInteger ("weapon", 2);
				setAllWeaponsUnactive ();
				weapons [6].SetActive (true);
			}
			if ((Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8)  || currentWeaponInt == 8) && unlocked[7]){
				currentWeapon = weaponFactory.getBaseballBat();
				currentWeaponInt = 8;
				playerAnimator.SetInteger ("weapon", 2);
				setAllWeaponsUnactive ();
				weapons [7].SetActive (true);
			}
			if(currentWeapon.getIfChangeable() && Input.GetMouseButtonDown(1)){
				if(currentWeapon.getType().getType() < 3){
					currentWeapon.setType(new Type(currentWeapon.getType().getType() + 1));
					typeOfWunderWaffenText.text = currentWeapon.getType().toString();
				}
				else{
					currentWeapon.setType(new Type(1));
					typeOfWunderWaffenText.text = currentWeapon.getType().toString();
				}
			}
			if (Input.GetAxis ("Mouse ScrollWheel") != 0f) {
				if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {
					int[] unlockedWeapons = new int[8];
					int index = 0;
					if (currentWeaponInt != 8) {
						for (int i = currentWeaponInt; i < 8; i++) {
							if (unlocked [i]) {
								unlockedWeapons [index] = i + 1;
								index++;
							}
						}
					}
					if (currentWeaponInt != 1) {
						for (int i = 0; i < currentWeaponInt; i++) {
							if (unlocked [i]) {
								unlockedWeapons [index] = i + 1;
								index++;
							}
						}
					}
					if (unlockedWeapons [0] != 0) {
						currentWeaponInt = unlockedWeapons [0];
					}
				}
				if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {
					int[] unlockedWeapons = new int[8];
					int index = 0;
					if (currentWeaponInt != 1) {
						for (int i = currentWeaponInt - 2; i >= 0; i--) {
							if (unlocked [i]) {
								unlockedWeapons [index] = i + 1;
								index++;
							}
						}
					}
					if (currentWeaponInt != 7) {
						for (int i = 7; i >= currentWeaponInt; i--) {
							if (unlocked [i]) {
								unlockedWeapons [index] = i + 1;
								index++;
							}
						}
					}
					if (unlockedWeapons [0] != 0) {
						currentWeaponInt = unlockedWeapons [0];
					}
				}
			}
		}
		
	}

	/// <summary>
	/// Makes the image of the current weapon on the weapon panel red.
	/// </summary>
	private void setActive(){
        weaponImages[currentWeaponInt - 1].color = Color.red;
        Analytics.setWeapons(currentWeaponInt - 1);
	}

	/// <summary>
	/// Makes all weapon images black if unlocked or gray if locked.
	/// </summary>
	private void setUnActive(){
        for(int i = 0; i < 8; i++)
        {
            if (!unlocked[i])
            {
                weaponImages[i].color = Color.gray;
            }
            else
            {
                weaponImages[i].color = Color.black;
            }
        }
		
		if (!unlocked [4]) {
			typeOfWunderWaffenText.color = Color.gray;
		} else {
			typeOfWunderWaffenText.color = Color.black;
		}
	}

	/// <summary>
	/// sets weapon number i unlocked. Called only when a gearshack is build.
	/// </summary>
	/// <param name="i">The index.</param>
	public static void unlock(int i){
		unlocked [i - 1] = true;
	}

	/// <summary>
	/// sets weapon number i unlocked. Called when a weapon is unlocked in the weapon smith.
	/// </summary>
	/// <param name="i">The index.</param>
	public void unlockInt(int i){
		if(PlayerController.getCount() >= weaponCost[i - 1]){
		    unlocked [i - 1] = true;
		    setTextOfLockUnlock ();

            PlayerController.setCount(weaponCost[i - 1]);
            unitCount.text = "Amount of units:" + PlayerController.getCount();
		}
	}

	/// <summary>
	/// Changed the text in the weapon unlock panel when a new weapon is unlocked.
	/// </summary>
	public void setTextOfLockUnlock(){
		for (int i = 0; i < 8; i ++) {
			if (unlocked [i]) {
				lockedUnlockedTexts [i].text = "Unlocked";
			} else {
				lockedUnlockedTexts [i].text = "Locked";
			}
		}
	}

	/// <summary>
	/// Sets all gameObjects of weapons unactive. Called when a weapon is switched.
	/// </summary>
	public void setAllWeaponsUnactive(){
		for (int i = 0; i < 8; i++) {
			weapons [i].SetActive (false);
		}
	}
}