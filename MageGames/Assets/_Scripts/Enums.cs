#region Itens

public enum ItemType
{
    Resource,
    Staff,
    Spellbook,
    Ring,
    Consumable
}

public enum CollectablesType
{
    None = 0,

    GoldCoin = 1,
    BlueCoin = 2,
    BloodStone = 26,

    StaffLeaf = 3,
    Staff02 = 4,

    Book01 = 5,
    Book02 = 6,
    Book03 = 7,
    Book04 = 8,
    Book05 = 9,

    Ring01 = 10,
    Ring02 = 11,
    Ring03 = 12,
    Ring04 = 13,
    Ring05 = 14,
    Ring06 = 15,
    Ring07 = 16,
    Ring08 = 17,

    SmallHealthPotion = 18,
    MediumHealthPotion = 19,
    BigHealthPotion = 20,
    DamagePotion = 21,
    SpeedPotion = 22,

    HeartFragment = 23,
    ManaFragment = 24,
    RingSlotFragment = 25,
}
public enum CoinType
{
    GoldCoin = 0,
    BlueCoin = 1,
}
public enum StaffType
{
    LeafStaff,
    Staff02
}
public enum SpellbookType
{
    Book01,
    Book02,
    Book03,
    Book04,
    Book05
}
public enum KeyType
{
    RedOrb,
    YellowOrb,
    BlueOrb,
    WhiteOrb,
    BlackOrb
}
public enum RingType
{
    Ring01,
    Ring02,
    Ring03,
    Ring04,
    Ring05,
    Ring06,
    Ring07,
    Ring08,
}
public enum ConsumablesType
{
    SmallHealthPotion,
    MediumHealthPotion,
    BigHealthPotion,
    DamagePotion,
    SpeedPotion
}
public enum FragmentType
{
    HeartFragment,
    ManaFragment,
    RingSlotFragment,
}
#endregion

#region Enemies
public enum EnemiesType
{
    SkeletonArcher = 0,
    SkeletonAxe =1,
    RedSkeletonArcher = 2,
    SkeletonBoss = 3
}

public enum EnemyAreaState
{
    Far,
    InArea,
    CloseArea,
}
#endregion

#region Weapons
public enum WandType
{
    None,
    LeafWand,
}
public enum ProjectilesType
{
    none = 0,
    Leaf = 1,
    RedArrow = 2,
    TurretBullet = 3,
    GiantRedArrow = 4,
    BulletCircleShot = 5
}
#endregion

#region Area
public enum Area
{
    None,
    HubIsland,
    RitualCave,
}
#endregion

#region Wave System
public enum WaveType
{
    IndividualWave,
    FullWave
}
#endregion

#region Cutscene
public enum CutsceneState
{
    None,
    Activated,
    Waiting,
    Finished,
}
#endregion

public enum InteractType
{
    Click,
    Hold
}
public enum DialogState
{
    None,
    tyiping,
    WaitingNextText,
}
public enum SpeechType
{
    none,
    EnterSpeech,
    InteractSpeech,
    ExitSpeech,
    HittedSpeech,
}
public enum DialogType
{
    FreeDialog,
    FreezeDialog,
    TimerDialog,
}
public enum CursorType
{
    Pointer,
    Crosshair,
}
public enum HealthState
{
    Health100,
    Health75,
    Health50,
    Health25,
}

public enum DoorDirection
{
    None,
    Up,
    Right,
    Down,
    Left
}

public enum RoomType
{
    None,
    Start,
    Regular,
    Hub,
    Reward,
    Boss
}