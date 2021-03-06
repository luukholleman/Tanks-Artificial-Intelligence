﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Scoring : MonoBehaviour {

        private float blueScore;
        private float redScore;

        private GUIStyle blueStyle;
        private GUIStyle redStyle;

        private GUIStyle blueWinStyle;
        private GUIStyle redWinStyle;

        public int WinningScore = 500;

        // Use this for initialization
        void Start()
        {
            blueStyle = new GUIStyle();
            redStyle = new GUIStyle();
            blueWinStyle = new GUIStyle();
            redWinStyle = new GUIStyle();

            blueStyle.normal.textColor = Color.blue;
            redStyle.normal.textColor = Color.red;

            blueWinStyle.normal.textColor = Color.blue;
            blueWinStyle.fontSize = 40;
            redWinStyle.normal.textColor = Color.red;
            redWinStyle.fontSize = 40;
        }

        void FixedUpdate()
        {
            int blue = 0;
            int red = 0;

            foreach(Transform flag in GameObject.Find("Flags").transform)
            {
                Flag f = flag.GetComponent<Flag>();

                if(f.Side == Player.Side.Blue)
                {
                    blue++;
                }
                else if (f.Side == Player.Side.Red)
                {
                    red++;
                }
            }

            blueScore += CalcScore(blue) * Time.deltaTime;
            redScore += CalcScore(red) * Time.deltaTime;

            if (blueScore > WinningScore)
                Time.timeScale = 0;

            if (redScore > WinningScore)
                Time.timeScale = 0;

        }

        float CalcScore(int flagCount)
        {
            int score = 0;

            switch(flagCount)
            {
                case 1:
                    return 0.8f;
                case 2:
                    return 1.1f;
                case 3:
                    return 1.7f;
                case 4:
                    return 3.3f;
                case 5:
                    return 30f;
                default:
                    return 0f;
            }
        }
	
        // Update is called once per frame
        void OnGUI()
        {
            GUI.Label(new Rect(Screen.width / 2, 10, 100, 20), "Red " + Math.Round(redScore), redStyle);
            GUI.Label(new Rect(Screen.width / 2, 25, 100, 20), "Blue " + Math.Round(blueScore), blueStyle);
            GUI.Label(new Rect(Screen.width / 2, 40, 150, 20), "Time elapsed " + (int)Time.timeSinceLevelLoad);

            if (blueScore > WinningScore)
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2, 150, 50), "Blue Won!!!", blueWinStyle);

            if (redScore > WinningScore)
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2, 150, 50), "Red Won!!!", redWinStyle);

            if (blueScore > WinningScore || redScore > WinningScore)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 60, 200, 30), "Restart Level"))
                {
                    Time.timeScale = 1f;
                    Application.LoadLevel(Application.loadedLevelName);
                }
            }
        }
    }
}
