using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    public GameObject game_area;
    public GameObject ship_prefab;

    public int UFO_count = 0;
    public int UFO_limit = 150;
    public int UFO_per_frame = 1;

    public float spawn_sphere_radius = 80.0f;
    public float death_sphere_radius = 90.0f;

    public float speed = 12.0f;


    void Start()
    {
        InitialPopulation();
    }

    void Update()
    {
        MaintainPopulation();
    }

    void InitialPopulation()
    {
        

        for (int i = 0; i < UFO_limit; i++)
        {
            Vector3 position = GetRandomPosition(true);
            UFO_Mover ship_script = AddShip(position);
            ship_script.transform.Rotate(Vector3.forward * Random.Range(0.0f, 360.0f));
        }
    }

    void MaintainPopulation()
    {
        
        if (UFO_count < UFO_limit)
        {
            for (int i = 0; i < UFO_per_frame; i++)
            {
                Vector3 position = GetRandomPosition(false);
                UFO_Mover ship_script = AddShip(position);
                ship_script.transform.Rotate(Vector3.forward * Random.Range(-45.0f, 45.0f));
            }
        }
    }

    Vector3 GetRandomPosition(bool within_camera)
    {
      

        Vector3 position = Random.insideUnitSphere;

        if (within_camera == false)
        {
            position = position.normalized;
        }

        position *= spawn_sphere_radius;

        return position;
    }

    UFO_Mover AddShip(Vector3 position)
    {
        

        UFO_count += 1;
        GameObject new_ship = Instantiate(
            ship_prefab,
            position,
            Quaternion.FromToRotation(Vector3.up, (game_area.transform.position - position)),
            gameObject.transform
        );

        UFO_Mover ship_script = new_ship.GetComponent<UFO_Mover>();
        ship_script.UFO_spawner = this;
        ship_script.game_area = game_area;
        ship_script.speed = speed;

        return ship_script;
    }
}