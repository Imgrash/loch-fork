using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    [SerializeField]
    private List<Tag> _tags;

    public List<Tag> All => _tags;
    public bool HasTag(Tag _tag) => _tags.Contains(_tag);
    public bool HasTag(string _tagName) => _tags.Exists(x => x.Name == _tagName);
}
