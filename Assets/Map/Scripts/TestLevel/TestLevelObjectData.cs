using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelObjectData : MonoBehaviour {

    [Header("Ground layer")]
    [SerializeField]
    public GameObject ground;
    public GameObject groundParent;

    [Header("Object Layer")]
    [SerializeField]
    public GameObject wood;
    public GameObject woodParent;

    [SerializeField]
    public GameObject rock;
    public GameObject rockParent;
}
