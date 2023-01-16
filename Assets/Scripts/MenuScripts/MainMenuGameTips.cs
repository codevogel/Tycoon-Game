using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MenuScripts
{
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
    
    
      Instantiate(gameTipPair.Obj, transform.position, Quaternion.identity);
      tmpText.text = gameTipPair.GameTipText;

    }
    //Pair the gametip and object together
    public struct GameTipPair
    {
      public GameObject Obj;
      public string GameTipText;
      public GameTipPair(GameObject obj, string gameTipText)
      {
        Obj = obj;
        GameTipText = gameTipText;
      }
    }
  }
}
