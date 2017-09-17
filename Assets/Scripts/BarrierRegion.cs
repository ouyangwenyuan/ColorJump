using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarrierRegion : MonoBehaviour {

    public Player player;
    public Barrier[] barrierPrefabs;
    public ColorChanger colorChangerPrefab;
    public GameObject starPrefab;
    private Vector3 spawnPosition;
    public static BarrierRegion instance;
    private List<Barrier> listBarrs;
    public GameObject playerExplosion, starExplosion, scorePrefab;
    public static int CIRCLE_BARRIER = 0, RECT_BARRIER = 1, X_BARRIER = 2, LINE_BARRIER = 3, DOT_BARRIER = 4, CIRCLE_BARRIER2 = 5, DOT_BARRIER2 = 6, TRIANGLE = 7;
    int barrierIndex = 0;
    public void Start()
    {
        instance = this;
        spawnPosition = new Vector3(0, 0);
        listBarrs = new List<Barrier>();
        for (barrierIndex = 0; barrierIndex < 5; barrierIndex++)
        {
            SpawnNewBarrier(barrierIndex);
        }
    }

    private void SpawnNewBarrier(int index)
    {
        int typeIndex = Random.Range(0, barrierPrefabs.Length);
        if (index == 0 && typeIndex == TRIANGLE) typeIndex = CIRCLE_BARRIER;
        bool doubleBarr = typeIndex != LINE_BARRIER && typeIndex != DOT_BARRIER2 && typeIndex != TRIANGLE
            && index != 0 && (Random.Range(0, 2) == 1);
        ColorChanger.ChangerType changerType = ColorChanger.ChangerType.All;
        if (doubleBarr)
        {
            SpawnNewBarrier(typeIndex, spawnPosition + Vector3.left * 120, Vector3.one * 0.75f, false);
            SpawnNewBarrier(typeIndex, spawnPosition + Vector3.right * 120, new Vector3(-1, 1, 1) * 0.75f, typeIndex != 4);
        }
        else
        {
            if (typeIndex == TRIANGLE)
            {
                changerType = ColorChanger.ChangerType.ExceptYellow;
            }
            if (typeIndex == X_BARRIER)
            {
                if (Random.Range(0, 2) == 1)
                    SpawnNewBarrier(typeIndex, spawnPosition + Vector3.left * 80, Vector3.one, false);
                else
                    SpawnNewBarrier(typeIndex, spawnPosition - Vector3.left * 80, Vector3.one, true);
            }
            else if (typeIndex == CIRCLE_BARRIER || typeIndex == CIRCLE_BARRIER2)
            {
                int nCir = (index == 0) ? 1 : Random.Range(1, 3);
                bool clockwise = Random.Range(0, 2) == 1;
                SpawnNewBarrier(typeIndex, spawnPosition, Vector3.one * (nCir == 1 ? 1 : 0.9f), clockwise);
                if (nCir == 2)
                {
                    SpawnNewBarrier(typeIndex, spawnPosition, Vector3.one * 1.25f, !clockwise);
                    changerType = typeIndex == CIRCLE_BARRIER ? ColorChanger.ChangerType.BlueRed : ColorChanger.ChangerType.GreenYellow;
                }
            }
            else
                SpawnNewBarrier(typeIndex, spawnPosition, Vector3.one, Random.Range(0, 2) == 1);
        }
        SpawnStar(spawnPosition);
        if(index > 0) SpawnChanger(spawnPosition + new Vector3(0, -300), changerType);
        spawnPosition += new Vector3(0, 600);
    }

    private void SpawnNewBarrier(int index, Vector3 localPos, Vector3 localScale, bool clockwiseRotation)
    {
        Barrier barr = (Barrier)Instantiate(barrierPrefabs[index], Vector3.zero, Quaternion.identity);
        barr.transform.SetParent(transform);
        barr.onMoveOut += DestroyBarrier;
        barr.transform.localPosition = localPos;
        barr.transform.localScale = localScale;
        barr.Rotate(clockwiseRotation);
        listBarrs.Add(barr);
    }

    public void ShowStarEffect(Vector3 localPosition, Vector3 position)
    {
        GameObject effect = (GameObject)Instantiate(starExplosion, position, Quaternion.identity);
        effect.transform.localScale = Vector3.one;

        GameObject score = (GameObject)Instantiate(scorePrefab, position, Quaternion.identity);
        score.transform.SetParent(transform);
        score.transform.localScale = Vector3.one;
        score.transform.localPosition = localPosition;
    }

    public void ShowPlayerEffect(Vector3 position)
    {
        GameObject effect = (GameObject)Instantiate(playerExplosion, position, Quaternion.identity);
        effect.transform.localScale = Vector3.one;
    }

    private void SpawnChanger(Vector3 localPos, ColorChanger.ChangerType type)
    {
        ColorChanger changer = (ColorChanger)Instantiate(colorChangerPrefab, Vector3.zero, Quaternion.identity);
        changer.type = type;
        changer.transform.SetParent(transform);
        changer.transform.localPosition = localPos;
        changer.transform.localScale = Vector3.one;
        changer.Rotate(true);
    }

    private void SpawnStar(Vector3 localPos)
    {
        GameObject star = (GameObject)Instantiate(starPrefab, Vector3.zero, Quaternion.identity);
        star.transform.SetParent(transform);
        star.transform.localPosition = localPos;
        star.transform.localScale = Vector3.one;
    }

    public void DestroyBarrier(Barrier barrier)
    {
        listBarrs.Remove(barrier);
        Destroy(barrier.gameObject);
        if (listBarrs.Count < 10)
        {
            SpawnNewBarrier(barrierIndex);
            barrierIndex++;
        }
    }

    public void Update()
    {
        if (player.highestDelta.y > 0)
        {
            transform.localPosition = -player.highestDelta;
        }
    }

    public void DestroyAll()
    {
        foreach (Barrier barr in listBarrs)
        {
            Destroy(barr.gameObject);
        }
        listBarrs.Clear();
    }
}
