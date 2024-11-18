using UnityEngine;
using VRC.SDK3.Data;

namespace sh0uRoom.PJ7S
{
    // Enum for assigning index of field DataTokens
    internal enum CharacterParameterField
    {
        Lv,
        MaxHp,
        MaxMp,
        MaxTp,
        Atk,
        Def,
        Mat,
        Mdf,
        Spd,
        Luk,

        Count
    }

    public class CharacterParameter : DataList
    {
        // Constructor
        public static CharacterParameter New(int Lv = 0, int MaxHp = 0, int MaxMp = 0, int MaxTp = 0, int Atk = 0, int Def = 0, int Mat = 0, int Mdf = 0, int Spd = 0, int Luk = 0)
        {
            var data = new DataToken[(int)CharacterParameterField.Count];

            data[(int)CharacterParameterField.Lv] = Lv;
            data[(int)CharacterParameterField.MaxHp] = MaxHp;
            data[(int)CharacterParameterField.MaxMp] = MaxMp;
            data[(int)CharacterParameterField.MaxTp] = MaxTp;
            data[(int)CharacterParameterField.Atk] = Atk;
            data[(int)CharacterParameterField.Def] = Def;
            data[(int)CharacterParameterField.Mat] = Mat;
            data[(int)CharacterParameterField.Mdf] = Mdf;
            data[(int)CharacterParameterField.Spd] = Spd;
            data[(int)CharacterParameterField.Luk] = Luk;

            return (CharacterParameter)new DataList(data);
        }

        public static CharacterParameter New(string json)
        {
            var data = new DataToken[(int)CharacterParameterField.Count];

            if (VRCJson.TryDeserializeFromJson(json, out DataToken result))
            {
                if (result.TokenType != TokenType.DataDictionary) return null;

                var dictionary = (DataDictionary)result;
                data[(int)CharacterParameterField.Lv] = dictionary["Lv"].Number;
                data[(int)CharacterParameterField.MaxHp] = dictionary["MaxHp"].Number;
                data[(int)CharacterParameterField.MaxMp] = dictionary["MaxMp"].Number;
                data[(int)CharacterParameterField.MaxTp] = dictionary["MaxTp"].Number;
                data[(int)CharacterParameterField.Atk] = dictionary["Atk"].Number;
                data[(int)CharacterParameterField.Def] = dictionary["Def"].Number;
                data[(int)CharacterParameterField.Mat] = dictionary["Mat"].Number;
                data[(int)CharacterParameterField.Mdf] = dictionary["Mdf"].Number;
                data[(int)CharacterParameterField.Spd] = dictionary["Spd"].Number;
                data[(int)CharacterParameterField.Luk] = dictionary["Luk"].Number;

                return (CharacterParameter)new DataList(data);
            }
            else
            {
                return null;
            }
        }
    }

    public static class CharacterParameterExt
    {
        // Get methods
        public static int Lv(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.Lv].Number;
        public static int MaxHp(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.MaxHp].Number;
        public static int MaxMp(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.MaxMp].Number;
        public static int MaxTp(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.MaxTp].Number;
        public static int Atk(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.Atk].Number;
        public static int Def(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.Def].Number;
        public static int Mat(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.Mat].Number;
        public static int Mdf(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.Mdf].Number;
        public static int Spd(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.Spd].Number;
        public static int Luk(this CharacterParameter instance)
            => (int)instance[(int)CharacterParameterField.Luk].Number;

        // Set methods
        public static void Lv(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.Lv] = arg;
        public static void MaxHp(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.MaxHp] = arg;
        public static void MaxMp(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.MaxMp] = arg;
        public static void MaxTp(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.MaxTp] = arg;
        public static void Atk(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.Atk] = arg;
        public static void Def(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.Def] = arg;
        public static void Mat(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.Mat] = arg;
        public static void Mdf(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.Mdf] = arg;
        public static void Spd(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.Spd] = arg;
        public static void Luk(this CharacterParameter instance, int arg)
            => instance[(int)CharacterParameterField.Luk] = arg;
    }
}
