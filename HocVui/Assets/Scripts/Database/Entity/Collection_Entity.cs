using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Collection_Entity
{
    public string Name;
    public List<Question_Entity> ListQuestion;
    public string Video;


    public Collection_Entity()
    {

    }
    public Collection_Entity(string Name, List<Question_Entity> ListQuestion, string Video)
    {
        this.Name = Name;
        this.ListQuestion = ListQuestion;
        this.Video = Video;
    }

}
