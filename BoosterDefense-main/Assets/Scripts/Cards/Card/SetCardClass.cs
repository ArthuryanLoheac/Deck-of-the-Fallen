using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SetCardClass : MonoBehaviour
{
    protected string GetSpriteType(TypeCard type)
    {
        switch (type)
        {
            case TypeCard.Sort:
                return "SPELL";
            case TypeCard.Npc:
                return "HEROS";
            case TypeCard.Batiment:
                return "BUILDING";
            case TypeCard.Vehicule:
                return "VEHICLE";
            case TypeCard.Equipement:
                return "EQUIPMENT";
            default:
                return "BUILDING";
        }
    }

    protected private Color GetColorTransparent(Color col, bool b)
    {
        if (b)
            return new Color(col.r, col.g, col.b, 1);
        else
            return new Color(col.r, col.g, col.b, .7f);
    }

    abstract public void MakeTransparent(bool b);
    abstract public void SetStats(CardStats stats);
    abstract public void SetActiveZoomDrag(bool b, bool reset);
}
