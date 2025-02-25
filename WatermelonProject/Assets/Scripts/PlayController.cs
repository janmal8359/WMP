using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    GameManager manager;

    public float clickCooltime = 0f;
    public NativeHashMap<int, FRUIT> fruitNum = new NativeHashMap<int, FRUIT>();

    private GameObject fruit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameManager.Instance;
        if (manager == null) return;

        fruit = manager.GetFruit();
    }

    // Update is called once per frame
    void Update()
    {
        if (fruit != null) 
            fruit.transform.position = new Vector2(Input.mousePosition.x, fruit.transform.position.y);

        if (manager.state != GAMESTATE.PLAYING || manager.startPage.activeSelf) return;
        if (fruit == null) fruit = manager.GetFruit();

        if (Input.GetMouseButtonUp(0))
        {
            // 클릭 쿨타임
            if (Time.realtimeSinceStartup < manager.lastPickTime + clickCooltime) return;
            if (fruit.GetComponent<FruitsStateManager>() == null) return;

            manager.lastPickTime = Time.realtimeSinceStartup;
            if (fruit == null) Debug.LogError("Not Exist Fruit");
            FruitsStateManager fruitSM = fruit.GetComponent<FruitsStateManager>();
            if (fruitSM == null)
            {
                Debug.LogError("Not Exist FSM");
            }
            fruitSM.state = FRUITSTATE.DROP;
            fruitSM.rigid.simulated = true;

            fruit = manager.GetFruit();
            FRUIT fIndex = (FRUIT)UnityEngine.Random.Range(0, 2);
            fruit.GetComponent<FruitsStateManager>()?.Init().SetFruitInfo(fIndex);
        }
    }
}
