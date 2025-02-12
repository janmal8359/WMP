using UnityEngine;
using UnityEngine.Rendering.Universal;

//public class GameEventClass : MonoBehaviour
//{
//}

public class InstNewObject
{
    public FRUIT fruit = FRUIT.APPLE;
    public Vector2 pos = Vector2.zero;

    public InstNewObject(FRUIT fruit, Vector2 pos)
    {
        this.fruit = fruit;
        this.pos = pos;
    }
}