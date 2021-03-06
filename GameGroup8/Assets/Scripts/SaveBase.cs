﻿using System.Collections;
using MySql.Data.MySqlClient;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MySql;
using Ionic.Zip;

public class SaveBase : MonoBehaviour
{
	MySql.Data.MySqlClient.MySqlConnection conn;
	MySql.Data.MySqlClient.MySqlCommand cmd;
	MySql.Data.MySqlClient.MySqlDataReader myData;
	MySqlCommand cmd2;

    public InputField signup_playername;
    public InputField signup_password;  


    public InputField login_username;
    public InputField login_password;

    public Text LoggedInPlayer;
    public Button SignInOut;

    public Button loadfromcloud;
    public Button savefromcloud;

    public static bool loggedIn = false;
	public static string LoggedInUser;
	
    // Not used but might be useful for expansion of code
    // static string LoggedInPwd;
    // static int LoggedInId;

    // output texts for errors
    public Text logerrorthrow;
    public Text signerrorthrow;

    public Canvas Main;
    public Canvas sign;
    public Canvas log;

    public Canvas feedback;
    public Text feedbackText;

    // ConnectionString
    string mySQLconnectionString = "Server=80.60.131.231;Database=savebase;UID=userw;Pwd=Minor#8;";

	// savegame path
	string path;

    // test code, currently on start but should move to a button
    public void Awake()
    {
        // Create saves directory if it doesn't exist
        Directory.CreateDirectory(Application.dataPath + "/saves/");
        path = Application.dataPath + "/saves/Package.zip";
    }

    /// <summary>
    /// Opens the the connection to the remote MySQL server.
    /// </summary>
    /// 

    public void openConnection(bool fb){
		try
		{
			conn = new MySql.Data.MySqlClient.MySqlConnection(mySQLconnectionString);
			conn.Open();
		}

		catch (Exception ex)
		{

			if (ex is MySqlException) 
			{				
				MySqlException ex2 = (MySqlException)ex;
				Debug.Log ("MySQL Error: " + ex2.Number);
                logerrorthrow.text = "Couldn't connect to server";
                signerrorthrow.text = "Couldn't connect to server";
                if (SceneManager.GetActiveScene().buildIndex == 0 && fb == true)
                {
                    feedback.enabled = true;
                    feedbackText.text = "Couldn't connect to server";
                }
            }
		}
	}

    /// <summary>
    /// Create a new Username + Password and upload it to the server
    /// </summary>
    /// <param name="un">Un.</param>
    /// <param name="pwd">Pwd.</param>

    public void CreateNamePassword()
    {


        string un = signup_playername.text;
        string pwd = signup_password.text;

        if (un.Length == 0)
        {
            signerrorthrow.text = "No username filled in";
        }
        else {
            if (pwd.Length < 4)
            {
                signerrorthrow.text = "password must be longer than 4 characters";
            }
            else { 
                {
                    // Open the Connection
                    openConnection(false);

                    try
                    {
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT INTO savebase.saves(username, password)VALUES(@username, @password)";
                        cmd.Parameters.AddWithValue("@username", un);
                        cmd.Parameters.AddWithValue("@password", pwd);
                        // Execute
                        cmd.ExecuteNonQuery();                        
                        conn.Close();
                        Login(un, pwd);
                        sign.enabled = false;
                        Main.enabled = true;
                        signup_password.text = "";
                        signup_playername.text = "";
                        signerrorthrow.text = "";
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message.ToString());
                        if (ex is MySqlException)
                        {
                            MySqlException ex2 = (MySqlException)ex;
                            Debug.Log(ex2.Number);
                            // catch duplicate entry exception
                            if (ex2.Number == 1062)
                            {

                                signerrorthrow.text = "Username already in use";
                            }

                        }
                        throw ex;
                    }

                    finally
                    {   // needs fixing
                        //if (conn.State ==)
                        //{
                        conn.Close();
                        //}
                    }
                }
            }
        }
    }

	/// <summary>
	/// Uploads the save.
	/// </summary>
	/// <param name="un">Lun, loggedin username</param>
	/// <param name="path">Path, path where save is saved.</param>
	public void UploadSave()
	{

         // first compress current savegame
        Compress();
        



        string lun = LoggedInUser;
		int FileSize;
		byte[] rawData;
		FileStream fs;

		// Open the Connection
		openConnection (true);

		try
		{

			fs = new FileStream(@path, FileMode.Open, FileAccess.Read);
			FileSize = (int)fs.Length;

			rawData = new byte[FileSize];
			fs.Read(rawData, 0, FileSize);
			fs.Close();

			cmd = conn.CreateCommand();
			cmd.CommandText = "UPDATE savebase.saves SET save=@save, filesize=@FileSize WHERE username=@Username";
			cmd.Parameters.AddWithValue("@save", rawData);
			cmd.Parameters.AddWithValue("@UserName", lun);
			cmd.Parameters.AddWithValue("@FileSize", FileSize);


			
			// Execute
			cmd.ExecuteNonQuery();
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                feedback.enabled = true;
                feedbackText.text = "Savegame succesfully uploaded";
            }

            conn.Close();
		}

		catch (Exception ex) {
            feedback.enabled = true;
            feedbackText.text = ex.Message.ToString();

            Debug.Log (ex.Message.ToString ());
			if (ex is MySqlException) {				
				MySqlException ex2 = (MySqlException)ex;
				Debug.Log (ex2.Number);

				// catch duplicate entry exception
				if (ex2.Number == 1062) {
					Debug.Log ("Same username: Pick another");
				}
			}


			Debug.Log (ex.ToString ());


			throw ex;
		}
		finally 
		{
			conn.Close ();
		}		
	}

    /// <summary>
    /// Login using the specified username and password in method.
    /// </summary>
    /// <param name="username">Username. the username</param>
    /// <param name="password">Password. the password</param>

    public void Login(string un, string pwd)
    {

        string username = un;
        string password = pwd;

        // int db_id;
        // string db_name;
        string db_password;

        openConnection(false);

        try
        {


            string sql = "SELECT id, username, password FROM savebase.saves WHERE username=@username";
            cmd = new MySqlCommand(sql, conn);
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@username", username);


            myData = cmd.ExecuteReader();

            while (myData.Read())
            {
                // db_id = myData.GetInt32("id"); ;
                // db_name = myData.GetString("username");
                db_password = myData.GetString("password");

                // verify if password matches username
                if (password == db_password)
                {
                    loggedIn = true;

                    LoggedInUser = username;
                    //LoggedInPwd = password;
                    //LoggedInId = db_id;
                    LoggedInPlayer.text = LoggedInUser;
                }

                else
                {
                    loggedIn = false;
                    Debug.Log("password doesn't match username");
                    // Insert error text to unity
                }

            }
            myData.Close();
            conn.Close();



        }

        catch (Exception ex)
        {
            Debug.Log(ex.Message.ToString());
            if (ex is MySqlException)
            {
                MySqlException ex2 = (MySqlException)ex;
                Debug.Log(ex2.Number);
            }


            Debug.Log(ex.ToString());


            throw ex;
        }
        finally
        {
            conn.Close();
        }

        // read database username
        // read database password

        // search username  -> which id
        // save id as int, find password
        // save password as string
        // does password match?


    }


   

    /// <summary>
    /// Log is the user, using specified name from the UI.
    /// </summary>
    public void Login()
	{

        string username = login_username.text;
        string password = login_password.text;

		// int db_id;
		//string db_name;
		string db_password;

		openConnection (false);

		try 
		{ 	


			string sql = "SELECT id, username, password FROM savebase.saves WHERE username=@username";
			cmd = new MySqlCommand(sql, conn);
			cmd.CommandText = sql;
			cmd.Parameters.AddWithValue("@username", username);

            
			myData = cmd.ExecuteReader();


			while (myData.Read())
			{
				// db_id = myData.GetInt32("id");;
				//db_name = myData.GetString("username");
				db_password = myData.GetString("password");



				// verify if password matches username
				if (password == db_password) {
					loggedIn = true;
					LoggedInUser = username;
					//LoggedInPwd = password;
                    //LoggedInId = db_id;
                    LoggedInPlayer.text = LoggedInUser;
                    log.enabled = false;
                    Main.enabled = true;
                    login_password.text = "";
                    login_username.text = "";
                    logerrorthrow.text = "";

                } 
				else 
				{
					loggedIn = false;
					logerrorthrow.text ="username or password is not correct";
					// Insert error text to unity
				}		
			
			}


			myData.Close();
			conn.Close();



		}

		catch (Exception ex) {
			Debug.Log (ex.Message.ToString ());
			if (ex is MySqlException) 
			{				
				MySqlException ex2 = (MySqlException)ex;
				Debug.Log (ex2.Number);
			}


			Debug.Log (ex.ToString ());


			throw ex;
		}
		finally 
		{
			conn.Close ();
		}			

		// read database username
		// read database password

		// search username  -> which id
		// save id as int, find password
		// save password as string
		// does password match?


	}

    /// <summary>
    /// Signs the current user out
    /// </summary>
    public void signout()
    {
        loggedIn = false;
        LoggedInUser = null;
        //LoggedInPwd = null;

        LoggedInPlayer.text = "Not logged in";
        savefromcloud.gameObject.SetActive(false);
        loadfromcloud.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SignInOut.gameObject.SetActive(false);
        }

    }

    public void Update()
    {

        // Check if an user is logged, if yes, display load and save from cloud
        if(loggedIn)
        {
            savefromcloud.gameObject.SetActive(true);
            loadfromcloud.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SignInOut.gameObject.SetActive(true);
                LoggedInPlayer.text = LoggedInUser;
            }
            

        }


    }

    /// <summary>
    /// Downloads the save.
    /// </summary>
    /// <param name="lun">Lun. Logged in User</param>
    /// <param name="path">Path. path for saving the zip</param>
    public void DownloadSave()
	{
		if (loggedIn) {

            string lun = LoggedInUser;
			
			int db_FileSize;
			byte[] rawData;
			FileStream fs;

			//int db_id;
			//string db_name;
			//string db_password;

			openConnection (true);

			try { 	


				string sql = "SELECT save, filesize FROM savebase.saves WHERE username=@username";
				cmd = new MySqlCommand (sql, conn);
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue ("@username", lun);

				myData = cmd.ExecuteReader ();

				if (! myData.HasRows){
					throw new Exception("There are no rows");
				}

				myData.Read();


				db_FileSize = myData.GetInt32("filesize");
                if (db_FileSize <= 0)
                {

                    // Debug.Log("no remote save found");
                }
                else
                {
                    rawData = new byte[db_FileSize];

                    myData.GetBytes(myData.GetOrdinal("save"), 0, rawData, 0, db_FileSize);

                    fs = new FileStream(@path, FileMode.Create, FileAccess.Write);
                    fs.Write(rawData, 0, db_FileSize);
                    fs.Close();
                    myData.Close();
                    conn.Close();
                    Decompress();

                    if (SceneManager.GetActiveScene().buildIndex == 0)
                    {
                        feedback.enabled = true;
                        feedbackText.text = "Savegame succesfully downloaded, press Load Game game now";
                    }
                }


			} catch (Exception ex) {
				Debug.Log (ex.Message.ToString ());
				if (ex is MySqlException) {				
					MySqlException ex2 = (MySqlException)ex;
					Debug.Log (ex2.Number);
				}
				Debug.Log (ex.ToString ());
                
				throw ex;
			} finally {
				conn.Close ();
			}
		}
	}


    /// <summary>
    /// Decompresses the downloaded savegame
    /// </summary>
    public void Decompress()
    {
        using (ZipFile zip = ZipFile.Read(path))
        {
            foreach (ZipEntry e in zip)
            {
                e.Extract(Application.dataPath + "/saves/", ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }
    /// <summary>
    /// Compresses the savegame files
    /// </summary>
    public void Compress()
    {

        using (ZipFile zip = new ZipFile())
        {
            zip.AddFile(Application.dataPath + "/saves/base.xml", "");
            zip.AddFile(Application.dataPath + "/saves/monsters.xml", "");
            zip.AddFile(Application.dataPath + "/saves/moon.xml", "");
            zip.AddFile(Application.dataPath + "/saves/outside.xml", "");
            zip.AddFile(Application.dataPath + "/saves/Player.xml", "");
            zip.AddFile(Application.dataPath + "/saves/sun.xml", "");
            zip.AddFile(Application.dataPath + "/saves/turrets.xml", "");
            zip.AddFile(Application.dataPath + "/saves/world.xml", "");
            zip.Save(Application.dataPath + "/saves/Package.zip");
        }
    }
}

