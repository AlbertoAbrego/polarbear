using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablesController : MonoBehaviour
{
    public static TablesController sharedInstance;
    private Table[] tables = new Table[9];

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        StartTables();
    }

    void StartTables()
    {
        for (int i = 0; i < 9; i++)
        {
            tables[i] = new Table();
        }
    }

    public Table[] GetTables()
    {
        return tables;
    }
}
