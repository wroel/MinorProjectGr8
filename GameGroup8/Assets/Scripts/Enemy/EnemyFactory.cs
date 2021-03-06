﻿using UnityEngine;
using System.Collections;

public class EnemyFactory {

	/// <summary>
	/// The constructor for the EnemyFactory.
	/// </summary>
	public EnemyFactory() {
	}

	/// <summary>
	/// Makes an enemy according to the name and level with the right attributes.
	/// </summary>
	/// <returns>The enemy.</returns>
	/// <param name="name">Name.</param>
	/// <param name="level">Level.</param>
	public Enemy getEnemy(string name, int level){
		if (name.Equals ("Hammerhead")) {
			return getHammerhead (level);
		} else if (name.Equals ("DesertEagle")) {
			return getDesertEagle (level);
		} else if (name.Equals ("FireFox")) {
			return getFireFox (level);
		} else if (name.Equals ("PolarBear")) {
			return getPolarBear (level);
		} else if (name.Equals ("MeepMeep")) {
			return getMeepMeep (level);
		} else if (name.Equals ("Oilphant")) {
			return getOilphant (level);
		}else{
            return getHammerhead(level);
        }
	}

	/// <summary>
	/// Returns a hammerHead enemy according to the level.
	/// </summary>
	/// <returns>The hammerhead.</returns>
	/// <param name="level">Level.</param>
	private Enemy getHammerhead(int level) {
		if (level == 1) {
			return new Enemy (1, 100, 10, 2f, new Type (3), "HammerHead");
		} else if (level == 2) {
			return new Enemy (2, 200, 15, 2f, new Type (3), "HammerHead");
		} else if (level == 3) {
			return new Enemy (3, 250, 20, 2f, new Type (3), "HammerHead");
		} else if (level == 4) {
			return new Enemy (4, 300, 22, 2f, new Type (3), "HammerHead");
		} else if (level == 5) {
			return new Enemy (5, 400, 25, 2f, new Type (3), "HammerHead");
		} else {
			return new Enemy (1, 100, 10, 2f, new Type (3), "HammerHead");
		}
	}

	/// <summary>
	/// Returns a desertEagle enemy according to the level.
	/// </summary>
	/// <returns>The desert eagle.</returns>
	/// <param name="level">Level.</param>
	private Enemy getDesertEagle(int level) {
		if (level == 1) {
			return new Enemy (1, 75, 7, 4f, new Type (1), "DesertEagle");
		} else if (level == 2) {
			return new Enemy (2, 125, 9, 4f, new Type (1), "DesertEagle");
		} else if (level == 3) {
			return new Enemy (3, 175, 14, 4f, new Type (1), "DesertEagle");
		} else if (level == 4) {
			return new Enemy (4, 250, 17, 4f, new Type (1), "DesertEagle");
		} else if (level == 5) {
			return new Enemy (5, 300, 20, 4f, new Type (1), "DesertEagle");
		} else {
			return new Enemy (1, 75, 7, 4f, new Type (1), "DesertEagle");
		}
	}

	/// <summary>
	/// Returns a fireFox according to the level.
	/// </summary>
	/// <returns>The fire fox.</returns>
	/// <param name="level">Level.</param>
	private Enemy getFireFox(int level) {
		if (level == 1) {
			return new Enemy (1, 20, 30, 5f, new Type (2), "FireFox");
		} else if (level == 2) {
			return new Enemy (2, 40, 45, 5f, new Type (2), "FireFox");
		} else if (level == 3) {
			return new Enemy (3, 45, 60, 5f, new Type (2), "FireFox");
		} else if (level == 4) {
			return new Enemy (4, 60, 75, 5f, new Type (2), "FireFox");
		} else if (level == 5) {
			return new Enemy (5, 75, 85, 5f, new Type (2), "FireFox");
		} else {
			return new Enemy (1, 20, 30, 5f, new Type (2), "FireFox");
		}
	}

	/// <summary>
	/// Returns a polarBear according to the level.
	/// </summary>
	/// <returns>The polar bear.</returns>
	/// <param name="level">Level.</param>
    private Enemy getPolarBear(int level){
        if (level == 1){
            return new Enemy(1, 125, 0, 5f, new Type(2), "PolarBear");
        }else if (level == 2){
            return new Enemy(2, 200, 0, 5f, new Type(2), "PolarBear");
        }else if (level == 3){
            return new Enemy(3, 280, 0, 5f, new Type(2), "PolarBear");
        }else if (level == 4){
            return new Enemy(4, 350, 0, 5f, new Type(2), "PolarBear");
        }else if (level == 5){
            return new Enemy(5, 435, 0, 5f, new Type(2), "PolarBear");
        }else {
            return new Enemy(1, 125, 0, 5f, new Type(2), "PolarBear");
        }
    }

	/// <summary>
	/// Returns a meepMeep enemy according to the level.
	/// </summary>
	/// <returns>The meep meep.</returns>
	/// <param name="level">Level.</param>
    private Enemy getMeepMeep(int level)
    {
        if (level == 1){
            return new Enemy(1, 75, 0, 10f, new Type(1), "MeepMeep");
        }else if (level == 2){
            return new Enemy(2, 125, 0, 10f, new Type(1), "MeepMeep");
        }else if (level == 3){
            return new Enemy(3, 150, 0, 10f, new Type(1), "MeepMeep");
        }else if (level == 4){
            return new Enemy(4, 220, 0, 10f, new Type(1), "MeepMeep");
        }else if (level == 5){
            return new Enemy(5, 250, 0, 10f, new Type(1), "MeepMeep");
        }else {
            return new Enemy(1, 75, 0, 10f, new Type(1), "MeepMeep");
        }
    }

	/// <summary>
	/// Returns an oilPhant enemy according to the level.
	/// </summary>
	/// <returns>The oilphant.</returns>
	/// <param name="level">Level.</param>
    private Enemy getOilphant(int level){
        if (level == 1){
            return new Enemy(1, 125, 15, 4f, new Type(3), "Oilphant");
        }else if (level == 2){
            return new Enemy(2, 210, 18, 4f, new Type(3), "Oilphant");
        }else if (level == 3){
            return new Enemy(3, 260, 22, 4f, new Type(3), "Oilphant");
        }else if (level == 4){
            return new Enemy(4, 320, 30, 4f, new Type(3), "Oilphant");
        }else if (level == 5){
            return new Enemy(5, 380, 32, 4f, new Type(3), "Oilphant");
        }else {
            return new Enemy(1, 125, 15, 4f, new Type(3), "Oilphant");
        }
    }
}