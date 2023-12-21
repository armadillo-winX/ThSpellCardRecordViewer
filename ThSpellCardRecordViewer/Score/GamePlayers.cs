using System.Reflection;

namespace ThSpellCardRecordViewer.Score
{
    internal class GamePlayers
    {
        public static string Th06Players => "博麗霊夢(霊),博麗霊夢(夢),霧雨魔理沙(魔),霧雨魔理沙(恋)";

        public static string Th07Players => "博麗霊夢(霊),博麗霊夢(夢),霧雨魔理沙(魔),霧雨魔理沙(恋),十六夜咲夜(幻),十六夜咲夜(時)";

        public static string Th08Players => "霊夢＆紫,魔理沙＆アリス,咲夜＆レミリア,妖夢＆幽々子,博麗霊夢,八雲紫,霧雨魔理沙,アリス・マーガトロイド,十六夜咲夜,レミリア・スカーレット,魂魄妖夢,西行寺幽々子";

        public static string Th09Players => "";

        public static string Th10Players => "霊夢(誘導),霊夢(前方集中),霊夢(封印),魔理沙(高威力),魔理沙(貫通),魔理沙(魔法使い)";

        public static string Th11Players => "霊夢&紫,霊夢&萃香,霊夢&文,魔理沙&アリス,魔理沙&パチュリー,魔理沙&にとり";

        public static string Th12Players => "博麗霊夢(霊),博麗霊夢(夢),霧雨魔理沙(恋),霧雨魔理沙(魔),東風谷早苗(蛇),東風谷早苗(蛙)";

        public static string Th13Players => "博麗霊夢,霧雨魔理沙,東風谷早苗,魂魄妖夢";

        public static string Th14Players => "博麗霊夢(A),博麗霊夢(B),霧雨魔理沙(A),霧雨魔理沙(B),十六夜咲夜(A),十六夜咲夜(B)";

        public static string Th15Players => "博麗霊夢(P),博麗霊夢(L),霧雨魔理沙(P),霧雨魔理沙(L),東風谷早苗(P),東風谷早苗(L),鈴仙・優曇華院・イナバ(P),鈴仙・優曇華院・イナバ(L)";

        public static string Th16Players => "博麗霊夢,日焼けしたチルノ,射命丸文,霧雨魔理沙";

        public static string Th17Players => "博麗霊夢(狼),博麗霊夢(獺),博麗霊夢(鷲),霧雨魔理沙(狼),霧雨魔理沙(獺),霧雨魔理沙(鷲),魂魄妖夢(狼),魂魄妖夢(獺),魂魄妖夢(鷲)";

        public static string Th18Players => "博麗霊夢,霧雨魔理沙,十六夜咲夜,東風谷早苗";

        public static string[]? GetGamePlayers(string gameId)
        {
            PropertyInfo? gamePlayersProperty = typeof(GamePlayers).GetProperty($"{gameId}Players");
            string[]? gamePlayers = null;
            if (gamePlayersProperty != null)
            {
                gamePlayers = gamePlayersProperty.GetValue(null, null) != null ?
                    gamePlayersProperty.GetValue(null, null).ToString().Split(',') :
                    null;
            }

            return gamePlayers;
        }
    }
}
