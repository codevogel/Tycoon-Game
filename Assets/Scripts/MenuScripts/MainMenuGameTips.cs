using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Instantiates the Main menu object and displays the gametip text.
/// </summary>
public class MainMenuGameTips : MonoBehaviour
{
  public List<GameObject> gameObjects;
  public List<string> gameTexts;
  public TMP_Text tmpText;

    private void Start()
  {
    int random = Random.Range(0, gameObjects.Count);
    GameTipPair gameTipPair = new GameTipPair(gameObjects[random], gameTexts[random]);
    
    
    Instantiate(gameTipPair.obj, transform.position, Quaternion.identity);
    tmpText.text = gameTipPair.gameTipText;

  }
  //Pair the gametip and object together
  public struct GameTipPair
  {
    public GameObject obj;
    public string gameTipText;
    public GameTipPair(GameObject obj, string gameTipText)
    {
      this.obj = obj;
      this.gameTipText = gameTipText;
    }
  }
}
