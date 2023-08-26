using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class References
{
    public static List<Spell_Entity> ListSpell = new List<Spell_Entity>()
    {
        new Spell_Entity("50/50", "Loại bỏ 2 phương án sai (chỉ cho mình bạn)", "Spell/Image_Skill_0", Spell.FiftyFifty),
        new Spell_Entity("X2 điểm", "Nhận gấp đôi số điểm cho câu hỏi này","Spell/Image_Skill_1", Spell.DoubleScore),
        new Spell_Entity("Ngưng thòi gian", "Điểm số của bạn sẽ không bị ảnh hưởng bởi thời gian trong câu hỏi này","Spell/Image_Skill_2", Spell.TimeFreeze),
        new Spell_Entity("Khóa chân", "Khiến 1 người chơi không thể di chuyển trong 5 giây","Spell/Image_Skill_4", Spell.PlayerFreeze),
        new Spell_Entity("Giấu giếm", "Làm 1 cọc bị biến mất trong 5 giây","Spell/Image_Skill_5", Spell.RemovePile),
        new Spell_Entity("Khóa cửa", "Khóa vùng cắm cọc trong 5 giây","Spell/Image_Skill_9", Spell.Lock),

    };

    public static List<Spell_Entity> ListSpell_Own = new List<Spell_Entity>();

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

public enum Spell
{
    FiftyFifty, DoubleScore, TimeFreeze, PlayerFreeze, RemovePile, Lock
}

public enum Role
{
    Student = 1, Teacher = 2
}