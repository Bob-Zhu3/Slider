using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dot : MonoBehaviour
{
    public Button button;
    
    public GameObject winMsg, playAgainMsg;
    public TextMeshProUGUI instruction;
    
    static int levelNum = 1;

    static int maxLevelNum = 3;

    static string[] lines;

    public int[] pos;
    
    static List<List<int>> arr = new List<List<int>>(31);

    static int Ok1 = 0, Ok2 = 0;

    private bool shake = false;
    static bool stopUpdate = false;

    public float moveSpeed, countDown = 1f;
    static Vector3 startPos, targetPos;
    void Start()
    {
        Invoke("DisableText", 5f);

        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject go in buttons)
        {
            go.GetComponent<Button>().interactable = false;
        }

        GameObject[] texts = GameObject.FindGameObjectsWithTag("Text");
        foreach (GameObject go in texts)
        {
            go.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 0);
        }

        for (int i = 0; i < 31; i++)
        {
            arr.Add(new List<int>(15));

            for (int j = 0; j < 15; j++)
            {
                arr[i].Add(0);
            }
        }

        TextAsset textFile = Resources.Load<TextAsset>("Level" + levelNum);
        lines = textFile.text.Split("\r\n");
        
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                arr[i][j] = lines[i][j] - 48;
                if (arr[i][j] == 1)
                {
                    Instantiate(Resources.Load("Square"), new Vector3(j - 7, 14 - i, 0), Quaternion.identity);
                }
                else if (arr[i][j] == 2)
                {
                    transform.position = new Vector3(j - 7, 14 - i);
                }
                else if (arr[i][j] == 3)
                {
                    Instantiate(Resources.Load("Target"), new Vector3(j - 7, 14 - i, 0), Quaternion.identity);
                }
            }
        }

        startPos = transform.position;
        pos[0] = (int)startPos.x + 7;
        pos[1] = 14 - (int)startPos.y;
        targetPos = startPos;
    }

    void DisableText()
    {
        instruction.enabled = false;
    }

    public void Move(Vector3 moveDirection)
    {
        targetPos += moveDirection;
    }

    private void Update()
    {
        if (stopUpdate)
            return;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) < 3 && Vector3.Distance(transform.position, targetPos) != 0)
        {
            shake = true;
        }
        if (transform.position.x <= -8 || transform.position.x >= 8 || transform.position.y <= -15 || transform.position.y >= 15)
        {
            pos[0] = (int)startPos.x + 7;
            pos[1] = 14 - (int)startPos.y;
            targetPos = startPos;
        }
    }

    void LateUpdate()
    {
        if (stopUpdate)
            return;

        if (Ok1 != 0 && Ok2 != 0)
        {
            transform.position = new Vector3(Ok1, Ok2);
            pos[0] = Ok1 + 7;
            pos[1] = 14 - Ok2;
            Ok1 = 0;
            Ok2 = 0;
        }

        if (shake)
        {
            ShakeBehavior.instance.StartShake(0.15f, 0.5f);
            shake = false;
        }
    }

    public int Calculate(int dir)
    {
        if (dir == 1)
        {   
            int x = pos[0];
            int temp = pos[1];
            for (int y = pos[1]; y >= 0; y--)
            {
                if (arr[y][x] == 1)
                {
                    pos[1] = y + 1;
                    return temp - y - 1;
                }
            }
            return temp + 1;
        }
        else if (dir == 2)
        {
            int x = pos[0];
            int temp = pos[1];
            for (int y = pos[1]; y <= 30; y++)
            {
                if (arr[y][x] == 1)
                {
                    pos[1] = y - 1;
                    return temp - y + 1;
                }
            }
            return temp - 31;
        }
        else if (dir == 3)
        {
            int y = pos[1];
            int temp = pos[0];
            for (int x = pos[0]; x >= 0; x--)
            {
                if (arr[y][x] == 1)
                {
                    pos[0] = x + 1;
                    return x + 1 - temp;
                }
            }
            return - temp - 1;
        }
        else
        {

            int y = pos[1];
            int temp = pos[0];
            
            for (int x = pos[0]; x <= 14; x++)
            {
                if (arr[y][x] == 1)
                {
                    pos[0] = x - 1;
                    return x - 1 - temp;
                }
            }
            return 15 - temp;
        }
    }

    public void advanceLevel()
    {
        levelNum++;
        stopUpdate = false;

        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject go in buttons)
        {
            go.GetComponent<Button>().interactable = false;
        }

        GameObject[] texts = GameObject.FindGameObjectsWithTag("Text");
        foreach (GameObject go in texts)
        {
            go.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 0);
        }

        GameObject[] squares = GameObject.FindGameObjectsWithTag("Square");
        foreach (GameObject square in squares)
            Destroy(square);

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in targets)
            Destroy(target);

        newLevel();
    }

    public void newLevel()
    {
        if (levelNum > maxLevelNum)
        {
            levelNum = 0;

            GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
            foreach (GameObject go in buttons)
            {
                go.GetComponent<Button>().interactable = true;
            }

            GameObject[] texts = GameObject.FindGameObjectsWithTag("Text");
            foreach (GameObject go in texts)
            {
                go.GetComponent<TextMeshProUGUI>().color = Color.white;
            }

            stopUpdate = true;
            return;
        }

        TextAsset textFile = Resources.Load<TextAsset>("Level" + levelNum);
        lines = textFile.text.Split("\r\n");
        
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                arr[i][j] = lines[i][j] - 48;
                if (arr[i][j] == 1)
                {
                    Instantiate(Resources.Load("Square"), new Vector3(j - 7, 14 - i, 0), Quaternion.identity);
                }
                else if (arr[i][j] == 2)
                {

                    Ok1 = j - 7;
                    Ok2 = 14 - i;
                }
                else if (arr[i][j] == 3)
                {
                    Instantiate(Resources.Load("Target"), new Vector3(j - 7, 14 - i, 0), Quaternion.identity);
                }
            }
        }
        startPos = new Vector3(Ok1, Ok2);
        targetPos = startPos;
    }

    public void printArr()
    {
        string temp = "";
         for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                temp += arr[i][j];
            }
            temp += "\n";
        }
        Debug.Log(temp);
    }
}