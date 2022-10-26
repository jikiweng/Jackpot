using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPrizeText : MonoBehaviour
{  
    [SerializeField] Score score;
    [SerializeField] Text balanceText=null;
    private Text prizeText;

    // Start is called before the first frame update
    void Awake()
    {    
        prizeText=GetComponent<Text>();   
    }

    public void showPrize()
    {        
        int prizeRan = Random.Range(1, 20);
        int balance=int.Parse(balanceText.text);

        switch (score.IsWin)
        {
            //small prize
            case 3:
                if (prizeRan <= 10)
                {
                    prizeText.text = "Congratulations!\n You win $100";  
                    balance+=100;    
                }              
                else if (prizeRan > 14)
                {
                    prizeText.text = "Congratulations!\n You win $300";
                    balance+=300;
                }
                else
                {
                    prizeText.text = "Congratulations!\n You win $500";
                    balance+=500;
                }
                break;
            //small punishment
            case 13:
                if (prizeRan <= 2)
                {
                    prizeText.text = "Oops!\n Your coins-700";
                    balance-=700;
                }
                else if (prizeRan > 10)
                {
                    prizeText.text = "Oops!\n Your coins-50";
                    balance-=50;
                }
                else
                {
                    prizeText.text = "Oops!\n Your coins-300";  
                    balance-=300;
                }

                balance=Mathf.Max(0,balance);
                break;
            //second prize
            case 2:
                if (prizeRan <= 10)
                {
                    prizeText.text = "Congratulations!\n You win $1000";
                    balance+=1000;
                }
                else
                {
                    prizeText.text = "Congratulations!\n You win $2000";
                    balance+=2000;
                }
                break;
            //big punishment
            case 12:
                if (prizeRan <= 7)
                {
                    prizeText.text = "Oops!\n Your coins-3000";
                    balance-=3000;
                }
                else if (prizeRan > 8)
                {
                    prizeText.text = "Oops!\n Your coins-2000";
                    balance-=2000;
                }
                else
                {
                    prizeText.text = "Oops!\n Your coins-5000";    
                    balance-=5000;
                }
                break;
            //first prize
            case 1:
                prizeText.text = "Congratulations!\n You win $10000";  
                balance+=10000;              
                break;   
            default:
                break;         
        }
        
        balanceText.text=balance+"";
    }
}
