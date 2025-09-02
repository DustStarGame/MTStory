using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUi : MonoBehaviour
{
    public RectTransform dayNightImage;     //旋转图片
    public RectTransform clockParent;       //时间块
    public Image seasonImage;               //季节图片
    public TextMeshProUGUI dateText;        //日期文本
    public TextMeshProUGUI timeText;        //时间文本

    public Sprite[] seasonSprites;          //季节图片数组

    public List<GameObject> clockBlocks = new List<GameObject>();   //时间块列表

    private void Awake()
    {
        for (int i = 0; i < clockParent.childCount; i++)
        {
            clockBlocks.Add(clockParent.GetChild(i).gameObject);
            clockParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameDateEvent += OnGameDateEvent;
    }


    private void OnDisable()
    {
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameDateEvent -= OnGameDateEvent;
    }


    private void OnGameMinuteEvent(int minute, int hour)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }

    private void OnGameDateEvent(int hour, int day, int mounth, int year, Season season)
    {
        dateText.text = year + "年" + mounth + "月" + day + "日";
        seasonImage.sprite = seasonSprites[(int)season];

        switchHourImage(hour);
        DayNightImageRotate(hour);
    }


    /// <summary>
    /// 根据小时切换时间块显示
    /// </summary>
    /// <param name="hour"></param>
    private void switchHourImage(int hour)
    {
        int index = hour / 4;
        //此处_的作用是让时间到1点时亮第一个时间块，而不是等4点时同时亮两个时间块
        int _ = hour % 4;
        if (index == 0 && _ == 0)
        {
            foreach (var item in clockBlocks)
            {
                item.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < clockBlocks.Count; i++)
            {
                //加一为了是的时间块的显示为1-6而不是0-5
                if (i < index + 1)
                {
                    clockBlocks[i].SetActive(true);
                }
                else
                    clockBlocks[i].SetActive(false);
            }
        }
    }

    private void DayNightImageRotate(int hour)
    {
        //360度/24小时=15度/小时 初始时间为7点为保证旋转为晨曦需要减去90度
        Vector3 target = new Vector3(0, 0, hour * 15 - 90);
        dayNightImage.DORotate(target, 1f, RotateMode.Fast);
    }

}
