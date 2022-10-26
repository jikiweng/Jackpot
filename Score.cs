using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Score : MonoBehaviour
{    
    [SerializeField] private GameObject buttonPressed;
    //win effect
    [SerializeField] private GameObject[] winLines=new GameObject[3];
    [SerializeField] private GameObject[] prizeEffects=new GameObject[3];
    //UI screen
    [SerializeField] private GameObject prizeScreen;
    [SerializeField] private GameObject punishScreen;
    [SerializeField] ShowPrizeText showPrizeText;
    
    //the parameter is to control which line is about to stop
    private int isStart = 0;
    public int IsStart { get { return isStart; } }
    //the parameter is to control what prize the player wins
    public int IsWin = 100;

    //the parameter is to check if there is a 4 in a row
    private int prize = 0;
    //the parameter is to check if there are more than 6 same icon on the screen
    private int[] totalIcon = new int[2]{0,0};
    
    private int[,] iconArray = new int[,] { { 0, 2, 2, 1, 3, 2, 3, 2, 3, 3, 0, 2, 2, 2, 2, 1 }, 
    { 3, 0, 1, 2, 2, 0, 3, 2, 0, 2, 2, 3, 2, 1, 3, 2},{ 0, 2, 3, 3, 2, 0, 2, 1, 1, 0, 2, 2, 3, 3, 2, 0 },
    { 2, 2, 3, 0, 0, 2, 2, 1, 3, 3, 0, 2, 2, 2, 0, 2 } };
    private int[,] finalArray = new int[3, 4];
    private GameObject[] punishEffects;

    private MoveIcon moveIcon;

    private void Start() 
    {
        moveIcon=GameObject.FindObjectOfType<MoveIcon>();
        punishEffects=GameObject.FindGameObjectsWithTag("Frame");
    }

    // Update is called once per frame
    void Update()
    {
        //win prize
        if (isStart != 5||!moveIcon.isFinish) return;

        for(int i=0;i<moveIcon.linePos.Length;i++)
        {
            putInArray(i,moveIcon.linePos[i]);
        }
        score();
        isStart = 0;  
    }

    //start to run
    public void startButton()   
    {        
        for(int i=0;i<totalIcon.Length;i++)
        { totalIcon[i]=0; }      

        buttonPressed.SetActive(true);
        isStart = 1;

        StartCoroutine(delayFunc());
    }

    //back to the main screen
    public void homeButton()
    {
        foreach(GameObject effect in prizeEffects)
        { effect.SetActive(false); }  
        foreach(GameObject line in winLines)
        { line.SetActive(false); }
        prizeScreen.SetActive(false);
        punishScreen.SetActive(false);
        showPrizeText.gameObject.SetActive(false);
    }

    //stop running
    IEnumerator delayFunc()
    {
        //wait 1~2 second before stop the line
        for(int i=0;i<4;i++)
        {
            int delayTime = Random.Range(1, 2);
            yield return new WaitForSeconds(delayTime);
            isStart += 1;
        }      
    }

    //put the result into an array
    void putInArray(int row,int linePos)
    {
        for (int i = 2; i >= 0; i--)
        {
            finalArray[i, row] = iconArray[row, linePos];
            linePos =(linePos==15)? 0: linePos+1;
        }
    }

    //to check if the player wins or not
    void score()
    {
        IsWin=100;
        int straightLine=-1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                //check if it is a 4 in a row
                if (j == 0)
                    prize = finalArray[i, 0];
                else if (finalArray[i, j] != prize)
                    prize = 0;                

                //count the amount for each icon
                switch (finalArray[i, j])
                {
                    case 2:
                        totalIcon[0] += 1;
                        break;
                    case 3:
                        totalIcon[1] += 1;
                        break;
                    default:
                        break;
                }
            }           

            if (prize!=0&&prize < IsWin)
            {                
                straightLine=i;
                IsWin=(prize==3)?12:prize;
            }            
        }
        
        //Debug.Log(finalArray[0,0]+","+finalArray[0,1]+","+finalArray[0,2]+","+finalArray[0,3]);

        if (totalIcon[0] >= 6&&IsWin>2) IsWin=3;
        else if(totalIcon[1] >= 6&&IsWin>12) IsWin=13;

        //which line get win
        if(straightLine>=0) winLines[straightLine].SetActive(true);
        if(IsWin<10) Invoke("showPrizeEffect", 0.5f);
        else if(IsWin<20) Invoke("showPunishEffect",0.5f);

        //if the player win
        if (IsWin < 100) Invoke("showUIscreen", 1);
        buttonPressed.SetActive(false);               
    }

    void showPrizeEffect()
    {
        for(int i=0;i<=3-IsWin;i++)
        {  prizeEffects[i].SetActive(true); }
    }

    void showPunishEffect()
    {
        foreach(GameObject effect in punishEffects)
        {
            effect.SetActive(true);
        }
    }

    void showUIscreen()
    {
        if(IsWin>10) punishScreen.SetActive(true);
        else prizeScreen.SetActive(true);

        showPrizeText.gameObject.SetActive(true);
        showPrizeText.showPrize();
    }
}
