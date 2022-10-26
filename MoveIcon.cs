using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIcon : MonoBehaviour
{
    [SerializeField] GameObject[] Lines=new GameObject[8];
    [SerializeField] float objectSpeed = 45f;

    //parameter of the movement
    private float resetPosition = -720f;
    private float startPosition=720f;

    //use to count where the line is
    private int[] lineCount = new int[4];
    //use to count which icon is on the screen
    public int[] linePos = new int[4];
    public bool isFinish = true;

    private Score score;

    // Start is called before the first frame update
    void Start()
    {
        score=GameObject.FindObjectOfType<Score>();
    }

    // Update is called once per frame
    void Update()
    {
        int isStart=score.IsStart;
        //move the line seperately
        if (isStart==0)
        {
            // if(!isFinish) return;

            for(int i=0;i<lineCount.Length;i++)
            { 
                if(linePos[i]!=-1)
                {
                    lineCount[i]=linePos[i]*2; 
                    linePos[i]=-1;
                }
            }
            isFinish = false;
        }
        else moveLines(isStart-1);
        
        if(isStart==5) isFinish = true;
    }

    private void moveLines(int currentLine)
    {
        for(int i=Mathf.Max(0,(currentLine-1)*2);i<Lines.Length;i++)
        {
            if(i<currentLine*2) 
            {
                if(linePos[Mathf.Min(currentLine,3)]>-1) continue;
                finalMove(Lines[i],i);
            }
            else move(Lines[i]);
        }  
        
        for(int j=currentLine;j<lineCount.Length;j++)
        { lineCount[j]+=1; }
  
    }

    //move the line
    void move(GameObject runLine)
    {
        float pos=runLine.transform.localPosition.y - objectSpeed;
        if(pos<=resetPosition) pos=startPosition;

        Vector3 newPos = new Vector3(runLine.transform.localPosition.x, pos, runLine.transform.localPosition.z);                
        runLine.transform.localPosition = newPos;    
    }

    //stop the line at right place
    void finalMove(GameObject runLine,int count)
    {
        int currentLine=count/2;
        if (lineCount[currentLine] % 2 != 0) move(runLine);   
        if(count%2!=0) 
        {
            if (lineCount[currentLine] % 2 != 0) lineCount[currentLine]+=1;
            linePos[currentLine] = lineCount[currentLine] / 2;
            linePos[currentLine] %= 16; 
            // Debug.Log(currentLine+","+linePos[currentLine]);
        }
    }
}
