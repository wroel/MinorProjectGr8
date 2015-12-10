﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {

	public static int level = 1;
	public static int experience = 0;
	public static int pointsToUpgrade = 0;

	private static int experienceNeededToLevelUp = 5;

	public int attackPoints = 0;
	public int speedPoints = 0;
	public int maxHealthPoints = 0;
	public int maxEnergyPoints = 0;

	private static float attackMultiplier = 1f;
	private static float walkingSpeed = 5f;
	private static float maxWalkingSpeed = 5f;
	private static float runningSpeed = walkingSpeed * 2f;
	private static int maxHealth = 100;
	private static int health = 100;
	private static int maxEnergy = 100;
	private static int energy = 100;

	private static bool running = false;
	private static float speed = 5f;

	public static int fatique = 10000;

	public GameObject upgradePanel;
	public Text pointsToAssignText;
	public Text attackPointsText;
	public Text speedPointsText;
	public Text healthPointsText;
	public Text energyPointsText;

	void Start() {
		pointsToAssignText.text = "Points to assign " + pointsToUpgrade;
		attackPointsText.text = "Attack Points: " + attackPoints;
		speedPointsText.text = "Speed Points: " + speedPoints;
		healthPointsText.text = "Max Health Points: " + maxHealthPoints;
		energyPointsText.text = "Max Energy Points: " + maxEnergyPoints;
		upgradePanel.SetActive (false);
		speed = walkingSpeed;
	}

	public void openUpgradePanel(){
		upgradePanel.SetActive (true);
		Time.timeScale = 0;
		pointsToAssignText.text = "Points to assign " + pointsToUpgrade;
		attackPointsText.text = "Attack Points: " + attackPoints;
		speedPointsText.text = "Speed Points: " + speedPoints;
		healthPointsText.text = "Max Health Points: " + maxHealthPoints;
		energyPointsText.text = "Max Energy Points: " + maxEnergyPoints;
	}

	public void closeUpgradePanel(){
		upgradePanel.SetActive (false);
		Time.timeScale = 1;
	}

	private static void levelUp(){
		level ++;
		pointsToUpgrade += 3;
	}

	public static int getLevel(){
		return level;
	}

	public static void getExperience(int xp){
		experience += xp;
		if (experience >= level * experienceNeededToLevelUp) {
			levelUp ();
		}
	}

	public void upgradeAttack(){
		if (pointsToUpgrade > 0) {
			attackPoints++;
			attackMultiplier = 1f + (float)attackPoints * 0.05f;
			attackPointsText.text = "Attack Points: " + attackPoints;
			pointsToUpgrade --;
			pointsToAssignText.text = "Points to assign " + pointsToUpgrade;
		}
	}

	public void upgradeSpeed(){
		if (pointsToUpgrade > 0) {
			speedPoints++;
			maxWalkingSpeed = 5f + (float)speedPoints * 0.5f;
			speedPointsText.text = "Speed Points: " + speedPoints;
			pointsToUpgrade --;
			pointsToAssignText.text = "Points to assign " + pointsToUpgrade;
		}
	}

	public void upgradeMaxHealth(){
		if (pointsToUpgrade > 0) {
			maxHealthPoints++;
			maxHealth = 100 + 5 * maxHealthPoints;
			healthPointsText.text = "Max Health Points: " + maxHealthPoints;
			pointsToUpgrade --;
			pointsToAssignText.text = "Points to assign " + pointsToUpgrade;
		}
	}

	public void upgradeMaxEnergy(){
		if (pointsToUpgrade > 0) {
			maxEnergyPoints++;
			maxEnergy = 100 + 5 * maxEnergyPoints;
			energyPointsText.text = "Max Energy Points: " + maxEnergyPoints;
			pointsToUpgrade --;
			pointsToAssignText.text = "Points to assign " + pointsToUpgrade;
		}
	}

	public static float getAttackMultiplier(){
		return attackMultiplier;
	}

	public static float getSpeed(){
		return speed;
	}

	public static int getMaxHealth(){
		return maxHealth;
	}

	public static void setMaxHealth(int res){
		maxHealth += res;
		health += res;
	}

	public static int getHealth(){
		return health;
	}

	public static void takeDamage(int damage){
		health -= damage;
	}

	public static void regenerate(){
		health ++;
	}

	public static int getMaxEnergy(){
		return maxEnergy;
	}

	public static void setMaxEnergy(int res){
		maxEnergy += res;
		energy += res;
	}

	public static int getEnergy(){
		return energy;
	}

	public static void setEnergyDown(){
		energy --;
	}

	public static void replenish(){
		energy ++;
	}

	public static void run(){
		speed = runningSpeed;
		running = true;
	}

	public static void dontRun(){
		speed = walkingSpeed;
		running = false;
	}

	public static bool isRunning(){
		return running;
	}

	public static int getFatique(){
		return fatique;
	}
	
	public static void getTired(){
		fatique--;
		walkingSpeed = maxWalkingSpeed * Mathf.Pow (0.8f, (10000f - (float)fatique) / 10000f);
	}

	public static void resetFatique(){
		fatique = 10000;
		walkingSpeed = maxWalkingSpeed;
	}
}