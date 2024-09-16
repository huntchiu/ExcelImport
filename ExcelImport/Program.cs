using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        // 测试数据
        var records = new List<Record>
        {
            new Record { SYS_ID = "4800AFCC-0220-4CFD-B958-0005AD4C66E0", STY_NO = "FN5446", RMK = "此款为202308BUY翻单，FN5446同FN5447，寄ID唛（原料仓）"},
            new Record { SYS_ID = "D36C759F-9E4E-4E1A-BA1C-009DF9A7BA48", STY_NO = "FV6623", RMK = "车接裤头丈巾成圆、定丈巾点（原料仓），寄ID唛（原料仓）"},
            new Record { SYS_ID = "4AFCE812-168E-448D-B584-00A3F67BC67B", STY_NO = "FJ3861", RMK = "注：此款为202306-FJ3901修改，FJ3861同FJ3862同FJ3864同FB8858（短款）FJ3901同FB8860（长款）"},
            new Record { SYS_ID = "E92BFC4A-ED00-4AC2-B1F3-00AE8C884FE6", STY_NO = "FN3217", RMK = "此款同202309-FN3216工序，寄ID唛（原料仓），车接裤头丈巾成圆、定丈巾点（原料仓定丈巾点除2XS-S码） FN3216同FN3217"},
            new Record { SYS_ID = "31971CE8-F626-4D66-A7B3-00D1705ED128", STY_NO = "FQ0702", RMK = "车接裤头丈巾成圆、定丈巾点（原料仓） 寄ID唛（原料仓）"},
            new Record { SYS_ID = "FB71D8DB-3D15-4FB0-961C-011DD9FEAD90", STY_NO = "FN6348", RMK = "此款同202306-FN6348工序， 车接衫脚丈巾成圆、定丈巾点（原料仓）， 车接袖口丈巾成圆X2（原料仓，除：XS码），寄ID唛（原料仓）， FN6348同FN6349"},
            new Record { SYS_ID = "2BE412D1-3E8B-4BF9-A449-01B949894DD1", STY_NO = "FZ0945", RMK = "注;此款为SS2308-FN5848工序修改，FZ0945同FZ0946约同FN5848同FN5849，车接袖口丈巾成圆X2（原料仓）（除：XS码）寄ID唛(原料仓)"},
            new Record { SYS_ID = "2635DD33-37CE-465D-B3F5-01E57FA7F08B", STY_NO = "FN4527", RMK = "此款同202305-FN4527工序， FN4527同FN4528约同（FN4649同FN4650）（SP24）"},
            new Record { SYS_ID = "648ABFF7-B833-4AE7-AECB-028620E9F3AD", STY_NO = "HM7158", RMK = "HM8241修改"},
            // 继续添加剩余的记录...
        };
        
        foreach (var record in records)
        {
            var sameStyNos = new HashSet<string>();
    
            // 正则表达式匹配紧邻“同”字前后的款号
            var pattern = @"([A-Z]{2}\d{4})\s*同\s*([A-Z]{2}\d{4})";
            var matches = Regex.Matches(record.RMK, pattern);

            foreach (Match match in matches)
            {
                // 分别获取“同”字前后的款号
                var styNoBefore = match.Groups[1].Value;
                var styNoAfter = match.Groups[2].Value;

                // 检查“同”字前的款号是否不是STY_NO，且“同”字后的款号也不是STY_NO
                if (styNoBefore != record.STY_NO && styNoAfter != record.STY_NO)
                {
                    // 如果两个款号都符合条件，添加到结果中
                    sameStyNos.Add(styNoBefore);
                    sameStyNos.Add(styNoAfter);
                }
                else if (styNoBefore == record.STY_NO && styNoAfter != record.STY_NO)
                {
                    // 如果仅“同”字后的款号符合条件，只添加它
                    sameStyNos.Add(styNoAfter);
                }
                else if (styNoBefore != record.STY_NO && styNoAfter == record.STY_NO)
                {
                    // 如果仅“同”字前的款号符合条件，只添加它
                    sameStyNos.Add(styNoBefore);
                }
            }

            record.SAME_STY_NO = string.Join(", ", sameStyNos);

            // 打印结果
            Console.WriteLine($"SYS_ID: {record.SYS_ID}, STY_NO: {record.STY_NO}, RMK: {record.RMK}, SAME_STY_NO: {record.SAME_STY_NO}");
        }

    }
}

public class Record
{
    public string SYS_ID { get; set; }
    public string STY_NO { get; set; }
    public string RMK { get; set; }
    public string SAME_STY_NO { get; set; } // 新增字段用于存放相同款号
}

// 代码逻辑：
//
// 第一条件：确保“同”字前后都紧跟款号且中间没有其他字符。僅需要查找紧邻“同”字的款号模式。
// 第二条件：“同”字前的款号（如果存在）必须不是STY_NO。
// 第三条件：“同”字后的款号必须不是STY_NO。
// 满足第一条件的情况下，才考虑第二和第三条件。