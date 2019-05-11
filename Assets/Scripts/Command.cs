using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Command{

    public Unit FromUnit { get; set; }
    public Unit ToUnit { get; set; }
    public Ability Ability { get; set; }
    //public string unitType;
    //public string commandType;

    //アイテムのID
    //public string itemID;

    //public Command()
    //{

    //}

    //public Command(Unit fromUnit, Unit toUnit, Ability ability)
    //{
    //    this.fromUnit = fromUnit;
    //    this.toUnit = toUnit;
    //    //this.unitType = unitType;
    //    this.ability = ability;
    //    //this.commandType = commandType;
    //    //this.itemID = itemID;
    //}

}
