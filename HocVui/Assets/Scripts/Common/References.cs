using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class References
{
    //public static Account_Entity account;
    public static Account_Entity account = new Account_Entity
    {
        Id = 4,
        Username = "thienthien"
    };


    public static int SelectedAnswer = -1;
    public static int Streak = 0;
    public static int Score = 0;
    public static int TimeAnswer = 0;



    public static int X2 = 1;
    public static bool TimeFreeze = false;

    public static List<Skin_Entity> listSkin = Skin_DAO.GetAll();
    public static List<Eye_Enity> listEye = Eye_DAO.GetAll();
    public static List<Mouth_Entity> listMouth = Mouth_DAO.GetAll();
    public static List<Hair_Entity> listHair = Hair_DAO.GetAll();


    public static List<Spell_Entity> ListSpell = new List<Spell_Entity>()
    {
        new Spell_Entity("50/50", "Bạn sẽ thấy 2 phương án sai (chỉ cho mình bạn)", "Spell/5050", Spell.FiftyFifty, Spell_Type.ClickToUse, Spell_Target.None),
        new Spell_Entity("X2 điểm", "Nhận gấp đôi số điểm cho câu hỏi này","Spell/X2", Spell.DoubleScore, Spell_Type.ClickToUse, Spell_Target.None),
        new Spell_Entity("Ngưng thòi gian", "Điểm số của bạn sẽ không bị ảnh hưởng bởi thời gian trong câu hỏi này","Spell/Oclock", Spell.TimeFreeze, Spell_Type.ClickToUse, Spell_Target.None),
        new Spell_Entity("Khóa chân", "Khiến 1 người chơi không thể di chuyển trong 5 giây","Spell/Freeze", Spell.PlayerFreeze, Spell_Type.ConfirmToUse, Spell_Target.Player),
        new Spell_Entity("Khóa cửa", "Khóa vùng cắm cọc trong 5 giây","Spell/Lock", Spell.Lock, Spell_Type.ConfirmToUse, Spell_Target.PileBase),
    };

    public static List<Question_Entity> ListQuestionCreate = new List<Question_Entity>();

    public static List<Spell_Entity> ListSpell_Own = new List<Spell_Entity>()
    {
        new Spell_Entity("50/50", "Bạn sẽ thấy 2 phương án sai (chỉ cho mình bạn)", "Spell/5050", Spell.FiftyFifty, Spell_Type.ClickToUse, Spell_Target.None),
        new Spell_Entity("X2 điểm", "Nhận gấp đôi số điểm cho câu hỏi này","Spell/X2", Spell.DoubleScore, Spell_Type.ClickToUse, Spell_Target.None),
        new Spell_Entity("Ngưng thòi gian", "Điểm số của bạn sẽ không bị ảnh hưởng bởi thời gian trong câu hỏi này","Spell/Oclock", Spell.TimeFreeze, Spell_Type.ClickToUse, Spell_Target.None),
    };


    public static Collection_Entity SelectCollection = new Collection_Entity();

    public static Dictionary<string, int> RankingList = new Dictionary<string, int>();

    public static string Chat_ServerName;

    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            sb.Append(chars[new System.Random().Next(chars.Length)]);
        }

        return sb.ToString();
    }

    public static void SetRandomSpell()
    {
        ListSpell_Own = GetRandomSpell(ListSpell, 3);
    }

    public static List<Spell_Entity> GetRandomSpell(List<Spell_Entity> sourceList, int count)
    {

        List<Spell_Entity> result = new List<Spell_Entity>();
        List<Spell_Entity> availableSpell_Entitys = new List<Spell_Entity>(sourceList);

        System.Random random = new System.Random();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = random.Next(0, availableSpell_Entitys.Count);
            Spell_Entity selectedSpell_Entity = availableSpell_Entitys[randomIndex];
            result.Add(selectedSpell_Entity);
            availableSpell_Entitys.RemoveAt(randomIndex);
        }

        return result;
    }

    public static string Hash(string text)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
        StringBuilder hashSb = new StringBuilder();
        foreach (byte b in hash)
        {
            hashSb.Append(b.ToString("X2"));
        }
        return hashSb.ToString();
    }

}

public enum Confirm_Type
{
    Leave, StopHost, Kick
}

public enum Spell_Target
{
    None, Player, PileBase
}

public enum Spell
{
    None, FiftyFifty, DoubleScore, TimeFreeze, PlayerFreeze, RemovePile, Lock
}

public enum Spell_Type
{
    ClickToUse, ConfirmToUse
}

public enum Role
{
    Student = 1, Teacher = 2
}

public enum TypeLayout
{
    Hair, Eye, Mouth, Skin
}