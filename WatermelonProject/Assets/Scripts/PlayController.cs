using Unity.VisualScripting;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    GameManager manager;

    public float clickCooltime = 0f;


    private GameObject fruit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameManager.Instance;
        if (manager == null) return;

        fruit = manager.fruit;
    }

    // Update is called once per frame
    void Update()
    {
        if (fruit != null) 
            fruit.transform.position = new Vector2(Input.mousePosition.x, fruit.transform.position.y);

        if (manager.state != GAMESTATE.PLAYING || manager.startPage.activeSelf) return;
        if (fruit == null) fruit = manager.fruit;

        if (Input.GetMouseButtonUp(0))
        {
            // 클릭 쿨타임
            if (Time.realtimeSinceStartup < manager.lastPickTime + clickCooltime) return;
            if (fruit.GetComponent<FruitsStateManager>() == null) return;

            manager.lastPickTime = Time.realtimeSinceStartup;
            FruitsStateManager fruitSM = fruit.GetComponent<FruitsStateManager>();
            fruitSM.state = FRUITSTATE.DROP;
            fruitSM.rigid.simulated = true;

            fruit = manager.GetFruit();
            FRUIT fIndex = UnityEngine.Random.Range(0, 2).ConvertTo<FRUIT>();
            fruit.GetComponent<FruitsStateManager>()?.Init().SetFruitInfo(fIndex);
        }
    }
}
