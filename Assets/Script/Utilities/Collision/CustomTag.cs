using UnityEngine;
using System.Collections.Generic;

public class CustomTag : MonoBehaviour
{
    public GameObject[] Children;

    public bool IsEnabled = true;
    private void Awake()
    {
        foreach (GameObject child in Children)
        {
            var check = child.GetComponent<CustomTag>();
            if (check != null)
            {
                child.AddComponent<CustomTag>();
                child.GetComponent<CustomTag>().tags = tags;
            }
        }
    }

    [SerializeField]
    private List<string> tags = new List<string>();
    
    public bool HasTag(string tag)
    {
        return tags.Contains(tag);
    }

    public IEnumerable<string> GetTags()
    {
        return tags;
    }

    public void Rename(int index, string tagName)
    {
        tags[index] = tagName;
    }

    public string GetAtIndex(int index)
    {
        return tags[index];
    }

    public int Count
    {
        get { return tags.Count; }
    }
}
