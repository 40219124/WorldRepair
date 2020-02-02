using System.Collections.Generic;
using UnityEngine;

public class Plant : BehaviourObj
{
    public static List<Plant> AllPlants = new List<Plant>();

    public Animator anim;
    public Sprite[] RandomSprites;

    public GameObject Spreading;
    public float MinSpreadWait;
    public float MaxSpreadWait;

    private void Awake()
    {
        AllPlants.Add(this);

        if (Spreading != null)
        {
            StartCoroutine(SpreadSubroutine());
        }
    }

    private void OnDestroy()
    {
        AllPlants.Remove(this);
    }

    public override IEnumerator<YieldInstruction> Run(PlayerController player, Interactable target)
    {
        // Place this plant.
        if (target == null)
        {
            transform.position = player.DropPoint.position;
        }
        else
        {
            transform.position = target.transform.position;
        }

        if (target != null)
        {
            // We are a plant being used on something.
            // It's probably another plant.
            // Lets destroy the other plant.
            Destroy(target.gameObject);
        }

        // Have a random graphic.
        var thisInteractable = GetComponent<Interactable>();
        thisInteractable.Renderer.sprite = RandomSprites[Random.Range(0, RandomSprites.Length)];

        yield break;
    }

    private IEnumerator<YieldInstruction> SpreadSubroutine()
    {
        var cache = new List<Plant>();

        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(Random.Range(MinSpreadWait, MaxSpreadWait));

            cache.Clear();
            float localDistance = 0.5f;

            foreach (var plant in AllPlants)
            {
                float thisDistance = Vector3.Distance(transform.position, plant.transform.position);

                if (thisDistance < localDistance)
                {
                    cache.Add(plant);
                }
            }

            if (cache.Count < 4)
            {
                var clone = Instantiate(Spreading);

                if (cache.Count == 1)
                {
                    clone.transform.position = transform.position
                        + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                }
                else
                {
                    var direction = Vector3.zero;
                    foreach (var plant in cache)
                    {
                        if (plant == this)
                        {
                            continue;
                        }
                        direction += (plant.transform.position - transform.position).normalized;
                    }

                    clone.transform.position = transform.position - (direction.normalized * 0.5f)
                    + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                }
            }
        }
    }
}
