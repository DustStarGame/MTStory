using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    private Season gameSeason = Season.春天;

    private int monthInSeason = 3;                  // 一个季节里面有几个月

    private bool gameClockPause;                    //事件暂停
    private float tikTime;                          
    
    private void Awake() {
        NewGameTime();
    }

    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;

            if (tikTime >= Settings.secondThreshold)
            {
                tikTime = 0f;
                UpdateGameTime();
            }
        }

    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2025;
        gameSeason = Season.春天;
    }
        

    private void UpdateGameTime()
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if (gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if (gameDay > Settings.dayHold)
                    {
                        gameMonth++;
                        gameDay = 1;

                        if (gameMonth > 12)
                            gameMonth = 1;

                        monthInSeason--;
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 3;

                            int CurSeasonNumber = (int)gameSeason;
                            CurSeasonNumber++;

                            if (CurSeasonNumber > Settings.seasonHold)
                            {
                                CurSeasonNumber = 0;
                                gameYear++;
                            }

                            gameSeason = (Season)CurSeasonNumber;

                            if (gameYear > 9999)
                            {
                                gameYear = 2025;
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Time: " + gameHour + ":" + gameMinute + ":" + gameSecond + " " + gameSeason + " " + gameMonth + "/" + gameDay + "/" + gameYear);
    }

}
