using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnumHolder;
using static Utility;

public class Venturer : Unit
{
    private Race race = Race.人間;
    private Job job = Job.warrior;

    //レア度？
    private int rarity = 1;

    private bool isUnique = false;

    //public AllyManager allyManager;
    public ItemGenerator itemGenerator;
    public SkillGenerator2 skillGenerator;

    //所持しているアイテム
    public List<Item> Items = new List<Item>();

    //経験値
    private int expSum = 0;

    //次レベルまでの経験値
    private int nextLvExp = 100;

    //???
    private List<int> lvUpHps = new List<int>();
    private List<int> lvUpSps = new List<int>();

    //装備の情報
    private Weapon equippedWeapon = new Weapon();
    private Guard equippedArmor = new Guard(guardType: GuardType.armor);
    private Guard equippedAccessory = new Guard(guardType: GuardType.accessory);

    private Dictionary<Status, int> initialStatuses = new Dictionary<Status, int>()
    {
        {Status.Lv,0},
        {Status.MaxHP, 0},
        {Status.currentHP, 0},
        {Status.MaxSP, 0},
        {Status.currentSP, 0},
        {Status.STR, 0},
        {Status.DEF, 0},
        {Status.INT, 0},
        {Status.MNT, 0},
        {Status.TEC, 0},
        {Status.AGI, 0},
        {Status.LUK, 0}
    };

    public Venturer(GameController gameController, ItemGenerator itemGenerator, SkillGenerator2 skillGenerator)
       : base (gameController)
    {
        this.itemGenerator = itemGenerator;
        this.skillGenerator = skillGenerator;
    }

    public void Init()
    {
        InitEquipmentsRandomly();
        InitItemsRandomyly();
    }


    public void InitEquipmentsRandomly()
    {
        if (isUnique)
        {

        }

        var weaponOptions = new List<Weapon>();
        var armorOptions = new List<Guard>();
        var accessoryOptions = new List<Guard>();

        for (int i = 0; i < 3; i++)
        {
            int num = Poisson(2.0);
            if (i == 0)
            {
                foreach (Equipment e in itemGenerator.EquipmentInfo)
                {
                    if (e.EquipType == EquipType.weapon &&
                        e.Rarity < num)
                    {
                        Weapon weapon = e as Weapon;
                        weaponOptions.Add(weapon);
                    }
                }
            }
            else if (i == 1)
            {
                foreach (Equipment e in itemGenerator.EquipmentInfo)
                {
                    if (e.EquipType == EquipType.guard)
                    {
                        Guard armor = e as Guard;
                        if (armor.GuardType == GuardType.armor &&
                            armor.Rarity < num)
                        {
                            armorOptions.Add(armor);
                        }
                    }
                }
            }
            else
            {
                foreach (Equipment e in itemGenerator.EquipmentInfo)
                {
                    if (e.EquipType == EquipType.guard)
                    {
                        Guard accesorry = e as Guard;
                        if (accesorry.GuardType == GuardType.accessory &&
                            accesorry.Rarity < num)
                        {
                            armorOptions.Add(accesorry);
                        }
                    }
                }
            }
        }

        int randomNum;

        if (weaponOptions.Count > 0)
        {
            Weapon weapon = itemGenerator.GenerateWeapon(GetRandomFromList(weaponOptions).Name);
            equippedWeapon = weapon;
        }
        if (armorOptions.Count > 0)
        {
            Guard armor = itemGenerator.GenerateGuard(GetRandomFromList(armorOptions).Name);
            equippedArmor = armor;
        }
        if (accessoryOptions.Count > 0)
        {
            Guard accessory = itemGenerator.GenerateGuard(GetRandomFromList(accessoryOptions).Name);
            equippedArmor = accessory;
        }

    }

    public void InitItemsRandomyly()
    {
        int randomRarity = Poisson(2.0);
        var itemOptions = new List<Item>();
        foreach (Item i in itemGenerator.ItemInfo)
        {
            if (i.Rarity < randomRarity)
            {
                itemOptions.Add(i);
            }
        }

        int randomNum = Poisson(3.0);
        for (int i = 0; i < randomNum; i++)
        {
            Item item = itemGenerator.Generate(GetRandomFromList(itemOptions).Name);
            Items.Add(item);
        }
    }


    public Ally ConvertToAlly()
    {
        return null;
    }

    public Enemy ConvertToEnemy()
    {
        return null;
    }

}

