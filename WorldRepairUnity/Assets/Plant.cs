using System.Collections.Generic;
using UnityEngine;

public class Plant : BehaviourObj
{
    public static List<Plant> AllPlants = new List<Plant>();
    public static float GlobalLockout;

    public Animator anim;
    public Sprite[] RandomSprites;
    public int TreeType;

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
        TreeType = Random.Range(0, RandomSprites.Length);
        thisInteractable.Renderer.sprite = RandomSprites[TreeType];

        yield break;
    }

    private IEnumerator<YieldInstruction> SpreadSubroutine()
    {
        var cache = new List<Plant>();

        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(Random.Range(MinSpreadWait, MaxSpreadWait));

            while (Time.time < GlobalLockout)
            {
                yield return null;
            }

            cache.Clear();
            float localDistance = 1.2f;

            foreach (var plant in AllPlants)
            {
                float thisDistance = Vector3.Distance(transform.position, plant.transform.position);

                if (thisDistance < localDistance)
                {
                    cache.Add(plant);
                }
            }

            if (cache.Count < 3)
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
                        + new Vector3(Random.Range(-0.65f, 0.65f), 0, Random.Range(-0.65f, 0.65f));

                    clone.transform.position = new Vector3(
                        clone.transform.position.x,
                        0,
                        clone.transform.position.z);
                }

                GlobalLockout = Time.time + 0.5f;
            }
        }
    }
}
